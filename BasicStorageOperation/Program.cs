using System;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;

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

			BasicBlobOperations(account);

			Console.WriteLine("Press key");
			Console.ReadKey();
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
