using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
	public class ConvexPolygon : IEnumerable<Point>
	{
		private List<Point> points;

		public int Count
		{
			get { return points == null ? 0 : points.Count; }
		}

		public ConvexPolygon(List<Point> points)
		{
			this.points = GeometryOperations.BuildConvexHull(points);
		}

		public ConvexPolygon(params Point[] points)
		{
			this.points = GeometryOperations.BuildConvexHull(points.ToList());
		}

		public Point this[int index]
		{
			get { return points[index]; }
		}

		public bool ContainsPoint(Point P)
		{
			return GeometryOperations.IsPointInPolygon(P, this);
		}

		public double DistanceToPoint(Point P)
		{
			return GeometryOperations.DistanceFromPointToPolygon(P, this);
		}

		double GetArea()
		{
			double area = 0;
			for (int i = 1; i < Count - 1; i++)
			{
				area += (points[i] - points[0]).CrossProductWith(points[i + 1] - points[0]);
			}
			return Math.Abs(area) / 2;
		}

		public IEnumerator<Point> GetEnumerator()
		{
			foreach (var point in points)
				yield return point;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public ConvexPolygon RotateAroundOrigin(double angle)
		{
			return new ConvexPolygon(
				this.Select(p => p.RotateAroundOrigin(angle)).ToList()
				);
		}

		public ConvexPolygon RotateAroundPoint(Point center, double angle)
		{
			return new ConvexPolygon(
				this.Select(p => p.RotateAroundPoint(center, angle)).ToList()
				);
		}

		public ConvexPolygon Move(Point direction)
		{
			return new ConvexPolygon(
				this.Select(p => p + direction).ToList()
				);
		}

		public ConvexPolygon IntersectWithPolygon(ConvexPolygon other)
		{
			return GeometryOperations.IntersectPolygons(this, other);
		}

		public Point GetCenterOfMass()
		{
			var centerOfMass = new Point(0, 0);
			double mass = 0;
			for (int i = 1; i < (int) points.Count - 1; i++)
			{
				var A = points[0];
				var B = points[i];
				var C = points[i + 1];
				var M = (A + B + C) / 3;
				var currentMass = new ConvexPolygon(A, B, C).GetArea();
				centerOfMass += M * currentMass;
				mass += currentMass;
			}
			return centerOfMass / mass;
		}
	}
}
