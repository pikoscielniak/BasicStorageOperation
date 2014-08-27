using System;
using System.Text;
using Microsoft.WindowsAzure.Storage.Queue;

namespace StorageQueues.Sender
{
	partial class Program
	{
		private static void TestMessageSize()
		{
			var sentanceQueue = GetQueue();
			var builder = new StringBuilder();
			//			sentanceQueue.EncodeMessage = false;
			while (true)
			{
				builder.Append(GetBigString(1000));
				WriteLineColor("Length: " + builder.Length, ConsoleColor.Cyan);
				try
				{
					var message = new CloudQueueMessage(builder.ToString());
					sentanceQueue.AddMessage(message);
					WriteLineColor("Sent...", ConsoleColor.Magenta);
				}
				catch (Exception ex)
				{
					WriteLineColor(ex.Message, ConsoleColor.Red);
					break;
				}
			}
		}
	}
}