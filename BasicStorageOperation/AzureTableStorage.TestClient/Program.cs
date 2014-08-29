﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using AzureTableStorage.AzureDataAccess;

namespace AzureTableStorage.TestClient
{
	class Program
	{
		static void Main(string[] args)
		{
			var watch = Stopwatch.StartNew();
			Console.WriteLine("Press enter to start");
			Console.ReadLine();

			//			InsertProducts();
			//			GetAllProducts();	
			//			GetAllProductsByCategory(Categories[2]);
//			GetAllProductsByColor(Colors[2]);

			watch.Stop();
			Console.WriteLine("Time: " + watch.Elapsed.ToString("mm\\:ss\\.fff"));
			Console.ReadLine();
		}

		private static void GetAllProductsByColor(string color)
		{
			Console.WriteLine("Getting all products by color...");
			Console.WriteLine();

			var productAccess = new ProductAccess();
			var products = productAccess.GetByColor(color);

			DisplayProducts(products);
			Console.WriteLine();
		}

		private static void GetAllProductsByCategory(string category)
		{
			Console.WriteLine("Getting all products by category...");
			Console.WriteLine();

			var productAccess = new ProductAccess();
			var products = productAccess.GetByCategory(category);

			DisplayProducts(products);
			Console.WriteLine();
		}

		private static void GetAllProducts()
		{
			Console.WriteLine("Getting all products data...");
			Console.WriteLine();

			var productAccess = new ProductAccess();
			var products = productAccess.GetAll();

			DisplayProducts(products);
			Console.WriteLine();
		}

		private static void DisplayProducts(IEnumerable<Product> products)
		{
			foreach (var product in products)
			{
				Console.WriteLine("{0} - {1} - {2}", product.PartitionKey, product.Name, product.ListPrice);
			}
		}

		private static void InsertProducts()
		{
			var productAccess = new ProductAccess();
			for (var i = 1001; i <= 1100; i++)
			{
				var product = GetRandomProduct(i);
				productAccess.Insert(product);
				Console.WriteLine("Product: " + i);
			}
		}

		private static Product GetRandomProduct(int number)
		{
			return new Product
			{
				PartitionKey = RandomCategory(),
				RowKey = number.ToString(CultureInfo.InvariantCulture),
				Name = RandomString(10),
				Color = RandomColor(),
				Model = RandomString(4),
				ListPrice = Random.NextDouble() * 1000,
			};
		}

		private static string RandomCategory()
		{
			return Categories[Random.Next(Categories.Count)];
		}

		private static string RandomColor()
		{
			return Colors[Random.Next(Colors.Count)];
		}

		private static readonly Random Random = new Random((int)DateTime.Now.Ticks);
		private static readonly IList<string> Categories = new List<string> { "Food", "Clothes", "Toys", "Forniture" };
		private static readonly IList<string> Colors = new List<string> { "Red", "Green", "Blue", "Black" };

		private static string RandomString(int size)
		{
			var builder = new StringBuilder();
			for (int i = 0; i < size; i++)
			{
				var ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * Random.NextDouble() + 65)));
				builder.Append(ch);
			}

			return builder.ToString();
		}
	}
}
