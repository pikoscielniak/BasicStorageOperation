using System.Collections;
using System.IO;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;

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

		public IEnumerable<Product> GetAll()
		{
			//			var query = (from q in _table.CreateQuery<Product>()
			//				select q);
			//			return query.ToList();

			//			var query = (from q in _table.CreateQuery<Product>()
			//						 select q).Take(10);
			//			return query.ToList();

			var query = new TableQuery<Product>();
			return _table.ExecuteQuery(query);
		}

		public IEnumerable<Product> GetByCategory(string category)
		{
			//			var query = from q in _table.CreateQuery<Product>()
			//						where q.PartitionKey.Equals(category)
			//						select q;

			var query = _table.CreateQuery<Product>();
			query.Where(c => c.PartitionKey == category);
			return query.ToList();
		}


		public IEnumerable<Product> GetByColor(string color)
		{
//			return (from q in _table.CreateQuery<Product>()
//					where q.Color.Equals(color)
//					select q).ToList();

			return _table.CreateQuery<Product>().Where(p => p.Color == color).ToList();
		}
	}
}