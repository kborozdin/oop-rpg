using NUnit.Framework;
using System;
using RPG;
using System.Linq;
using System.IO;

namespace TestRPG
{
	[TestFixture()]
	public class TestSmartAi
	{
		[Test()]
		public void TestFirstMap()
		{
			var forest = TextFormatForestParser.ParseFullForest(File.ReadAllLines("forest1"));
			for (int i = 0; i < 100; i++)
				forest.Simulate();
			Assert.AreEqual(forest.FindForester(0).Position, new Position(3, 12));
		}

		[Test()]
		public void TestSecondMap()
		{
			var forest = TextFormatForestParser.ParseFullForest(File.ReadAllLines("forest2"));
			for (int i = 0; i < 100; i++)
				forest.Simulate();
			Assert.AreEqual(forest.FindForester(0).Position, new Position(3, 9));
		}
	}
}

