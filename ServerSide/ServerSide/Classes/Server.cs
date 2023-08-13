using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServerSide.Classes
{


    public class Server
	{
		// List of connected clients
		private static List<Socket> connectedClients = new List<Socket>();

		// Parameters
		public IPAddress ip;
		public int port = 5000;
		public string serverName;
		
		// Setting IP and Server Name
		public Server(string serverName, IPAddress ip)
		{
			this.ip = ip;
		}
		
		// Setting Port
		public Server(int port)
		{
			this.port = port;
		}

		// Starting server with current parameters
		public void StartServer()
		{
			try
			{
				TcpListener listener = new TcpListener(this.ip, this.port);

				listener.Start();

				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine($"Server Running on port: {this.port}");
				Console.WriteLine("Local endpoint: " + listener.LocalEndpoint);

				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Waiting for connections...");

				while (true)
				{
					Socket socket = listener.AcceptSocket();
					connectedClients.Add(socket);

					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("\nNew connection accepted from: " + socket.RemoteEndPoint);

					Thread clientThread = new Thread(() =>
					{
						HandleClient(socket);
					});
					clientThread.Start();

				}
			} catch (Exception e)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Error: " + e.Message);
			}
			
		}

		private static void HandleClient(Socket clientSocket)
		{
			try
			{
				while (true)
				{
					byte[] dataBuffer = new byte[8192];
					// Receive data from the current client
					int bytesRead = clientSocket.Receive(dataBuffer);

					string jsonString = Encoding.ASCII.GetString(dataBuffer, 0, bytesRead);
					ChatMessage receivedMessage = JsonSerializer.Deserialize<ChatMessage>(jsonString);

					string username = receivedMessage.username;
					string message = receivedMessage.message;

					Console.ForegroundColor = ConsoleColor.DarkCyan;
					Console.Write($"\n[{DateTime.Now.ToString("HH:mm:ss")}] ");
					Console.ForegroundColor = ConsoleColor.Cyan;
					Console.Write($"From {username}: ");
					Console.ForegroundColor = ConsoleColor.White;
					Console.Write($"{message}\n");

					// Checking if there is more than one client connected
					if (connectedClients.Count > 0)
					{
						List<Socket> tempClients = new List<Socket>(connectedClients);
						// Remove current client so it doesn't send to itself.
						tempClients.Remove(clientSocket);

						// Sending to each client except the client sending.
						foreach (Socket client in tempClients)
						{
							ASCIIEncoding encoding = new ASCIIEncoding();
							client.Send(encoding.GetBytes($"From {username}: {message}"));
						}
					}
					ASCIIEncoding asen = new ASCIIEncoding();
					clientSocket.Send(asen.GetBytes("\nMessage Sent!\n"));

				}
			} catch (Exception e)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("\nClient Disconnected Due to Error or Manual Disconnection.\n");
				// Remove client that disconnected to not cause errors while sending data.
				connectedClients.Remove(clientSocket);
			}
		}
		


	}
}
