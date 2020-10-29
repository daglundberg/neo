using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NeoTestApp.Code
{
	public class Rectangle
	{
		public enum Techniques
		{
			Normal,
			Water
		}

		private Effect _effect;

		private TextureVertex[] _vertices;
		private Techniques _technique;

		public Rectangle(Vector2 position, Techniques technique, float scale = 1, Texture2D flowmap = null)
		{
			_effect = Game1.MyContent.Load<Effect>(@"Rectangle");

			_technique = technique;
		
			CreateWith16BitIndices(position, scale);
		}

		private void CreateWith16BitIndices(Vector2 position, float scale)
		{
			_vertices = new TextureVertex[6];

			_vertices[0].Position = new Vector3((-1 * scale) + position.X, (-1 * scale) + position.Y, 0);
			_vertices[1].Position = new Vector3((-1 * scale) + position.X, (1 * scale) + position.Y, 0);
			_vertices[2].Position = new Vector3((1 * scale) + position.X, (-1 * scale) + position.Y, 0);
			_vertices[4].Position = new Vector3((1 * scale) + position.X, (1 * scale) + position.Y, 0);

			_vertices[3].Position = _vertices[1].Position;
			_vertices[5].Position = _vertices[2].Position;

			_vertices[0].TextureCoordinate = new Vector2(0, 0);
			_vertices[1].TextureCoordinate = new Vector2(0, 1);
			_vertices[2].TextureCoordinate = new Vector2(1, 0);
			_vertices[4].TextureCoordinate = new Vector2(1, 1);

			_vertices[3].TextureCoordinate = _vertices[1].TextureCoordinate;
			_vertices[5].TextureCoordinate = _vertices[2].TextureCoordinate;
		}

		public void Draw(GameTime gameTime, Camera camera, Vector3 position, float alpha = 1, float scale = 1, float rotation = 0)
		{
			//_effect.Parameters["World"].SetValue(Matrix.CreateRotationZ(rotation) * Matrix.CreateScale(scale) * Matrix.CreateTranslation(position));
			//_effect.Parameters["View"].SetValue(camera.View);
			//_effect.Parameters["Projection"].SetValue(camera.Projection);
			//_effect.Parameters["Alpha"].SetValue(alpha);
			if (Game1.GraphicsDevice != null && _effect != null)
			{
				_effect.CurrentTechnique = _effect.Techniques[_technique.ToString()];
				_effect.CurrentTechnique.Passes[0].Apply();
				Game1.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, _vertices, 0, 2);

				Game1.GraphicsDevice.Indices = null;
				Game1.GraphicsDevice.SetVertexBuffer(null);
			}
		}
	}

	public struct RectangleVertex : IVertexType
	{
		public Vector3 Position;
		public Vector2 TextureCoordinate;

		public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration
		(
			new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
			new VertexElement(sizeof(float) * 3, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
		);

		VertexDeclaration IVertexType.VertexDeclaration
		{
			get { return VertexDeclaration; }
		}
	}
}
