using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Neo.Components
{
	public class Container : Control
	{
		public Container(){}
		public Container(ScreenUnit positionOffset, ScreenUnit size)
		{
			Size = size;
			PositionOffset = positionOffset;
		}

		//Neo-init
		GraphicsDeviceManager _graphics;
		public override void Initialize(Neo neo, GraphicsDeviceManager graphics)
		{
			_graphics = graphics;
		}

		public override void SetPositionAndScale(Rectangle parentBounds, float scale, int numSiblings, int siblingIndex)
		{
			_scale = scale;

			if (Size == null)
				PixelBounds = parentBounds;
			else
				PixelBounds = GetPixelBoundsFromFlow(parentBounds, Flow, scale, PositionOffset, Size, numSiblings, siblingIndex);

			_position = PixelBounds.Location.ToVector2();
			IsCalculated = true;

			basicEffect = new BasicEffect(_graphics.GraphicsDevice);
			basicEffect.VertexColorEnabled = true;

			//Because the coordinate systems are different
			Vector2 size2 = new Vector2((float)PixelBounds.Size.X / (float)_graphics.PreferredBackBufferWidth, (float)PixelBounds.Size.Y / (float)_graphics.PreferredBackBufferHeight);
			Vector2 _pos = new Vector2((((_position.X / (float)_graphics.PreferredBackBufferWidth + size2.X / 2) * 2f) - 1f) * -1.0f, (((_position.Y / (float)_graphics.PreferredBackBufferHeight + size2.Y / 2) * 2f ) -1f ) * -1.0f);

			Color color = new Color(10, 10, 10, 200);
			_vertices[0].Position = new Vector3(-1 * size2.X + _pos.X, -1 * size2.Y + _pos.Y, 0);
			_vertices[1].Position = new Vector3(-1 * size2.X + _pos.X, 1 * size2.Y + _pos.Y, 0);
			_vertices[2].Position = new Vector3(1 * size2.X + _pos.X, -1 * size2.Y + _pos.Y, 0);
			_vertices[4].Position = new Vector3(1 * size2.X + _pos.X, 1 * size2.Y + _pos.Y, 0);

			_vertices[0].Color = color;
			_vertices[1].Color = color;
			_vertices[2].Color = color;
			_vertices[4].Color = color;

			_vertices[3] = _vertices[1];
			_vertices[5] = _vertices[2];

			vertexBuffer = new VertexBuffer(_graphics.GraphicsDevice, typeof(VertexPositionColor), 6, BufferUsage.WriteOnly);
			vertexBuffer.SetData(_vertices);
		}

		//Drawable
		BasicEffect basicEffect;
		VertexBuffer vertexBuffer;
		VertexPositionColor[] _vertices = new VertexPositionColor[6];
		public override void Draw(SpriteBatch spriteBatch)
		{
			_graphics.GraphicsDevice.SetVertexBuffer(vertexBuffer);
			spriteBatch.End();
			basicEffect.Techniques[0].Passes[0].Apply();
			basicEffect.Techniques[1].Passes[0].Apply();
			basicEffect.Techniques[2].Passes[0].Apply();

			_graphics.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
			spriteBatch.Begin();
		}
	}
}
