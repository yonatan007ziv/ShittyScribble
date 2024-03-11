using Client.Components;
using System.Net.Sockets;

namespace Client.UserControls;

public partial class LoginRegisterUserControl : UserControl
{
	private const string ServerIp = "127.0.0.1";
	private const int ServerPort = 5000;

	private TcpClientHandler tcpClientHandler;

	public LoginRegisterUserControl()
	{
		InitializeComponent();
	}


	private async void LoginButton(object sender, EventArgs e)
	{
		loginButton.Enabled = false;

		tcpClientHandler = new TcpClientHandler(new TcpClient());
		if (!await tcpClientHandler.Connect(ServerIp, ServerPort))
		{
			MessageBox.Show("Couldn't connect to server");
			loginButton.Enabled = true;
			return;
		}

		_ = tcpClientHandler.WriteMessage($"Login:{loginUsernameField.Text},{loginPasswordField.Text}");

		string? response = await tcpClientHandler.ReadMessage();
		if (response == null)
		{
			MessageBox.Show("Unknown error occured");
			loginButton.Enabled = true;
			return;
		}

		if (response.Contains("Timedout"))
			MessageBox.Show("Please wait before making any more requests");

		if (response.Contains("LoginResponse"))
		{
			string result = response.Split(':')[1];
			if (result == "UsernameNotFound")
				MessageBox.Show("Username not found");
			else if (result == "IncorrectPassword")
				MessageBox.Show("Incorrect password");
			else if (result == "LoginSuccessful")
			{
				MessageBox.Show("Login successful");
				MainForm.Instance.NavigateTo(new LobbySelector(tcpClientHandler));
			}
		}

		loginButton.Enabled = true;
	}

	private async void RegisterButton(object sender, EventArgs e)
	{
		registerButton.Enabled = false;

		tcpClientHandler = new TcpClientHandler(new TcpClient());
		if (!await tcpClientHandler.Connect(ServerIp, ServerPort))
		{
			MessageBox.Show("Couldn't connect to server");
			loginButton.Enabled = true;
			return;
		}

		_ = tcpClientHandler.WriteMessage($"Register:{registerUsernameField.Text},{registerPasswordField.Text},{registerEmailField.Text}");

		string? response = await tcpClientHandler.ReadMessage();
		if (response == null)
		{
			MessageBox.Show("Unknown error occured");
			loginButton.Enabled = true;
			return;
		}

		if (response.Contains("Timedout"))
			MessageBox.Show("Please wait before making any more requests");

		if (response.Contains("RegisterResponse"))
		{
			string result = response.Split(':')[1];
			if (result == "UsernameFound")
				MessageBox.Show("Username already exists");
			else if (result == "PasswordTooShort")
				MessageBox.Show("Password is too short");
			else if (result == "InvalidEmail")
				MessageBox.Show("Email is not valid");
			else if (result == "ValidateEmail")
				MessageBox.Show("Please check your email inbox");
		}

		registerButton.Enabled = true;
	}

	private async void Register2FAButton(object sender, EventArgs e)
	{
		register2FAButton.Enabled = false;
		_ = tcpClientHandler.WriteMessage($"2FA:{register2FAField.Text}");

		string? response = await tcpClientHandler.ReadMessage();
		if (response == null)
		{
			MessageBox.Show("Unknown error occured");
			loginButton.Enabled = true;
			return;
		}

		if (response.Contains("Timedout"))
			MessageBox.Show("Please wait before making any more requests");

		if (response.Contains("2FAResponse"))
		{
			string result = response.Split(':')[1];
			if (result == "WrongCode")
				MessageBox.Show("Wrong 2FA Code");
			else if (result == "RegisterSuccessful")
				MessageBox.Show("Successfully registered");
		}

		register2FAButton.Enabled = true;
	}
}
