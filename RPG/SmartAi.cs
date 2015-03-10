using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPG
{
	public class SmartAi : IAi
	{
		private static readonly Direction[] directions = new[] { Direction.Right, Direction.Up, Direction.Left, Direction.Down };

		private readonly int height, width;
		private IForester forester;
		private Position finish;

		private readonly bool[,] harmful;
		private readonly bool[,] impassable;
		private readonly bool[,] visited;

		private Position oldPosition;
		private Position desiredPosition;
		private int oldHealth;
		private bool waitForInfo;

		public SmartAi(int height, int width, Position finish)
		{
			this.height = height;
			this.width = width;
			this.finish = finish;

			harmful = new bool[height, width];
			impassable = new bool[height, width];
			visited = new bool[height, width];
		}

		public void CleanState()
		{
			waitForInfo = false;
			for (int i = 0; i < height; i++)
				for (int j = 0; j < width; j++)
					visited[i, j] = false;
		}

		public Direction GetNextMove()
		{
			if (waitForInfo)
			{
				waitForInfo = false;
				if (oldPosition == forester.Position)
					impassable[desiredPosition.Row, desiredPosition.Column] = true;
				visited[desiredPosition.Row, desiredPosition.Column] = true;
				if (oldHealth > forester.Health)
					harmful[desiredPosition.Row, desiredPosition.Column] = true;
			}

			if (forester.Health <= 0)
				throw new InvalidOperationException();
			if (forester.Position == finish)
				return Direction.None;

			var distance = new int[height, width];
			var parents = new Direction[height, width];
			CalculateDistances(distance, parents);

			var bestNewPosition = new Position(-1, -1);
			var linkedNewPosition = new Position(-1, -1);
			var bestValue = Tuple.Create<int, int>(int.MaxValue, int.MaxValue);

			for (int i = 0; i < height; i++)
				for (int j = 0; j < width; j++)
				{
					var me = new Position(i, j);
					if (visited[i, j])
						continue;

					int minNeighbour = int.MaxValue;
					var curLinked = new Position(-1, -1);
					foreach (var direction in directions)
					{
						var newPosition = me.MovedInDirection(direction);
						if (!IsInForestBoundaries(newPosition))
							continue;
						if (distance[newPosition.Row, newPosition.Column] < minNeighbour)
						{
							minNeighbour = distance[newPosition.Row, newPosition.Column];
							curLinked = newPosition;
						}
					}

					var currentValue = Tuple.Create<int, int>(minNeighbour, me.DistanceTo(finish));
					//TODO
					if (currentValue.Item1 < bestValue.Item1 || (currentValue.Item1 == bestValue.Item1 && currentValue.Item2 < bestValue.Item2))
					{
						bestValue = currentValue;
						linkedNewPosition = curLinked;
						bestNewPosition = me;
					}
				}

			if (bestValue.Item1 == int.MaxValue) //TODO
				throw new Exception();

			if (forester.Position.DistanceTo(bestNewPosition) == 1)
			{
				foreach (var direction in directions)
				{
					var newPosition = forester.Position.MovedInDirection(direction);
					if (newPosition == bestNewPosition)
					{
						oldPosition = forester.Position;
						desiredPosition = newPosition;
						oldHealth = forester.Health;
						waitForInfo = true;
						return direction;
					}
				}
				throw new Exception(); //TODO
			}

			return RestoreFirstMove(linkedNewPosition, parents);
		}

		private Direction RestoreFirstMove(Position end, Direction[,] parents)
		{
			while (true)
			{
				var direction = parents[end.Row, end.Column];
				end = end.MovedInDirection(direction.Inversed());
				if (end == forester.Position)
					return direction;
			}
		}

		private void CalculateDistances(int[,] distance, Direction[,] parents)
		{
			for (int i = 0; i < height; i++)
				for (int j = 0; j < width; j++)
					distance[i, j] = int.MaxValue;
			distance[forester.Position.Row, forester.Position.Column] = 0;

			var firstQueue = new Queue<Position>();
			var secondQueue = new Queue<Position>();
			firstQueue.Enqueue(forester.Position);

			while (firstQueue.Count > 0 || secondQueue.Count > 0)
			{
				Position position = (firstQueue.Count > 0 ? firstQueue.Dequeue() : secondQueue.Dequeue());
				int currentDistance = distance[position.Row, position.Column];

				foreach (var direction in directions)
				{
					var newPosition = position.MovedInDirection(direction);
					if (!IsInForestBoundaries(newPosition))
						continue;
					if (impassable[newPosition.Row, newPosition.Column])
						continue;
					if (!visited[newPosition.Row, newPosition.Column])
						continue;

					if (!harmful[newPosition.Row, newPosition.Column])
					{
						if (distance[newPosition.Row, newPosition.Column] > currentDistance)
						{
							distance[newPosition.Row, newPosition.Column] = currentDistance;
							parents[newPosition.Row, newPosition.Column] = direction;
							firstQueue.Enqueue(newPosition);
						}
					}
					else
					{
						if (distance[newPosition.Row, newPosition.Column] > currentDistance + 1)
						{
							distance[newPosition.Row, newPosition.Column] = currentDistance + 1;
							parents[newPosition.Row, newPosition.Column] = direction;
							secondQueue.Enqueue(newPosition);
						}
					}
				}
			}
		}

		public void SetForester(IForester newForester)
		{
			forester = newForester;
			CleanState();
		}

		public void SetFinish(Position newFinish)
		{
			finish = newFinish;
			CleanState();
		}

		private bool IsInForestBoundaries(Position position)
		{
			return position.Row >= 0 && position.Row < height &&
				position.Column >= 0 && position.Column < width;
		}
	}
}
