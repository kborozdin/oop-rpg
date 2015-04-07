using System;
using System.IO;

namespace RPG
{
	public static class TextFormatForestParser
	{
		public static Forest LoadForest(string filename)
		{
			var lines = File.ReadAllLines(filename);
			return ParseFullForest(lines);
		}

		public static Forest ParseMapForest(string[] lines, int fogRadius)
		{
			return ParseMapForestInternal(lines, 0, lines.Length, lines[0].Length, fogRadius);
		}

		private static Forest ParseMapForestInternal(string[] lines, int rowShift, int rows, int columns, int fogRadius)
		{
			var forest = new Forest(rows, columns, fogRadius);
			for (int i = rowShift; i < rows + rowShift; i++)
			{
				for (int j = 0; j < columns; j++)
				{
					IGameObject gameObject = GameObjectFactory.CreateObject(lines[i][j]);
					forest.SetGameObject(new Position(i - rowShift, j), gameObject);
				}
			}
			return forest;
		}

		public static Forest ParseFullForest(string[] lines, int fogRadius = 0)
		{
			int height = int.Parse(lines[0]);
			int width = lines[1].Length;
			var forest = ParseMapForestInternal(lines, 1, height, width, fogRadius);

			for (int i = height + 1; i < lines.Length; i++)
			{
				var tokens = lines[i].Split();
				var forester = new Forester(
					i - (height + 1),
					tokens[0], int.Parse(tokens[1]), new Position(
					int.Parse(tokens[2]) - 1, int.Parse(tokens[3]) - 1),
					new SmartAi(height, width, new Position(int.Parse(tokens[4]) - 1, int.Parse(tokens[5]) - 1)));
				forest.AddForester(forester);
			}

			return forest;
		}
	}
}

