using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DrawingExtensions;
using Geometry;
using Point = Geometry.Point;

namespace GameTask
{
	class Ground : GameObject
	{
		private static readonly Image[] GroundImagesMain =
		{
			Image.FromFile("../../../pictures/ground1.png"),
			Image.FromFile("../../../pictures/ground2.png"),
			Image.FromFile("../../../pictures/ground3.png")
		};

		private static readonly Image[] GroundImagesShadow =
		{
			Image.FromFile("../../../pictures/ground1.png").ChangeOpacity(0.2f),
			Image.FromFile("../../../pictures/ground2.png").ChangeOpacity(0.2f),
			Image.FromFile("../../../pictures/ground3.png").ChangeOpacity(0.2f)
		};

		private static Random random = new Random(1);
		private double width, height;
		private int type;
		public Ground(Point center, double width, double height)
			: base(Physics.Material.Grass, new Point(0, 0), true, null)
		{
			type = random.Next(3);
			this.width = width;
			this.height = height;
			var v = new Point(width / 2, height / 2);
			var points = new[]
			{
				center + v, center + v.RotateAroundOrigin(Math.PI / 2),
				center - v, center - v.RotateAroundOrigin(Math.PI / 2)
			};
			Shape = new ConvexPolygon(points);
		}

		public Image Representation(GameWorld gameWorld)
		{
			return gameWorld.Type == WorldType.MainWorld ? GroundImagesMain[type] : GroundImagesShadow[type];
		}

		public override void OnPaint(GameWorld gameWorld, Graphics graphics)
		{
			graphics.DrawImage(Representation(gameWorld),
				(float)Shape[0].x, (float)Shape[0].y - 10, (float)width, (float)height + 20);
		}
	}
}
