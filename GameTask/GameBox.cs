using System;
using System.Collections.Generic;
using System.Drawing;
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
	class GameBox : GameObject
	{
		private static Random random = new Random();
		private static readonly Image[] BoxImagesMain =
		{
			Image.FromFile("../../../pictures/box1.png"),
			Image.FromFile("../../../pictures/box2.png"),
			Image.FromFile("../../../pictures/box3.png"),
		};

		private static readonly Image[] BoxImagesShadow =
		{
			Image.FromFile("../../../pictures/box1.png").ChangeOpacity(0.2f),
			Image.FromFile("../../../pictures/box2.png").ChangeOpacity(0.2f),
			Image.FromFile("../../../pictures/box3.png").ChangeOpacity(0.2f)
		};

		const double Size = 50;
		private int type;
		public GameBox(Point center)
			: base(Physics.Material.Wood, new Point(0, 0), false, null)
		{
			type = random.Next(0, 3);
			Point v = new Point(Size / 2, Size / 2);
			var points = new[]
			{
				center + v, center + v.RotateAroundOrigin(Math.PI / 2),
				center - v, center - v.RotateAroundOrigin(Math.PI / 2)
			};
			Shape = new ConvexPolygon(points);
		}

		public Image Representation(GameWorld gameWorld)
		{
			return gameWorld.Type == WorldType.MainWorld ? BoxImagesMain[type] : BoxImagesShadow[type];
		}

		public override void OnPaint(GameWorld gameWorld, PaintEventArgs e)
		{
			float opacity = gameWorld.Type == WorldType.MainWorld ? 1.0f : 0.2f;
			var graphics = e.Graphics;
			graphics.DrawImage(Representation(gameWorld), 
				(float)Shape[0].x, (float)Shape[0].y, (float)Size, (float)Size);
		}

	}
}
