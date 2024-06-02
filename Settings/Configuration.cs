namespace Settings;

public class Configuration
{
	public static string ServerIp { get; } = "127.0.0.1";
	public static int ServerPort { get; } = 5000;
	public static string ConnectionString { get; } = @"";
	public static string EmailHost { get; } = "";
	public static string EmailPassword { get; } = "";
}