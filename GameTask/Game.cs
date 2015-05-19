using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NUnit.Framework;

namespace GameTask
{
	class Game
	{
		public double Width, Height;
		public GameStatus status;
		public TextModule textModule;
		public GameWorld mainWorld;
		public GameWorld shadowWorld;

		public Game(GameWorld main, GameWorld shadow, TextModule textModule)
		{
			this.textModule = textModule;
			Width = Height = 800;
			status = GameStatus.Running;
			mainWorld = main;
			shadowWorld = shadow;
			mainWorld.game = shadowWorld.game = this;
			mainWorld.anotherWorld = shadowWorld;
			shadowWorld.anotherWorld = mainWorld;
		}

		public void SwitchWorlds()
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

		public void OnPaint(PaintEventArgs e)
		{
			textModule.OnPaint(e);
			shadowWorld.OnPaint(e);
			mainWorld.OnPaint(e);
		}

		public void HandleButtons()
		{
			bool switched = false;
			foreach (var obj in mainWorld.Shapes)
			{
				if (obj is Button)
				{
					var button = obj as Button;
					if (button.Activated)
					{
						switched = true;
						button.Disable();
					}
				}
			}
			if (switched)
				SwitchWorlds();
		}

		public void HandleTeleports()
		{
			foreach (var obj in mainWorld.Shapes)
			{
				if (obj is Teleport)
				{
					var telepor = obj as Teleport;
					if (telepor.Activated)
					{
												
					}
				}
			}
		}

		public void CheckPlayer()
		{
			if (!mainWorld.player.Alive)
				status = GameStatus.GameOver;
			else if (mainWorld.player.Exit)
				status = GameStatus.NextLevel;	
		}

		private void CenterThePlayer()
		{
			double shift = 0;
			if (mainWorld.player.Shape[0].x + mainWorld.currentShift < 100)
				shift = 300;
			else if (mainWorld.player.Shape[0].x + mainWorld.currentShift > Width - 200)
				shift = -300;
			mainWorld.ShiftWorld(shift);
			shadowWorld.ShiftWorld(shift);
		}

		public void OnTick(double dt)
		{
			if (status == GameStatus.GameOver)
				return;
			shadowWorld.OnTick(dt);
			mainWorld.OnTick(dt);
			HandleButtons();
			HandleTeleports();
			CenterThePlayer();
			CheckPlayer();
		}

		public void KeyPress(Keys key)
		{
			if (status == GameStatus.GameOver)
				return;
			if (key == Keys.Left)
				mainWorld.SetVelocityToPlayer(new Geometry.Point(-20, 0));
			if (key == Keys.Right)
				mainWorld.SetVelocityToPlayer(new Geometry.Point(20, 0));
			if (key == Keys.Up)
				mainWorld.SetVelocityToPlayer(new Geometry.Point(0, -50));
		}

		public void KeyDown(Keys key)
		{
			if (status == GameStatus.GameOver)
				return;
			if (key == Keys.Space)
				SwitchWorlds();
		}
	}
}
