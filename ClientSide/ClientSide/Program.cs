﻿using System;
using System.Net.Sockets;
using System.Text;
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
				Console.WriteLine("Connected");

				Console.Write("Type anything and press enter to send a message! (type '/exit' to quit.): ");

				NetworkStream stream = tcpClient.GetStream();

				// Start a separate thread to continuously read messages from the server
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
								Console.ForegroundColor = ConsoleColor.Cyan;
								Console.WriteLine(receivedMessage);
								Console.ForegroundColor = ConsoleColor.White;
							}
						}
					}
					catch (Exception ex)
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("Error while receiving: " + ex.Message);
					}
				});
				receiveThread.Start();

				while (true)
				{
					Console.ForegroundColor = ConsoleColor.White;
					String str = Console.ReadLine();

					if (str.ToLower() == "/exit")
					{
						tcpClient.Close();
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine("Disconnected from the server");
						break;
					}

					byte[] data = Encoding.ASCII.GetBytes(str);
					stream.Write(data, 0, data.Length);
				}
			}
			catch (Exception ex)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Error... " + ex.StackTrace);
			}
		}
	}
}
