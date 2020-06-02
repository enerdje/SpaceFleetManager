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
		private static string pathToReadXml;

		private static XDocument xdoc;

		/// <summary> Необходимо во избежание парсинга при чтении XML </summary>
		private static void CultureInfo(string name) => Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(name);

		/// <summary> Считывает и записывает путь и саму XML  </summary>
		public static void Initialize(string xmlName)
		{
			CultureInfo("en-US");
			pathToReadXml = Path.Combine(Directory.GetCurrentDirectory(), xmlName);
			if (!File.Exists(pathToReadXml)) throw new Exception($"Не найден файл: {xmlName}");
			xdoc = XDocument.Load(xmlName);
		}

		/// <summary> Считываем корабли и возращаем список </summary>
		public static List<ASpaceship> ReadShips()
		{
			List<ASpaceship> spaceShips = new List<ASpaceship>();
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

		/// <summary> Считываем планеты и возращаем список </summary>
		public static List<Planet> ReadPlanets()
		{
			List<Planet> planets = new List<Planet>();
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
	}
}