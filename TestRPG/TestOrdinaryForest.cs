using NUnit.Framework;
using System;
using RPG;
using System.Linq;

namespace TestRPG
{
	[TestFixture()]
	public class TestOrdinaryForest
	{
		Forest forest;
		bool invoked;

		[SetUp()]
		public void SetUp()
		{
			forest = new Forest(5, 5, 0);
			invoked = false;
			forest.OnChange += () => invoked = true;
		}

		[Test()]
		public void TestAddForester()
		{
			var forester = new Forester(0, "X", 10, new Position(0, 1), new StubAi(new Position(0, 1)));
			forest.AddForester(forester);

			var enumerated = forest.EnumerateForesters().ToList();
			Assert.AreEqual(enumerated.Count, 1);
			var insider = enumerated[0];
			Assert.AreEqual(forester.Name, insider.Name);
			Assert.AreEqual(forester.Health, insider.Health);
			Assert.AreEqual(forester.Position, insider.Position);

			Assert.IsTrue(invoked);
		}

		[Test()]
		public void TestAddGameObject()
		{
			var gameObject = new MedKitObject();
			forest.SetGameObject(new Position(1, 1), gameObject);
			Assert.AreEqual(forest.GetGameObject(new Position(1, 1)), gameObject);

			Assert.IsTrue(invoked);
		}

		[Test()]
		public void TestMoveForester()
		{
			var forester = new Forester(0, "X", 10, new Position(2, 2), new StubAi(new Position(2, 2)));
			forest.AddForester(forester);
			invoked = false;
			forest.MoveForester(forester.Id, Direction.Right);
			Assert.IsTrue(invoked);

			var position = forest.EnumerateForesters().First().Position;
			Assert.AreEqual(position, new Position(2, 3));
		}
	}
}

