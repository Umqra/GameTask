using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DrawingExtensions;
using Geometry;
using NUnit.Framework;
using Physics;
using Point = Geometry.Point;

namespace GameTask
{
	class Teleport : GameObject
	{
		private static readonly Image TeleportImageMainOn = Image.FromFile("../../../pictures/teleportOn.png");
		private static readonly Image TeleportImageShadowOn = TeleportImageMainOn.ChangeOpacity(0.2f);

		private static readonly Image TeleportImageMainOff = Image.FromFile("../../../pictures/teleportOff.png");
		private static readonly Image TeleportImageShadowOff = TeleportImageMainOff.ChangeOpacity(0.2f);

		public const double Width = 60;
		public const double Height = 10;
		public const double ShapeHeight = 50;
		public GameObject teleported;
		public bool Activated { get; set; }
		public bool Enabled { get; set; }
		public Teleport(Point center)
			: base(Physics.Material.Adamantium, new Point(0, 0), false, null)
		{
			teleported = null;
			Activated = false;
			Enabled = true;
			Shape = ConvexPolygon.Rectangle(center, Width, Height);
		}

		public override void HandleCollision(Collision collision)
		{
			if (!Enabled) return;
			var target = collision.a == this ? collision.b : collision.a;
			if (target.IsStatic) return;
			var delta = PhysicalWorld.GetVectorForResolveCollision(target, this);
			if (delta.DotProductWith(PhysicalWorld.acceleration).IsLess(0))
			{
				teleported = (GameObject) target;
				Activated = true;
			}
		}

		public void Disable()
		{
			Activated = Enabled = false;
			teleported = null;
		}

		public Image Representation(GameWorld gameWorld)
		{
			if (Enabled)
				return gameWorld.Type == WorldType.MainWorld ? TeleportImageMainOn : TeleportImageShadowOn;
			return gameWorld.Type == WorldType.MainWorld ? TeleportImageMainOff : TeleportImageShadowOff;
		}

		public override void OnPaint(GameWorld gameWorld, Graphics graphics)
		{
			graphics.DrawImage(Representation(gameWorld),
				(float)Shape[0].x, (float)Shape[0].y - (float)(ShapeHeight - Height), (float)Width, (float)ShapeHeight);
		}
	}
}
