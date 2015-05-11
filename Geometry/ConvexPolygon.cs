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

		public Point this[int index]
		{
			get { return points[index]; }
		}

		public bool ContainsPoint(Point P)
		{
			return GeometryOperations.IsPointInPolygon(P, this);
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
	}
}
