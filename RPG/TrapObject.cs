using System;

namespace RPG
{
	public class TrapObject : IGameObject
	{
		public GameObjectInteractionResult InteractWith(IForester forester, Direction direction)
		{
			return new GameObjectInteractionResult(
				new EmptyObject(), forester.movedInDirection(direction).
				withHealth(forester.Health - 1));
		}

		public char GetVisualRepresentation()
		{
			return '\u271D';
		}

		public string GetDescription()
		{
			return "Dangerous trap";
		}
	}
}

