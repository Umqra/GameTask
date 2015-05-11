using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry;

namespace Physics
{
	static class PhysicalWorld
	{
		static public bool IsBodiesContact(PhysicalBody a, PhysicalBody b)
		{
			return a.shape.IntersectWithPolygon(b.shape).Count > 0;
		}

		static public void CorrectBodyCollision(PhysicalBody a, PhysicalBody b)
		{
			
		}
	}
}
