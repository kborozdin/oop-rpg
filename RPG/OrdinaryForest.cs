using System;
using System.Collections.Generic;

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

		public void AddForester(IForester forester)
		{
			foresters.Add(forester);
			OnChange();
		}

		public bool MoveForester(string name, Direction direction)
		{
			IForester forester = foresters.Find(f => f.Name == name);
			foresters.RemoveAll(f => f.Name == name);
			Position destination = forester.Position.movedInDirection(direction);
			var interactionResult = GetGameObject(destination).InteractWith(forester, direction);
			SetGameObject(destination, interactionResult.GameObject);
			foresters.Add(interactionResult.Forester);
			OnChange();
			return interactionResult.Forester.Position == destination;
		}

		public IEnumerable<IForester> EnumerateForesters()
		{
			return foresters;
		}

		public IGameObject GetGameObject(Position position)
		{
			return gameObjects[position.Row, position.Column];
		}

		public void SetGameObject(Position position, IGameObject gameObject)
		{
			gameObjects[position.Row, position.Column] = gameObject;
			OnChange();
		}
	}
}

