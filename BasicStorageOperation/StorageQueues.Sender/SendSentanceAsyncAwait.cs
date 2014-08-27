using System;
using System.Globalization;
using Microsoft.WindowsAzure.Storage.Queue;

namespace StorageQueues.Sender
{
	partial class Program
	{
		private static async void SendSentanceAsyncAwait(string text)
		{
			var sentenceQueue = GetQueue();

			foreach (var letter in text)
			{
				var message = new CloudQueueMessage(letter.ToString(CultureInfo.InvariantCulture));
				await sentenceQueue.AddMessageAsync(message);
				WriteColor(letter.ToString(CultureInfo.InvariantCulture), ConsoleColor.Magenta);
			}
			BreakLine();
		}
	}
}