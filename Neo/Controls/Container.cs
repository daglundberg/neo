using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Neo.Components
{
	public class Container : Control
	{
		public Container() { }
		public Container(PixelUnit positionOffset, PixelUnit size)
		{
			Size = size;
			PositionOffset = positionOffset;
		}

		//Neo-init
		Neo _neo;
		GraphicsDeviceManager _graphics;
		public override void Initialize(Neo neo, GraphicsDeviceManager graphics)
		{
			_neo = neo;
			_graphics = graphics;
		}

		public override void SetPositionAndScale(Rectangle parentBounds, float scale)
		{
			_scale = scale;

			Point size;

			if (Size == null)
			{
				Bounds = parentBounds;
				size = parentBounds.Size;
			}
			else
			{
				Bounds = GetPixelBoundsFromFlow(parentBounds, Flow, scale, PositionOffset, Size);
				size = new Point((int)Size.GetScaled(_neo.Scale).X, (int)Size.GetScaled(_neo.Scale).Y);
			}

			_position = Bounds.Location.ToVector2();
			IsCalculated = true;

			basicEffect = new BasicEffect(_graphics.GraphicsDevice);
			basicEffect.VertexColorEnabled = true;

			Point position = new Point(0, 0);
			Vector2 size2 = new Vector2((float)size.X / (float)_graphics.PreferredBackBufferWidth, (float)size.Y / (float)_graphics.PreferredBackBufferHeight);

			Color color = new Color(10, 10, 10, 200);
			_vertices[0].Position = new Vector3(-1 * size2.X + position.X, -1 * size2.Y + position.Y, 0);
			_vertices[1].Position = new Vector3(-1 * size2.X + position.X, 1 * size2.Y + position.Y, 0);
			_vertices[2].Position = new Vector3(1 * size2.X + position.X, -1 * size2.Y + position.Y, 0);
			_vertices[4].Position = new Vector3(1 * size2.X + position.X, 1 * size2.Y + position.Y, 0);

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
