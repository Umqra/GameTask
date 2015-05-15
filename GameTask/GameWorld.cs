using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Geometry;
using Physics;
using Point = Geometry.Point;

namespace GameTask
{
	class GameWorld
	{
		public Game game;
		private PhysicalWorld world;
		public List<GameObject> Shapes { get; private set; }
		public GamePlayer player;
		public WorldType Type { get; set; }
		private Bitmap StaticImage;

		public double currentShift, neededShift;

		private void ChangeShift()
		{
			double delta = neededShift - currentShift;
			if (delta.IsNotEqual(0))
			{
				if (Math.Abs(delta) < 1)
					currentShift += delta;
				else
				currentShift += delta / Math.Abs(delta) * 2;
				InitializeStaticImage();
			}
		}

		public GameWorld()
		{
			game = null;
			currentShift = neededShift = 0;
			StaticImage = new Bitmap(2000, 800);
			world = new PhysicalWorld();
			Shapes = new List<GameObject>();
			player = null;
		}

		public GameWorld(WorldType type) : this()
		{
			Type = type;
		}

		public GameWorld(Point playerPosition, WorldType type) : this(type)
		{
			player = new GamePlayer(playerPosition);
			AddGameObject(player);
		}

		public void ShiftWorld(double shift)
		{
			if (neededShift.IsEqual(currentShift))
				neededShift += shift;
		}

		public void SwitchWorldType()
		{
			Type = Type == WorldType.MainWorld ? WorldType.ShadowWorld : WorldType.MainWorld;
			InitializeStaticImage();
		}

		public void InitializeStaticImage()
		{
			StaticImage = new Bitmap(800, 800);
			var graphics = Graphics.FromImage(StaticImage);
			graphics.TranslateTransform((float)currentShift, 0);
			foreach (var shape in Shapes)
			{
				if (shape.IsStatic)
					shape.OnPaint(this, graphics);
			}
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

		public void AddGameObject(GameObject obj, int layer = 0)
		{
			obj.Layer = layer;
			world.AddBody(obj);
			Shapes.Add(obj);
			Shapes = Shapes.OrderBy(x => x.Layer).ToList();
			InitializeStaticImage();
		}

		public void RemoveGameObject(GameObject obj)
		{
			world.RemoveBody(obj);
			Shapes.Remove(obj);
			InitializeStaticImage();
		}

		public void SetVelocityToPlayer(Point adding)
		{
			if (adding.IsCollinear(new Point(1, 0))) //Horizontal vector
			{
				player.Velocity = new Point(adding.x, player.Velocity.y);
			}
			else if (world.IsBodyOnGround(player, PhysicalWorld.acceleration))
			{
				player.Velocity = new Point(player.Velocity.x, adding.y);
			}
		}

		public void HandleCollisions()
		{
			foreach (var collision in world.collisions)
			{
				var first = collision.a as GameObject;
				var second = collision.b as GameObject;
				if (first != null && second != null)
				{
					first.HandleCollision(collision);
					second.HandleCollision(collision);
				}
			}
		}

		public void OnTick(double dt)
		{
			world.OnTick(dt);
			ChangeShift();
			HandleCollisions();

		}

		public void OnPaint(PaintEventArgs e)
		{
			e.Graphics.TranslateTransform((float)currentShift, 0);
			foreach (var obj in Shapes)
			{
				if (obj.Shape.Any(p => GeometryOperations.IsPointInRectangle(p, 
					-currentShift, 0, -currentShift + game.Width, game.Height)))
					obj.OnPaint(this, e);
			}
			e.Graphics.TranslateTransform(-(float) currentShift, 0);
			//e.Graphics.DrawImage(StaticImage, new PointF(0, 0));
		}
	}
}
