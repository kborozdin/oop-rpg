using System;

namespace RPG
{
	public class Direction : Position
	{
		public static readonly Direction None = new Direction(0, 0);
		public static readonly Direction Right = new Direction(0, 1);
		public static readonly Direction Up = new Direction(-1, 0);
		public static readonly Direction Left = new Direction(0, -1);
		public static readonly Direction Down = new Direction(1, 0);

		public static readonly Direction[] directions = new[] { Direction.Right, Direction.Up, Direction.Left, Direction.Down };

		private Direction(int row, int column): base(row, column)
		{
		}

		public int ToIndex()
		{
			if (this == None)
				throw new InvalidOperationException();
			if (this == Right)
				return 1;
			if (this == Up)
				return 0;
			if (this == Left)
				return 3;
			if (this == Down)
				return 2;
			throw new NotSupportedException();
		}

		public Direction Inversed()
		{
			if (this == None)
				return None;
			if (this == Right)
				return Left;
			if (this == Up)
				return Down;
			if (this == Left)
				return Right;
			if (this == Down)
				return Up;
			throw new NotSupportedException();
		}

		public static Direction FromIndex(int d)
		{
			if (d == 1)
				return Right;
			if (d == 0)
				return Up;
			if (d == 3)
				return Left;
			if (d == 2)
				return Down;
			throw new NotSupportedException();
		}
	}
}

