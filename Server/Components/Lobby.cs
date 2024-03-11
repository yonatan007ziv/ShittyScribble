using Server.Components.ClientStages;
using System.Collections.ObjectModel;

namespace Server.Components;

internal class Lobby
{
	public static readonly Dictionary<int, Lobby> lobbies = new Dictionary<int, Lobby>();
	public static event Action? OnLobbiesRefresh;

	private readonly ObservableCollection<GameClientHandler> players = new ObservableCollection<GameClientHandler>();
	private readonly TcpClientHandler host;

	private GameClientHandler currentPlayerTurn;
	private string currentWord;
	private bool chosenWord;

	private CancellationTokenSource skipRoundCts = new CancellationTokenSource();

	private bool gameStarted;
	private int startingScorePool = 200;
	private int scorePool;
	private int numOfCorrectGuesses;

	public int Id { get; set; }
	public string Name { get; set; } = "";
	public string Description { get; set; } = "";
	public string HostName => host.Username;
	public int MaxNumOfPlayers { get; set; }
	public int NumOfPlayers { get; set; }

	public bool LobbyAlive { get; private set; }

	public static bool AddPlayerToLobby(TcpClientHandler tcpClientHandler, int lobbyId)
	{
		if (!lobbies.ContainsKey(lobbyId))
			return false;

		Lobby lobby = lobbies[lobbyId];
		if (lobby.NumOfPlayers >= lobby.MaxNumOfPlayers)
			return false;

		GameClientHandler gameClientHandler = new GameClientHandler(tcpClientHandler);
		gameClientHandler.InGame = true;

		lobbies[lobbyId].players.Add(gameClientHandler);
		lobbies[lobbyId].NumOfPlayers++;
		lobbies[lobbyId].PlayerRead(gameClientHandler);

		Console.WriteLine("Refreshing lobbies");
		OnLobbiesRefresh?.Invoke();

		return true;
	}

	public Lobby(TcpClientHandler host, int id, string name, string description, int maxNumOfPlayers, int numOfPlayers)
	{
		this.host = host;
		Id = id;
		Name = name;
		Description = description;
		MaxNumOfPlayers = maxNumOfPlayers;
		NumOfPlayers = numOfPlayers;

		GameClientHandler hostGameClientHandler = new GameClientHandler(host);
		hostGameClientHandler.InGame = true;

		players.Add(hostGameClientHandler);
		players.CollectionChanged += (s, e) => BroadcastPlayerList();

		lobbies.Add(Id, this);

		LobbyAlive = true;
		OnLobbiesRefresh?.Invoke();
		BroadcastPlayerList();
		TellHost();
		PlayerRead(hostGameClientHandler);

		PeriodicHostAliveCheck();
	}

	private async void DecodePlayerMessage(GameClientHandler tcpClientHandler, string msg)
	{
		// Host wants to start
		if (msg.Contains("StartGame") && tcpClientHandler.tcpClientHandler == host)
		{
			Console.WriteLine("Starting the game");

			string options = msg.Split(':')[1];
			if (!int.TryParse(options.Split(',')[0], out int timePerRound)
				|| !int.TryParse(options.Split(',')[1], out int numOfRounds))
				_ = host.WriteMessage("ErrorParsingGameOptions");
			else if (players.Count < 2) // Check if enough players
				_ = host.WriteMessage("NotEnoughPlayers");
			else // Starts the game
				StartGame(timePerRound, numOfRounds);
		}
		else if (msg.Contains("SendMessage"))
		{
			if (tcpClientHandler == currentPlayerTurn)
				return;

			string word = msg.Split(":")[1];
			if (word.ToLower() == currentWord.ToLower())
			{
				Console.WriteLine($"{tcpClientHandler.tcpClientHandler.Username} guessed the word");

				tcpClientHandler.score += scorePool;
				_ = tcpClientHandler.tcpClientHandler.WriteMessage($"CorrectGuess");
				scorePool -= scorePool / 4;

				numOfCorrectGuesses++;

				if (numOfCorrectGuesses == players.Count - 1)
					skipRoundCts.Cancel();
			}
		}
		else if (msg.Contains("ChosenWord") && tcpClientHandler == currentPlayerTurn)
		{
			chosenWord = true;
			currentWord = msg.Split(':')[1];

			Console.WriteLine($"Chose word: {currentWord}");
		}
		else if (msg.Contains("SendingFrame") && tcpClientHandler == currentPlayerTurn)
		{
			byte[]? imageBytes = await tcpClientHandler.tcpClientHandler.ReadBytes();
			if (imageBytes == null)
				return;

			foreach (GameClientHandler player in players)
			{
				if (player == tcpClientHandler)
					continue;
				_ = player.tcpClientHandler.WriteMessage("SendingFrame");
				_ = player.tcpClientHandler.WriteBytes(imageBytes);
			}
		}
	}

	private void StartGame(int timeToDraw, int numOfRounds)
	{
		gameStarted = true;

		// Remove lobby from selection 
		lobbies.Remove(Id);
		OnLobbiesRefresh?.Invoke();

		// Disable host's GameOptions view
		_ = host.WriteMessage("DisableGameOptions");

		// Unlock scoreboard panel and chat for all players
		foreach (GameClientHandler player in players)
			_ = player.tcpClientHandler.WriteMessage("ShowScoreboardPanel");

		RunRound(timeToDraw, numOfRounds);
	}
	
