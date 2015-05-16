using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Geometry;
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
		}
		protected GameObject()
		{
			Layer = 0;
		}

		public abstract void OnPaint(GameWorld sender, Graphics graphics);

		public virtual void OnPaint(GameWorld sender, PaintEventArgs e)
		{
			OnPaint(sender, e.Graphics);
		}

		public virtual void OnDelete()
		{
			
		}

		public virtual void HandleCollision(Collision collision)
		{
			
		}

		public int Layer { get; set; }
		
	}
}
