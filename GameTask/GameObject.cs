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
		protected GameObject(Material material, Point velocity, bool isStatic,
			ConvexPolygon shape) : base(material, velocity, isStatic, shape)
		{
			
		}
		protected GameObject()
	    {
		    
	    }
		public abstract void OnPaint(GameWorld sender, PaintEventArgs e);
		public int Layer { get; set; }
	}
}
