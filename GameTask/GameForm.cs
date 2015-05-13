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
		{;
			BackColor = System.Drawing.Color.DimGray;
			pressedKeys = new List<Keys>();
		
			shadowWorld = new GameWorld(WorldType.ShadowWorld);
			shadowWorld.AddGameObject(new GameWall(new[]
			{
				new Geometry.Point(500, 250),
				new Geometry.Point(600, 300),
				new Geometry.Point(600, 250),
				new Geometry.Point(500, 300)
			}, 2));
			
			mainWorld = new GameWorld(new Geometry.Point(590, 100), WorldType.MainWorld);
			
			mainWorld.AddGameObject(new GameBox(new Geometry.Point(540, 100), 10, 1));
			mainWorld.AddGameObject(new GameBox(new Geometry.Point(570, 200), 10, 1));
			mainWorld.AddGameObject(new GameBox(new Geometry.Point(600, 300), 20, 1));

			mainWorld.AddGameObject(new GameWall(new []
			{
				new Geometry.Point(0, 200),
				new Geometry.Point(500, 200),
				new Geometry.Point(500, 600),
				new Geometry.Point(0, 600)
			}, 2));

			mainWorld.AddGameObject(new GameWall(new[]
			{
				new Geometry.Point(500, 500),
				new Geometry.Point(1000, 500),
				new Geometry.Point(1000, 600),
				new Geometry.Point(500, 600)
			}, 2));

			ClientSize = new Size(800, 800);
			DoubleBuffered = true;
			var time = 0;
			var timer = new Timer();
			timer.Interval = (int) (1 / fps * 1000);
			timer.Tick += (sender, args) =>
			{
				time++;
				mainWorld.OnTick(fps / 1000);
				shadowWorld.OnTick(fps / 1000);
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
