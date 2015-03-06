using System;
using System.Collections.Generic;
using System.Linq;

namespace RPG
{
	public static class GameObjectFactory
	{
		private static readonly Dictionary<char, Func<IGameObject>> gameObjects =
			new Dictionary<char, Func<IGameObject>>
		{
			{ '0', () => new EmptyObject() },
			{ '1', () => new WallObject() },
			{ 'K', () => new TrapObject() },
			{ 'L', () => new MedKitObject() }
		};

		public static IGameObject CreateObject(char code)
		{
			Console.WriteLine(code);
			return gameObjects[code]();
		}

		public static IEnumerable<IGameObject> EnumerateSubclasses()
		{
			return gameObjects.Values.Select(f => f());
		}
	}
}

