using System;

namespace RPG
{
	public class TrapObject : IGameObject
	{
		public IGameObject InteractWith(Forester forester, Direction direction)
		{
			forester.IncreaseHealth(-1);
			forester.MoveInDirection(direction);
			return this;
		}

		public char GetVisualRepresentation()
		{
			return 'x';
		}

		public string GetDescription()
		{
			return "Dangerous trap";
		}

		public int ToIndex()
		{
			return 3;
		}
	}
}

