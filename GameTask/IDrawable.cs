using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameTask
{
	interface IDrawable
	{
		void OnPaint(GameWorld sender, Graphics g);
		void OnPaint(GameWorld sender, PaintEventArgs e);
		int Layer { get; set; }
		bool IsStatic { get; set; }
	}
}
