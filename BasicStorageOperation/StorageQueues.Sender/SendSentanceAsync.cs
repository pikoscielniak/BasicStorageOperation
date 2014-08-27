using System;
using System.Diagnostics;
using System.Globalization;
using Microsoft.WindowsAzure.Storage.Queue;

namespace StorageQueues.Sender
{
	partial class Program
	{
		private static void SendSentanceAsync(string text)
		{
			var sentenceQueue = GetSentancSentenceQueue();

			foreach (var letter in text)
			{
				var message = new CloudQueueMessage(letter.ToString(CultureInfo.InvariantCulture));
				sentenceQueue.AddMessageAsync(message);
				sentenceQueue.BeginAddMessage(message, AddMessageCallback, sentenceQueue);
				WriteColor(letter.ToString(CultureInfo.InvariantCulture), ConsoleColor.Magenta);
			}
			BreakLine();
		}

		private static void AddMessageCallback(IAsyncResult result)
		{
			WriteLineColor("AddMessageCallback", ConsoleColor.Yellow);
			try
			{
				(result.AsyncState as CloudQueue).EndAddMessage(result);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.ToString());
			}
		}
	}
}