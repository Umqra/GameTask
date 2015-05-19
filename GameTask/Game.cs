using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Geometry;
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
			
			status = GameStatus.ShowLevelText;

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
			if (status == GameStatus.ShowLevelText || status == GameStatus.ShowEndText)
				textModule.OnPaint(e);
			else
			{
				shadowWorld.OnPaint(e);
				mainWorld.OnPaint(e);
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
			double shiftX = 0, shiftY = 0;
			double maxX = Math.Min(300, Width / 2.0f);
			double maxY = Math.Min(300, Height / 2.0f);
			if (mainWorld.player.Shape[0].x + mainWorld.currentShiftX < maxX / 2)
				shiftX = Math.Min(maxX, -mainWorld.currentShiftX);
			else if (mainWorld.player.Shape[0].x + mainWorld.currentShiftX > Width - maxX / 2)
				shiftX = Math.Min(0, Math.Max((-mainWorld.currentShiftX + Width) - WorldWidth, -maxX));

			if (mainWorld.player.Shape[0].y + mainWorld.currentShiftY < maxY / 2)
				shiftY = Math.Min(maxY, -mainWorld.currentShiftY);
			else if (mainWorld.player.Shape[0].y + mainWorld.currentShiftY > Height - maxY / 2)
				shiftY = Math.Min(0, Math.Max((-mainWorld.currentShiftY + Height) - WorldHeight, -maxY));

			mainWorld.ShiftWorld(shiftX, shiftY);
			shadowWorld.ShiftWorld(shiftX, shiftY);
		}

		public void OnTick(double dt)
		{
			if (textModule.Empty() && status == GameStatus.ShowLevelText)
				status = GameStatus.Running;
			if (textModule.Empty() && status == GameStatus.ShowEndText)
				status = GameStatus.GameEnd;
			if (mainWorld.Shapes.Any(p => p is PlayerBrother) &&
			    mainWorld.world.IsBodyOnGround(mainWorld.player, new Point(0, 1)) &&
			    status == GameStatus.Running)
			{
				textModule = new TextModule(
					   new[]
					{
						"- Братья! Я так долго искал вас!",
						"- Не шуми. Мы тоже рады тебе. Но нам стоит поторопиться...",
						"Следуй за нами...",
						"MAY BE TO BE CONTINUED..."
					});
				status = GameStatus.ShowEndText;
			}
			if (status == GameStatus.GameOver || 
				status == GameStatus.ShowLevelText || 
				status == GameStatus.GameEnd ||
				status == GameStatus.ShowEndText)
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
			if (status == GameStatus.ShowLevelText)
			{
				if (key == Keys.Space)
					status = GameStatus.Running;
				return;
			}
			if (key == Keys.Left)
				mainWorld.SetVelocityToPlayer(new Geometry.Point(-20, 0));
			if (key == Keys.Right)
				mainWorld.SetVelocityToPlayer(new Geometry.Point(20, 0));
			if (key == Keys.Up)
				mainWorld.SetVelocityToPlayer(new Geometry.Point(0, -50));
		}

		public void KeyDown(Keys key)
		{
		}
	}
}
