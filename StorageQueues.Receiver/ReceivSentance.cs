using System;
using System.Linq;

namespace StorageQueues.Receiver
{
	partial class Program
	{
		private static void ReceivSentance()
		{
			var sentenceQueue = GetQueue();
			while (true)
			{
				var messages = sentenceQueue.GetMessages(32).ToList();
				WriteLineColor("Received " + messages.Count + " messages.", ConsoleColor.Cyan);

				if (messages.Count > 0)
				{
					foreach (var message in messages)
					{
						WriteColor(message.AsString, ConsoleColor.Magenta);
						sentenceQueue.DeleteMessage(message);
					}
				}
				else
				{
					break;
				}
			}
			Console.WriteLine();
		}
	}
}