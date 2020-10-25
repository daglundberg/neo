using Microsoft.Xna.Framework;

namespace Neo
{
	public class PixelUnit
	{
		public PixelUnit(int width, int height) { Width = width; Height = height; }
		public PixelUnit(Point point) { Width = point.X; Height = point.Y; }
		private int Width;
		private int Height;

		public Vector2 GetScaled(float scale)
		{
			return new Vector2(Width * scale, Height * scale);
		}
	}
}
