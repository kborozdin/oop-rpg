using System;
using RPG;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ConsoleVisualizerRPG
{
	static class MainClass
	{
		private static readonly Dictionary<ConsoleKey, Direction> keyboardActions =
			new Dictionary<ConsoleKey, Direction>
		{
			{ConsoleKey.RightArrow, Direction.Right},
			{ConsoleKey.UpArrow, Direction.Up},
			{ConsoleKey.LeftArrow, Direction.Left},
			{ConsoleKey.DownArrow, Direction.Down}
		};

		private static IForest forest;

		public static void Main(string[] args)
		{
			forest = TextFormatForestParser.LoadForest("../../../forest");
			forest.OnChange += () =>
			{
				Repaint();
				Thread.Sleep(1000);
			};
			Repaint();

			var ai = new StupidAi(forest.Height, forest.Width, forest.EnumerateForesters().First(), new Position(3, 3));
			var runner = new AiRunner(forest, ai);
			runner.Interact();

			//Mainloop();
		}
		/*
		private static void Mainloop()
		{
			ConsoleKey key = ConsoleKey.NoName;
			do
			{
				key = Console.ReadKey().Key;
				try
				{
					var direction = keyboardActions[key];
					forest.MoveForester(hero, direction);
				}
				catch (KeyNotFoundException)
				{
				}
			}
			while (key != ConsoleKey.Enter);
		}
		*/
		private static void Repaint()
		{
			//Console.Clear();
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
