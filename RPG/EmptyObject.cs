using System;

namespace RPG
{
	public class EmptyObject : IGameObject
	{
		public IGameObject InteractWith(Forester forester, Direction direction)
		{
			forester.MoveInDirection(direction);
			return this;
		}

		public char GetVisualRepresentation()
		{
			return ' ';
		}

		public string GetDescription()
		{
			return "Empty cell";
		}

		public int ToIndex()
		{
			return 1;
		}
	}
}

