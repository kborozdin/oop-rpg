using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPG
{
	public interface IAi
	{
		Position Finish { get; }
		Direction GetNextMove();
		void SetForester(Forester forester);
		void SetFinish(Position finish);
		void CleanState();
		void Inform(bool successfull, int[,] visibleMap, bool gameOver);
	}
}
