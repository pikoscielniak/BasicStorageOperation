using System;
using System.Globalization;
using Microsoft.WindowsAzure.Storage.Queue;

namespace StorageQueues.Sender
{
	partial class Program
	{
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
				WriteColor(letter.ToString(), ConsoleColor.Magenta);
				visibilityDelay++;
			}
			BreakLine();

		}
	}
}