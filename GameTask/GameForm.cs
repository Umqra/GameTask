using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Configuration;
using System.Linq;
using System.Reflection;
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
		private GameWorld mainWorld, shadowWorld;
		private List<Keys> pressedKeys;
		public GameForm()
		{

			BackColor = System.Drawing.Color.DimGray;
			pressedKeys = new List<Keys>();

			var worlds = LevelLoader.LoadLevel("../../../level1.txt");
			mainWorld = worlds.Item1;
			shadowWorld = worlds.Item2;
	
			ClientSize = new Size(800, 800);
			DoubleBuffered = true;
			var time = 0;
			var timer = new Timer();
			timer.Interval = (int) (1000 / fps);
			timer.Tick += (sender, args) =>
			{
				time++;
				mainWorld.OnTick(10 / fps);
				shadowWorld.OnTick(10 / fps);
				ProcessKeys();
				Invalidate();
			};
			
			timer.Start();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			
			shadowWorld.OnPaint(e);
			mainWorld.OnPaint(e);
		}

		private void SwitchWorlds()
		{
			shadowWorld.SwitchWorldType();
			mainWorld.SwitchWorldType();
			var player = mainWorld.player;
			mainWorld.RemoveGamePlayer(player);
			shadowWorld.AddGamePlayer(player);
			var tmp = mainWorld;
			mainWorld = shadowWorld;
			shadowWorld = tmp;
		}

		private void ProcessKeys()
		{
			foreach (var key in pressedKeys)
			{
				if (key == Keys.Left)
					mainWorld.SetVelocityToPlayer(new Geometry.Point(-20, 0));
				if (key == Keys.Right)
					mainWorld.SetVelocityToPlayer(new Geometry.Point(20, 0));
				if (key == Keys.Up)
					mainWorld.SetVelocityToPlayer(new Geometry.Point(0, -40));
			}
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Space)
				SwitchWorlds();
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
