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
	class GameWall : GameObject
	{
		public GameWall(Point[] points, double friction)
			: base(1, new Point(0, 0), friction, true, null, null)
		{
			Shape = new ConvexPolygon(points);
			CenterOfMass = Shape.GetCenterOfMass();
		}

		public override void OnPaint(object sender, PaintEventArgs e)
		{
			var graphics = e.Graphics;
			graphics.FillPolygon(Brushes.Black,
				Shape.Select(p => new PointF((float)p.x, (float)p.y)).ToArray()
				);
		}
	}
}
