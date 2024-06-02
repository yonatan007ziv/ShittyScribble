using Microsoft.Data.SqlClient;

namespace Server.Components;

internal static class DatabaseHandler
{
	private readonly static SqlConnection conn = new SqlConnection(Settings.Configuration.ConnectionString);

	public static bool UsernameExists(string username)
	{
		string sql = @"SELECT COUNT(*) FROM [Users] WHERE Username = @Username";

		SqlCommand cmd = conn.CreateCommand();
		cmd.CommandText = sql;

		cmd.Parameters.AddWithValue("@Username", username);

		conn.Open();
		int count = (int)cmd.ExecuteScalar();
		conn.Close();
		return count > 0;
	}

	public static bool EmailExists(string email)
	{
		string sql = @"SELECT COUNT(*) FROM [Users] WHERE Email = @Email";

		SqlCommand cmd = conn.CreateCommand();
		cmd.CommandText = sql;

		cmd.Parameters.AddWithValue("@Email", email);

		conn.Open();
		int count = (int)cmd.ExecuteScalar();
		conn.Close();
		return count > 0;
	}

	public static string GetSaltedPasswordHash(string username)
	{
		string sql = @"SELECT SaltedPasswordHash FROM [Users] WHERE Username = @Username";

		SqlCommand cmd = conn.CreateCommand();
		cmd.CommandText = sql;

		cmd.Parameters.AddWithValue("@Username", username);

		conn.Open();
		string result = (string)cmd.ExecuteScalar();
		conn.Close();
		return result;
	}

	public static string GetPasswordSalt(string username)
	{
		string sql = @"SELECT PasswordSalt FROM [Users] WHERE Username = @Username";

		SqlCommand cmd = conn.CreateCommand();
		cmd.CommandText = sql;

		cmd.Parameters.AddWithValue("@Username", username);

		conn.Open();
		string result = (string)cmd.ExecuteScalar();
		conn.Close();
		return result;
	}

	public static void InsertUser(string username, string email, string saltedPasswordHash, string passwordSalt)
	{

		string sql = @"INSERT INTO [Users] (Username, Email, SaltedPasswordHash, PasswordSalt, TemporarySaltedPasswordHash) VALUES (@Username, @Email, @SaltedPasswordHash, @PasswordSalt, '')";

		SqlCommand cmd = conn.CreateCommand();
		cmd.CommandText = sql;

		cmd.Parameters.AddWithValue("@Username", username);
		cmd.Parameters.AddWithValue("@Email", email);
		cmd.Parameters.AddWithValue("@SaltedPasswordHash", saltedPasswordHash);
		cmd.Parameters.AddWithValue("@PasswordSalt", passwordSalt);

		conn.Open();
		cmd.ExecuteNonQuery();
		conn.Close();
	}

	public static void UpdateTemporaryHash(string username, string saltedPasswordHash)
	{

		string sql = @"UPDATE [Users] SET TemporarySaltedPasswordHash = @SaltedPasswordHash WHERE Username = @Username";

		SqlCommand cmd = conn.CreateCommand();
		cmd.CommandText = sql;

		cmd.Parameters.AddWithValue("@Username", username);
		cmd.Parameters.AddWithValue("@SaltedPasswordHash", saltedPasswordHash);

		conn.Open();
		cmd.ExecuteNonQuery();
		conn.Close();
	}

	public static string GetTemporaryHash(string username)
	{
		string sql = @"SELECT TemporarySaltedPasswordHash FROM [Users] WHERE Username = @Username";

		SqlCommand cmd = conn.CreateCommand();
		cmd.CommandText = sql;

		cmd.Parameters.AddWithValue("@Username", username);

		conn.Open();
		string result = (string)cmd.ExecuteScalar();
		conn.Close();
		return result;
	}
}