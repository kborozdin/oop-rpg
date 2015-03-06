using System;

namespace RPG
{
	public interface IForester
	{
		string Name { get; }
		int Health { get; }
		Position Position { get; }

		IForester movedInDirection(Direction direction);
		IForester withHealth(int health);
		char GetVisualRepresentation();
	}
}

