using System;
using DataContracts;
using Microsoft.WindowsAzure.Storage.Queue;

namespace StorageQueues.Sender
{
	partial class Program
	{
		private static void SendStringSeralizedMessages()
		{
			var queue = GetQueue();
			queue.EncodeMessage = true;
			Console.WriteLine("SendStringSeralizedMessages");
			for (int i = 1; i <= 3; i++)
			{
				var lapData = GetTestLapData("Player-" + i);
				var stringMessage = new CloudQueueMessage(SerializationUtils.SerializeToString(lapData));
				queue.AddMessage(stringMessage);
				Console.WriteLine(".");
			}
		}
	}
}