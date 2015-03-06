using System;

namespace RPG
{
	public class MedKitObject : IGameObject
	{
		public GameObjectInteractionResult InteractWith(IForester forester, Direction direction)
		{
			return new GameObjectInteractionResult(
				new EmptyObject(), forester.MovedInDirection(direction).
				WithHealth(forester.Health + 1));
		}

		public char GetVisualRepresentation()
		{
			return 'h';
		}

		public string GetDescription()
		{
			return "First aid kit";
		}
	}
}

