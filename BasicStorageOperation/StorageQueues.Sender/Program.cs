using System;
using System.Globalization;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace StorageQueues.Sender
{
	class Program
	{
		private static CloudQueueClient _queueClient;

		static void Main()
		{
			Console.WriteLine("Hit enter to send...");
			Console.ReadLine();

			var connectionString = File.ReadAllText("../../environment.txt");

			var account = CloudStorageAccount.Parse(connectionString);
			_queueClient = account.CreateCloudQueueClient();

			SendSentanceSync("Windows Azure Storage Queues, sending messages.");

			Console.WriteLine("Done!");
		}

		private static void SendSentanceSync(string text)
		{
			var sentenceQueue = _queueClient.GetQueueReference("sentancequeue");
			sentenceQueue.CreateIfNotExists();
			WriteLineColor("Sending: ", ConsoleColor.Cyan);

			foreach (var letter in text.ToCharArray())
			{
				var message = new CloudQueueMessage(letter.ToString(CultureInfo.InvariantCulture));
				sentenceQueue.AddMessage(message);
				WriteColor(letter.ToString(CultureInfo.InvariantCulture), ConsoleColor.Magenta);
			}
			Console.WriteLine();
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
