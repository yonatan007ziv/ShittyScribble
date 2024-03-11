using Client.Components;

namespace Client.UserControls;

public partial class LobbySelector : UserControl
{
	private bool isInLobbySelector = false;
	private readonly TcpClientHandler tcpClientHandler;

	internal LobbySelector(TcpClientHandler tcpClientHandler)
	{
		InitializeComponent();
		this.tcpClientHandler = tcpClientHandler;

		isInLobbySelector = true;
		RequestAndReadLobbies();
	}

	private async void RequestAndReadLobbies()
	{
		await tcpClientHandler.WriteMessage("RequestLobbyList");
		string? msg = await tcpClientHandler.ReadMessage();

		if (msg == null)
		{
			MessageBox.Show("Something went wrong while reading lobbies data");
			return;
		}

		UpdateLobbyPanel(msg.Split(':')[1]);
		ReadLoop();
	}

	private void UpdateLobbyPanel(string lobbiesRepresentation)
	{
		int lobbiesCount = int.Parse(lobbiesRepresentation.Split('(')[0]);
		lobbiesList.Controls.Clear();

		if (lobbiesCount == 0)
			lobbiesList.Controls.Add(new Button() { Text = "No lobbies!" });
		else
		{
			int offsetY = 0;
			for (int i = 0; i < lobbiesCount; i++)
			{
				string currentLobby = lobbiesRepresentation.Split('(')[i + 1].Split(')')[0];
				string[] currentLobbyParts = currentLobby.Split('/');

				int id = int.Parse(currentLobbyParts[0]);
				string lobbyName = currentLobbyParts[1];
				string lobbyDescription = currentLobbyParts[2];
				string lobbyHostName = currentLobbyParts[3];
				int maxPlayerCount = int.Parse(currentLobbyParts[4]);
				int playerCount = int.Parse(currentLobbyParts[5]);

				Button btn = new Button();
				btn.Text = $"{lobbyName}, {lobbyDescription} ({playerCount}/{maxPlayerCount})\nHost: {lobbyHostName}";
				btn.Click += (s, e) => OnLobbyButtonClick(id);
				btn.Width = lobbiesList.Width;
				btn.Height *= 2;

				btn.BackColor = Color.Orange;
				btn.ForeColor = Color.White;
				btn.Font = new Font("Arial", 12, FontStyle.Bold);

				btn.Location = new Point(btn.Location.X, btn.Location.Y + offsetY);
				offsetY += btn.Height;

				lobbiesList.Controls.Add(btn);
			}
		}
	}

	private async void ReadLoop()
	{
		while (isInLobbySelector)
		{
			string? msg = await tcpClientHandler.ReadMessage();
			if (msg is null)
			{
				if (isInLobbySelector)
					MainForm.Instance.NavigateTo(new LoginRegisterUserControl());
				break;
			}

			DecodeMessage(msg);
		}
	}

	private void DecodeMessage(string msg)
	{
		if (msg.Contains("LobbyList"))
			UpdateLobbyPanel(msg.Split(':')[1]);
		else if (msg.Contains("JoinLobbyResponse"))
		{
			string lobbyResponse = msg.Split(':')[1];
			if (lobbyResponse == "Failed")
				MessageBox.Show("Failed to join lobby!");
			else if (lobbyResponse.Split(':')[0] == "JoinedLobby")
			{
				isInLobbySelector = false;
				MainForm.Instance.NavigateTo(new GameView(tcpClientHandler));
			}
		}
		else if (msg.Contains("CreateLobbyResponse"))
		{
			if (msg.Split(':')[1] == "Ok")
			{
				isInLobbySelector = false;
				MainForm.Instance.NavigateTo(new GameView(tcpClientHandler));
			}
		}
	}

	private void OnLobbyButtonClick(int id)
	{
		_ = tcpClientHandler.WriteMessage($"JoinLobby:{id}");
	}

	private void CreateLobbyButton(object sender, EventArgs e)
	{
		if (nameField.Text == "" || descriptionField.Text == "" || !int.TryParse(maxPlayersField.Text, out int maxPlayersCount) || maxPlayersCount < 2)
		{
			MessageBox.Show("Invalid lobby");
			return;
		}

		_ = tcpClientHandler.WriteMessage($"CreateLobby:{nameField.Text},{descriptionField.Text},{maxPlayersField.Text}");
	}
}
