using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Geometry;

namespace GameTask
{
	class LevelLoader
	{
		public const double CellWidth = 50;
		public const double CellHeight = 50;
		public static Game LoadLevel(int level, string filename)
		{
			var lines = File.ReadAllLines(filename, Encoding.UTF8);
			var sizes = lines[0].Split(' ');
			int h = int.Parse(sizes[0]), w = int.Parse(sizes[1]);
			var firstWorld = LoadWorld(lines.Skip(2).Take(h).ToList(), WorldType.MainWorld);
			var secondWorld = LoadWorld(lines.Skip(3 + h).Take(h).ToList(), WorldType.ShadowWorld);
			double worldWidth = w * CellWidth;
			double worldHeight = h * CellHeight;
			return new Game(firstWorld, secondWorld, new TextModule(level, lines.Skip(3 + 2 * h).ToArray()),
				worldWidth, worldHeight);
		}

		public static GameObject GetGameObject(int x, int y, char type)
		{
			var A = new Point(x * CellHeight, y * CellWidth);
			var B = new Point((x + 1) * CellHeight, y * CellWidth);
			var C = new Point((x + 1) * CellHeight, (y + 1) * CellWidth);
			var D = new Point(x * CellHeight, (y + 1) * CellWidth);
			Point middle = (A + C) / 2;
			switch (type)
			{
				case 'P':
					return new Player(middle);
				case '#':
					return new Wall(middle, CellWidth, CellHeight);
				case 'B':
					return new Box(middle);
				case 'G':
					return new Ground(middle, CellWidth, CellHeight);
				case 'K':
					return new Button(middle);
				case 'E':
					return new Exit(middle);
				case 'T':
					return new Teleport(middle);
				case 'I':
					return new IceWall(middle, CellWidth, CellHeight);
				case '1':
					return new PlayerBrother(middle, 1);
				case '2':
					return new PlayerBrother(middle, 2);
			}
			return null;
		}

		public static GameWorld LoadWorld(List<string> lines, WorldType type)
		{
			var world = new GameWorld(type);
			for (var i = 0; i < lines.Count; i++)
			{
				for (int s = 0; s < lines[i].Length; s++)
				{
					char c = lines[i][s];
					var newObject = GetGameObject(s, i, c);
					if (newObject == null)
						continue;
					if (newObject is Player)
						world.AddGamePlayer(newObject as Player);
					else if (newObject.IsStatic || newObject is Teleport)
						world.AddGameObject(newObject, 1);
					else
						world.AddGameObject(newObject);
				}
			}
			return world;
		}
	}
}
