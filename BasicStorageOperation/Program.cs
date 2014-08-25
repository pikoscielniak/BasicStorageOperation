using System.IO;

namespace BasicStorageOperation
{
	class Program
	{
		static void Main(string[] args)
		{
			var environment = File.ReadAllLines("../../environment.txt");
			string name = environment[0];
			string key = environment[1];

		}
	}
}
