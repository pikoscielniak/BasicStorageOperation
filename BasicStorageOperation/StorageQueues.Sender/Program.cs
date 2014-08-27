using System;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace StorageQueues.Sender
{
	partial class Program
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
			//			SendSentanceWithOptions("Windows Azure Storage Queues, sending messages with options.");

//			TestMessageSize();
			SendStringSeralizedMessages();
			SendBinarySerializedMessages();

			Console.WriteLine("Done!");
			Console.ReadLine();
		}
	}
}
