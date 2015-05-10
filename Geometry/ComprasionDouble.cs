using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
	public static class ComprasionDouble
	{
		public const double Epsilon = 1e-6;

		public static int Signum(double a)
		{
			if (a.IsLess(0))
				return -1;
			else if (a.IsEqual(0))
				return 0;
			else
				return 1;
		}

		public static bool AreEqual(double a, double b, double eps)
		{
			return Math.Abs(a - b) < eps;
		}
		public static bool AreEqual(double a, double b)
		{
			return AreEqual(a, b, Epsilon);
		}

		public static bool IsEqual(this double a, double b, double eps)
		{
			return AreEqual(a, b, eps);
		}
		public static bool IsEqual(this double a, double b)
		{
			return AreEqual(a, b);
		}

		public static bool IsLess(this double a, double b, double eps)
		{
			return a < b && !AreEqual(a, b);
		}

		public static bool IsLess(this double a, double b)
		{
			return IsLess(a, b, Epsilon);
		}

		public static bool IsGreater(this double a, double b, double eps)
		{
			return a > b && !AreEqual(a, b);
		}

		public static bool IsGreater(this double a, double b)
		{
			return IsGreater(a, b, Epsilon);
		}

		public static bool AreNotEqual(double a, double b, double eps)
		{
			return !AreEqual(a, b, eps);
		}

		public static bool AreNotEqual(double a, double b)
		{
			return AreNotEqual(a, b, Epsilon);
		}

		public static bool IsNotEqual(this double a, double b, double eps)
		{
			return AreNotEqual(a, b, eps);
		}

		public static bool IsNotEqual(this double a, double b)
		{
			return AreNotEqual(a, b);
		}

		public static bool IsLessOrEqual(this double a, double b, double eps)
		{
			return IsLess(a, b, eps) || IsEqual(a, b, eps);
		}
		public static bool IsLessOrEqual(this double a, double b)
		{
			return IsLess(a, b) || IsEqual(a, b);
		}

		public static bool IsGreaterOrEqual(this double a, double b, double eps)
		{
			return IsGreater(a, b, eps) || IsEqual(a, b, eps);
		}
		public static bool IsGreaterOrEqual(this double a, double b)
		{
			return IsGreater(a, b) || IsEqual(a, b);
		}
	}
}
