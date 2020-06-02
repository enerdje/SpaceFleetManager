using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceFleetManager
{
	static class Program
	{
		private static void Initialize(string xmlName) 
		{
			XmlHelper.Initialize(xmlName);
			Dispatcher.Initialize(XmlHelper.ReadPlanets(), XmlHelper.ReadShips());
		}

		static void Main(string[] args)
		{
			var outputType = (OutputHelper.OutputType)Enum.Parse(typeof(OutputHelper.OutputType), args.Last());

			Initialize(args.First());
			OutputHelper.WriteAs(outputType, Dispatcher.CreateRoutes());
		}
	}
}
