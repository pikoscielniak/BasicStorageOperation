using System;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace StorageQueues.Receiver
{
	class Program
	{
		private static CloudQueueClient _queueClient;
		static void Main(string[] args)
		{
			var connectionString = File.ReadAllText("../../environment.txt");

			var account = CloudStorageAccount.Parse(connectionString);
			_queueClient = account.CreateCloudQueueClient();

			Console.WriteLine("Hit enter to receive...");
			Console.ReadLine();

			ReceiveCharacters();

			Console.WriteLine("Done!");
			Console.ReadLine();
		}

		private static void ReceiveCharacters()
		{
			var sentenceQueue = _queueClient.GetQueueReference("sentancequeue");
			sentenceQueue.CreateIfNotExists();

			while (true)
			{
				var message = sentenceQueue.GetMessage();
				if (message != null)
				{
					WriteColor(message.AsString,ConsoleColor.Magenta);
					sentenceQueue.DeleteMessage(message);
				}
				else
				{
					break;
				}
			}
			Console.WriteLine();
		}

		private static void WriteColor(string message, ConsoleColor color)
		{
			Console.ForegroundColor = color;
			Console.Write(message);
			Console.ResetColor();
		}

		private static void WriteLineColor(string message, ConsoleColor color)
		{
			Console.ForegroundColor = color;
			Console.WriteLine(message);
			Console.ResetColor();
		}
	}
}
