using System;
using DataContracts;
using Microsoft.WindowsAzure.Storage.Queue;

namespace StorageQueues.Sender
{
	partial class Program
	{
		private static void SendBinarySerializedMessages()
		{
			var queue = GetQueue();
			for (int i = 1; i <= 3; i++)
			{
				var lapData = GetTestLapData("Player-" + i);
				var stringMessage = new CloudQueueMessage(SerializationUtils.SerializeToByteArray<LapData>(lapData));
				queue.AddMessage(stringMessage);
				Console.Write(".");
			}
		}
	}
}