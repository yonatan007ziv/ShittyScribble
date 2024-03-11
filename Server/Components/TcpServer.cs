using Server.Components.ClientStages;
using System.Net;
using System.Net.Sockets;

namespace Server.Components;

internal class TcpServer
{
    private readonly TcpListener listener;

	private Dictionary<IPAddress, int> ddosTable = new Dictionary<IPAddress, int>();

    public TcpServer(string ip, int port)
    {
        listener = new TcpListener(IPAddress.Parse(ip), port);
	}

    public async Task Run()
    {
        listener.Start();

        while (true)
        {
            TcpClient client = await listener.AcceptTcpClientAsync();
            IPAddress address = ((IPEndPoint)client.Client.RemoteEndPoint!).Address;

            if (!ddosTable.ContainsKey(address))
                ddosTable[address] = 0;
			ddosTable[address]++;

            if (ddosTable[address] > 5)
            {
                if (ddosTable[address] == 6)
                    TimeoutCounter(address);

				TcpClientHandler timedoutClient = new TcpClientHandler(client);
                await timedoutClient.InitializeEncryption();
                _ = timedoutClient.WriteMessage("Timedout");
				continue;
            }

            new LoginRegisterClientHandler(new TcpClientHandler(client));
        }
    }

	private async void TimeoutCounter(IPAddress address)
	{
        await Task.Delay(TimeSpan.FromMinutes(1));
        ddosTable[address] = 0;
	}
}