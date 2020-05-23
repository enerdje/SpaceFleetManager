namespace SpaceFleetManager
{
	public sealed class Planet
	{
		public string Name { get; set; }

		public int Need { get; set; }

		public double X { get; set; }

		public double Y { get; set; }

		public Planet(string Name, double X, double Y, int Need)
		{
			this.Name = Name;
			this.X = X;
			this.Y = Y;
			this.Need = Need;
		}
	}
}
