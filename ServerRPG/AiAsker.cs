using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG;
using System.Net.Sockets;

namespace ServerRPG
{
	class AiAsker : IAi
	{
		private const int QueryTimeout = 1000;

		private Connection teller;
		private Forester forester;
		public Position Finish { get; private set; }

		public AiAsker(Connection teller, Position finish)
		{
			this.teller = teller;
			this.Finish = finish;
		}

		public Direction GetNextMove()
		{
			Move direction;
			try
			{
				direction = teller.Read<Move>(QueryTimeout);
			}
			catch (System.Net.Sockets.SocketException)
			{
				return Direction.None;
			}
			return Direction.FromIndex(direction.Direction);
		}

		public void SetForester(Forester forester)
		{
			this.forester = forester;
		}

		public void SetFinish(Position finish)
		{
			this.Finish = finish;
		}

		public void CleanState()
		{
			throw new NotImplementedException();
		}

		public void Inform(bool successfull, int[,] visibleMap, bool gameOver)
		{
			teller.Write<MoveResultInfo>(
				new MoveResultInfo
				{
					Result = gameOver ? 2 : (successfull ? 0 : 1),
					VisibleMap = visibleMap
				});
		}
	}
}
