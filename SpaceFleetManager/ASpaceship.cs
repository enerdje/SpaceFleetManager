using System;

namespace SpaceFleetManager
{
	public abstract class ASpaceship
	{
		public string Name { get; protected set; }
		public int Capacity { get; protected set; }

		public ASpaceship(string name, int capacity)
		{
			Name = name;
			Capacity = capacity;
		}

		public static ASpaceship ShipCreator(string name, int capacity, string type)
		{
			switch (type)
			{
				case "LongRange": return new LongRange(name, capacity);
				case "MiddleRange": return new MiddleRange(name, capacity);
				case "NearRange": return new NearRange(name, capacity);
				default: throw new Exception($"Тип {type} не поддерживается.");
			}
		}

		public double Get => GetRange();

		public override string ToString() => $"{Name,-10}\t{Capacity,-10}\t{GetType().Name,-15}\t{GetRange():f5}";

		protected abstract double GetRange();
	}
	class LongRange : ASpaceship
	{
		public LongRange(string name, int capacity) : base(name, capacity) { }
		protected override double GetRange() => 0.001d;
	}

	class MiddleRange : ASpaceship
	{
		public MiddleRange(string name, int capacity) : base(name, capacity) { }
		protected override double GetRange() => 0.0001d;
	}

	class NearRange : ASpaceship
	{
		public NearRange(string name, int capacity) : base(name, capacity) { }
		protected override double GetRange() => 0.00003d;
	}
}
