using System;
using System.Collections.Generic;
using System.Linq;

namespace RPG
{
	public delegate void OnChangeHandler();

	public class Forest
	{
		private IGameObject[,] gameObjects;
		private List<Forester> foresters;
		private int fogRadius;

		public int Height
		{
			get { return gameObjects.GetLength(0); }
		}

		public int Width
		{
			get { return gameObjects.GetLength(1); }
		}

		public Forest(int height, int width, int fogRadius)
		{
			this.fogRadius = fogRadius;
			gameObjects = new IGameObject[height, width];
			for (int i = 0; i < height; i++)
				for (int j = 0; j < width; j++)
					gameObjects[i, j] = new EmptyObject();
			foresters = new List<Forester>();
		}

		public Forest(WorldInfo worldInfo)
		{
			this.fogRadius = 0;
			int height = worldInfo.Map.GetLength(0);
			int width = worldInfo.Map.GetLength(1);

			gameObjects = new IGameObject[height, width];
			for (int i = 0; i < height; i++)
				for (int j = 0; j < width; j++)
					gameObjects[i, j] = GameObjectFactory.CreateObject(worldInfo.Map[i, j]);

			foresters = worldInfo.Players.Select(p =>
				new Forester(
					p.Id, p.Nick, p.Hp,
					Position.FromPoint(p.StartPosition),
					new StubAi(Position.FromPoint(p.Target)))).ToList();
		}

		public event OnChangeHandler OnChange = delegate { };

		public void AddForester(Forester forester, bool emit = true)
		{
			foresters.Add(forester);
			if (emit)
				OnChange();
		}

		public bool MoveForester(int id, Direction direction)
		{
			var forester = FindForester(id);
			var oldPosition = forester.Position;

			var destination = forester.Position.MovedInDirection(direction);
			var interactionResult = GetGameObject(destination).InteractWith(forester, direction);

			SetGameObject(destination, interactionResult);
			return forester.Position != oldPosition;
		}

		public IEnumerable<Forester> EnumerateForesters()
		{
			return foresters;
		}

		public Forester FindForester(int id)
		{
			return foresters.FirstOrDefault(f => f.Id == id);
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

		public LastMoveInfo Simulate()
		{
			var lastMoveInfo = new LastMoveInfo()
			{
				ChangedCells = new Tuple<Point, int>[foresters.Count],
				PlayersChangedPosition = new Tuple<int, Point, int>[foresters.Count]
			};
			bool winner = false;
			var anybodyStillAlive = false;
			var alreadyGameOvered = new bool[foresters.Count];

			for (int i = 0; i < foresters.Count; i++)
			{
				var forester = foresters[i];

				var direction = forester.GetNextMove();
				Position target = forester.Position.MovedInDirection(direction);
				var map = GetVisibleMap(target, fogRadius);
				bool successfull = direction == Direction.None || MoveForester(forester.Id, direction);
				if (!successfull)
					map = GetVisibleMap(forester.Position, 0);

				lastMoveInfo.ChangedCells[i] = new Tuple<Point, int>(target.ToPoint(), GetGameObject(target).ToIndex());
				lastMoveInfo.PlayersChangedPosition[i] = new Tuple<int, Point, int>(
					forester.Id, forester.Position.ToPoint(), forester.Health);

				bool iWon = forester.Position == forester.Finish;
				bool iDied = forester.Health <= 0;
				winner |= iWon;
				anybodyStillAlive |= !iDied;
				alreadyGameOvered[i] = iWon || iDied;
				forester.Inform(successfull, map, iWon || iDied);
			}

			if (!anybodyStillAlive)
				winner = true;
			if (winner)
			{
				for (int i = 0; i < foresters.Count; i++)
					if (!alreadyGameOvered[i])
					{
						var forester = foresters[i];
						forester.Inform(false, null, true);
					}
			}

			CollectDeads();
			lastMoveInfo.GameOver = winner;
			return lastMoveInfo;
		}

		public void CollectDeads()
		{
			foresters = foresters.Where(f => f.Health > 0).ToList();
		}

		public int[,] GetVisibleMap(Position position, int radius)
		{
			//TODO fog of war
			return new[,] { { GetGameObject(position).ToIndex() } };
		}

		public WorldInfo GenerateWorldInfoPacket()
		{
			var players = foresters
				.Select(f => new Player(f.Id, f.Name, f.Position.ToPoint(), f.Finish.ToPoint(), f.Health))
				.ToArray();
			var map = new int[Height, Width];
			for (int i = 0; i < Height; i++)
				for (int j = 0; j < Width; j++)
					map[i, j] = gameObjects[i, j].ToIndex();
			return new WorldInfo { Players = players, Map = map };
		}

		public void UpdateWithLastMoveInfo(LastMoveInfo info)
		{
			foreach (var element in info.ChangedCells)
			{
				var position = Position.FromPoint(element.Item1);
				var gameObject = GameObjectFactory.CreateObject(element.Item2);
				SetGameObject(position, gameObject);
			}

			foreach (var element in info.PlayersChangedPosition)
			{
				var forester = foresters.Find(f => f.Id == element.Item1);
				var position = Position.FromPoint(element.Item2);
				var health = element.Item3;
				forester.Position = position;
				forester.Health = health;
			}

			CollectDeads();
		}
	}
}

