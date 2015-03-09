using System;
using System.Collections.Generic;
using System.Linq;

namespace RPG
{
	public class OrdinaryForest : IForest
	{
		private IGameObject[,] gameObjects;
		private List<IForester> foresters;

		public int Height
		{
			get
			{
				return gameObjects.GetLength(0);
			}
		}

		public int Width
		{
			get
			{
				return gameObjects.GetLength(1);
			}
		}

		public OrdinaryForest(int height, int width)
		{
			gameObjects = new IGameObject[height, width];
			for (int i = 0; i < height; i++)
				for (int j = 0; j < width; j++)
					gameObjects[i, j] = new EmptyObject();
			foresters = new List<IForester>();
		}

		public event OnChangeHandler OnChange = delegate { };

		public void AddForester(IForester forester, bool emit = true)
		{
			foresters.Add(forester);
			if (emit)
				OnChange();
		}

		public bool MoveForester(string name, Direction direction)
		{
			var forester = FindForester(name);
			var oldPosition = forester.Position;

			var destination = forester.Position.MovedInDirection(direction);
			var interactionResult = GetGameObject(destination).InteractWith(forester, direction);

			SetGameObject(destination, interactionResult);
			return forester.Position != oldPosition;
		}

		public IEnumerable<IForester> EnumerateForesters()
		{
			return foresters;
		}

		public IForester FindForester(string name)
		{
			return foresters.FirstOrDefault(f => f.Name == name);
		}

		public IGameObject GetGameObject(Position position)
		{
			return gameObjects[position.Row, position.Column];
		}

		public void SetGameObject(Position position, IGameObject gameObject, bool emit = true)
		{
			gameObjects[position.Row, position.Column] = gameObject;
			if (emit)
				OnChange();
		}

		public void Simulate()
		{
			foreach (var forester in foresters)
			{
				var direction = forester.GetNextMove();
				MoveForester(forester.Name, direction);
			}

			CollectDeads();
		}

		public void CollectDeads()
		{
			foresters = foresters.Where(f => f.Health > 0).ToList();
		}
	}
}

