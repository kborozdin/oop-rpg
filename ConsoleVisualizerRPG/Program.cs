using System;
using RPG;
using System.Collections.Generic;
using System.Linq;

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
		private static string hero;

		public static void Main(string[] args)
		{
			forest = TextFormatForestParser.LoadForest("../../../forest");
			hero = forest.EnumerateForesters().First().Name;
			forest.OnChange += Repaint;
			Repaint();
			Mainloop();
		}

		private static void Mainloop()
		{
			ConsoleKey key = ConsoleKey.NoName;
			do
			{
				key = Console.ReadKey().Key;
				Direction direction;
				/*
				Next line uses the feature of TryGetValue to
				return a default value when there is no such key.
				Default value for Direction is None (zero vector)
				so nothing happens.
				*/
				keyboardActions.TryGetValue(key, out direction);
				forest.MoveForester(hero, direction);
			}
			while (key != ConsoleKey.Enter);
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
