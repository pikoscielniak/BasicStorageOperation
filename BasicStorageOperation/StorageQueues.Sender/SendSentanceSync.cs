using System;
using System.Globalization;
using Microsoft.WindowsAzure.Storage.Queue;

namespace StorageQueues.Sender
{
	partial class Program
	{
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

	}
}