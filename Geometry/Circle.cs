using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
	class Circle
	{
		public readonly Point center;
		public readonly double radius;

		public Circle(Point center, double radius)
		{
			this.center = center;
			this.radius = radius;
		}

		public bool ContainsPoint(Point P)
		{
			return (center - P).Length.IsLessOrEqual(radius);
		}

		public Segment IntersectWithLine(Line line)
		{
			return GeometryOperations.IntersectCircleLine(this, line);
		}

		public Segment IntersectWithSegment(Segment segment)
		{
			return GeometryOperations.IntersectCircleSegment(this, segment);
		}

		protected bool Equals(Circle other)
		{
			return Equals(center, other.center) && radius.IsEqual(other.radius);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((Circle)obj);
		}
	}
}
