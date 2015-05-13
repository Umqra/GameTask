using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Physics;
using Point = Geometry.Point;

namespace GameTask
{
	class GameWorld
	{
		private PhysicalWorld world;
		private List<IDrawable> shapes;
		public GamePlayer player;
		private WorldType type;

		public GameWorld(WorldType type)
		{
			this.type = type;
			world = new PhysicalWorld();
			shapes = new List<IDrawable>();
			player = null;
		}

		public GameWorld(Point playerPosition, WorldType type) : this(type)
		{
			player = new GamePlayer(playerPosition);
			AddGameObject(player);
		}

		public void SwitchWorldType()
		{
			type = type == WorldType.MainWorld ? WorldType.ShadowWorld : WorldType.MainWorld;
		}

		public void AddGamePlayer(GamePlayer player)
		{
			this.player = player;
			AddGameObject(player);
		}

		public void RemoveGamePlayer(GamePlayer player)
		{
			if (this.player == player)
			{
				this.player = null;
				RemoveGameObject(player);
			}
		}

		public void AddGameObject(GameObject obj)
		{
			world.AddBody(obj);
			shapes.Add(obj);
		}

		public void RemoveGameObject(GameObject obj)
		{
			world.RemoveBody(obj);
			shapes.Remove(obj);
		}

		public void SetVelocityToPlayer(Point adding)
		{
			if (adding.IsCollinear(new Point(1, 0))) //Horizontal vector
			{
				player.Velocity = new Point(adding.x, player.Velocity.y);
			}
			else if (world.IsBodyOnGround(player, world.acceleration))
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
