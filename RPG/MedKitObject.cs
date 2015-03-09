using System;

namespace RPG
{
	public class MedKitObject : IGameObject
	{
		public IGameObject InteractWith(IForester forester, Direction direction)
		{
			forester.IncreaseHealth(1);
			forester.MoveInDirection(direction);
			return new EmptyObject();
		}

		public char GetVisualRepresentation()
		{
			return (char)3;
		}

		public string GetDescription()
		{
			return "First aid kit";
		}
	}
}

