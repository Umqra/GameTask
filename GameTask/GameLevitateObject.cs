using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Geometry;
using Point = Geometry.Point;

namespace GameTask
{
	class GameLevitateObject : GameObject
	{
		static readonly Random Random = new Random(31415);
		public double time;
		public double amplitude;
		public GameLevitateObject(Point[] points, double mass)
			: base(mass, new Point(0, 0), 1, false, null)
		{
			time = Random.NextDouble();
			amplitude = 7;
			Shape = new ConvexPolygon(points);
		}

		public override Point Acceleration 
		{ 
			get { return new Point(0, 0);}
			set { }
		}

		public override void OnTick(double dt)
		{
			base.OnTick(dt);
			Velocity = new Point(Velocity.x, Math.Sin(time) * amplitude);
			time += dt;
		}

		public override void OnPaint(object sender, PaintEventArgs e)
		{
			var graphics = e.Graphics;
			graphics.FillPolygon(Brushes.Red,
				Shape
				.Select(p => new PointF((float)p.x, (float)p.y)).ToArray()
				);	
		}
	}
}
