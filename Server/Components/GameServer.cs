using Server.Components.ClientStages;
using System.Net;
using System.Net.Sockets;

namespace Server.Components;

internal class GameServer
{
	private readonly TcpListener listener;

	private Dictionary<IPAddress, int> dosTable = new Dictionary<IPAddress, int>();
	private int ddosCounter = 0;

	public GameServer(string ip, int port)
	{
		listener = new TcpListener(IPAddress.Parse(ip), port);
	}

	public void Start()
	{
		Console.WriteLine("Started the game server");
		listener.Start();
	}

	public async Task AcceptClient()
	{
		TcpClient client = await listener.AcceptTcpClientAsync();

		// 500 Requests in a 5 minute timer frame
		UpdateDDoSCounter();
		if (ddosCounter > 500)
			return;

		// 20 Requests in a 5 minute timer frame
		IPAddress address = ((IPEndPoint)client.Client.RemoteEndPoint!).Address;
		UpdateDoSCounter(address);
		if (dosTable[address] > 20)
			return;

		_ = new LoginRegisterClientHandler(new TcpClientHandler(client));
	}


	private async void UpdateDDoSCounter()
	{
		ddosCounter++;
		await Task.Delay(TimeSpan.FromMinutes(5));
		ddosCounter--;
	}

	private async void UpdateDoSCounter(IPAddress address)
	{
		if (!dosTable.ContainsKey(address))
			dosTable[address] = 0;

		dosTable[address]++;
		await Task.Delay(TimeSpan.FromMinutes(5));
		dosTable[address]--;
	}
}