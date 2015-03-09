using System;

namespace RPG
{
	public interface IGameObject
	{
		IGameObject InteractWith(IForester forester, Direction direction);
		char GetVisualRepresentation();
		string GetDescription();
	}
}

