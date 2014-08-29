using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureTableStorage.AzureDataAccess
{
	public class Product : TableEntity	
	{
		public string Name { get; set; }
		public string Color { get; set; }
		public double ListPrice { get; set; }
		public string Model { get; set; }	
	}

	public class ProductAccess
	{
		private readonly CloudTable _table;

		public ProductAccess()
		{
			var connectionString = File.ReadAllText("../../environment.txt");

			var account = CloudStorageAccount.Parse(connectionString);
			var client = account.CreateCloudTableClient();
			_table = client.GetTableReference("TableLabProducts");
			_table.CreateIfNotExists();
		}

		public void Insert(Product product)
		{
			var operation = TableOperation.Insert(product);
			_table.Execute(operation);
		}
	}
}