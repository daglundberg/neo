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

		public VertexPositionTexture vertexTL;
		public VertexPositionTexture vertexTR;
		public VertexPositionTexture vertexBL;
		public VertexPositionTexture vertexBR;

		public GuiBatchItem()
		{
			Position = new Vector2();
			Size = new Vector2();
			Color = new Vector4();
			TexCoordTL = new Vector2();
			TexCoordBR = new Vector2();

			vertexTL = new VertexPositionTexture();
			vertexTR = new VertexPositionTexture();
			vertexBL = new VertexPositionTexture();
			vertexBR = new VertexPositionTexture();
		}

/*		public void Set(float x, float y, float dx, float dy, float w, float h, float sin, float cos, Vector4 color, Vector2 texCoordTL, Vector2 texCoordBR, float depth, float radius)
		{
			TexCoordTL = texCoordTL;
			TexCoordBR = texCoordBR;

			Color = color;
			Position.X = x;
			Position.Y = y;
			Size.X = w;
			Size.Y = h;
			Radius = radius;
		}*/

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

			vertexTL.Position.X = x;
			vertexTL.Position.Y = y;
			vertexTL.Position.Z = depth;
			vertexTL.TextureCoordinate.X = texCoordTL.X;
			vertexTL.TextureCoordinate.Y = texCoordTL.Y;

			vertexTR.Position.X = x + w;
			vertexTR.Position.Y = y;
			vertexTR.Position.Z = depth;
			vertexTR.TextureCoordinate.X = texCoordBR.X;
			vertexTR.TextureCoordinate.Y = texCoordTL.Y;

			vertexBL.Position.X = x;
			vertexBL.Position.Y = y + h;
			vertexBL.Position.Z = depth;
			vertexBL.TextureCoordinate.X = texCoordTL.X;
			vertexBL.TextureCoordinate.Y = texCoordBR.Y;

			vertexBR.Position.X = x + w;
			vertexBR.Position.Y = y + h;
			vertexBR.Position.Z = depth;
			vertexBR.TextureCoordinate.X = texCoordBR.X;
			vertexBR.TextureCoordinate.Y = texCoordBR.Y;
		}

		public enum Types
		{
			None = 0,
			Texture,
			Rectangle,
		}
	}
}
