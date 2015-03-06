using System;

namespace RPG
{
	public interface IForester
	{
		string Name { get; }
		int Health { get; }
		Position Position { get; }

		IForester MovedInDirection(Direction direction);
		IForester WithHealth(int health);
		char GetVisualRepresentation();
	}
}

