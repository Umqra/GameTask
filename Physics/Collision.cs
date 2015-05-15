using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics
{
	public class Collision
	{
		public readonly PhysicalBody a, b;
		public double penetration;

		public Collision(PhysicalBody a, PhysicalBody b, double p)
		{
			this.a = a;
			this.b = b;
			penetration = p;
		}
	}
}
