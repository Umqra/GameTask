using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry;

namespace GameTask
{
	class LevelLoader
	{
		public const double CellWidth = 50;
		public const double CellHeight = 50;
		public static Tuple<GameWorld, GameWorld> LoadLevel(string filename)
		{
			var lines = File.ReadAllLines(filename);
			var sizes = lines[0].Split(' ');
			int h = int.Parse(sizes[0]), w = int.Parse(sizes[1]);
			var firstWorld = LoadWorld(lines.Skip(2).Take(h).ToList(), WorldType.MainWorld);
			var secondWorld = LoadWorld(lines.Skip(3 + h).Take(h).ToList(), WorldType.ShadowWorld);
			return Tuple.Create(firstWorld, secondWorld);
		}

		public static GameObject GetGameObject(int x, int y, char type)
		{
			var A = new Point(x * CellHeight, y * CellWidth);
			var B = new Point((x + 1) * CellHeight, y * CellWidth);
			var C = new Point((x + 1) * CellHeight, (y + 1) * CellWidth);
			var D = new Point(x * CellHeight, (y + 1) * CellWidth);
			switch (type)
			{
				case 'P':
					return new GamePlayer((A + B) / 2);
				case '#':
					return new GameWall((A + C) / 2, CellWidth, CellHeight);
				case 'B':
					return new GameBox((A + C) / 2);
				case 'G':
					return new GameGround((A + C) / 2, CellWidth, CellHeight);
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
					if (newObject is GamePlayer)
						world.AddGamePlayer(newObject as GamePlayer);
					else if (newObject is GameGround || newObject is GameWall)
						world.AddGameObject(newObject, 1);
					else
						world.AddGameObject(newObject);
				}
			}
			return world;
		}
	}
}
