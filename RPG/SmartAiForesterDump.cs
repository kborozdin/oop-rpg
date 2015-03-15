using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPG
{
	class SmartAiForesterDump
	{
		public Position OldPosition { get; set; }
		public Position DesiredPosition { get; set; }
		public int OldHealth { get; set; }

		public SmartAiForesterDump(Position oldPosition, Position desiredPosition, int oldHealth)
		{
			OldPosition = oldPosition;
			DesiredPosition = desiredPosition;
			OldHealth = oldHealth;
		}
	}
}
