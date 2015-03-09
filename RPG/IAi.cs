using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPG
{
	public interface IAi
	{
		Direction GetNextMove();
		void CleanState();
		void SetForester(IForester forester);
		void SetFinish(Position finish);
	}
}
