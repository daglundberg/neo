using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Neo
{
	internal class GuiBatchItem
	{
		public Texture2D Texture;
		public Vector2 Position;
		public Vector2 Size;
		public Vector4 Color;
		public Vector2 TexCoordTL;
		public Vector2 TexCoordBR;
		public float Radius;
		public Types Type;

		public GuiBatchItem()
		{
			Position = new Vector2();
			Size = new Vector2();
			Color = new Vector4();
			TexCoordTL = new Vector2();
			TexCoordBR = new Vector2();
		}

		public void Set(float x, float y, float dx, float dy, float w, float h, float sin, float cos, Vector4 color, Vector2 texCoordTL, Vector2 texCoordBR, float depth, float radius)
		{
			TexCoordTL = texCoordTL;
			TexCoordBR = texCoordBR;

			Color = color;
			Position.X = x;
			Position.Y = y;
			Size.X = w;
			Size.Y = h;
			Radius = radius;
		}

		public void Set(float x, float y, float w, float h, Vector4 color, Vector2 texCoordTL, Vector2 texCoordBR, float depth, float radius)
		{
			TexCoordTL = texCoordTL;
			TexCoordBR = texCoordBR;

			Color = color;
			Position.X = x;
			Position.Y = y;
			Size.X = w;
			Size.Y = h;
			Radius = radius;
		}

		public enum Types
		{
			None = 0,
			Texture,
			Rectangle,
		}
	}
}
