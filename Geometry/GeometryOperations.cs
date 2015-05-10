using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
	class GeometryOperations
	{
		// ReSharper disable InconsistentNaming
		public static bool IsPointOnLine(Point P, Line line)
		{
			return (line.A - P).CrossProductWith(line.B - P).IsEqual(0);
		}

		public static bool IsPointOnSegment(Point P, Segment segment)
		{
			return (segment.A - P).CrossProductWith(segment.B - P).IsEqual(0) &&
			       (segment.A - P).DotProductWith(segment.B - P).IsLessOrEqual(0);
		}

		public static Point ProjectPointOnLine(Point P, Line line)
		{
			var AP = P - line.A;
			var AB = line.Direction;
			return line.A + (AP.DotProductWith(AB) * AB) / (AB.DotProductWith(AB));
		}

		public static double DistanceFromPointToLine(Point P, Line line)
		{
			return ProjectPointOnLine(P, line).DistanceTo(P);
		}

		public static Point IntersectLines(Line a, Line b)
		{
			if (a.Direction.CrossProductWith(b.Direction).IsEqual(0))
				return null;
			var v = a.Direction;
			var u = b.Direction;
			var k = (a.A.CrossProductWith(v) - b.A.CrossProductWith(v)) / u.CrossProductWith(v);
			return b.B + u * k;
		}

		public static Point IntersectSegmentLine(Segment a, Line b)
		{
			var intersection = IntersectLines(a.BaseLine, b);
			if (intersection == null || a.ContainsPoint(intersection))
				return null;
			return intersection;
		}

		private static Segment IntersectSegmentsOnLine(Segment a, Segment b)
		{
			var v = a.BaseLine.Direction.SetLength(1);
			// ReSharper disable once ConvertToConstant.Local
			var al = 0;
			var ar = (a.B - a.A).DotProductWith(v);
			var bl = (b.A - a.A).DotProductWith(v);
			var br = (b.B - a.A).DotProductWith(v);

			var l = Math.Max(Math.Min(al, ar), Math.Min(bl, br));
			var r = Math.Min(Math.Max(al, ar), Math.Max(bl, br));
			return l.IsGreater(r) ? null : new Segment(a.A + v * l, a.A + v * r);
		}

		public static Segment IntersectSegments(Segment a, Segment b)
		{
			var intersection = IntersectLines(a.BaseLine, b.BaseLine);
			if (a.BaseLine.IsCollinear(b.BaseLine))
			{
				if (!a.BaseLine.ContainsPoint(b.A))
					return null;
				return IntersectSegmentsOnLine(a, b);
			}
			return new Segment(intersection, intersection);
		}

		public static Segment IntersectCircleLine(Circle circle, Line line)
		{
			var H = circle.center.ProjectToLine(line);
			if (!circle.ContainsPoint(H))
				return null;
			Point v = line.Direction.SetLength(1);
			double h = (circle.center - H).Length;
			double d = Math.Sqrt(circle.radius * circle.radius - h * h);
			return new Segment(H + v * d, H - v * d);
		}

		public static Segment IntersectCircleSegment(Circle circle, Segment segment)
		{
			Segment intersection = IntersectCircleLine(circle, segment.BaseLine);
			return IntersectSegments(intersection, segment);
		}

		class CompareByAngle : IComparer<Point>
		{
			private Point origin;

			public CompareByAngle(Point origin)
			{
				this.origin = origin;
			}

			public int Compare(Point x, Point y)
			{
				if (Equals(x, y))
					return 0;
				Point v = x - origin;
				Point u = y - origin;
				if (v.Quarter != u.Quarter)
					return v.Quarter < u.Quarter ? -1 : 1;
				if (v.CrossProductWith(u).IsNotEqual(0))
					return v.CrossProductWith(u).IsLess(0) ? -1 : 1;
				return v.Length < u.Length ? -1 : 1;
			}
		}

		public static List<Point> BuildConvexHull(List<Point> points)
		{
			var sortedPoint = points.ToList();
			sortedPoint.Sort((a, b) =>
			{
				if (Equals(a, b))
					return 0;
				return a.x.IsLess(b.x) || (a.x.IsEqual(b.x) && a.y.IsLess(b.y)) ? -1 : 1;
			});
			var leftBottom = new Point(sortedPoint[0].x, sortedPoint[0].y);
			sortedPoint = sortedPoint.Distinct().OrderBy(x => x, new CompareByAngle(leftBottom)).ToList();
			var convexHull = new List<Point>();
			foreach (var point in sortedPoint)
			{
				while (convexHull.Count > 1)
				{
					int sz = convexHull.Count;
					if ((convexHull[sz - 1] - convexHull[sz - 2]).CrossProductWith(
						point - convexHull[sz - 2]).IsLessOrEqual(0))
						convexHull.RemoveAt(sz - 1);
					else
						break;
				}
				convexHull.Add(point);
			}
			return convexHull;
		}

		public static bool IsPointInPolygon(Point P, ConvexPolygon polygon)
		{
			for (int i = 0; i < polygon.Count; i++)
			{
				Point A = polygon[i];
				Point B = polygon[(i + 1) % polygon.Count];
				if ((B - A).CrossProductWith(P).IsLess(0))
					return false;
			}
			return true;
		}

		public static ConvexPolygon IntersectPolygons(ConvexPolygon a, ConvexPolygon b)
		{
			var interestingPoints = new List<Point>().Concat(a).Concat(b).ToList();
			for (int i = 0; i < a.Count; i++)
			{
				Point A = a[i], B = a[(i + 1) % a.Count];
				for (int s = 0; s < b.Count; s++)
				{
					Point C = b[s], D = b[(s + 1) % b.Count];
					Segment intersection = IntersectSegments(new Segment(A, B), new Segment(C, D));
					if (intersection != null)
					{
						interestingPoints.Add(intersection.A);
						interestingPoints.Add(intersection.B);
					}
				}
			}
			interestingPoints = interestingPoints.Where(p => a.ContainsPoint(p) && b.ContainsPoint(p)).ToList();
			return new ConvexPolygon(interestingPoints);
		}
	}
	// ReSharper restore InconsistentNaming
}
