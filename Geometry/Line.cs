using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
	public class Line
	{
		public readonly Point A, B;

		public Line(Point A, Point B)
		{
			this.A = A;
			this.B = B;
		}

		public Point Direction 
		{
			get { return B - A; }
		}

		public bool IsCollinear(Line line)
		{
			return Direction.IsCollinear(line.Direction);
		}

		public bool ContainsPoint(Point P)
		{
			return GeometryOperations.IsPointOnLine(P, this);
		}

		public Point IntersectWithLine(Line line)
		{
			return GeometryOperations.IntersectLines(this, line);
		}

		public Point IntersectWithSegment(Segment segment)
		{
			return GeometryOperations.IntersectSegmentLine(segment, this);
		}

		public Line RotateAroundOrigin(double angle)
		{
			return new Line(A.RotateAroundOrigin(angle),
							   B.RotateAroundOrigin(angle));
		}

		public Line RotateAroundPoint(Point center, double angle)
		{
			return new Line(A.RotateAroundPoint(center, angle),
							   B.RotateAroundPoint(center, angle));
		}

		public Line Move(Point direction)
		{
			return new Line(A + direction, B + direction);
		}

		public override string ToString()
		{
			return String.Format("Line(A:{0}, B:{1})", A, B);
		}

		protected bool Equals(Line other)
		{
			return Direction.CrossProductWith(other.Direction).IsEqual(0) &&
			       ContainsPoint(other.A);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((Line) obj);
		}
	}
}
