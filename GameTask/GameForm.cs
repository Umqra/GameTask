using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GameTask
{
	class GameForm : Form
	{
		private const double fps = 100;
		private Game game;
		private List<Keys> pressedKeys;
		public GameForm()
		{
			BackColor = System.Drawing.Color.DimGray;
			pressedKeys = new List<Keys>();

			game = LevelLoader.LoadLevel("../../../level1.txt");
	
			ClientSize = new Size(800, 800);
			DoubleBuffered = true;
			var time = 0;
			var timer = new Timer {Interval = (int) (1000 / fps)};
			timer.Tick += (sender, args) =>
			{
				time++;
				game.OnTick(10 / fps);
				ProcessKeys();
				Invalidate();
			};
			
			timer.Start();
		}


		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			game.OnPaint(e);
		}

		private void ProcessKeys()
		{
			foreach (var key in pressedKeys)
			{
				game.KeyPress(key);
			}
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			game.Width = Width;
			game.Height = Height;
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			game.KeyDown(e.KeyCode);
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
