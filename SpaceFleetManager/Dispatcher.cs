using System;
using System.Linq;
using System.Collections.Generic;

namespace SpaceFleetManager
{
	static class Dispatcher
	{
		/// <summary> Список планет </summary>
		public static List<Planet> planets = new List<Planet>();

		/// <summary> Список кораблей </summary>
		public static IReadOnlyList<ASpaceship> spaceShips = new List<ASpaceship>();

		private static double GetEarth_X => planets.Where(o => o.Name == "Earth").First().X;

		private static double GetEarth_Y => planets.Where(o => o.Name == "Earth").First().Y;

		/// <summary> Возращает дистанцию до планеты Земля </summary>
		public static double DistanceToEarth(double x1, double y1) => Math.Sqrt(Math.Pow(x1 - GetEarth_X, 2) + Math.Pow(y1 - GetEarth_Y, 2));

		public static IReadOnlyDictionary<Planet, List<ASpaceship>> GetDictionary => DistanceForEarthDictoinary();

		public static void Initialize(List<Planet> Planets, List<ASpaceship> SpaceShips)
		{
			planets = Planets; spaceShips = SpaceShips;
		}

		/// <summary> Распределить задачи и создать маршруты </summary>
		public static IReadOnlyDictionary<Planet, List<ASpaceship>> CreateRoutes()
		{
			List<ASpaceship> shipsWay = new List<ASpaceship>();
			Dictionary<Planet, List<ASpaceship>> planetsAndWaysShips = new Dictionary<Planet, List<ASpaceship>>();

			foreach (KeyValuePair<Planet, List<ASpaceship>> keyValue in GetDictionary)
			{
				int planetNeed = keyValue.Key.Need;

				if (keyValue.Key.Name != "Earth")
				{
					while (planetNeed > 0)
					{
						foreach (var ship in keyValue.Value)
						{
							if (planetNeed > 0)
							{
								shipsWay.Add(ASpaceship.ShipCreator(ship.Name, ((ship.Capacity > planetNeed) ? planetNeed : ship.Capacity), ship.GetType().Name));
								planetNeed -= ship.Capacity;
							}
						}
					}
					planetsAndWaysShips.Add(keyValue.Key, shipsWay.ToList());
					shipsWay.Clear();
				}
			}
			return planetsAndWaysShips;
		}

		private static IReadOnlyDictionary<Planet, List<ASpaceship>> DistanceForEarthDictoinary()
		{
			Dictionary<Planet, List<ASpaceship>> planetsAndShips = new Dictionary<Planet, List<ASpaceship>>();

			foreach (var planet in planets)
			{
				if (planet.Name != null)
				{
					// Формируем лист кораблей летящих до n-ой планеты
					var listShips = spaceShips.Where(o => o.Get > DistanceToEarth(planet.X, planet.Y));
					planetsAndShips.Add(planet, listShips.ToList());
				}
			}
			return planetsAndShips;
		}
	}
}
