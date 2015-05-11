using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
    public class Point
    {
	    public readonly double x, y;

	    public Point(double x, double y)
	    {
		    this.x = x;
		    this.y = y;
	    }

		public static Point operator + (Point A, Point B)
		{
			return new Point(A.x + B.x, A.y + B.y);
		}

	    public static Point operator -(Point A, Point B)
	    {
		    return new Point(A.x - B.x, A.y - B.y);
	    }

	    public static Point operator -(Point A)
	    {
		    return new Point(-A.x, -A.y);
	    }

	    public static Point operator *(Point A, double k)
	    {
		    return new Point(A.x * k, A.y * k);
	    }

	    public static Point operator *(double k, Point A)
	    {
		    return new Point(A.x * k, A.y * k);
	    }

	    public static Point operator /(Point A, double k)
	    {
		    return new Point(A.x / k, A.y / k);
	    }

	    public static double DotProduct(Point A, Point B)
	    {
		    return A.x * B.x + A.y * B.y;
	    }

	    public double DotProductWith(Point B)
	    {
		    return DotProduct(this, B);
	    }

	    public static double CrossProduct(Point A, Point B)
	    {
		    return A.x * B.y - A.y * B.x;
	    }

	    public double CrossProductWith(Point B)
	    {
		    return CrossProduct(this, B);
	    }

	    public Point RotateAroundOrigin(double angle)
	    {
		    double sina = Math.Sin(angle);
		    double cosa = Math.Cos(angle);
			return new Point(x * cosa - y * sina, x * sina + y * cosa);
	    }

	    public Point RotateAroundPoint(Point center, double angle)
	    {
		    Point v = this - center;
		    return center + v.RotateAroundOrigin(angle);
	    }

	    public double Length
	    {
		    get { return Math.Sqrt(x * x + y * y); }
	    }

	    public Point SetLength(double length)
	    {
		    if (Length.IsEqual(0))
		    {
			    if (length.IsNotEqual(0))
					throw new ArgumentException("Try set zero length to non-zero vector");
			    return new Point(0, 0);
		    }
		    return this / Length * length;
	    }

	    public double GetAngle()
	    {
		    return Math.Atan2(y, x);
	    }

	    public double DistanceToPoint(Point P)
	    {
		    return (this - P).Length;
	    }

	    public double DistanceToLine(Line line)
	    {
		    return GeometryOperations.DistanceFromPointToLine(this, line);
	    }

	    public double DistanceToSegment(Segment segment)
	    {
		    return GeometryOperations.DistanceFromPointToSegment(this, segment);
	    }

	    public double DistanceToPolygon(ConvexPolygon polygon)
	    {
		    return GeometryOperations.DistanceFromPointToPolygon(this, polygon);
	    }

	    public Point ProjectToLine(Line line)
	    {
		    return GeometryOperations.ProjectPointOnLine(this, line);
	    }

	    public int Quarter
	    {
		    get
		    {
			    if (x.IsGreater(0) && y.IsGreaterOrEqual(0))
				    return 1;
			    if (x.IsLessOrEqual(0) && y.IsGreater(0))
				    return 2;
			    if (x.IsLess(0) && y.IsLessOrEqual(0))
				    return 3;
			    return 4;
		    }
	    }

	    public bool IsCollinear(Point v)
	    {
		    return CrossProductWith(v).IsEqual(0);
	    }
	
		public override string ToString()
		{
			return String.Format("Point({0} {1})", x, y);
		}

		protected bool Equals(Point other)
		{
			return x.IsEqual(other.x) && y.IsEqual(other.y);
		}

	    public override bool Equals(object obj)
	    {
		    if (ReferenceEquals(null, obj)) return false;
		    if (ReferenceEquals(this, obj)) return true;
		    if (obj.GetType() != this.GetType()) return false;
		    return Equals((Point) obj);
	    }
    }
}
