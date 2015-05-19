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
		private int levelNumber;
		public GameForm()
		{
			levelNumber = 1;
			BackColor = System.Drawing.Color.DimGray;
			pressedKeys = new List<Keys>();

			game = LevelLoader.LoadLevel(levelNumber, "../../../level5.txt");
	
			ClientSize = new Size(800, 800);
			DoubleBuffered = true;
			var time = 0;
			var timer = new Timer {Interval = (int) (1000 / fps)};
			timer.Tick += (sender, args) =>
			{
				time++;
				CheckGameStatus();
				game.OnTick(10 / fps);
				ProcessKeys();
				Invalidate();
			};
			
			timer.Start();
		}

		private void CheckGameStatus()
		{
			if (game.status == GameStatus.NextLevel)
			{
				levelNumber++;
				game = LevelLoader.LoadLevel(levelNumber, String.Format("../../../level{0}.txt", levelNumber));
				game.Width = Width;
				game.Height = Height;
			}
			if (game.status == GameStatus.GameOver)
			{
				game = LevelLoader.LoadLevel(levelNumber, String.Format("../../../level{0}.txt", levelNumber));
				game.Width = Width;
				game.Height = Height;
				game.status = GameStatus.Running;
			}
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
			game.Width = ClientSize.Width;
			game.Height = ClientSize.Height;
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.R)
				game.status = GameStatus.GameOver;
			else if (e.KeyCode == Keys.N)
				game.status = GameStatus.NextLevel;
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
