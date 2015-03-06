using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPG
{
	public interface IAi
	{
		Direction GetNextMove();
		void UpdateInformation(IForester forester);
		string GetForesterName();
		Position GetFinishPosition();
	}
}
