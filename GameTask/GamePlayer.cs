using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Geometry;
using Point = Geometry.Point;

namespace GameTask
{
	class GamePlayer : GameObject
	{
		private const double PlayerWidth = 10;
		private const double PlayerHeight = 20;
		public GamePlayer(Point center) : base(Physics.Material.Wood, new Point(0, 0), 1, false, null)
		{
			Point v = new Point(PlayerWidth, PlayerHeight);
			Shape = new ConvexPolygon(
				new[]
				{
					center + v, center + v.RotateAroundOrigin(Math.PI - 2 * v.GetAngle()),
					center - v, center - v.RotateAroundOrigin(Math.PI - 2 * v.GetAngle())
				});
		}
		public override void OnPaint(object sender, PaintEventArgs e)
		{
			var graphics = e.Graphics;
			graphics.FillPolygon(Brushes.MediumAquamarine,
				Shape.Select(p => new PointF((float)p.x, (float)p.y)).ToArray()
				);
		}
	}
}
