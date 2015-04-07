using System;

namespace RPG
{
	public interface IGameObject
	{
		IGameObject InteractWith(Forester forester, Direction direction);
		char GetVisualRepresentation();
		string GetDescription();
		int ToIndex();
	}
}

