using RPG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerRPG
{
	class Program
	{
		static void Main(string[] args)
		{
			var nick = args.Length > 0 ? args[0] : "default";
			var createAi = (args.Length < 2 || args[1] == "2") ?
				(Func<int, int, Position, IAi>) ((a, b, c) => new SmartAi(a, b, c)) :
				(Func<int, int, Position, IAi>) ((a, b, c) => new StupidAi(a, b, c));

			var client = new GameClient(createAi, nick);
			client.Run();
		}
	}
}
