using Client.Components;

namespace Client.UserControls;

internal partial class GameView : UserControl
{
	private readonly TcpClientHandler tcpClientHandler;
	private bool inGame;
	private HostOptionsArea? hostAreaOptions;

	private string[] playerNames = Array.Empty<string>();
	private bool draw;

	public GameView(TcpClientHandler tcpClientHandler)
	{
		InitializeComponent();
		this.tcpClientHandler = tcpClientHandler;

		chatPanel.Visible = false;

		scoreboardLabel.Visible = false;
		scoreboardPanel.Visible = false;

		wordButton1.Visible = false;
		wordButton2.Visible = false;
		wordButton3.Visible = false;

		chatTextBox.Enabled = false;
		chatSendButton.Enabled = false;

		wordButton1.Click += WordButtonClick;
		wordButton2.Click += WordButtonClick;
		wordButton3.Click += WordButtonClick;

		chatSendButton.Click += SendMessage;

		inGame = true;
		ReadLoop();
	}

	private async void SendMessage(object? sender, EventArgs e)
	{
		if (chatTextBox.Text == "")
			return;

		string text = chatTextBox.Text;
		chatTextBox.Text = "";
		if(!await tcpClientHandler.WriteMessage($"SendMessage:{text}"))
			OnNetworkError();
	}

	private void OnNetworkError()
	{
		draw = false;
		inGame = false;
		MessageBox.Show("Network error occurred");
		MainForm.Instance.NavigateTo(new LoginRegisterUserControl());
	}

	private async void WordButtonClick(object? sender, EventArgs e)
	{
		string word = ((Button)sender!).Text;
		if (!await tcpClientHandler.WriteMessage($"ChosenWord:{word}"))
		{
			OnNetworkError();
			return;
		}

		wordButton1.Visible = false;
		wordButton2.Visible = false;
		wordButton3.Visible = false;

		currentlyDrawingLabel.Text = word;
	}

	private async void ReadLoop()
	{
		while (inGame)
		{
			string? msg = await tcpClientHandler.ReadMessage();
			if (msg is null)
				break;

			DecodeMessage(msg);
		}
	}

	private async void DecodeMessage(string msg)
	{
        await Console.Out.WriteLineAsync($"Received message: {msg}");

        if (msg.Contains("SendingFrame"))
			await ReadFrameData();
		if (msg == "LobbyClosed")
			MainForm.Instance.NavigateTo(new LoginRegisterUserControl());
		else if (msg.Contains("PlayerList"))
			UpdatePlayerList(msg.Split(':')[1]);
		else if (msg.Contains("NotEnoughPlayers"))
			MessageBox.Show("Not enough players to start the game!");
		else if (msg.Contains("ErrorParsingGameOptions"))
			MessageBox.Show("Error parsing game options!");
		else if (msg.Contains("DisableGameOptions"))
			Controls.Remove(hostAreaOptions);
		else if (msg.Contains("YourTurn"))
			EnableYourTurn(msg.Split(':')[1]);
		else if (msg.Contains("RemainingSeconds"))
			remainingTimeToDrawLabel.Text = $"Remaining seconds to draw: {msg.Split(':')[1]}";
		else if (msg.Contains("RemainingRounds"))
			remainingNumOfRoundsLabel.Text = $"Remaining rounds: {msg.Split(':')[1]}";
		else if (msg.Contains("CorrectGuess"))
			EnableChat(false);
		else if (msg.Contains("PlayerScores"))
			UpdatePlayerScores(msg.Split(':')[1]);
		else if (msg.Contains("ClearCanvas"))
			drawingCanvas.Clear();
		else if (msg.Contains("YouAreHost"))
			OpenHostOptions();
		else if (msg.Contains("Winner"))
			EndGameDisplayWinner(msg.Split(":")[1]);
		else if (msg.Contains("EnableCanvas"))
			EnableCanvasAndShare();
		else if (msg.Contains("DisableCanvas"))
			DisableCanvasAndShare();
		else if (msg.Contains("EnableChat"))
			EnableChat(true);
		else if (msg.Contains("DisableChat"))
			EnableChat(false);
		else if (msg.Contains("ShowScoreboardPanel"))
			ShowScoreboardPanel();
	}

	private void ShowScoreboardPanel()
	{
		scoreboardLabel.Visible = true;
		scoreboardPanel.Visible = true;

		// Initialize all player score lables
		int yOffset = scoreboardLabel.Height + scoreboardLabel.Location.Y;
		foreach (string playerName in playerNames)
		{
			Label currentLabel = new Label();
			currentLabel.Text = $"{playerName}: 0";
			currentLabel.Width = scoreboardPanel.Width;
			currentLabel.Height = 25;
			currentLabel.Location = new Point(0, yOffset);
			scoreboardPanel.Controls.Add(currentLabel);

			yOffset += currentLabel.Height;
		}
	}

