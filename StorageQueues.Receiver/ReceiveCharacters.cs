using System;

namespace StorageQueues.Receiver
{
	partial class Program
	{
		private static void ReceiveCharacters()
		{
			var sentenceQueue = GetQueue();

			while (true)
			{
				var message = sentenceQueue.GetMessage();
				if (message != null)
				{
					WriteColor(message.AsString, ConsoleColor.Magenta);
					sentenceQueue.DeleteMessage(message);
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