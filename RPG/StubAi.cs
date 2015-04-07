using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPG
{
	public class StubAi : IAi
	{
		public Position Finish { get; private set; }

		public StubAi(Position finish)
		{
			this.Finish = finish;
		}

		public Direction GetNextMove()
		{
			return Direction.None;
		}

		public void CleanState()
		{
		}

		public void SetForester(Forester forester)
		{
		}

		public void SetFinish(Position finish)
		{
			Finish = finish;
		}

		public void Inform(bool successfull, int[,] visibleMap, bool gameOver)
		{
		}
	}
}
