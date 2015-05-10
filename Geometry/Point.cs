using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
    public class Point
    {
	    private double x, y;

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

	    public static Point operator *(Point A, double k)
	    {
		    return new Point(A.x * k, A.y * k);
	    }

	    public static Point operator *(double k, Point A)
	    {
		    return new Point(A.x * k, A.y * k);
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

    }
}
