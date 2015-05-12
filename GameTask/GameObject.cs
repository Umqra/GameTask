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
		protected GameObject(double mass, Point velocity, double friction, bool isStatic,
			ConvexPolygon shape, Point centerOfMass) : base(mass, velocity, friction, isStatic, shape, centerOfMass)
		{
			
		}
		protected GameObject()
	    {
		    
	    }
		public abstract void OnPaint(object sender, PaintEventArgs e);
	}
}
