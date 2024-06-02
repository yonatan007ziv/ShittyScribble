using Server.Components;
using Settings;

namespace Server;

internal class Program
{
	public static async Task Main()
	{
		GameServer server = new GameServer(Configuration.ServerIp, Configuration.ServerPort);
		server.Start();
		while (true)
			await server.AcceptClient();
	}
}