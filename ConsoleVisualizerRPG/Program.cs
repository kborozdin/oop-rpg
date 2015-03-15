using System;
using RPG;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ConsoleVisualizerRPG
{
	static class MainClass
	{
		private static IForest forest;

		public static void Main(string[] args)
		{
			forest = TextFormatForestParser.LoadForest("forest");

			Repaint();
			Thread.Sleep(200);

			while (true)
			{
				forest.Simulate();
				Repaint();
				Thread.Sleep(200);
			}
		}

		private static void Repaint()
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
