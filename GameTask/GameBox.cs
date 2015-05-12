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
	class GameBox : GameObject
	{
		public GameBox(Point center, double size, double mass, double friction)
			: base(mass, new Point(0, 0), friction, false, null)
		{
			Point v = new Point(size / 2, size / 2);
			var points = new[]
			{
				center + v, center + v.RotateAroundOrigin(Math.PI / 2),
				center - v, center - v.RotateAroundOrigin(Math.PI / 2)
			};
			Shape = new ConvexPolygon(points);
		}

		public override void OnPaint(object sender, PaintEventArgs e)
		{
			var graphics = e.Graphics;
			graphics.FillPolygon(Brushes.BurlyWood, 
				Shape.Select(p => new PointF((float)p.x, (float)p.y)).ToArray()
				);
		}

	}
}
