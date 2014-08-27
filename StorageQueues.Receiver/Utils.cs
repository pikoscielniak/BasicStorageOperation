using System;
using DataContracts;
using Microsoft.WindowsAzure.Storage.Queue;

namespace StorageQueues.Receiver
{
	partial class Program
	{
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

		private static CloudQueue GetQueue()
		{
			var sentenceQueue = _queueClient.GetQueueReference("queue");
			sentenceQueue.CreateIfNotExists();
			return sentenceQueue;
		}

		private static void Write(LapData lapData)
		{
			WriteColor("PlayerName: "+lapData.PlayerName,ConsoleColor.Cyan);
			WriteColor("GameStartTime: "+lapData.GameStarTime,ConsoleColor.Cyan);
		}
	}
}