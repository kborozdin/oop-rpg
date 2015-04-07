using System;
using System.Collections.Generic;
using System.Linq;

namespace RPG
{
	public static class GameObjectFactory
	{
		private static readonly Dictionary<char, Func<IGameObject>> gameObjectsByChar =
			new Dictionary<char, Func<IGameObject>>
		{
			{ '0', () => new EmptyObject() },
			{ '1', () => new WallObject() },
			{ 'K', () => new TrapObject() },
			{ 'L', () => new MedKitObject() }
		};

		private static readonly Dictionary<int, Func<IGameObject>> gameObjectsByIndex =
			new Dictionary<int, Func<IGameObject>>
		{
			{ 0, () => null },
			{ 1, () => new EmptyObject() },
			{ 2, () => new WallObject() },
			{ 3, () => new TrapObject() },
			{ 4, () => new MedKitObject() }
		};

		public static IGameObject CreateObject(char code)
		{
			return gameObjectsByChar[code]();
		}

		public static IGameObject CreateObject(int index)
		{
			return gameObjectsByIndex[index]();
		}

		public static IEnumerable<IGameObject> EnumerateSubclasses()
		{
			return gameObjectsByChar.Values.Select(f => f());
		}
	}
}

