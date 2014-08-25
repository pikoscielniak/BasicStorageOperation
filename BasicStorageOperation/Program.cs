using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;

namespace BasicStorageOperation
{
	class Program
	{
		static void Main(string[] args)
		{
			var environment = File.ReadAllLines("../../environment.txt");
			var name = environment[0];
			var key = environment[1];

			var account = CreateAccount(name, key);

			//			BasicBlobOperations(account);
			//BasicTableOperations(account);
			BasicQueueOperations(account.QueueEndpoint, name, key);

			Console.WriteLine("Press key to exit.");
			Console.ReadKey();
		}

		private static void BasicQueueOperations(Uri queueEndpoint, string name, string key)
		{
			var creds = new StorageCredentials(name, key);
			var queueClient = new CloudQueueClient(queueEndpoint, creds);

			var testQueue = queueClient.GetQueueReference("testqueue");
			testQueue.CreateIfNotExists();

			Console.WriteLine("Sending: ");
			foreach (var letter in "Windows Azure Storge Queue".ToCharArray())
			{
				var message = new CloudQueueMessage(letter.ToString(CultureInfo.InvariantCulture));
				testQueue.AddMessage(message);
				Console.Write(letter);
			}
			Console.WriteLine();
			Console.WriteLine();
			
			Console.WriteLine("Receiving: ");
			while (true)
			{
				var message = testQueue.GetMessage();
				if (message != null)
				{
					Console.Write(message.AsString);
					testQueue.DeleteMessage(message);
				}
				else
				{
					break;
				}
			}
			Console.WriteLine();
		}

		private static void BasicTableOperations(CloudStorageAccount account)
		{
			var tableClient = account.CreateCloudTableClient();
			var metricsTransactionsBlobTable = tableClient.GetTableReference("$MetricsHourPrimaryTransactionsBlob");

			var query = from q in metricsTransactionsBlobTable.CreateQuery<DynamicTableEntity>()
						select q;

			var totals = CalculateTotal(query);

			DisplayTotal(totals);
		}

		private static void DisplayTotal(IEnumerable<KeyValuePair<string, long>> totals)
		{
			foreach (var totalKvp in totals)
			{
				Console.WriteLine(totalKvp.Key.PadRight(30) + totalKvp.Value);
			}
			Console.WriteLine();
		}

		private static IEnumerable<KeyValuePair<string, long>> CalculateTotal(IEnumerable<DynamicTableEntity> query)
		{
			var totals = new Dictionary<string, long>();

			foreach (var entity in query.ToList())
			{
				foreach (var propertyKvp in entity.Properties)
				{
					if (propertyKvp.Value.PropertyType == EdmType.Int64)
					{
						if (totals.ContainsKey(propertyKvp.Key))
						{
							totals[propertyKvp.Key] += (long)propertyKvp.Value.Int64Value;
						}
						else
						{
							totals.Add(propertyKvp.Key, (long)propertyKvp.Value.Int64Value);
						}
					}
				}
			}
			return totals;
		}

		private static void BasicBlobOperations(CloudStorageAccount account)
		{
			var blobClient = account.CreateCloudBlobClient();

			Console.WriteLine("Containers:");
			foreach (var contianer in blobClient.ListContainers())
			{
				Console.WriteLine("\t" + contianer.Name);
			}
			Console.WriteLine();

			var testContainer = blobClient.GetContainerReference("testcontainer");
			testContainer.CreateIfNotExists();

			var blob = testContainer.GetBlockBlobReference("image.png");
			blob.UploadFromFile("../../image.png", FileMode.Open);

			Console.WriteLine("Blobs");
			foreach (var blobItem in testContainer.ListBlobs())
			{
				Console.WriteLine("\t" + blobItem.Uri.AbsolutePath);
			}
			Console.WriteLine();
		}

		private static CloudStorageAccount CreateAccount(string name, string key)
		{
			var creds = new StorageCredentials(name, key);
			var account = new CloudStorageAccount(creds, true);

			PrintInfoAboutAccount(account);
			return account;
		}

		private static void PrintInfoAboutAccount(CloudStorageAccount account)
		{
			Console.WriteLine("Account created");
			Console.WriteLine("\tBlobEndpoint: " + account.BlobEndpoint.AbsoluteUri);
			Console.WriteLine("\tTableEndpoint: " + account.TableEndpoint.AbsoluteUri);
			Console.WriteLine("\tQueueEndpoint: " + account.QueueEndpoint.AbsoluteUri);
		}
	}
}
