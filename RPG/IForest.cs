using System;
using System.Collections.Generic;

namespace RPG
{
	public delegate void OnChangeHandler();

	public interface IForest
	{
		event OnChangeHandler OnChange;

		int Height { get; }
		int Width { get; }

		void AddForester(IForester forester, bool emit);
		IForester MoveForester(string name, Direction direction);
		IEnumerable<IForester> EnumerateForesters();
		IForester FindForester(string name);
		IGameObject GetGameObject(Position position);
		void SetGameObject(Position position, IGameObject gameObject, bool emit);
	}
}

