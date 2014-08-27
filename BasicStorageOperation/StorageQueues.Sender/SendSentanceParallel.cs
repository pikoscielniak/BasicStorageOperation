using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Queue;

namespace StorageQueues.Sender
{
	partial class Program
	{
		private static void SendSentanceParallel(string text)
		{
			var sentanceQueue = GetQueue();

			Parallel.ForEach<char>(text, letter =>
			{
				var message = new CloudQueueMessage(letter.ToString(CultureInfo.InvariantCulture));
				sentanceQueue.AddMessage(message);
				WriteColor(letter.ToString(CultureInfo.InvariantCulture), ConsoleColor.Magenta);
			});
			BreakLine();
		}
	}
}