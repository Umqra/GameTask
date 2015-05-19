using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DrawingExtensions;
using Geometry;
using Physics;
using Point = Geometry.Point;
	
namespace GameTask
{
	class Wall : GameObject
	{
		private static readonly Image WallImageMain = Image.FromFile("../../../pictures/wall1.png");
		private static readonly Image WallImageShadow = WallImageMain.ChangeOpacity(0.2f);
		private double width, height;

		public Wall(Point center, double width, double height)
			: base(Physics.Material.Rock, new Point(0, 0), true, null)
		{
			this.width = width;
			this.height = height;
			Point v = new Point(width / 2, height / 2);
			var points = new[]
			{
				center + v, center + v.RotateAroundOrigin(Math.PI / 2),
				center - v, center - v.RotateAroundOrigin(Math.PI / 2)
			};
			Shape = new ConvexPolygon(points);
		}

		public Image Representation(GameWorld gameWorld)
		{
			return gameWorld.Type == WorldType.MainWorld ? WallImageMain : WallImageShadow;
		}

		public override void OnPaint(GameWorld gameWorld, Graphics graphics)
		{
			graphics.DrawImage(Representation(gameWorld),
				(float)Shape[0].x, (float)Shape[0].y, (float)width, (float)height);
		}
	}
}
