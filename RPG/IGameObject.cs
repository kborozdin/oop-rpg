using System;

namespace RPG
{
	public interface IGameObject
	{
		GameObjectInteractionResult InteractWith(IForester forester, Direction direction);
		char GetVisualRepresentation();
		string GetDescription();
	}
}

