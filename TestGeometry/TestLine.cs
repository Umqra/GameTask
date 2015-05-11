using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry;
using NUnit.Framework;

namespace TestGeometry
{
	// ReSharper disable InconsistentNaming
	[TestFixture]
	internal class TestLine
	{
		[Test]
		public void TestEqualLines()
		{
			var A = new Point(0, 0);
			var B = new Point(5, 2);
			var line1 = new Line(A, B);

			var C = new Point(2.5, 1);
			var D = new Point(-5, -2);
			var line2 = new Line(C, D);

			Assert.AreEqual(line1, line2);
		}

		[Test]
		public void TestNotEqualLines()
		{
			var A = new Point(0, 0);
			var B = new Point(5, 2);
			var line1 = new Line(A, B);

			var C = new Point(1, 1);
			var D = new Point(6, 3);
			var line2 = new Line(C, D);

			Assert.AreNotEqual(line1, line2);
		}

		[Test]
		public void TestDirection()
		{
			var A = new Point(0, -3);
			var B = new Point(5, -7);
			var line = new Line(A, B);
			Assert.IsTrue(line.Direction.IsCollinear(B - A));
		}

		[Test]
		public void TestCollinear()
		{
			var A = new Point(0, -3);
			var B = new Point(5, -7);
			var C = new Point(2, 0);
			var D = new Point(12, -8);
			Assert.IsTrue(new Line(A, B).IsCollinear(new Line(C, D)));
			Assert.IsFalse(new Line(A, C).IsCollinear(new Line(B, D)));
		}

		[Test]
		public void TestContainsPoint()
		{
			var A = new Point(0, 0);
			var B = new Point(4, -7);
			var line = new Line(A, B);
			var P1 = new Point(0, 0);
			var P2 = new Point(-4, 7);
			var P3 = new Point(1, -7.0 / 4);

			var Q1 = new Point(1, 1);
			var Q2 = new Point(4, 7);
			Assert.IsTrue(line.ContainsPoint(P1));
			Assert.IsTrue(line.ContainsPoint(P2));
			Assert.IsTrue(line.ContainsPoint(P3));

			Assert.IsFalse(line.ContainsPoint(Q1));
			Assert.IsFalse(line.ContainsPoint(Q2));
		}

		[Test]
		public void TestIntersectWithLineCoincide()
		{
			var A = new Point(1, 3);
			var B = new Point(-2, 0);
			var line1 = new Line(A, B);

			var C = new Point(4, 6);
			var D = new Point(5, 7);
			var line2 = new Line(C, D);
			Assert.IsNull(line1.IntersectWithLine(line2));
		}

		[Test]
		public void TestIntersectWithLineParallel()
		{
			var A = new Point(1, 3);
			var B = new Point(-2, 1);
			var line1 = new Line(A, B);

			var C = new Point(0, 10);
			var D = new Point(3, 12);
			var line2 = new Line(C, D);
			Assert.IsNull(line1.IntersectWithLine(line2));
		}

		[Test]
		public void TestIntersectWithLine1()
		{
			var A = new Point(0, 0);
			var B = new Point(2, 2);
			var line1 = new Line(A, B);

			var C = new Point(0, 1);
			var D = new Point(2, -1);
			var line2 = new Line(C, D);

			var expected = new Point(0.5, 0.5);

			Assert.AreEqual(expected, line1.IntersectWithLine(line2));
		}

		[Test]
		public void TestIntersectWithLine2()
		{
			var A = new Point(0, 0);
			var B = new Point(2, 1);
			var line1 = new Line(A, B);

			var C = new Point(1, 0);
			var D = new Point(2, 2);
			var line2 = new Line(C, D);

			var expected = new Point(4.0 / 3, 2.0 / 3);

			Assert.AreEqual(expected, line1.IntersectWithLine(line2));
		}


		[Test]
		public void TestIntersectWithSegment1()
		{
			var A = new Point(0, 0);
			var B = new Point(2, 1);
			var line1 = new Line(A, B);

			var C = new Point(1, 0);
			var D = new Point(2, -2);
			var segment2 = new Segment(C, D);

			Assert.IsNull(line1.IntersectWithSegment(segment2));
		}

		[Test]
		public void TestIntersectWithSegment2()
		{
			var A = new Point(0, 1);
			var B = new Point(2, 1);
			var line1 = new Line(A, B);

			var C = new Point(3, 10);
			var D = new Point(5, 1);
			var segment2 = new Segment(C, D);

			Assert.AreEqual(D, line1.IntersectWithSegment(segment2));
		}

		[Test]
		public void TestRotatingAroundOrigin()
		{
			var A = new Point(0, 0);
			var B = new Point(2, 1);
			var line1 = new Line(A, B);

			var expectedPiDiv2 = new Line(new Point(0, 0), new Point(-1, 2));
			Assert.AreEqual(expectedPiDiv2, line1.RotateAroundOrigin(Math.PI / 2));

			var expectedPi = new Line(new Point(0, 0), new Point(-2, -1));
			Assert.AreEqual(expectedPi, line1.RotateAroundOrigin(Math.PI));
		}

		[Test]
		public void TestRotatingAroundPoint()
		{
			var A = new Point(0, 0);
			var B = new Point(2, 1);
			var line1 = new Line(A, B);

			var center = new Point(2, 2);
			var C = new Point(3, 2);
			var D = new Point(4, 0);
			var expected = new Line(C, D);

			Assert.AreEqual(expected, line1.RotateAroundPoint(center, Math.PI / 2));
		}

		[Test]
		public void TestMove()
		{
			var A = new Point(0, 0);
			var B = new Point(2, 1);
			var line = new Line(A, B);

			var move = new Point(-1, 3);
			var C = A + move;
			var D = B + move;
			var expected = new Line(C, D);

			Assert.AreEqual(expected, line.Move(move));
		}
	}
	// ReSharper restore InconsistentNaming
}
