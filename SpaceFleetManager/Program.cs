using System.Collections.Generic;
using System.Linq;

namespace SpaceFleetManager
{
	static class Program
	{
		/// <summary> Список планет </summary>
		public static List<Planet> planets = new List<Planet>();

		/// <summary> Список кораблей </summary>
		public static List<ASpaceship> spaceShips = new List<ASpaceship>();

		/// <summary> Словарь планет и долетающих до неё кораблей </summary>
		public static Dictionary<Planet, List<ASpaceship>> XmlPlanetsAndShips = new Dictionary<Planet, List<ASpaceship>>();

		/// <summary> Сформированный словарь планет и летящих до неё кораблей </summary>
		public static Dictionary<Planet, List<ASpaceship>> plan = new Dictionary<Planet, List<ASpaceship>>();

		static void Main(string[] args)
		{
			XmlHelper.ReadXml(args.First());
			plan = Dispatcher.CreateRoutes(XmlPlanetsAndShips);
			OutputHelper.WriteAs(args.Last());
		}
	}
}
