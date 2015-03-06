using System;

namespace RPG
{
	public class WallObject : IGameObject
	{
		public GameObjectInteractionResult InteractWith(IForester forester, Direction direction)
		{
			return new GameObjectInteractionResult(this, forester);
		}

		public char GetVisualRepresentation()
		{
			return '\u2588';
		}

		public string GetDescription()
		{
			return "Impassable cell";
		}
	}
}

