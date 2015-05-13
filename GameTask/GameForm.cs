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
		private const double fps = 100;
		private GameWorld world;
		private List<Keys> pressedKeys; 
		public GameForm()
		{
			pressedKeys = new List<Keys>();
			world = new GameWorld(new Geometry.Point(50, 50));

			world.AddGameObject(new GameBox(new Geometry.Point(100, 100), 50, 10, 1));
			world.AddGameObject(new GameBox(new Geometry.Point(150, 100), 50, 10, 1));
			world.AddGameObject(new GameBox(new Geometry.Point(225, 100), 100, 20, 1));

			world.AddGameObject(new GameWall(new []
			{
				new Geometry.Point(0, 200),
				new Geometry.Point(500, 200),
				new Geometry.Point(500, 600),
				new Geometry.Point(0, 600)
			}, 2));

			world.AddGameObject(new GameWall(new[]
			{
				new Geometry.Point(500, 500),
				new Geometry.Point(1000, 500),
				new Geometry.Point(1000, 600),
				new Geometry.Point(500, 600)
			}, 2));

			ClientSize = new Size(600, 600);
			DoubleBuffered = true;
			var time = 0;
			var timer = new Timer();
			timer.Interval = (int) (1 / fps * 1000);
			timer.Tick += (sender, args) =>
			{
				time++;
				world.OnTick(fps / 1000);
				ProcessKeys();
				Invalidate();
			};
			
			timer.Start();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
			world.OnPaint(e);
		}

		private void ProcessKeys()
		{
			foreach (var key in pressedKeys)
			{
				if (key == Keys.Left)
					world.SetVelocityToPlayer(new Geometry.Point(-20, 0));
				if (key == Keys.Right)
					world.SetVelocityToPlayer(new Geometry.Point(20, 0));
				if (key == Keys.Up)
					world.SetVelocityToPlayer(new Geometry.Point(0, -40));
			}
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (pressedKeys.Contains(e.KeyCode))
				return;
			pressedKeys.Add(e.KeyCode);
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			pressedKeys.Remove(e.KeyCode);
		}
	}
}
