using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NeoTestApp.Code
{
	public class Rectangle
	{

		private Effect _effect;

		private TextureVertex[] _vertices;
		private Color _color;
		private Vector2 _size;

		public Rectangle(Vector2 position, Color color, Vector2 size, float scale = 1)
		{
			_effect = Game1.MyContent.Load<Effect>(@"Rectangle");
			_size = size;
			_color = Color.AliceBlue;
		
			CreateWith16BitIndices(position, scale);
		}

		private void CreateWith16BitIndices(Vector2 position, float scale)
		{
			scale = 1f;
			_vertices = new TextureVertex[6];

/*			_vertices[0].Position = new Vector3((-1 * scale) + position.X, (-1 * scale) + position.Y, 0);
			_vertices[1].Position = new Vector3((-1 * scale) + position.X, (1 * scale) + position.Y, 0);
			_vertices[2].Position = new Vector3((1 * scale) + position.X, (-1 * scale) + position.Y, 0);
			_vertices[4].Position = new Vector3((1 * scale) + position.X, (1 * scale) + position.Y, 0);*/

			//Because the coordinate systems are different
			Vector2 _size2 = new Vector2((float)_size.X / (float)1920f, (float)_size.Y / (float)1080f);
			Vector2 _pos = new Vector2((((position.X / (float)1920f + _size2.X / 2) * 2f) - 1f) * -1.0f, (((position.Y / (float)1080f + _size2.Y / 2) * 2f) - 1f) * -1.0f);

						_vertices[0].Position = new Vector3( (position.X / (float)1920f) * 2 - 1f, (position.Y / (float)1080f) * 2 - 1f, 0);
						_vertices[1].Position = new Vector3( (position.X / (float)1920f) * 2 - 1f, (position.Y / (float)1080f) * 2 - 1f + (_size2.Y * 2), 0);
						_vertices[2].Position = new Vector3( (position.X / (float)1920f) * 2 - 1f + (_size2.X*2), (position.Y / (float)1080f) * 2 - 1f, 0);
						_vertices[4].Position = new Vector3( (position.X / (float)1920f) * 2 - 1f + (_size2.X * 2), (position.Y / (float)1080f) * 2 - 1f + ( _size2.Y * 2), 0);

			_vertices[3].Position = _vertices[1].Position;
			_vertices[5].Position = _vertices[2].Position;

			_vertices[0].TextureCoordinate = new Vector2(0, 0);
			_vertices[1].TextureCoordinate = new Vector2(0, 1);
			_vertices[2].TextureCoordinate = new Vector2(1, 0);
			_vertices[4].TextureCoordinate = new Vector2(1, 1);

			_vertices[3].TextureCoordinate = _vertices[1].TextureCoordinate;
			_vertices[5].TextureCoordinate = _vertices[2].TextureCoordinate;
		}

		public void Draw(GameTime gameTime)
		{
			//_effect.Parameters["World"].SetValue(Matrix.CreateRotationZ(rotation) * Matrix.CreateScale(scale) * Matrix.CreateTranslation(position));
			//_effect.Parameters["View"].SetValue(camera.View);
			//_effect.Parameters["Projection"].SetValue(camera.Projection);
			_effect.Parameters["Color2"].SetValue(Color.AliceBlue.ToVector4());
		//	_effect.Parameters["Size"].SetValue(_size);
			if (Game1.GraphicsDevice != null && _effect != null)
			{
				_effect.CurrentTechnique = _effect.Techniques["Normal"];
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
