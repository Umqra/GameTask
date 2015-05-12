using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Geometry;
using Physics;

namespace GameTask
{
	class GameWorld
	{
		private PhysicalWorld world;
		private List<IDrawable> shapes;
		private GamePlayer player;

		public GameWorld(Point playerPosition)
		{
			world = new PhysicalWorld();
			shapes = new List<IDrawable>();
			player = new GamePlayer(playerPosition);
			world.AddBody(player);
			shapes.Add(player);
		}

		public void AddGameObject(GameObject obj)
		{
			world.AddBody(obj);
			shapes.Add(obj);
		}

		public void SetVelocityToPlayer(Point adding)
		{
			if (adding.IsCollinear(new Point(1, 0))) //Horizontal vector
			{
				player.Velocity = new Point(adding.x, player.Velocity.y);
			}
			else if (!world.CanMove(player, -adding.SetLength(0.1)))
			{
				player.Velocity = new Point(player.Velocity.x, adding.y);
			}
		}

		public void OnTick(double dt)
		{
			world.OnTick(dt);
		}

		public void OnPaint(PaintEventArgs e)
		{
			foreach (var shape in shapes)
				shape.OnPaint(this, e);
		}
	}
}
