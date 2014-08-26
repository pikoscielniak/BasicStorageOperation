using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
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

//			SendSentanceSync("Windows Azure Storage Queues, sending messages.");
//			SendSentanceAsync("Windows Azure Storage Queues, sending messages asynchronously");
//			SendSentanceAsyncAwait("Windows Azure Storage Queues, sending messages async await");
//			SendSentanceParallel("Windows Azure Storage Queues, sending messages in parallel.");
			SendSentanceWithOptions("Windows Azure Storage Queues, sending messages with options.");

			Console.WriteLine("Done!");
			Console.ReadLine();
		}

		private static void SendSentanceWithOptions(string text)
		{
			var sentanceQueue = GetSentancSentenceQueue();

			int visibilityDelay = 1;

			WriteLineColor("Sending: ", ConsoleColor.Cyan);
			foreach (var letter in text.ToCharArray())
			{
				var message = new CloudQueueMessage(letter.ToString(CultureInfo.InvariantCulture));
				sentanceQueue.AddMessage(
					message,
					TimeSpan.FromMinutes(2),
					TimeSpan.FromSeconds(visibilityDelay));
				WriteColor(letter.ToString(),ConsoleColor.Magenta);
				visibilityDelay++;
			}
			BreakLine();
		
		}

		private static void SendSentanceParallel(string text)
		{
			var sentanceQueue = GetSentancSentenceQueue();

			Parallel.ForEach<char>(text, letter =>
			{
				var message = new CloudQueueMessage(letter.ToString(CultureInfo.InvariantCulture));
				sentanceQueue.AddMessage(message);
				WriteColor(letter.ToString(CultureInfo.InvariantCulture),ConsoleColor.Magenta);
			});
			BreakLine();
		}

		private static async void SendSentanceAsyncAwait(string text)
		{
			var sentenceQueue = GetSentancSentenceQueue();

			foreach (var letter in text)
			{
				var message = new CloudQueueMessage(letter.ToString(CultureInfo.InvariantCulture));
				await sentenceQueue.AddMessageAsync(message);
				WriteColor(letter.ToString(CultureInfo.InvariantCulture),ConsoleColor.Magenta);
			}
			BreakLine();
		}

		private static void BreakLine()
		{
			Console.WriteLine();
			Console.WriteLine();
		}

		private static void SendSentanceAsync(string text)
		{
			var sentenceQueue = GetSentancSentenceQueue();

			foreach (var letter in text)
			{
				var message = new CloudQueueMessage(letter.ToString(CultureInfo.InvariantCulture));
				sentenceQueue.AddMessageAsync(message);
				sentenceQueue.BeginAddMessage(message, AddMessageCallback, sentenceQueue);
				WriteColor(letter.ToString(CultureInfo.InvariantCulture),ConsoleColor.Magenta);
			}
			BreakLine();
		}

		private static void AddMessageCallback(IAsyncResult result)
		{
			WriteLineColor("AddMessageCallback",ConsoleColor.Yellow);
			try
			{
				(result.AsyncState as CloudQueue).EndAddMessage(result);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.ToString());
			}
		}

		private static void SendSentanceSync(string text)
		{
			var sentenceQueue = GetSentancSentenceQueue();

			foreach (var letter in text.ToCharArray())
			{
				var message = new CloudQueueMessage(letter.ToString(CultureInfo.InvariantCulture));
				sentenceQueue.AddMessage(message);
				WriteColor(letter.ToString(CultureInfo.InvariantCulture), ConsoleColor.Magenta);
			}
			Console.WriteLine();
			Console.WriteLine();
		}

		private static CloudQueue GetSentancSentenceQueue()
		{
			var sentenceQueue = _queueClient.GetQueueReference("sentancequeue");
			sentenceQueue.CreateIfNotExists();
			WriteLineColor("Sending: ", ConsoleColor.Cyan);
			return sentenceQueue;
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
