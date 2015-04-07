using NUnit.Framework;
using System;
using RPG;
using System.Linq;

namespace TestRPG
{
	[TestFixture()]
	public class TestTextFormatForestParser
	{
		Forest forest;

		[TestFixtureSetUp()]
		public void SetUp()
		{
			forest = TextFormatForestParser.ParseFullForest(
				new [] { "4", "11111", "10001", "1L0K1", "11111", "X 10 3 3 4 4" });
		}

		[Test()]
		public void TestParser()
		{
			Assert.AreEqual(forest.Height, 4);
			Assert.AreEqual(forest.Width, 5);
			Assert.IsInstanceOf<WallObject>(forest.GetGameObject(new Position(0, 0)));
			Assert.IsInstanceOf<TrapObject>(forest.GetGameObject(new Position(2, 3)));
			Assert.AreEqual(forest.EnumerateForesters().Count(), 1);
			Assert.AreEqual(forest.EnumerateForesters().First().Name, "X");
		}
	}
}

