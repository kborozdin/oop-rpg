using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPG
{
	public class StupidAi : IAi
	{
		private readonly int height, width;
		private IForester forester;
		private Position finish;

		private readonly bool[,] visited;
		private IEnumerator<Direction> movings;

		public StupidAi(int height, int width, Position finish)
		{
			this.height = height;
			this.width = width;
			this.finish = finish;

			visited = new bool[height, width];
		}

		public void CleanState()
		{
			for (int i = 0; i < height; i++)
				for (int j = 0; j < width; j++)
					visited[i, j] = false;
			movings = null;
		}

		public Direction GetNextMove()
		{
			if (movings == null)
				movings = FindPath(forester.Position).GetEnumerator();
			if (forester.Health <= 0)
				throw new InvalidOperationException();
			movings.MoveNext();
			var direction = movings.Current;
			return direction;
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

		private IEnumerable<Direction> FindPath(Position position)
		{
			visited[position.Row, position.Column] = true;
			if (position == finish)
			{
				while (true)
					yield return Direction.None;
			}

			foreach (var direction in Direction.directions)
			{
				var newPosition = position.MovedInDirection(direction);
				if (!IsInForestBoundaries(newPosition))
					continue;
				if (visited[newPosition.Row, newPosition.Column])
					continue;

				yield return direction;
				if (forester.Position != newPosition)
				{
					visited[newPosition.Row, newPosition.Column] = true;
					continue;
				}

				foreach (var dir in FindPath(newPosition))
					yield return dir;
				yield return direction.Inversed();
			}
		}
	}
}
