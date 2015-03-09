using System;

namespace RPG
{
	public class EmptyObject : IGameObject
	{
		public IGameObject InteractWith(IForester forester, Direction direction)
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
	}
}

