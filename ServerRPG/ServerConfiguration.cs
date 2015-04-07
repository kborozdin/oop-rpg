using RPG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerRPG
{
	class ServerConfiguration
	{
		public int FogRadius { get; set; }
		public string[] Map { get; set; }
		public List<PlayerDescription> Players;

		public ServerConfiguration()
		{
			Players = new List<PlayerDescription>();
		}

		public PlayerDescription GetRandomForester(Random random)
		{
			int index = random.Next(Players.Count);
			var description = Players[index];
			Players.RemoveAt(index);
			return description;
		}

		public void AddPlayerDescription(PlayerDescription description)
		{
			Players.Add(description);
		}

		public bool HasPlayers()
		{
			return Players.Count > 0;
		}
	}
}
