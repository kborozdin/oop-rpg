using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPG
{
	public class AiRunner
	{
		private IForest forest;
		private IAi ai;

		public AiRunner(IForest forest, IAi ai)
		{
			this.forest = forest;
			this.ai = ai;
		}

		public bool Interact()
		{
			var finish = ai.GetFinishPosition();
			var name = ai.GetForesterName();

			while (forest.FindForester(name).Position != finish && forest.FindForester(name).Health > 0)
			{
				var direction = ai.GetNextMove();
				var newForester = forest.MoveForester(ai.GetForesterName(), direction);
				ai.UpdateInformation(newForester);
			}

			return forest.FindForester(name).Health > 0;
		}
	}
}
