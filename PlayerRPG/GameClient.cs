using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using RPG;
using System.Threading;

namespace PlayerRPG
{
	class GameClient
	{
		private const string hostname = "127.0.0.1";

		private Func<int, int, Position, IAi> createAi;
		private string nick;

		public GameClient(Func<int, int, Position, IAi> createAi, string nick)
		{
			this.createAi = createAi;
			this.nick = nick;
		}

		public void Run()
		{
			var connection = new Connection(new TcpClient(hostname, ConnectionDetails.Port).Client);

			connection.Write(new Hello { Name = nick, IsVisualizator = false });
			var hello = connection.Read<ClientInfo>();
			var ai = createAi(hello.MapSize.Y, hello.MapSize.X, Position.FromPoint(hello.Target));
			var forester = new Forester(0, null, hello.Hp,
				Position.FromPoint(hello.StartPosition), ai);

			while (true)
			{
				var direction = ai.GetNextMove();
				connection.Write(new Move { Direction = direction.ToIndex() });
				var packet = connection.Read<MoveResultInfo>();
				if (!ApplyMoveResult(forester, direction, packet))
					break;
			}
		}

		private bool ApplyMoveResult(Forester forester, Direction direction, MoveResultInfo result)
		{
			//TODO fog of war
			if (result.Result == 2)
				return false;
			if (result.Result == 0)
				forester.MoveInDirection(direction);
			int cx = result.VisibleMap.GetLength(0) / 2;
			int cy = result.VisibleMap.GetLength(1) / 2;
			if (result.Result == 0 && result.VisibleMap[cx, cy] == 3)
				forester.IncreaseHealth(-1);
			if (result.VisibleMap[cx, cy] == 4)
				forester.IncreaseHealth(1);
			return true;
		}
	}
}
