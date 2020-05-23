using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Threading;
using System.Collections.Generic;

namespace SpaceFleetManager
{
	static class XmlHelper
	{
		/// <summary> Путь до XML </summary>
		static string pathToReadXml;

		static XDocument xdoc;

		/// <summary> Необходимо во избежание парсинга при чтении XML </summary>
		static void CultureInfo(string name) => Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(name);

		public static void Initialize(string xmlName)
		{
			CultureInfo("en-US");
			pathToReadXml = Path.Combine(Directory.GetCurrentDirectory(), xmlName);
			if (!File.Exists(pathToReadXml)) throw new Exception($"Не найден файл: {xmlName}");
			xdoc = XDocument.Load(xmlName);
		}

		public static void ReadXml(string xmlName)
		{
			Initialize(xmlName);
			ReadShips(Program.spaceShips);
			ReadPlanets(Program.planets);
			ReadPlanetsAndShips(Program.XmlPlanetsAndShips);
		}

		public static List<ASpaceship> ReadShips(List<ASpaceship> spaceShips)
		{
			foreach (XElement ship in xdoc.Element("space").Element("ships").Elements("ship"))
			{
				XAttribute shipName = ship.Attribute("name");
				XElement rangeElement = ship.Element("range");
				XElement capacityElement = ship.Element("capacity");

				if (shipName != null && rangeElement != null && capacityElement != null)
					spaceShips.Add(ASpaceship.ShipCreator(shipName.Value, Int32.Parse(capacityElement.Value), rangeElement.Value));
			}
			return spaceShips;
		}

		public static List<Planet> ReadPlanets(List<Planet> planets)
		{
			foreach (XElement planet in xdoc.Element("space").Element("planets").Elements("planet"))
			{
				XAttribute planetName = planet.Attribute("name");
				XElement xElement = planet.Element("x");
				XElement yElement = planet.Element("y");
				XElement needElement = planet.Element("need");

				if (planetName != null && xElement != null && yElement != null)
					planets.Add(new Planet(planetName.Value, double.Parse(xElement.Value), double.Parse(yElement.Value), Int32.Parse(needElement.Value)));
			}
			return planets;
		}

		public static Dictionary<Planet, List<ASpaceship>> ReadPlanetsAndShips(Dictionary<Planet, List<ASpaceship>> planetsAndShips)
		{
			foreach (XElement planet in xdoc.Element("space").Element("planets").Elements("planet"))
			{
				XAttribute planetName = planet.Attribute("name");
				XElement xElement = planet.Element("x");
				XElement yElement = planet.Element("y");
				XElement needElement = planet.Element("need");

				if (planetName != null && xElement != null && yElement != null)
				{
					var ff = Program.spaceShips.Where(o => o.Get > Dispatcher.DistanceToEarth(double.Parse(xElement.Value), double.Parse(yElement.Value)));
					planetsAndShips.Add(new Planet(planetName.Value, double.Parse(xElement.Value), double.Parse(yElement.Value), Int32.Parse(needElement.Value)), ff.ToList());
				}
			}
			return planetsAndShips;
		}

		public static void WriteToXml()
		{
			XDocument xdoc = new XDocument();
			XElement planet;
			XElement space = new XElement("space");
			foreach (KeyValuePair<Planet, List<ASpaceship>> keyValue in Program.plan)
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

				if (Equals(keyValue, Program.plan.Last()))
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