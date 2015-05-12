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
			world = new GameWorld();

			world.AddGameObject(new GameBox(new Geometry.Point(100, 100), 50, 10, 1));
			world.AddGameObject(new GameWall(new []
			{
				new Geometry.Point(200, 200),
				new Geometry.Point(300, 200),
				new Geometry.Point(300, 300),
				new Geometry.Point(200, 300)
			}, 1));

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
			world.OnPaint(e);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
		
		}
	}
}
