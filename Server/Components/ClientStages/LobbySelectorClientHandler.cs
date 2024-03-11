namespace Server.Components.ClientStages;

internal class LobbySelectorClientHandler
{
	public bool InLobbySelection { get; set; }

	public readonly TcpClientHandler tcpClientHandler;

	public LobbySelectorClientHandler(TcpClientHandler tcpClientHandler)
	{
		this.tcpClientHandler = tcpClientHandler;
	}
}