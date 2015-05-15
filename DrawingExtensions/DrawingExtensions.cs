using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrawingExtensions
{
    public static class Extensions
    {
		public static Bitmap ChangeOpacity(this Image img, float opacity)
		{
			var bmp = new Bitmap(img.Width, img.Height);
			Graphics graphics = Graphics.FromImage(bmp);
			var colormatrix = new ColorMatrix {Matrix33 = opacity};
			var imgAttribute = new ImageAttributes();
			imgAttribute.SetColorMatrix(colormatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
			graphics.DrawImage(img, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imgAttribute);
			graphics.Dispose();
			return bmp;
		}
    }
}
