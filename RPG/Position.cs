using System;

namespace RPG
{
	public class Position
	{
		public int Row { get; private set; }
		public int Column { get; private set; }

		public Position(int row, int column)
		{
			Row = row;
			Column = column;
		}

		public static Position operator + (Position first, Position second)
		{
			return new Position(first.Row + second.Row, first.Column + second.Column);
		}

		public Position MovedInDirection(Direction direction)
		{
			return this + direction;
		}

		public int DistanceTo(Position other)
		{
			return Math.Abs(Row - other.Row) + Math.Abs(Column - other.Column);
		}

		public override bool Equals(object other)
		{
			if (ReferenceEquals(other, null))
				return false;
			if (ReferenceEquals(this, other))
				return true;
			var casted = other as Position;
			if (ReferenceEquals(casted, null))
				return false;
			return Equals(casted);
		}

		public override int GetHashCode()
		{
			return Row.GetHashCode() ^ Column.GetHashCode();
		}

		public bool Equals(Position other)
		{
			if (ReferenceEquals(other, null))
				return false;
			if (ReferenceEquals(this, other))
				return true;
			return Row == other.Row && Column == other.Column;
		}

		public static bool operator == (Position first, Position second)
		{
			return first.Equals(second);
		}

		public static bool operator != (Position first, Position second)
		{
			return !first.Equals(second);
		}

		public Position ToZeroBased()
		{
			return new Position(Row - 1, Column - 1);
		}

		public static Position FromPoint(Point point)
		{
			return new Position(point.Y, point.X);
		}

		public Point ToPoint()
		{
			return new Point(Column, Row);
		}
	}
}

