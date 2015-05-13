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
		private static Random random = new Random();
		const double Size = 70;
		private int type;
		public GameBox(Point center, double mass, double friction)
			: base(Physics.Material.Wood, new Point(0, 0), friction, false, null)
		{
			type = random.Next(1, 4);
			Point v = new Point(Size / 2, Size / 2);
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
			graphics.DrawImage(Image.FromFile(String.Format("../../../box{0}.png", type)), 
				(float)Shape[0].x, (float)Shape[0].y, (float)Size, (float)Size);
		}

	}
}
