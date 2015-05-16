using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Geometry;

namespace GameTask
{
	class TextModule
	{
		class Text
		{
			public string value;
			public int x, y;
			public int ticks;

			public Text(string value, int x, int y, int ticks)
			{
				this.value = value;
				this.x = x;
				this.y = y;
				this.ticks = ticks;
			}
		}

		public int levelNumber;
		public string levelName;
		public PrivateFontCollection fontCollection = new PrivateFontCollection();
		public Font font;
		public Brush brush = new SolidBrush(Color.Black);
		public StringFormat format = StringFormat.GenericTypographic;
		private List<Text> showingText; 

		public TextModule(int level, string[] lines)
		{
			showingText = new List<Text>();
			levelNumber = level;
			fontCollection.AddFontFile("../../../font.ttf");
			font = new Font(fontCollection.Families[0], 30, FontStyle.Regular);
			levelName = "Неизвестный уровень";
			for (int i = 0; i < lines.Length; i++)
			{
				if (lines[i] == "levelName" && i < lines.Length - 1)
					levelName = lines[i + 1];
			}
			ShowText(String.Format("Уровень {0}", levelNumber), 400, 30, 150);
			ShowText(levelName, 400, 30, 150);
			foreach (var text in lines.SkipWhile(line => line != "levelText").Skip(1))
				ShowText(text, 400, 30, 80);

		}

		public void ShowText(string text, int x, int y, int ticks)
		{
			showingText.Add(new Text(text, x, y, ticks));
		}

		public void OnPaint(PaintEventArgs e)
		{
			var graphics = e.Graphics;
			if (showingText.Count == 0) return;
			var text = showingText[0];
			var clipboard = e.ClipRectangle;
			float center = (clipboard.Left + clipboard.Right) / 2.0f;
			float length = text.value.Length * 20;
			int opacity = (int) (Math.Min(100, text.ticks) / 100.0 * 255);
			graphics.DrawString(text.value, font, new SolidBrush(Color.FromArgb(opacity, Color.Black)), center - length / 2, text.y, format);
			text.ticks--;
			showingText = showingText.Where(t => t.ticks > 0).ToList();
		}
	}
}
