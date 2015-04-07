using System;

namespace ServerRPG
{
	class Program
	{
		static void Main(string[] args)
		{
			var server = new GameServer();
			server.Run();
		}
	}
}
