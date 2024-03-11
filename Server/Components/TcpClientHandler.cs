using System.Net.Sockets;
using System.Text;

namespace Server.Components;

internal class TcpClientHandler
{
	protected const string EncryptionTestWord = "Success";

	public string Username = "";

	private NetworkStream networkStream { get => client.GetStream(); }
	protected TcpClient client;
	protected EncryptionHandler encryption;
	protected CancellationTokenSource disconnectedCts;
	protected TaskCompletionSource encryptionTask;

	public TcpClientHandler(TcpClient client)
	{
		this.client = client;

		encryption = new EncryptionHandler();
		disconnectedCts = new CancellationTokenSource();
		encryptionTask = new TaskCompletionSource();
	}

	private async Task<bool> EstablishEncryption()
	{
		try
		{
			// Import Rsa Details
			byte[] rsaPublicKey = await UnsafeReadBytes();
			encryption.ImportRsa(rsaPublicKey);

			// Send Aes Private Key
			byte[] aesPrivateKey = encryption.ExportAesPrivateKey();
			byte[] encryptedRsaPrivateKey = encryption.EncryptRsa(aesPrivateKey);
			_ = UnsafeWriteBytes(encryptedRsaPrivateKey);

			// Send Aes Iv
			byte[] aesIv = encryption.ExportAesIv();
			byte[] encryptedRsaIv = encryption.EncryptRsa(aesIv);
			_ = UnsafeWriteBytes(encryptedRsaIv);

			// Test Encryption: Send
			string msgTest = EncryptionTestWord;
			byte[] decryptedTest = Encoding.UTF8.GetBytes(msgTest);
			byte[] encryptedTest = encryption.EncryptAes(decryptedTest);
			_ = UnsafeWriteBytes(encryptedTest);

			// Test Encryption: Receive
			encryptedTest = await UnsafeReadBytes();
			decryptedTest = encryption.DecryptAes(encryptedTest);
			msgTest = Encoding.UTF8.GetString(decryptedTest);

			if (msgTest != EncryptionTestWord)
				throw new Exception();
		}
		catch { return false; }
		return true;
	}

	public async Task<bool> InitializeEncryption()
	{
		if (!await EstablishEncryption())
			return false;

		encryptionTask.SetResult();
		return true;
	}

	public async Task<bool> WriteMessage(string message)
	{
		await encryptionTask.Task;

		try
		{
			byte[] decryptedWriteBuffer = Encoding.UTF8.GetBytes(message);
			byte[] writeBuffer = encryption.EncryptAes(decryptedWriteBuffer);
			await UnsafeWriteBytes(writeBuffer);
			return true;
		}
		catch { disconnectedCts.Cancel(); return false; }
	}

	public async Task<string?> ReadMessage()
	{
		await encryptionTask.Task;

		try
		{
			byte[] encryptedReadBuffer = await UnsafeReadBytes();
			byte[] readBuffer = encryption.DecryptAes(encryptedReadBuffer);

            return Encoding.UTF8.GetString(readBuffer);
		}
		catch { disconnectedCts.Cancel(); return null; }
	}

	public async Task<bool> WriteBytes(byte[] writeBuffer)
	{
		await encryptionTask.Task;

		// Prefixes 4 Bytes Indicating Message Length
		byte[] length = BitConverter.GetBytes(writeBuffer.Length);
		byte[] prefixedBuffer = new byte[writeBuffer.Length + sizeof(int)];

		Array.Copy(length, 0, prefixedBuffer, 0, sizeof(int));
		Array.Copy(writeBuffer, 0, prefixedBuffer, sizeof(int), writeBuffer.Length);

		try
		{
			await networkStream.WriteAsync(prefixedBuffer, disconnectedCts.Token);
			return true;
		}
		catch { return false; }
	}

	public async Task<byte[]?> ReadBytes()
	{
		await encryptionTask.Task;

		byte[] readBufer;
		int bytesRead;
		try
		{
			// Reads 4 Bytes Indicating Message Length
			byte[] lengthBuffer = new byte[4];
			await networkStream.ReadAsync(lengthBuffer, disconnectedCts.Token);

			int length = BitConverter.ToInt32(lengthBuffer);
			readBufer = new byte[length];
			bytesRead = await networkStream.ReadAsync(readBufer, disconnectedCts.Token);
		}
		catch { return null; }

		if (bytesRead == 0)
			return null;

		return readBufer;
	}

	public async Task UnsafeWriteBytes(byte[] writeBuffer)
	{
		// Prefixes 4 Bytes Indicating Message Length
		byte[] length = BitConverter.GetBytes(writeBuffer.Length);
		byte[] prefixedBuffer = new byte[writeBuffer.Length + sizeof(int)];

		Array.Copy(length, 0, prefixedBuffer, 0, sizeof(int));
		Array.Copy(writeBuffer, 0, prefixedBuffer, sizeof(int), writeBuffer.Length);

		try
		{
			await networkStream.WriteAsync(prefixedBuffer, disconnectedCts.Token);
		}
		catch { throw; }
	}

	public async Task<byte[]> UnsafeReadBytes()
	{
		byte[] readBufer;
		int bytesRead;
		try
		{
			// Reads 4 Bytes Indicating Message Length
			byte[] lengthBuffer = new byte[4];
			await networkStream.ReadAsync(lengthBuffer, disconnectedCts.Token);

			int length = BitConverter.ToInt32(lengthBuffer);
			readBufer = new byte[length];
			bytesRead = await networkStream.ReadAsync(readBufer, disconnectedCts.Token);
		}
		catch { throw; }

		if (bytesRead == 0)
			throw new Exception();

		return readBufer;
	}

	public void Dispose()
	{
		disconnectedCts.Cancel();
		client.Close();

		client = new TcpClient();
		encryption = new EncryptionHandler();
		disconnectedCts = new CancellationTokenSource();
		encryptionTask = new TaskCompletionSource();
	}
}