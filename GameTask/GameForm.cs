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
		private GameWorld world;
		public GameForm()
		{
			world = new GameWorld(new Geometry.Point(50, 50));

			world.AddGameObject(new GameBox(new Geometry.Point(100, 100), 50, 10, 1));
			world.AddGameObject(new GameBox(new Geometry.Point(200, 100), 50, 10, 1));
			world.AddGameObject(new GameBox(new Geometry.Point(300, 100), 100, 100, 1));

			world.AddGameObject(new GameLevitateObject(new[]
			{
				new Geometry.Point(340, 340),
				new Geometry.Point(340, 380),
				new Geometry.Point(300, 380),
				new Geometry.Point(300, 340)
			}, 10));

			world.AddGameObject(new GameLevitateObject(new[]
			{
				new Geometry.Point(440, 320),
				new Geometry.Point(440, 360),
				new Geometry.Point(400, 360),
				new Geometry.Point(400, 320)
			}, 10));

			world.AddGameObject(new GameWall(new []
			{
				new Geometry.Point(0, 500),
				new Geometry.Point(500, 500),
				new Geometry.Point(500, 600),
				new Geometry.Point(0, 600)
			}, 2));

			ClientSize = new Size(600, 600);
			DoubleBuffered = true;
			var time = 0;
			var timer = new Timer();
			timer.Interval = 10;
			timer.Tick += (sender, args) =>
			{
				time++;
				world.OnTick(0.1);
				Invalidate();
			};
			
			timer.Start();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			world.OnPaint(e);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Left)
				world.SetVelocityToPlayer(new Geometry.Point(-20, 0));
			if (e.KeyCode == Keys.Right)
				world.SetVelocityToPlayer(new Geometry.Point(20, 0));
			if (e.KeyCode == Keys.Up)
				world.SetVelocityToPlayer(new Geometry.Point(0, -35));
		}
	}
}
