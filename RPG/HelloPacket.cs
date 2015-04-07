using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG;

namespace RPG
{
	public class HelloPacket
	{
		public int Height { get; set; }
		public int Width { get; set; }
		public Position Finish { get; set; }
		public int FogRadius { get; set; }
	}
}
