using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text.Json;

namespace ServerSide
{
	public class Program
	{
		public static List<Socket> connectedClients = new List<Socket>();
		public static void Main(string[] args)
		{
			try
			{
				IPAddress ipAd = IPAddress.Parse("INPUT_IP_HERE");
				TcpListener listener = new TcpListener(ipAd, 5000);


				listener.Start();

				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Server Running on port: 5000");
				Console.WriteLine("The local endpoint is: " + listener.LocalEndpoint);

				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Waiting for a connection...");

				while (true)
				{
					Socket socket = listener.AcceptSocket();
					connectedClients.Add(socket);
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("\nConnection accepted from: " + socket.RemoteEndPoint);

					// Start a new thread to handle communication with this client
					Thread clientThread = new Thread(() =>
					{
						HandleClient(socket);
					});
					clientThread.Start();
				}
			}
			catch (Exception ex)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Error... " + ex.StackTrace);
			}
		}

		private static void HandleClient(Socket clientSocket)
		{
			try
			{
				while (true)
				{
					byte[] dataBuffer = new byte[8192];
					int bytesRead = clientSocket.Receive(dataBuffer);

					string jsonString = Encoding.ASCII.GetString(dataBuffer, 0, bytesRead);
					ChatMessage receivedMessage = JsonSerializer.Deserialize<ChatMessage>(jsonString);
					string username = receivedMessage.username;
					string message = receivedMessage.message;
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.Write("\nMessage Transferred to Server: ");
					Console.ForegroundColor = ConsoleColor.Green;
					Console.Write($"{{ {username}: ");
					Console.ForegroundColor = ConsoleColor.White;
					Console.Write($"{message} }}\n");




					if (connectedClients.Count > 0)
					{
						List<Socket> tempClients = new List<Socket>(connectedClients);
						tempClients.Remove(clientSocket);

						foreach (Socket client in tempClients)
						{
							ASCIIEncoding encoding = new ASCIIEncoding();
							Console.ForegroundColor = ConsoleColor.White;
							client.Send(encoding.GetBytes($"From {{ {username} }}: {message}"));
						}
					}
					ASCIIEncoding asen = new ASCIIEncoding();
					clientSocket.Send(asen.GetBytes("\nMessage Sent!\n"));
				}
			}
			catch (Exception ex)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("\nClient Disconnected Due to Error or Manual Disconnection.\n" + ex.Message);
			}
		}
	}
}