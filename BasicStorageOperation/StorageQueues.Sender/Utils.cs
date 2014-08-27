using System;
using System.Text;
using DataContracts;
using Microsoft.WindowsAzure.Storage.Queue;

namespace StorageQueues.Sender
{
	partial class Program
	{
		private static void BreakLine()
		{
			Console.WriteLine();
			Console.WriteLine();
		}

		private static CloudQueue GetQueue()
		{
			var sentenceQueue = _queueClient.GetQueueReference("queue");
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

		private static string GetBigString(int number)
		{
			var chars = "$%#@!*abcdefghijklmnopqrstuvwxyz1234567890?;:ABCDEFGHIJKLMNOPQRSTUVWXYZ^&".ToCharArray();
			var random = new Random();
			var builder = new StringBuilder();
			for (var i = 0; i < number; i++)
			{
				var ind = random.Next(chars.Length);
				builder.Append(chars[ind]);
			}
			return builder.ToString();
		}

		private static LapData GetTestLapData(string name)
		{
			var lapData = new LapData
			{
				PlayerName = name,
				LapId = Guid.NewGuid().ToString()
			};
			for (int i = 0; i <= 6; i++)
			{
				lapData.SectorTimesMs.Add(i * 8923);
			}
			lapData.LapTimeMs = lapData.SectorTimesMs[lapData.SectorTimesMs.Count - 1];
			return lapData;
		}
	}
}