using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Geometry;
using Physics;

namespace GameTask
{
	abstract class GameObject : PhysicalBody, IDrawable
	{
		protected GameObject(Material material, Point velocity, double friction, bool isStatic,
			ConvexPolygon shape) : base(material, velocity, friction, isStatic, shape)
		{
			
		}
		protected GameObject()
	    {
		    
	    }
		public abstract void OnPaint(object sender, PaintEventArgs e);
	}
}
