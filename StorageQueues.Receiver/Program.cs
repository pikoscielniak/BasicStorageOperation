using System;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace StorageQueues.Receiver
{
	partial class Program
	{
		private static CloudQueueClient _queueClient;
		static void Main(string[] args)
		{
			var connectionString = File.ReadAllText("../../environment.txt");

			var account = CloudStorageAccount.Parse(connectionString);
			_queueClient = account.CreateCloudQueueClient();

			Console.WriteLine("Hit enter to receive...");
			Console.ReadLine();

			//			ReceiveCharacters();
			//			ReceivSentance();
			ReceiveSerializedMessages();
			Console.WriteLine("Done!");
			Console.ReadLine();
		}

		private static void ReceiveSerializedMessages()
		{
			while (true)
			{
				Thread.Sleep(2000);
				ReceiveStringSerializedMessages();
				Thread.Sleep(2000);
				ReceivedBinarySerializedMessages();
			}
		}
	}
}
