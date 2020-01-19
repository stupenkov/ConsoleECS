using System;
using System.Reflection;

namespace Test
{
	class Program
	{
		static void Main(string[] args)
		{
			var t = Assembly.GetCallingAssembly().GetTypes();
			foreach (var item in t)
			{
				Console.WriteLine(item.Name);
			}
		}
	}
}
