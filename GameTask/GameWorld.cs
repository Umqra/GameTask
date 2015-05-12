using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Physics;

namespace GameTask
{
	class GameWorld
	{
		private PhysicalWorld world;
		private List<IDrawable> shapes; 

		public GameWorld()
		{
			world = new PhysicalWorld();
			shapes = new List<IDrawable>();
		}

		public void AddGameObject(GameObject obj)
		{
			world.AddBody(obj);
			shapes.Add(obj);
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
