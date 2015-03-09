using System;

namespace RPG
{
	public interface IForester
	{
		string Name { get; }
		int Health { get; }
		Position Position { get; }

		void MoveInDirection(Direction direction);
		void IncreaseHealth(int delta);
		char GetVisualRepresentation();
		Direction GetNextMove();
	}
}