	private void EndGameDisplayWinner(string winner)
	{
		MessageBox.Show($"The winner is: {winner}");
		MainForm.Instance.NavigateTo(new LoginRegisterUserControl());
		inGame = false;
	}

	private async Task ReadFrameData()
	{
		byte[]? imageBytes = await tcpClientHandler.ReadBytes();
		if (imageBytes is null)
		{
			OnNetworkError();
			return;
		}

		DrawCanvasFrame(imageBytes);
	}

	private void OpenHostOptions()
	{
		Controls.Add(hostAreaOptions = new HostOptionsArea(this) { Location = new Point(350, 20) });
		hostAreaOptions.BringToFront();
	}

	private void UpdatePlayerScores(string playerScores)
	{
		if (!int.TryParse(playerScores.Split(',')[0], out int playerScoresCount))
			return;

		scoreboardPanel.Controls.Clear();
		int yOffset = scoreboardLabel.Height + scoreboardLabel.Location.Y;
		for (int i = 0; i < playerScoresCount; i++)
		{
			string currentScoreRepresentation = playerScores.Split('(')[i + 1].Split(')')[0];
			string username = currentScoreRepresentation.Split(',')[0];
			string score = currentScoreRepresentation.Split(',')[1];

			Label currentLabel = new Label();
			currentLabel.Text = $"{username}: {score}";
			currentLabel.Width = scoreboardPanel.Width;
			currentLabel.Height = 25;
			currentLabel.Location = new Point(0, yOffset);
			scoreboardPanel.Controls.Add(currentLabel);

			yOffset += currentLabel.Height;
		}
	}

	private void EnableChat(bool enable)
	{
		chatPanel.Visible = true;
		chatPanel.Enabled = true;

		chatSendButton.Enabled = enable;
		chatTextBox.Enabled = enable;
		chatTextBox.Text = "";
	}

	private void DrawCanvasFrame(byte[] bytes)
	{
		drawingCanvas.SetFrame(bytes);
	}

	private void DisableCanvasAndShare()
	{
		currentlyDrawingLabel.Text = "";
		drawingCanvas.SetCanvasEnabled(false);
		draw = false;
	}

	private async void EnableCanvasAndShare()
	{
		drawingCanvas.SetCanvasEnabled(true);
		draw = true;

		while (draw)
		{
			byte[]? bytes = drawingCanvas.GetFrame();
			if (bytes is not null)
			{
				if (!await tcpClientHandler.WriteMessage($"SendingFrame"))
				{
					Console.WriteLine("Error sending frame header");
					OnNetworkError();
					return;
				}

				if (!await tcpClientHandler.WriteBytes(bytes))
				{
					Console.WriteLine("Error sending frame data");
					OnNetworkError();
					return;
				}
			}

			// Send 15 frames a second
			await Task.Delay(1000 / 30);
		}
	}

	private void EnableYourTurn(string wordList)
	{
		string[] words = wordList.Split(',');

		// Disable chat funcionality
		chatTextBox.Enabled = false;
		chatSendButton.Enabled = false;

		// Display 3 words as selection
		wordButton1.Text = words[0];
		wordButton2.Text = words[1];
		wordButton3.Text = words[2];
		wordButton1.Visible = true;
		wordButton2.Visible = true;
		wordButton3.Visible = true;
	}

	private void UpdatePlayerList(string playerListRepresentation)
	{
		int playerCount = int.Parse(playerListRepresentation.Split(',')[0]);
		playerNames = new string[playerCount];

		int yOffset = 0;
		for (int i = 0; i < playerCount; i++)
		{
			string playerName = playerListRepresentation.Split(",")[i + 1];
			playerNames[i] = playerName;

			Label playerTextBox = new Label();
			playerTextBox.Text = playerName;
			playerTextBox.Width = playerList.Width;
			playerTextBox.Height = 25;
			playerTextBox.Location = new Point(0, yOffset);
			yOffset += playerTextBox.Height;

			playerList.Controls.Add(playerTextBox);
		}
	}

	public async void HostSendStartGame(int timePerRound, int numOfRounds)
	{
		if (!await tcpClientHandler.WriteMessage($"StartGame:{timePerRound},{numOfRounds}"))
			OnNetworkError();
	}
}