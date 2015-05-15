using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawingExtensions;
using Geometry;
using Physics;
using Point = Geometry.Point;

namespace GameTask
{
	class GameButton : GameObject
	{
		private static readonly Image ButtonImageMain = Image.FromFile("../../../pictures/button.png");
		private static readonly Image ButtonImageShadow = ButtonImageMain.ChangeOpacity(0.2f);
		public const double Width = 40;
		public const double Height = 15;
		public bool Activated { get; set; }
		public GameButton(Point center) : base(Physics.Material.Adamantium, new Point(0, 0), false, null)
		{
			Shape = ConvexPolygon.Rectangle(center, Width, Height);
		}

		public override void HandleCollision(Collision collision)
		{
			var target = collision.a == this ? collision.b : collision.a;
			var delta = PhysicalWorld.GetVectorForResolveCollision(target, this);
			if (delta.DotProductWith(PhysicalWorld.acceleration).IsLess(0))
				Activated = true;
		}

		public Image Representation(GameWorld gameWorld)
		{
			return gameWorld.Type == WorldType.MainWorld ? ButtonImageMain : ButtonImageShadow;
		}

		public override void OnPaint(GameWorld gameWorld, Graphics graphics)
		{
			graphics.DrawImage(Representation(gameWorld),
				(float)Shape[0].x, (float)Shape[0].y, (float)Width, (float)Height);
		}
	}
}
