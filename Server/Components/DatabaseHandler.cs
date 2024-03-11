using Microsoft.Data.SqlClient;

namespace Server.Components;

internal static class DatabaseHandler
{
	private const string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""D:\Code\VS Community\Scribble\Server\Components\Sql\Database.mdf"";Integrated Security=True";
	private readonly static SqlConnection conn = new SqlConnection(connString);

	public static bool UsernameExists(string username)
	{
		SqlCommand cmd = conn.CreateCommand();
		cmd.CommandText = $"SELECT Count(Username) FROM [Users] WHERE Username='{username}'";

		conn.Open();
		int result = (int)cmd.ExecuteScalar();
		conn.Close();
		return result > 0;
	}

	public static bool EmailExists(string email)
	{
		SqlCommand cmd = conn.CreateCommand();
		cmd.CommandText = $"SELECT Count(Email) FROM [Users] WHERE Email='{email}'";

		conn.Open();
		int result = (int)cmd.ExecuteScalar();
		conn.Close();
		return result > 0;
	}

	public static string GetSaltedPasswordHash(string username)
	{
		SqlCommand cmd = conn.CreateCommand();
		cmd.CommandText = $"SELECT SaltedPasswordHash FROM [Users] WHERE Username='{username}'";
		conn.Open();
		string result = (string)cmd.ExecuteScalar();
		conn.Close();
		return result;
	}

	public static string GetPasswordSalt(string username)
	{
		SqlCommand cmd = conn.CreateCommand();
		cmd.CommandText = $"SELECT PasswordSalt FROM [Users] WHERE Username='{username}'";
		conn.Open();
		string result = (string)cmd.ExecuteScalar();
		conn.Close();
		return result;
	}

	public static void InsertUser(string username, string email, string saltedPasswordHash, string passwordSalt)
	{
		SqlCommand cmd = conn.CreateCommand();
		cmd.CommandText = $"INSERT INTO [Users] (Username, Email, SaltedPasswordHash, PasswordSalt, TemporarySaltedPasswordHash) VALUES ('{username}', '{email}', '{saltedPasswordHash}', '{passwordSalt}', '')";
		conn.Open();
		cmd.ExecuteScalar();
		conn.Close();
	}

	public static void UpdateTemporaryHash(string username, string saltedPasswordHash)
	{
		SqlCommand cmd = conn.CreateCommand();
		cmd.CommandText = $"UPDATE [Users] SET TemporarySaltedPasswordHash = '{saltedPasswordHash}' WHERE Username = '{username}'";
		conn.Open();
		cmd.ExecuteScalar();
		conn.Close();
	}

	public static string GetTemporaryHash(string username)
	{
		SqlCommand cmd = conn.CreateCommand();
		cmd.CommandText = $"SELECT TemporarySaltedPasswordHash FROM [Users] WHERE Username='{username}'";
		conn.Open();
		string result = (string)cmd.ExecuteScalar();
		conn.Close();
		return result;
	}
}