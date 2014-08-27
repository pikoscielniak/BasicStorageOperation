using System;
using DataContracts;

namespace StorageQueues.Receiver
{
	partial class Program
	{
		private static void ReceiveStringSerializedMessages()
		{
			var queue = GetQueue();
			Console.WriteLine("ReceiveStringSerializedMessages");
			Console.WriteLine();
			while (true)
			{
				var message = queue.GetMessage(TimeSpan.FromSeconds(1));
				if (message != null)
				{
					try
					{
						var lapData = SerializationUtils.Deserialize<LapData>(message.AsString);
						Write(lapData);
						queue.DeleteMessage(message);
					}
					catch (Exception ex)
					{
						WriteColor(ex.Message, ConsoleColor.Red);
						WriteColor(message.Id + " - "+message.DequeueCount,ConsoleColor.Magenta);
						Console.WriteLine();
						if (message.DequeueCount > 5)
						{
							WriteColor(message.Id + " is a poison messsage",ConsoleColor.Cyan);
							queue.DeleteMessage(message);
						}
					}
				}
				else
				{
					break;
				}
			}
		}
	}
}