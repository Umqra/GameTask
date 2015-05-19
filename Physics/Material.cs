using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics
{
	public class Material
	{
		public readonly double density;
		public readonly double elasticity;
		public readonly double frictionCoefficient;

		public static readonly Material Rock = new Material(0.6, 0.1, 0.6);
		public static readonly Material Wood = new Material(0.3, 0.2, 0.6);
		public static readonly Material Metal = new Material(1.2, 0.05, 0.1);
		public static readonly Material BouncyBall = new Material(0.3, 0.8, 0.3);
		public static readonly Material Pillow = new Material(0.1, 0.2, 0.6);
		public static readonly Material Grass = new Material(0.1, 0.1, 0.5);
		public static readonly Material Adamantium = new Material(2, 0.1, 5);
		public static readonly Material Ice = new Material(0.1, 0.1, 0);

		public Material(double density, double elasticity, double friction)
		{
			this.density = density;
			this.elasticity = elasticity;
			this.frictionCoefficient = friction;
		}
	}
}
