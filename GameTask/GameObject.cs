using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Geometry;
using NUnit.Framework.Constraints;
using Physics;
using Point = Geometry.Point;

namespace GameTask
{
	abstract class GameObject : PhysicalBody
	{
		protected GameObject(Material material, Point velocity, bool isStatic,
			ConvexPolygon shape) : base(material, velocity, isStatic, shape)
		{
			Layer = 0;
			Alive = true;
		}
		protected GameObject()
		{
			Layer = 0;
			Alive = true;
		}

		public abstract void OnPaint(GameWorld gameWorld, Graphics graphics);

		public virtual void OnPaint(GameWorld gameWorld, PaintEventArgs e)
		{
			OnPaint(gameWorld, e.Graphics);
		}

		public virtual void HandleCollision(Collision collision)
		{
			if (collision.penetration.IsGreaterOrEqual(10))
				Alive = false;
		}

		public int Layer { get; set; }
		public bool Alive { get; set; }
	}
}
