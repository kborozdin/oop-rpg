using System;

namespace RPG
{
	public class MedKitObject : IGameObject
	{
		public GameObjectInteractionResult InteractWith(IForester forester, Direction direction)
		{
			return new GameObjectInteractionResult(
				new EmptyObject(), forester.movedInDirection(direction).
				withHealth(forester.Health + 1));
		}

		public char GetVisualRepresentation()
		{
			return '\u2764';
		}

		public string GetDescription()
		{
			return "First aid kit";
		}
	}
}

