using System;

namespace RPG
{
	public class OrdinaryForester : IForester
	{
		public string Name { get; private set; }
		public int Health { get; private set; }
		public Position Position { get; private set; }

		public OrdinaryForester(string name, int health, Position position)
		{
			Name = name;
			Health = health;
			Position = position;
		}

		public IForester movedInDirection(Direction direction)
		{
			return new OrdinaryForester(Name, Health, Position.movedInDirection(direction));
		}

		public IForester withHealth(int health)
		{
			return new OrdinaryForester(Name, health, Position);
		}

		public char GetVisualRepresentation()
		{
			return Name[0];
		}
	}
}

