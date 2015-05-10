using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;

namespace Geometry
{
	// ReSharper disable InconsistentNaming
	[TestFixture]
	internal class TestPoint
	{
		private static Random random = new Random(1);

		public double GetRandomCoordinate(double minC, double maxC)
		{
			return minC + random.NextDouble() * (maxC - minC);
		}

		public Point GetRandomPoint(double minC, double maxC)
		{
			return new Point(GetRandomCoordinate(minC, maxC),
				GetRandomCoordinate(minC, maxC));
		}

		[Test]
		public void TestEqualNearPoints()
		{
			var A = new Point(1, 2);
			var B = new Point(1 - 1e-10, 2);
			Assert.AreEqual(A, B);

			var C = new Point(10, -100);
			var D = new Point(10, -100.0001);
			Assert.AreNotEqual(C, D);
		}

		[Test]
		public void TestNotEqual()
		{
			var A = new Point(0, 0);
			var B = new Point(1, 20);
			Assert.AreNotEqual(A, B);

			var C = new Point(-100, -100);
			var D = new Point(100, 100);
			Assert.AreNotEqual(C, D);
		}

		[Test]
		public void TestPlusOperator()
		{
			var A = new Point(-10, 2.5);
			var B = new Point(3, 0.1);
			var expected = new Point(-7, 2.6);
			Assert.AreEqual(expected, A + B);
		}

		[Test]
		public void TestMinusOperator()
		{
			var A = new Point(123, -11);
			var B = new Point(10, -4);
			var expected = new Point(113, -7);
			Assert.AreEqual(expected, A - B);
		}

		[Test]
		public void TestMultiplyOperator()
		{
			var A = new Point(12, -3);
			var expectedMult = new Point(12 * -2.0, -3.0 * -2);
			var expectedDiv = new Point(12 / 5.0, -3 / 5.0);
			Assert.AreEqual(expectedMult, A * -2);
			Assert.AreEqual(expectedDiv, A / 5);
		}

		[Test]
		public void TestDotProductOrthogonal()
		{
			var A = new Point(14, 16);
			var B = new Point(-4, 3.5);
			Assert.AreEqual(0, A.DotProductWith(B));
		}

		[Test]
		public void TestDotProductRandom()
		{
			var A = new Point(13, -4);
			var B = new Point(-3, 22);
			Assert.AreEqual(-127, A.DotProductWith(B));

			var C = new Point(3, 2);
			var D = new Point(-2, 5);
			Assert.AreEqual(4, C.DotProductWith(D));
		}

		[Test]
		public void TestDotProductCommutative()
		{
			for (int test = 0; test < 100; test++)
			{
				var A = GetRandomPoint(-10, 10);
				var B = GetRandomPoint(-10, 10);
				Assert.AreEqual(A.DotProductWith(B), B.DotProductWith(A));
			}
		}

		[Test]
		public void TestCrossProductCollinear()
		{
			var A = new Point(7, 6);
			var B = new Point(-3.5, -3);
			Assert.AreEqual(0, A.CrossProductWith(B));
		}

		[Test]
		public void TestCrossProductRandom()
		{
			var A = new Point(4, 7);
			var B = new Point(-11, 3);
			Assert.AreEqual(89, A.CrossProductWith(B));
		}

		[Test]
		public void TestCrossProductAntiCommutative()
		{
			for (int test = 0; test < 100; test++)
			{
				var A = GetRandomPoint(-10, 10);
				var B = GetRandomPoint(-10, 10);
				Assert.AreEqual(A.CrossProductWith(B), -B.CrossProductWith(A));
			}
		}

		[Test]
		public void TestRotateAroundOrigin()
		{
			var A = new Point(3, 4);
			var expectedPiDiv2 = new Point(-4, 3);
			var expectedPi = new Point(-3, -4);
			Assert.AreEqual(expectedPiDiv2, A.RotateAroundOrigin(Math.PI / 2));
			Assert.AreEqual(-expectedPiDiv2, A.RotateAroundOrigin(-Math.PI / 2));
			Assert.AreEqual(expectedPi, A.RotateAroundOrigin(Math.PI));

			var B = new Point(5, 5);
			var expectedPiDiv4 = new Point(0, 5 * Math.Sqrt(2));
			Assert.AreEqual(expectedPiDiv4, B.RotateAroundOrigin(Math.PI / 4));
		}

		[Test]
		public void TestRotateAroundPoint()
		{
			var A = new Point(7, 4);
			var center = new Point(5, 2);
			var expected = new Point(5, 2 + 2 * Math.Sqrt(2));
			Assert.AreEqual(expected, A.RotateAroundPoint(center, Math.PI / 4));
		}

		[Test]
		public void TestLength()
		{
			var A = new Point(-1, 4);
			Assert.IsTrue(Math.Sqrt(17).IsEqual(A.Length));
		}

		[Test]
		public void TestGetAngle()
		{
			var A = new Point(-1, 0);
			Assert.IsTrue(Math.PI.IsEqual(A.GetAngle()));

			var B = new Point(-1e10, -1e-5);
			Assert.IsTrue((-Math.PI).IsEqual(B.GetAngle()));

			var C = new Point(3, 4);
			Assert.IsTrue(Math.Asin(4.0 / 5).IsEqual(C.GetAngle()));

			var D = new Point(1, 0);
			Assert.IsTrue(0.0.IsEqual(D.GetAngle()));

			var E = new Point(0, 0);
			Assert.IsTrue(0.0.IsEqual(E.GetAngle()));
		}

		[Test]
		public void TestDistanceTo()
		{
			var A = new Point(10, -3);
			var B = new Point(7, 1);
			Assert.IsTrue(5.0.IsEqual(A.DistanceTo(B)));
		}

		[Test]
		public void TestQuerter()
		{
			var testPoint = new[]
			{
				new Point(1, 0),
				new Point(1, 1),
				new Point(0, 1),
				new Point(-1, 1),
				new Point(-1, 0),
				new Point(-1, -1),
				new Point(0, -1),
				new Point(1, -1)
			};
			var expectedResult = new[]
			{
				1, 1, 2, 2, 3, 3, 4, 4
			};
			for (int i = 0; i < 8; i++)
				Assert.AreEqual(expectedResult[i], testPoint[i].Quarter);
		}
	}

	// ReSharper restore InconsistentNaming
}
