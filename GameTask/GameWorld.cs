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
		private PhysicalWorld world;
		private List<IDrawable> shapes;
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
			currentShift = neededShift = 0;
			StaticImage = new Bitmap(2000, 800);
			world = new PhysicalWorld();
			shapes = new List<IDrawable>();
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
			foreach (var shape in shapes)
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
			shapes.Add(obj);
			shapes = shapes.OrderBy(x => x.Layer).ToList();
			InitializeStaticImage();
		}

		public void RemoveGameObject(GameObject obj)
		{
			world.RemoveBody(obj);
			shapes.Remove(obj);
			InitializeStaticImage();
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

		public void HandleCollisions()
		{
			foreach (var pair in world.collisions)
			{
				var first = pair.Item1 as GameObject;
				var second = pair.Item2 as GameObject;
				if (first != null && second != null)
				{
					first.CollisionWith(second);
					second.CollisionWith(first);
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
			foreach (var shape in shapes)
			{
				//if (shape.IsStatic) continue;
				shape.OnPaint(this, e);
			}
			e.Graphics.TranslateTransform(-(float) currentShift, 0);
			//e.Graphics.DrawImage(StaticImage, new PointF(0, 0));
		}
	}
}
