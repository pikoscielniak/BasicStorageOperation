using System;
using DataContracts;

namespace StorageQueues.Receiver
{
	partial class Program
	{
		private static void ReceivedBinarySerializedMessages()
		{
			var queue = GetQueue();
			queue.EncodeMessage = true;
			Console.WriteLine("ReceivedBinarySerializedMessages");
			Console.WriteLine();
			while (true)
			{
				var message = queue.GetMessage(TimeSpan.FromSeconds(1));
				if (message != null)
				{
					try
					{
						var lapData = SerializationUtils.Deserialize<LapData>(message.AsBytes);
						Write(lapData);
						queue.DeleteMessage(message);
					}
					catch (Exception ex)
					{
						WriteColor(ex.Message, ConsoleColor.Red);
						Console.WriteLine();
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