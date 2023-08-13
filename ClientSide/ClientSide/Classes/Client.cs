using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace ClientSide.Classes
{
	public class Client
	{
		IPAddress ip;
		int port = 5000;
		public Client(IPAddress ip, int port)
		{ 
			this.ip = ip;
			this.port = port;
		}

		public void Connect()
		{
			try
			{
				TcpClient client = new TcpClient();
				//Connecting to server
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Connecting...");

				// Connect to server
				client.Connect(this.ip, this.port);
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("Connected to the chat server");

				string username;
				do
				{
					Console.Write("Enter Your Username: ");
					username = Console.ReadLine();
				} while (string.IsNullOrEmpty(username));

				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine($"Welcome, {username}!");

				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("Type your message and press Enter to send (type '/exit' to quit):");

				NetworkStream stream = client.GetStream();

				Thread receiveThread = new Thread(() =>
				{
					try
					{
						byte[] receiveBuffer = new byte[4096];
						while (true)
						{
							// Receive messages from stream
							int bytesRead = stream.Read(receiveBuffer, 0, receiveBuffer.Length);

							if (bytesRead > 0)
							{
								string receivedMessage = Encoding.ASCII.GetString(receiveBuffer, 0, bytesRead);
								Console.ForegroundColor = ConsoleColor.DarkCyan;
								Console.WriteLine($"[{DateTime.Now.ToString("HH:mm:ss")}] {receivedMessage}");
								Console.ForegroundColor = ConsoleColor.White;
							}
						}
					}
					catch (Exception ex)
					{
						Console.ForegroundColor = ConsoleColor.DarkRed;
						Console.WriteLine("Error while receiving: " + ex.Message);
					}
				});
				receiveThread.Start();

				while (true)
				{
					Console.ForegroundColor = ConsoleColor.Green;
					String messageData = Console.ReadLine();

					// Check if they type /exit
					if (messageData.ToLower() == "/exit")
					{
						// Close the client to disconnect from the server
						client.Close();
						Console.ForegroundColor = ConsoleColor.DarkRed;
						Console.WriteLine("Disconnected from the server");
						break;
					}

					// Initialize the clients message
					ChatMessage message = new ChatMessage
					{
						username = username,
						message = messageData,
					};

					// Send the data to the server
					string jsonString = JsonSerializer.Serialize(message);
					byte[] messageBytes = Encoding.ASCII.GetBytes(jsonString);
					stream.Write(messageBytes, 0, messageBytes.Length);
				}
			} catch (Exception e)
			{
				Console.ForegroundColor = ConsoleColor.DarkRed;
				Console.WriteLine("Error while communicating with server to send message.");
			}
			
		}

}
}
