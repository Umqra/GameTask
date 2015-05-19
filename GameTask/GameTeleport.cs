using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawingExtensions;
using Geometry;
using NUnit.Framework;
using Physics;
using Point = Geometry.Point;

namespace GameTask
{
	class GameTeleport : GameObject
	{
		private static readonly Image TeleportImageMainOn = Image.FromFile("../../../pictures/teleportOn.png");
		private static readonly Image TeleportImageShadowOn = TeleportImageMainOn.ChangeOpacity(0.2f);

		public const double Width = 60;
		public const double Height = 10;
		public const double ShapeHeight = 50;
		public GameObject teleported;
		public bool Activated { get; set; }
		public bool Enabled { get; set; }
		public GameTeleport(Point center)
			: base(Physics.Material.Adamantium, new Point(0, 0), false, null)
		{
			teleported = null;
			Activated = false;
			Enabled = true;
			Shape = ConvexPolygon.Rectangle(center, Width, Height);
		}

		public override void HandleCollision(Collision collision)
		{
			var target = collision.a == this ? collision.b : collision.a;
			if (target.IsStatic) return;
			teleported = (GameObject)target;
			Activated = true;
		}

		public Image Representation(GameWorld gameWorld)
		{
			return gameWorld.Type == WorldType.MainWorld ? TeleportImageMainOn : TeleportImageShadowOn;
		}

		public override void OnPaint(GameWorld gameWorld, Graphics graphics)
		{
			graphics.DrawImage(Representation(gameWorld),
				(float)Shape[0].x, (float)Shape[0].y - (float)(ShapeHeight - Height), (float)Width, (float)ShapeHeight);
		}
	}
}
