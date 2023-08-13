// Client Side
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using ClientSide.Classes;

namespace ClientSide
{
    public class Program
	{
		public static void Main(string[] args)
		{
			IPAddress ip = IPAddress.Loopback;
			int port = 0;
			Console.WriteLine("Please setup your client");
			try
			{
				Console.Write("Enter Server IP: ");
				ip = IPAddress.Parse(Console.ReadLine());
			}
			catch (FormatException e)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Value entered is not in an IP format. Please restart the setup and try again.");
			}

			try
			{
				Console.Write("\nEnter Server Port: ");
				port = int.Parse(Console.ReadLine());
			}
			catch (FormatException e)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Value entered is not a number. Please restart the setup and try again.");
			}

			try
			{
				Client client = new Client(ip, port);

				client.Connect();
			} catch (Exception e)
			{
				Console.ForegroundColor= ConsoleColor.Red;
				Console.WriteLine("Error while connecting to server: " + e.Message);
			}
		}
	}
}