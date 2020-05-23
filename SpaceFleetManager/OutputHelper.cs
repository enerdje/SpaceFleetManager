using System;
using System.Linq;
using System.Collections.Generic;

namespace SpaceFleetManager
{
	static class OutputHelper
	{
		enum OutputType
		{
			Xml,
			Console
		}

		public static void WriteAs(string output)
		{
			switch (Enum.Parse(typeof(OutputType), output))
			{
				case OutputType.Console:
					WriteOnConsole();
					break;
				case OutputType.Xml:
					XmlHelper.WriteToXml();
					break;
			}
		}

		static void WriteOnConsole()
		{
			string Separator = string.Concat(Enumerable.Repeat("=", 50));
			Console.WriteLine(Separator);
			foreach (KeyValuePair<Planet, List<ASpaceship>> keyValue in Program.plan)
			{
				var query = keyValue.Value.OrderBy(o => o.GetType().Name); //Группировка
				Console.WriteLine($"Планета: {keyValue.Key.Name,-10}\tПотребность: {keyValue.Key.Need}");
				Console.WriteLine("Корабль\t\tКласс\t\t\tНа борту");
				foreach (var ship in query)
				{
					Console.WriteLine($"{ship.Name,-15} {ship.GetType().Name,-23} {ship.Capacity}");
				}
				Console.WriteLine(Separator);
			}
			Console.Read();
		}
	}
}
