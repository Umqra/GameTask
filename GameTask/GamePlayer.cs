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
		private const double PlayerWidth = 50;
		private const double PlayerHeight = 50;
		public GamePlayer(Point center) : base(Physics.Material.Wood, new Point(0, 0), false, null)
		{
			Point v = new Point(PlayerWidth / 2, PlayerHeight / 2);
			Shape = new ConvexPolygon(
				new[]
				{
					center + v, center + v.RotateAroundOrigin(Math.PI - 2 * v.GetAngle()),
					center - v, center - v.RotateAroundOrigin(Math.PI - 2 * v.GetAngle())
				});
		}
		public override void OnPaint(GameWorld gameWorld, PaintEventArgs e)
		{
			var graphics = e.Graphics;
			graphics.DrawImage(Image.FromFile("../../../pictures/player.png"),
				(float)Shape[0].x, (float)Shape[0].y, (float)PlayerWidth, (float)PlayerHeight);
		}
	}
}
