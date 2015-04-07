using System;

namespace RPG
{
	public class WallObject : IGameObject
	{
		public IGameObject InteractWith(Forester forester, Direction direction)
		{
			return this;
		}

		public char GetVisualRepresentation()
		{
			return '\u2588';
		}

		public string GetDescription()
		{
			return "Impassable cell";
		}

		public int ToIndex()
		{
			return 2;
		}
	}
}

