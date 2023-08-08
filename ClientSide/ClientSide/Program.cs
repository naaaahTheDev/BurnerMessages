// Client Side
using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace ClientSide
{
	public class Program
	{
		public static void Main(string[] args)
		{
			try
			{
				TcpClient tcpClient = new TcpClient();
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Connecting...");

				tcpClient.Connect("INPUT_IP_HERE", 5000);

				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("Connected to the chat server");

				string username;

				do
				{
					Console.Write("Enter Your Username: ");
					username = Console.ReadLine();
				} while (string.IsNullOrWhiteSpace(username));

				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine($"Welcome, {username}!");

				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("Type your message and press Enter to send (type '/exit' to quit):");

				NetworkStream stream = tcpClient.GetStream();

				Thread receiveThread = new Thread(() =>
				{
					try
					{
						byte[] receiveBuffer = new byte[4096];
						while (true)
						{
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

					if (messageData.ToLower() == "/exit")
					{
						tcpClient.Close();
						Console.ForegroundColor = ConsoleColor.DarkRed;
						Console.WriteLine("Disconnected from the server");
						break;
					}

					ChatMessage message = new ChatMessage
					{
						username = username,
						message = messageData,
					};

					string jsonString = JsonSerializer.Serialize(message);
					byte[] messageBytes = Encoding.ASCII.GetBytes(jsonString);
					stream.Write(messageBytes, 0, messageBytes.Length);
				}
			}
			catch (Exception ex)
			{
				Console.ForegroundColor = ConsoleColor.DarkRed;
				Console.WriteLine("Error while communicating with server to send message.");
			}
		}
	}
}