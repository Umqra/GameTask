using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Geometry;
using Physics;

namespace GameTask
{
	class GameForm : Form
	{
		private PhysicalBody body;
		public GameForm()
		{
			var A = new Geometry.Point(0, 0);
			var B = new Geometry.Point(10, 0);
			var C = new Geometry.Point(10, 10);
			var D = new Geometry.Point(0, 10);
			body = new PhysicalBody(new ConvexPolygon(A, B, C, D), false, 10);

			DoubleBuffered = true;
			var time = 0;
			var timer = new Timer();
			timer.Interval = 20;
			timer.Tick += (sender, args) =>
			{
				time++;
				body.Move(0.1);
				Invalidate();
			};
			timer.Start();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			var graphics = e.Graphics;
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

			graphics.DrawPolygon(new Pen(Color.Red, 2), body.shape.Select(p => new PointF((float)p.x, (float)p.y)).ToArray());
		}
	}
}
