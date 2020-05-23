using System;
using System.Linq;
using System.Collections.Generic;

namespace SpaceFleetManager
{
	static class Dispatcher
	{
		/// <summary> Получить координату X планеты земля </summary>
		public static double GetEarth_X => Program.planets.Where(o => o.Name == "Earth").First().X;

		/// <summary> Получить координату Y планеты земля </summary>
		public static double GetEarth_Y => Program.planets.Where(o => o.Name == "Earth").First().Y;

		/// <summary> Возращает дистанцию до планеты Земля </summary>
		public static double DistanceToEarth(double x1, double y1) => Math.Sqrt(Math.Pow(x1 - GetEarth_X, 2) + Math.Pow(y1 - GetEarth_Y, 2));

		/// <summary> Распределить задачи и создать маршруты </summary>
		public static Dictionary<Planet, List<ASpaceship>> CreateRoutes(Dictionary<Planet, List<ASpaceship>> XmlPlanetsAndShips)
		{
			List<ASpaceship> shipsWay = new List<ASpaceship>();
			Dictionary<Planet, List<ASpaceship>> planetsAndWaysShips = new Dictionary<Planet, List<ASpaceship>>();

			foreach (KeyValuePair<Planet, List<ASpaceship>> keyValue in XmlPlanetsAndShips)
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
	}
}
