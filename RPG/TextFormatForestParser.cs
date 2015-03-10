using System;
using System.IO;

namespace RPG
{
	public static class TextFormatForestParser
	{
		public static IForest LoadForest(string filename)
		{
			var lines = File.ReadAllLines(filename);
			return ParseForest(lines);
		}

		public static IForest ParseForest(string[] lines)
		{
			int height = int.Parse(lines[0]);
			int width = lines[1].Length;
			var forest = new OrdinaryForest(height, lines[1].Length);

			for (int i = 1; i <= height; i++)
			{
				for (int j = 0; j < lines[i].Length; j++)
				{
					IGameObject gameObject = GameObjectFactory.CreateObject(lines[i][j]);
					forest.SetGameObject(new Position(i - 1, j), gameObject);
				}
			}

			for (int i = height + 1; i < lines.Length; i++)
			{
				var tokens = lines[i].Split();
				var forester = new OrdinaryForester(
					tokens[0], int.Parse(tokens[1]), new Position(
					int.Parse(tokens[2]) - 1, int.Parse(tokens[3]) - 1),
					new SmartAi(height, width, new Position(int.Parse(tokens[4]) - 1, int.Parse(tokens[5]) - 1)));
				forest.AddForester(forester);
			}

			return forest;
		}
	}
}

