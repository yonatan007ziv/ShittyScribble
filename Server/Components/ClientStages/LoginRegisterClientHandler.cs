namespace Server.Components.ClientStages;

internal class LoginRegisterClientHandler
{
	private const int MinimumPasswordLength = 5;

	private readonly TcpClientHandler tcpClientHandler;

	private bool inLoginPage = true;

	private string current2FACode = "";
	private string username = "";
	private string email = "";
	private string password = "";

	public LoginRegisterClientHandler(TcpClientHandler tcpClientHandler)
	{
		this.tcpClientHandler = tcpClientHandler;

		BeginCommunication();
	}

	private async void BeginCommunication()
	{
		if (!await tcpClientHandler.InitializeEncryption())
		{
			await Console.Out.WriteLineAsync("Encryption unsuccessful");
			return;
		}

		await Console.Out.WriteLineAsync("Client entered login page");
		while (inLoginPage)
		{
			string? msg = await tcpClientHandler.ReadMessage();
			if (msg == null)
				break;

			DecodeMessage(msg);
		}
		await Console.Out.WriteLineAsync("Client exited login page");
	}

	private void DecodeMessage(string msg)
	{
		if (msg.Contains("Login"))
			LoginRequest(msg.Split(':')[1]);
		else if (msg.Contains("Register"))
			RegisterRequest(msg.Split(':')[1]);
		else if (msg.Contains("2FA"))
			Validate2FaRequest(msg.Split(":")[1]);
	}

	private void LoginRequest(string credentials)
	{
		string username = credentials.Split(',')[0];
		string password = credentials.Split(',')[1];

		if (!DatabaseHandler.UsernameExists(username))
		{
			_ = tcpClientHandler.WriteMessage("LoginResponse:UsernameNotFound");
			return;
		}

		string passwordHash = PasswordHasherSalter.HashPassword(password);
		string passwordSalt = DatabaseHandler.GetPasswordSalt(username);

		string saltedPasswordHash = PasswordHasherSalter.SaltHash(passwordHash, passwordSalt);
		DatabaseHandler.UpdateTemporaryHash(username, saltedPasswordHash);
		saltedPasswordHash = DatabaseHandler.GetTemporaryHash(username);

		string storedSaltedPasswordHash = DatabaseHandler.GetSaltedPasswordHash(username);

		if (saltedPasswordHash != storedSaltedPasswordHash)
		{
			_ = tcpClientHandler.WriteMessage("LoginResponse:IncorrectPassword");
			return;
		}

		inLoginPage = false;
		_ = tcpClientHandler.WriteMessage("LoginResponse:LoginSuccessful");

		tcpClientHandler.Username = username;
		LobbySelectorHandler.AddClientToSelector(tcpClientHandler);
	}

	private void RegisterRequest(string credentials)
	{
		username = credentials.Split(',')[0];
		password = credentials.Split(',')[1];
		email = credentials.Split(',')[2];

		if (DatabaseHandler.UsernameExists(username))
		{
			_ = tcpClientHandler.WriteMessage("RegisterResponse:UsernameFound");
			return;
		}

		if (DatabaseHandler.EmailExists(email))
		{
			_ = tcpClientHandler.WriteMessage("RegisterResponse:EmailFound");
			return;
		}

		if (password.Length < MinimumPasswordLength)
		{
			_ = tcpClientHandler.WriteMessage("RegisterResponse:PasswordTooShort");
			return;
		}

		current2FACode = TwoFactorAuthenticationCodeGenerator.Generate2FACode();
		if (!EmailSender.SendEmail(email, "2FA Code", $"Your 2FA code is: {current2FACode}"))
		{
			_ = tcpClientHandler.WriteMessage("RegisterResponse:InvalidEmail");
			return;
		}

		_ = tcpClientHandler.WriteMessage("RegisterResponse:ValidateEmail");
	}

	private void Validate2FaRequest(string TwoFACode)
	{
		if (TwoFACode != current2FACode)
		{
			_ = tcpClientHandler.WriteMessage("2FAResponse:WrongCode");
			return;
		}
		_ = tcpClientHandler.WriteMessage("2FAResponse:RegisterSuccessful");

		string salt = PasswordHasherSalter.RandomSalt();
		string saltedPasswordHash = PasswordHasherSalter.SaltHash(PasswordHasherSalter.HashPassword(password), salt);
		DatabaseHandler.InsertUser(username, email, saltedPasswordHash, salt);
	}
}