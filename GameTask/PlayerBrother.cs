using System;
using System.Collections.Generic;
using System.Data;
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
	class PlayerBrother : GameObject
	{
		private static readonly Image BrotherImage1 = Image.FromFile("../../../pictures/brother1.png");
		private static readonly Image BrotherImage2 = Image.FromFile("../../../pictures/brother2.png");
		
		private const double Width = 50;
		private const double Height = 50;
		private readonly int index;
		public PlayerBrother(Point center, int index) : base(Material.Wood, new Point(0, 0), false, null)
		{
			this.index = index;
			Shape = ConvexPolygon.Rectangle(center, Width, Height);
		}

		
		public Image Representation(GameWorld gameWorld)
		{
			if (gameWorld.Type == WorldType.ShadowWorld)
				return new Bitmap((int)Width, (int)Height);
			return index == 1 ? BrotherImage1 : BrotherImage2;
		}

		public override void OnPaint(GameWorld gameWorld, Graphics graphics)
		{
			graphics.DrawImage(Representation(gameWorld),
				(float)Shape[0].x, (float)Shape[0].y, (float)Width, (float)Height);
		}
	}
}
