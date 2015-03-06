using System;

namespace RPG
{
	public class EmptyObject : IGameObject
	{
		public GameObjectInteractionResult InteractWith(IForester forester, Direction direction)
		{
			return new GameObjectInteractionResult(this, forester.movedInDirection(direction));
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

