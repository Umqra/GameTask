using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawingExtensions;
using Geometry;
using Point = Geometry.Point;

namespace GameTask
{
	class IceWall : GameObject
	{
		private static readonly Image IceImageMain = Image.FromFile("../../../pictures/ice.png");
		private static readonly Image IceImageShadow = IceImageMain.ChangeOpacity(0.2f);
		
		protected double width, height;

		public IceWall(Point center, double width, double height)
			: base(Physics.Material.Ice, new Point(0, 0), true, null)
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
			return gameWorld.Type == WorldType.MainWorld ? IceImageMain : IceImageShadow;
		}

		public override void OnPaint(GameWorld gameWorld, Graphics graphics)
		{
			graphics.DrawImage(Representation(gameWorld),
				(float)Shape[0].x, (float)Shape[0].y, (float)width, (float)height);
		}
	}
}
