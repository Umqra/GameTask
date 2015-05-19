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
		public double WorldWidth, WorldHeight;
		public double Width, Height;
		public GameStatus status;
		public TextModule textModule;
		public GameWorld mainWorld;
		public GameWorld shadowWorld;

		public Game(GameWorld main, GameWorld shadow, TextModule textModule, double worldWidth, double worldHeight)
		{
			this.textModule = textModule;
			Width = Height = 800;

			WorldHeight = worldHeight;
			WorldWidth = worldWidth;
			
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

		public void CheckPlayer()
		{
			if (!mainWorld.player.Alive)
				status = GameStatus.GameOver;
			else if (mainWorld.player.Exit)
				status = GameStatus.NextLevel;	
		}

		private void CenterThePlayer()
		{
			double shiftX = 0, shiftY = 0;
			if (mainWorld.player.Shape[0].x + mainWorld.currentShiftX < 100)
				shiftX = Math.Min(300, -mainWorld.currentShiftX);
			else if (mainWorld.player.Shape[0].x + mainWorld.currentShiftX > Width - 200)
				shiftX = Math.Max((-mainWorld.currentShiftX + Width) - WorldWidth, -300);

			if (mainWorld.player.Shape[0].y + mainWorld.currentShiftY < 100)
				shiftY = Math.Min(300, -mainWorld.currentShiftY);
			else if (mainWorld.player.Shape[0].y + mainWorld.currentShiftY > Height - 200)
				shiftY = Math.Max((-mainWorld.currentShiftY + Height) - WorldHeight, -300);

			mainWorld.ShiftWorld(shiftX, shiftY);
			shadowWorld.ShiftWorld(shiftX, shiftY);
		}

		public void OnTick(double dt)
		{
			if (status == GameStatus.GameOver)
				return;
			shadowWorld.OnTick(dt);
			mainWorld.OnTick(dt);
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