	private async void RunRound(int timeToDraw, int numOfRounds)
	{
		if (numOfRounds == 0)
		{
			EndGameScoreboard();
			return;
		}

		GameClientHandler[] playersArr = players.ToArray();
		ShuffleArray(playersArr);

		foreach (GameClientHandler player in playersArr)
		{
			_ = player.tcpClientHandler.WriteMessage($"RemainingRounds:{numOfRounds}");
			_ = player.tcpClientHandler.WriteMessage($"RemainingSeconds:{timeToDraw}");
		}

		scorePool = startingScorePool;

		int originalTimeToDraw = timeToDraw;
		foreach (GameClientHandler player in playersArr)
		{
			skipRoundCts = new CancellationTokenSource();
			currentPlayerTurn = player;

			// Gets 3 random words
			string[] randomWords = WordBank.GetRandomWords(3);
			await player.tcpClientHandler.WriteMessage($"YourTurn:{randomWords[0]},{randomWords[1]},{randomWords[2]}");

			while (!chosenWord)
				await Task.Delay(50);


			foreach (GameClientHandler chatEnabledPlayer in playersArr)
				if (chatEnabledPlayer != player)
					_ = chatEnabledPlayer.tcpClientHandler.WriteMessage("EnableChat");
				else
					_ = player.tcpClientHandler.WriteMessage("DisableChat");

			await player.tcpClientHandler.WriteMessage("EnableCanvas");

			// Timer to draw
			while (timeToDraw >= 0)
			{
				foreach (GameClientHandler playerA in playersArr)
					_ = playerA.tcpClientHandler.WriteMessage($"RemainingSeconds:{timeToDraw}");
				try
				{
					await Task.Delay(1000, skipRoundCts.Token);
				}
				catch { break; }

				timeToDraw--;
			}

			// Disable drawing ability
			_ = player.tcpClientHandler.WriteMessage("DisableCanvas");

			// Add score for drawing player
			player.score += scorePool;

            // Send scores
            BroadcastPlayerScores();

			// Reset if chosen word
			chosenWord = false;

			// Reset correct guesses
			numOfCorrectGuesses = 0;

			// Reset score pool
			scorePool = startingScorePool;

			// Reset time to draw
			timeToDraw = originalTimeToDraw;

			foreach (GameClientHandler playerA in playersArr)
				_ = playerA.tcpClientHandler.WriteMessage("ClearCanvas");

			// Wait 3 seconds idk why
			await Task.Delay(3000);
		}

		RunRound(timeToDraw, numOfRounds - 1);
	}

	private void EndGameScoreboard()
	{
		string currentWinner = players[0].tcpClientHandler.Username;
		int currentHighScore = players[0].score;
		foreach (GameClientHandler player in players)
			if (player.score > currentHighScore)
			{
				currentHighScore = player.score;
				currentWinner = player.tcpClientHandler.Username;
			}

		foreach (GameClientHandler player in players)
		{
			_ = player.tcpClientHandler.WriteMessage("DisableCanvas");
			_ = player.tcpClientHandler.WriteMessage("ClearCanvas");
			_ = player.tcpClientHandler.WriteMessage($"Winner:{currentWinner}");
		}
	}

	public static void ShuffleArray<T>(T[] array)
	{
		Random rand = new Random();
		for (int i = array.Length - 1; i > 0; i--)
		{
			int j = rand.Next(0, i + 1);
			T temp = array[i];
			array[i] = array[j];
			array[j] = temp;
		}
	}

	private async void TellHost()
	{
		// Wait for host to get into the GameView, hacky fix but whatever
		await Task.Delay(1000);
		_ = host.WriteMessage("YouAreHost");
	}

	private async void BroadcastPlayerList()
	{
		// Wait for players to get into the GameView, hacky fix but whatever
		await Task.Delay(1000);

		string playerList = $"PlayerList:{players.Count},";

		foreach (GameClientHandler player in players)
			playerList += $"{player.tcpClientHandler.Username},";

		foreach (GameClientHandler player in players)
			_ = player.tcpClientHandler.WriteMessage(playerList);
	}

	private void BroadcastPlayerScores()
	{
		string playerList = $"PlayerScores:{players.Count},";

		foreach (GameClientHandler player in players)
			playerList += $"({player.tcpClientHandler.Username},{player.score}),";

        foreach (GameClientHandler player in players)
			_ = player.tcpClientHandler.WriteMessage(playerList);
	}

	private async void PlayerRead(GameClientHandler tcpClientHandler)
	{
		while (tcpClientHandler.InGame)
		{
			string? msg = await tcpClientHandler.tcpClientHandler.ReadMessage();
			if (msg is null)
			{
				players.Remove(tcpClientHandler);
				NumOfPlayers--;

				OnLobbiesRefresh?.Invoke();

				if (gameStarted)
					CloseLobby();

				return;
			}

			DecodePlayerMessage(tcpClientHandler, msg);
		}
	}

	private async void PeriodicHostAliveCheck()
	{
		while (LobbyAlive)
		{
			if (!await host.WriteMessage("ALIVECHECK"))
			{
				CloseLobby();
				break;
			}

			// Wait 0.5 seconds before asking again
			await Task.Delay(500);
		}
	}

	private void CloseLobby()
	{
		if (!LobbyAlive)
			return;

		Console.WriteLine("Closing lobby");

		lobbies.Remove(Id);
		LobbyAlive = false;

		GameClientHandler[] players = this.players.ToArray();
		foreach (GameClientHandler player in players)
		{
			player.InGame = false;
			_ = player.tcpClientHandler.WriteMessage("LobbyClosed");
		}

		OnLobbiesRefresh?.Invoke();
	}
}