using RPG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace ServerRPG
{
	class GameServer
	{
		private Random random = new Random();

		private ServerConfiguration configuration;
		private Connection visualizer;
		private Forest forest;
		private object tempLock = new object();

		public void Run()
		{
			configuration = JsonConvert.DeserializeObject<ServerConfiguration>(File.ReadAllText("server_config.txt"));
			forest = TextFormatForestParser.ParseMapForest(configuration.Map, configuration.FogRadius);

			RunClientListener();
			visualizer.Write(forest.GenerateWorldInfoPacket());

			while (true)
			{
				CheckVisualizerAnswer();
				LastMoveInfo info = forest.Simulate();
				visualizer.Write(info);
				if (info.GameOver)
					break;
			}
		}

		private void CheckVisualizerAnswer()
		{
			var returnCode = visualizer.Read<Answer>();
			if (returnCode.AnswerCode != 0)
				throw new InvalidDataException();
		}

		private void ProcessClient(Connection client)
		{
			var hello = client.Read<Hello>();

			if (!hello.IsVisualizator)
			{
				Forester forester;
				lock (tempLock)
				{
					if (!configuration.HasPlayers())
						return;
					var player = configuration.GetRandomForester(random);
					forester = new Forester(
						forest.EnumerateForesters().Count(),
						hello.Name, player.Health, player.Start.ToZeroBased(),
						new AiAsker(client, player.Finish.ToZeroBased()));
					forest.AddForester(forester);
				}

				Console.WriteLine(hello.Name + " knocked!");

				client.Write(
					new ClientInfo
					{
						Hp = forester.Health,
						MapSize = new Point(forest.Width, forest.Height),
						StartPosition = forester.Position.ToPoint(),
						Target = forester.Finish.ToPoint(),
						VisibleMap = forest.GetVisibleMap(forester.Position, configuration.FogRadius)
					});
			}
			else
			{
				lock (tempLock)
					if (visualizer == null)
					{
						visualizer = client;
						Console.WriteLine("Visualizer knocked!");
					}
			}
		}

		private void RunClientListener()
		{
			var listener = new TcpListener(IPAddress.Any, ConnectionDetails.Port);
			listener.Start();

			while (true)
			{
				lock (tempLock)
					if (visualizer != null && !configuration.HasPlayers())
						break;
				if (listener.Pending())
				{
					var client = listener.AcceptTcpClient();
					new Thread(() => ProcessClient(new Connection(client.Client))).Start();
				}
			}
		}
	}
}
