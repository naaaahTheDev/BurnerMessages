// ServerSide
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text.Json;
using ServerSide.Classes;

namespace ServerSide
{
	public class Program
	{
		public static List<Socket> connectedClients = new List<Socket>();
		public static void Main(string[] args)
		{
			Console.WriteLine("Please setup your server!\n");
			Console.WriteLine("Enter Server Name: ");

			IPAddress ip = IPAddress.Loopback; // Default IP address
			string serverName = Console.ReadLine();
			int port = 0;

			if (serverName.Length <= 2)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Server name should be at least 3 characters long. Please restart the setup and try again.");
				return;
			}

			try
			{
				Console.Write("Enter IP Address that you would like to run the server on: ");
				ip = IPAddress.Parse(Console.ReadLine());
			} catch (FormatException e)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Value entered is not in an IP format. Please restart the setup and try again.");
			}

			try
			{
				Console.Write("\nEnter Server Port (enter 0 for default): ");
				port = int.Parse(Console.ReadLine());

				if (port == 0)
				{
					port = 0;
				}
			} catch (FormatException e)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Value entered is not a number. Please restart the setup and try again.");
			}

			// Starting server
			Server server = new Server(serverName, ip);

			// Setting port if different port is provided.
			if (port > 0)
			{
				server.port = port;
			}

			server.StartServer();
		}
	}
}