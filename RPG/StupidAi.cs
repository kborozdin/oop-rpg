using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPG
{
	public class StupidAi : IAi
	{
		private static readonly Direction[] directions = new[] { Direction.Right, Direction.Up, Direction.Left, Direction.Down };

		private readonly int height, width;
		private IForester forester;
		private readonly Position finish;

		private readonly bool[,] visited;
		private IEnumerable<Direction> movings;
		private bool sucessfullyMoved;

		public StupidAi(int height, int width, IForester forester, Position finish)
		{
			this.height = height;
			this.width = width;
			this.forester = forester;
			this.finish = finish;

			visited = new bool[height, width];
			movings = FindPath(forester.Position);
		}

		public Direction GetNextMove()
		{
			if (forester.Health <= 0)
				return null;
			var direction = movings.First();
			movings = movings.Skip(1); //TODO fix
			return direction;
		}

		public void UpdateInformation(IForester newForester)
		{
			sucessfullyMoved = newForester.Position != forester.Position;
			forester = newForester;
		}

		public string GetForesterName()
		{
			return forester.Name;
		}

		public Position GetFinishPosition()
		{
			return finish;
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
				yield break;

			foreach (var direction in directions)
			{
				var newPosition = position.MovedInDirection(direction);
				if (!IsInForestBoundaries(newPosition))
					continue;
				if (visited[newPosition.Row, newPosition.Column])
					continue;

				yield return direction;
				if (!sucessfullyMoved)
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
