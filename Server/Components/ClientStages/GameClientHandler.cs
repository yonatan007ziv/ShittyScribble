namespace Server.Components.ClientStages;

internal class GameClientHandler
{
	public bool InGame { get; set; }

	public readonly TcpClientHandler tcpClientHandler;
	public int score;

	public GameClientHandler(TcpClientHandler tcpClientHandler)
	{
		this.tcpClientHandler = tcpClientHandler;
	}
}
