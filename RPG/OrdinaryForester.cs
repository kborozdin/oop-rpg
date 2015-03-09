using System;

namespace RPG
{
	public class OrdinaryForester : IForester
	{
		public string Name { get; private set; }
		public int Health { get; private set; }
		public Position Position { get; private set; }
		private readonly IAi ai;

		public OrdinaryForester(string name, int health, Position position, IAi ai)
		{
			this.Name = name;
			this.Health = health;
			this.Position = position;
			this.ai = ai;
			ai.SetForester(this);
		}

		public void MoveInDirection(Direction direction)
		{
			Position = Position.MovedInDirection(direction);
		}

		public void IncreaseHealth(int delta)
		{
			Health += delta;
		}

		public char GetVisualRepresentation()
		{
			return Name[0];
		}

		public Direction GetNextMove()
		{
			return ai.GetNextMove();
		}
	}
}

