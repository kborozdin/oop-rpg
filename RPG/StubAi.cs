using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPG
{
	public class StubAi : IAi
	{
		public Direction GetNextMove()
		{
			return Direction.None;
		}

		public void CleanState()
		{
		}

		public void SetForester(IForester forester)
		{
		}

		public void SetFinish(Position finish)
		{
		}
	}
}
