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
	class GamePlayer : GameObject
	{
		private static readonly Image PlayerImageMain = Image.FromFile("../../../pictures/player.png");
		private static readonly Image PlayerImageShadow = PlayerImageMain.ChangeOpacity(0.2f);
		
		public bool Alive { get; set; }
		private const double Width = 50;
		private const double Height = 50;
		public GamePlayer(Point center) : base(Physics.Material.Wood, new Point(0, 0), false, null)
		{
			Alive = true;
			Shape = ConvexPolygon.Rectangle(center, Width, Height);
		}

		public override void HandleCollision(Collision collision)
		{
			if (collision.penetration.IsGreaterOrEqual(10))
				Alive = false;
		}

		public Image Representation(GameWorld gameWorld)
		{
			return gameWorld.Type == WorldType.MainWorld ? PlayerImageMain : PlayerImageShadow;
		}

		public override void OnPaint(GameWorld gameWorld, Graphics graphics)
		{
			graphics.DrawImage(Representation(gameWorld),
				(float)Shape[0].x, (float)Shape[0].y, (float)Width, (float)Height);
		}
	}
}
