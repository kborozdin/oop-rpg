using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace RPG
{
	public static class ArrayExtensions
	{
		public static bool IsLexicographicallyLessThan(this int[] first, int[] second)
		{
			int length = Math.Min(first.Length, second.Length);
			for (int i = 0; i < length; i++)
			{
				if (first[i] < second[i])
					return true;
				if (first[i] > second[i])
					return false;
			}
			if (first.Length < second.Length)
				return true;
			return false;
		}
	}

	public class SmartAi : IAi
	{
		private readonly int height, width;
		private IForester forester;
		private Position finish;

		private readonly SmartAiCellState[,] map;
		private SmartAiForesterDump dump;

		public SmartAi(int height, int width, Position finish)
		{
			this.height = height;
			this.width = width;
			this.finish = finish;

			map = new SmartAiCellState[height, width];
			for (int i = 0; i < height; i++)
				for (int j = 0; j < width; j++)
					map[i, j] = new SmartAiCellState();
		}

		private void CleanState()
		{
			dump = null;
			for (int i = 0; i < height; i++)
				for (int j = 0; j < width; j++)
					map[i, j].Visited = false;
		}

		private void ProcessDump()
		{
			int row = dump.DesiredPosition.Row;
			int column = dump.DesiredPosition.Column;

			if (dump.OldPosition == forester.Position)
				map[row, column].Impassable = true;
			map[row, column].Visited = true;
			if (dump.OldHealth > forester.Health)
				map[row, column].Harmful = true;

			dump = null;
		}

		private Tuple<Position, Position> GetBestPositions(int[,] distance)
		{
			var bestNewPosition = new Position(-1, -1);
			var linkedNewPosition = new Position(-1, -1);
			var bestValue = new[] { int.MaxValue, int.MaxValue, int.MaxValue };

			for (int i = 0; i < height; i++)
				for (int j = 0; j < width; j++)
				{
					var me = new Position(i, j);
					if (map[i, j].Visited)
						continue;

					int minNeighbour = int.MaxValue;
					var curLinked = new Position(-1, -1);
					foreach (var direction in Direction.directions)
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

					var currentValue = new[] { minNeighbour, me.DistanceTo(finish), forester.Position.DistanceTo(me) };
					if (currentValue.IsLexicographicallyLessThan(bestValue))
					{
						bestValue = currentValue;
						linkedNewPosition = curLinked;
						bestNewPosition = me;
					}
				}

			if (bestValue[0] == int.MaxValue)
				throw new InvalidOperationException();

			return Tuple.Create<Position, Position>(bestNewPosition, linkedNewPosition);
		}

		public Direction GetNextMove()
		{
			if (dump != null)
				ProcessDump();

			if (forester.Health <= 0)
				throw new InvalidOperationException();
			if (forester.Position == finish)
				return Direction.None;

			var distance = int[height, width];
			var parents = new Direction[height, width];
			CalculateDistances(distance, parents);

			var bestPositions = GetBestPositions(distance);
			var bestNewPosition = bestPositions.Item1;
			var linkedNewPosition = bestPositions.Item2;

			if (forester.Position.DistanceTo(bestNewPosition) == 1)
			{
				foreach (var direction in Direction.directions)
				{
					var newPosition = forester.Position.MovedInDirection(direction);
					if (newPosition == bestNewPosition)
					{
						dump = new SmartAiForesterDump(forester.Position, newPosition, forester.Health);
						return direction;
					}
				}
				Debug.Assert(false);
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

				foreach (var direction in Direction.directions)
				{
					var newPosition = position.MovedInDirection(direction);
					if (!IsInForestBoundaries(newPosition))
						continue;
					if (map[newPosition.Row, newPosition.Column].Impassable)
						continue;
					if (!map[newPosition.Row, newPosition.Column].Visited)
						continue;

					if (!map[newPosition.Row, newPosition.Column].Harmful)
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
