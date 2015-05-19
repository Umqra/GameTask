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
	class Player : GameObject
	{
		private static readonly Image PlayerImageMain = Image.FromFile("../../../pictures/player.png");
		private static readonly Image PlayerImageShadow = PlayerImageMain.ChangeOpacity(0.2f);
		
		public bool Exit { get; set; }
		private const double Width = 50;
		private const double Height = 50;
		public Player(Point center) : base(Material.Wood, new Point(0, 0), false, null)
		{
			Exit = false;
			Shape = ConvexPolygon.Rectangle(center, Width, Height);
		}

		public override void HandleCollision(Collision collision)
		{
			var target = collision.a == this ? collision.b : collision.a;
			if (target is Exit)
				Exit = true;
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
