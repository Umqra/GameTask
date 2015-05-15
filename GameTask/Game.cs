using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameTask
{
	class Game
	{
		public double Width, Height;
		public GameStatus status;
		public GameWorld mainWorld;
		public GameWorld shadowWorld;

		public Game(GameWorld main, GameWorld shadow)
		{
			Width = Height = 800;
			status = GameStatus.Running;
			mainWorld = main;
			shadowWorld = shadow;
			mainWorld.game = shadowWorld.game = this;
		}

		public static Game LoadGame(string filename)
		{
			return LevelLoader.LoadLevel(filename);
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

		public void OnPaint(PaintEventArgs e)
		{
			shadowWorld.OnPaint(e);
			mainWorld.OnPaint(e);
		}

		public void HandleButtons()
		{
			bool switched = false;
			foreach (var obj in mainWorld.Shapes)
			{
				if (obj is GameButton)
				{
					var button = obj as GameButton;
					if (button.Activated)
					{
						switched = true;
						button.Activated = false;
					}
				}
			}
			if (switched)
				SwitchWorlds();
		}

		public void CheckPlayer()
		{
			if (!mainWorld.player.Alive)
				status = GameStatus.GameOver;
		}

		private void CenterThePlayer()
		{
			double shift = 0;
			if (mainWorld.player.Shape[0].x + mainWorld.currentShift < 40)
				shift = 100;
			else if (mainWorld.player.Shape[0].x + mainWorld.currentShift > Width - 100)
				shift = -100;
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
