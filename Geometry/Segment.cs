using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
	public class Segment
	{
		public readonly Point A, B;

		public Segment(Point A, Point B)
		{
			this.A = A;
			this.B = B;
		}

		public Line BaseLine
		{
			get { return new Line(A, B); }
		}

		public double Length
		{
			get { return (A - B).Length; }
		}

		public bool ContainsPoint(Point P)
		{
			return GeometryOperations.IsPointOnSegment(P, this);
		}

		public double DistanceToPoint(Point P)
		{
			return GeometryOperations.DistanceFromPointToSegment(P, this);
		}

		public Point IntersectWithLine(Line line)
		{
			return GeometryOperations.IntersectSegmentLine(this, line);
		}

		public Segment IntersectWithSegment(Segment segment)
		{
			return GeometryOperations.IntersectSegments(this, segment);
		}

		public Segment RotateAroundOrigin(double angle)
		{
			return new Segment(A.RotateAroundOrigin(angle),
							   B.RotateAroundOrigin(angle));
		}

		public Segment RotateAroundPoint(Point center, double angle)
		{
			return new Segment(A.RotateAroundPoint(center, angle),
							   B.RotateAroundPoint(center, angle));
		}

		public Segment Move(Point direction)
		{
			return new Segment(A + direction, B + direction);
		}

		protected bool Equals(Segment other)
		{
			return (Equals(A, other.A) && Equals(B, other.B)) ||
			       (Equals(A, other.B) && Equals(B, other.A));
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((Segment)obj);
		}
	}
}
