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
			public int ticks;

			public Text(string value, int ticks)
			{
				this.value = value;
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

		public TextModule()
		{
			showingText = new List<Text>();
			fontCollection.AddFontFile("../../../font.ttf");
			font = new Font(fontCollection.Families[0], 30, FontStyle.Regular);
		}

		public TextModule(int level, string[] lines) : this()
		{
			levelNumber = level;
			levelName = "Неизвестный уровень";
			for (int i = 0; i < lines.Length; i++)
			{
				if (lines[i] == "levelName" && i < lines.Length - 1)
					levelName = lines[i + 1];
			}
			ShowText(String.Format("Уровень {0}", levelNumber), 150);
			ShowText(levelName, 150);
			foreach (var text in lines.SkipWhile(line => line != "levelText").Skip(1))
				ShowText(text, 100);
		}

		public TextModule(string[] lines) : this()
		{
			foreach (var text in lines)
				ShowText(text, 150);
		}

		public bool Empty()
		{
			return showingText.Count == 0;
		}

		public void ShowText(string text, int ticks)
		{
			showingText.Add(new Text(text, ticks));
		}

		public void OnPaint(PaintEventArgs e)
		{
			var graphics = e.Graphics;
			if (showingText.Count == 0) return;
			var text = showingText[0];
			var clipboard = e.ClipRectangle;
			float centerX = (clipboard.Left + clipboard.Right) / 2.0f;
			float centerY = (clipboard.Bottom + clipboard.Top) / 2.0f;
			int opacity = (int) (Math.Min(100, text.ticks) / 100.0 * 255);
			var textSize = graphics.MeasureString(text.value, font);

			graphics.DrawString(text.value, font, new SolidBrush(Color.FromArgb(opacity, Color.Black)), centerX - textSize.Width / 2, centerY - textSize.Height / 2 - 30, format);
			
			text.ticks--;
			showingText = showingText.Where(t => t.ticks > 0).ToList();
		}
	}
}
