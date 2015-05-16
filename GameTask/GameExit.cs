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
	class GameExit : GameObject
	{
		private static readonly Image ExitImageMain = Image.FromFile("../../../pictures/exit.png");
		private static readonly Image ExitImageShadow = ExitImageMain.ChangeOpacity(0.2f);
		
		public const double Width = 40;
		public const double Height = 50;
		private Point corner;
		public GameExit(Point center) : base(Physics.Material.Adamantium, new Point(0, 0), true, 
			ConvexPolygon.Rectangle(center + new Point(0, Height / 2 + 1), Width, 2))
		{
			corner = center - new Point(Width / 2, Height / 2);
		}

		public Image Representation(GameWorld gameWorld)
		{
			return gameWorld.Type == WorldType.MainWorld ? ExitImageMain : ExitImageShadow;
		}

		public override void OnPaint(GameWorld gameWorld, Graphics graphics)
		{
			graphics.DrawImage(Representation(gameWorld),
				(float)corner.x, (float)corner.y, (float)Width, (float)Height);
		}
	}
}
