namespace Server.Components.ClientStages;

internal static class LobbySelectorHandler
{
	private static int currentLobbyId;
	private static readonly List<LobbySelectorClientHandler> clients = new List<LobbySelectorClientHandler>();

	static LobbySelectorHandler()
	{
		Lobby.OnLobbiesRefresh += ResendLobbies;
	}

	private static void ResendLobbies()
	{
		Console.WriteLine("Resending lobbies...");
		foreach (LobbySelectorClientHandler client in clients)
			_ = client.tcpClientHandler.WriteMessage(GetLobbiesRepresentation());
	}

	public static void AddClientToSelector(TcpClientHandler tcpClientHandler)
	{
		LobbySelectorClientHandler lobbySelectorClientHandler = new LobbySelectorClientHandler(tcpClientHandler);
		lobbySelectorClientHandler.InLobbySelection = true;

		clients.Add(lobbySelectorClientHandler);
		BeginRead(lobbySelectorClientHandler);
	}

	private static async void BeginRead(LobbySelectorClientHandler lobbySelectorClientHandler)
	{
		while (lobbySelectorClientHandler.InLobbySelection)
		{
			string? msg = await lobbySelectorClientHandler.tcpClientHandler.ReadMessage();
			if (msg == null)
				return;
			DecodeMessage(lobbySelectorClientHandler, msg);
		}
	}

	private static void DecodeMessage(LobbySelectorClientHandler sender, string msg)
	{
		if (msg.Contains("JoinLobby"))
		{
			if (!int.TryParse(msg.Split(':')[1], out int id))
			{
				_ = sender.tcpClientHandler.WriteMessage("LobbySelectionResponse:InvalidLobbyId");
				return;
			}

			if (!Lobby.AddPlayerToLobby(sender.tcpClientHandler, id))
				_ = sender.tcpClientHandler.WriteMessage("JoinLobbyResponse:Failed");
			else
			{
				clients.Remove(sender);
				sender.InLobbySelection = false;
				_ = sender.tcpClientHandler.WriteMessage($"JoinLobbyResponse:JoinedLobby");
			}
		}
		else if (msg.Contains("CreateLobby"))
		{
			string lobbyRepresentation = msg.Split(":")[1];

			int id = currentLobbyId++;
			string name = lobbyRepresentation.Split(',')[0];
			string description = lobbyRepresentation.Split(',')[1];

			if (!int.TryParse(lobbyRepresentation.Split(',')[2], out int maxNumOfPlayers))
				maxNumOfPlayers = 8;
			int numOfPlayers = 1;

			clients.Remove(sender);
			sender.InLobbySelection = false;

			_ = sender.tcpClientHandler.WriteMessage("CreateLobbyResponse:Ok");
			_ = new Lobby(sender.tcpClientHandler, id, name, description, maxNumOfPlayers, numOfPlayers);
		}
		else if (msg.Contains("RequestLobbyList"))
			_ = sender.tcpClientHandler.WriteMessage(GetLobbiesRepresentation());
	}

	private static string GetLobbiesRepresentation()
	{
		string lobbyModelsRepresentation = $"LobbyList:{Lobby.lobbies.Count}";
		foreach (Lobby lobbyModel in Lobby.lobbies.Values)
		{
			if (!lobbyModel.LobbyAlive)
				continue;

			lobbyModelsRepresentation +=
				$"(" +
				$"{lobbyModel.Id}" +
				$"/{lobbyModel.Name}" +
				$"/{lobbyModel.Description}" +
				$"/{lobbyModel.HostName}" +
				$"/{lobbyModel.MaxNumOfPlayers}" +
				$"/{lobbyModel.NumOfPlayers}" +
				$")";
		}

		return lobbyModelsRepresentation;
	}
}