using System;

namespace RPG
{
	public class Forester
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int Health { get; set; }
		public Position Position { get; set; }
		private readonly IAi ai;

		public Position Finish
		{
			get { return ai.Finish; }
		}

		public Forester(int id, string name, int health, Position position, IAi ai)
		{
			this.Id = id;
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

		public void Inform(bool successfull, int[,] visibleMap, bool gameOver)
		{
			ai.Inform(successfull, visibleMap, gameOver);
		}
	}
}

