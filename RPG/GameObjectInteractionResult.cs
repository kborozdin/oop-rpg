using System;

namespace RPG
{
	public class GameObjectInteractionResult
	{
		public IGameObject GameObject { get; private set; }
		public IForester Forester { get; private set; }

		public GameObjectInteractionResult(
			IGameObject gameObject, IForester forester)
		{
			GameObject = gameObject;
			Forester = forester;
		}
	}
}

