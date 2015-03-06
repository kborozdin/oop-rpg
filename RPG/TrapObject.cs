using System;

namespace RPG
{
	public class TrapObject : IGameObject
	{
		public GameObjectInteractionResult InteractWith(IForester forester, Direction direction)
		{
			return new GameObjectInteractionResult(
				new EmptyObject(), forester.MovedInDirection(direction).
				WithHealth(forester.Health - 1));
		}

		public char GetVisualRepresentation()
		{
			return 'x';
		}

		public string GetDescription()
		{
			return "Dangerous trap";
		}
	}
}

