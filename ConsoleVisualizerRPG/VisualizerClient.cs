using RPG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ConsoleVisualizerRPG
{
	class VisualizerClient
	{
		private const string hostname = "127.0.0.1";
		private readonly string AsteriskDelim = new String('*', 20);

		Forest forest;

		public void Run()
		{
			DrawIntro();

			var connection = new Connection(new TcpClient(hostname, ConnectionDetails.Port).Client);
			connection.Write(new Hello { IsVisualizator = true, Name = null });
			var worldInfo = connection.Read<WorldInfo>();
			forest = new Forest(worldInfo);

			while (true)
			{
				Thread.Sleep(500);
				connection.Write(new Answer { AnswerCode = 0 });
				var state = connection.Read<LastMoveInfo>();
				forest.UpdateWithLastMoveInfo(state);
				Repaint();
				if (state.GameOver)
					break;
			}

			DrawEnding();
		}

		private void DrawIntro()
		{
			Console.WriteLine(AsteriskDelim + "\nHELLO!\n" + AsteriskDelim + "\n\nWAITING FOR CLIENTS");
		}

		private void DrawEnding()
		{
			Console.Clear();
			Console.WriteLine(AsteriskDelim + "\nGAME OVER\n" + AsteriskDelim + "\n");
			Console.WriteLine("WINNERS:");
			foreach (var forester in forest.EnumerateForesters().Where(f => f.Position == f.Finish))
				Console.WriteLine("  " + forester.Name);
			Console.WriteLine("\nTHANKS FOR WATCHING\n(press any key to exit)");
			Console.ReadKey();
		}

		private void Repaint()
		{
			Console.Clear();
			var board = new char[forest.Height, forest.Width];
			var foresters = forest.EnumerateForesters().OrderBy(f => f.Name);

			for (int i = 0; i < forest.Height; i++)
			{
				for (int j = 0; j < forest.Width; j++)
				{
					IGameObject gameObject = forest.GetGameObject(new Position(i, j));
					board[i, j] = gameObject.GetVisualRepresentation();
				}
			}

			foreach (var forester in foresters)
			{
				Position position = forester.Position;
				board[position.Row, position.Column] = forester.GetVisualRepresentation();
			}

			for (int i = 0; i < forest.Height; i++)
			{
				for (int j = 0; j < forest.Width; j++)
					Console.Write(board[i, j]);
				Console.WriteLine();
			}

			Console.WriteLine("\nLegend:");
			foreach (var gameObject in GameObjectFactory.EnumerateSubclasses())
			{
				Console.WriteLine("'{0}' = {1}", gameObject.GetVisualRepresentation(),
								  gameObject.GetDescription());
			}

			Console.WriteLine("\nHeroes:");
			foreach (var forester in foresters)
			{
				Console.WriteLine("{0} ({1} HP)", forester.Name, forester.Health);
			}
		}
	}
}
