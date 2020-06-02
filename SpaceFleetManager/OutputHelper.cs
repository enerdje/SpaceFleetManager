using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;

namespace SpaceFleetManager
{
	public static class OutputHelper
	{
		public enum OutputType
		{
			Xml,
			Console
		}

		public static void WriteAs(OutputType output, IReadOnlyDictionary<Planet, List<ASpaceship>> plan)
		{
			switch (output)
			{
				case OutputType.Console:	WriteToConsole(plan); break;
				case OutputType.Xml:		WriteToXml(plan); break;
			}
		}

		private static void WriteToConsole(IReadOnlyDictionary<Planet, List<ASpaceship>> plan)
		{
			string Separator = string.Concat(Enumerable.Repeat("=", 50));
			Console.WriteLine(Separator);
			foreach (KeyValuePair<Planet, List<ASpaceship>> keyValue in plan)
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

		private static void WriteToXml(IReadOnlyDictionary<Planet, List<ASpaceship>> plan)
		{
			XDocument xdoc = new XDocument();
			XElement planet;
			XElement space = new XElement("space");
			foreach (KeyValuePair<Planet, List<ASpaceship>> keyValue in plan)
			{
				var query = keyValue.Value.OrderBy(o => o.GetType().Name);
				planet = new XElement("planet");
				XAttribute planetNameAttr = new XAttribute("name", keyValue.Key.Name);
				planet.Add(planetNameAttr);
				XAttribute planetNeedAttr = new XAttribute("need", keyValue.Key.Need);
				planet.Add(planetNeedAttr);

				foreach (var ship in query)
				{
					XElement iphoneCompanyElem = new XElement("ship", ship);
					planet.Add(iphoneCompanyElem);
				}

				if (Equals(keyValue, plan.Last()))
				{
					space.Add(planet);
					xdoc.Add(space);
					xdoc.Save("SpacePlan.xml");
				}
				else space.Add(planet);
			}
		}
	}
}
