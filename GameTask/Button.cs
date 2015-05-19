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
	class Button : GameObject
	{
		private static readonly Image ButtonImageMainOn = Image.FromFile("../../../pictures/buttonOn.png");
		private static readonly Image ButtonImageShadowOn = ButtonImageMainOn.ChangeOpacity(0.2f);

		private static readonly Image ButtonImageMainOff = Image.FromFile("../../../pictures/buttonOff.png");
		private static readonly Image ButtonImageShadowOff = ButtonImageMainOff.ChangeOpacity(0.2f);

		public const double Width = 40;
		public const double Height = 15;
		public bool Activated { get; set; }
		public bool Enabled { get; set; }
		public Button(Point center) : base(Physics.Material.Adamantium, new Point(0, 0), false, null)
		{
			Activated = false;
			Enabled = true;
			Shape = ConvexPolygon.Rectangle(center, Width, Height);
		}

		public override void HandleCollision(Collision collision)
		{
			if (!Enabled) return;
			var target = collision.a == this ? collision.b : collision.a;
			var delta = PhysicalWorld.GetVectorForResolveCollision(target, this);
			if (delta.DotProductWith(PhysicalWorld.acceleration).IsLess(0))
				Activated = true;
		}

		public void Disable()
		{
			Enabled = Activated = false;
		}

		public Image Representation(GameWorld gameWorld)
		{
			if (Enabled)
				return gameWorld.Type == WorldType.MainWorld ? ButtonImageMainOn : ButtonImageShadowOn;
			return gameWorld.Type == WorldType.MainWorld ? ButtonImageMainOff : ButtonImageShadowOff;
		}

		public override void OnPaint(GameWorld gameWorld, Graphics graphics)
		{
			graphics.DrawImage(Representation(gameWorld),
				(float)Shape[0].x, (float)Shape[0].y, (float)Width, (float)Height);
		}
	}
}
