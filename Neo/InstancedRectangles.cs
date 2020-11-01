using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace Neo
{
	public class InstancedRectangles : DrawableGameComponent
	{
		private Effect effect;
		private VertexBuffer instanceBuffer, geometryBuffer;
		private IndexBuffer indexBuffer;

		private VertexBufferBinding[] bindings;

		public InstancedRectangles(Game game) : base(game) { effect = Game.Content.Load<Effect>("InstancingRectangleShader"); Initialize(); }

		protected override void LoadContent()
		{
			
			base.LoadContent();
		}

		public override void Initialize()
		{
			GenerateCommonGeometryRect();
			CreateInstances();

			// Creates the binding between the geometry and the instances.
			bindings = new VertexBufferBinding[2];
			bindings[0] = new VertexBufferBinding(geometryBuffer);
			bindings[1] = new VertexBufferBinding(instanceBuffer, 0, 1);

			base.Initialize();
		}

		private void GenerateCommonGeometryRect()
		{
			VertexPositionTexture[] _vertices = new VertexPositionTexture[4];
			_vertices[0].Position = new Vector3(0, 0, 0);
			_vertices[1].Position = new Vector3(0, 1, 0);
			_vertices[2].Position = new Vector3(1, 0, 0);
			_vertices[3].Position = new Vector3(1, 1, 0);

			_vertices[0].TextureCoordinate = new Vector2(0, 0);
			_vertices[1].TextureCoordinate = new Vector2(0, 1);
			_vertices[2].TextureCoordinate = new Vector2(1, 0);
			_vertices[3].TextureCoordinate = new Vector2(1, 1);

			geometryBuffer = new VertexBuffer(GraphicsDevice, VertexPositionTexture.VertexDeclaration, 4, BufferUsage.WriteOnly);
			geometryBuffer.SetData(_vertices);

			short[] _indices = new short[6];
			_indices[5] = 0;
			_indices[4] = 1;
			_indices[3] = 2;
			_indices[2] = 1;
			_indices[1] = 3;
			_indices[0] = 2;

			indexBuffer = new IndexBuffer(GraphicsDevice, typeof(short), 6, BufferUsage.WriteOnly);
			indexBuffer.SetData(_indices);
		}

		private Block[] instances;
				private void CreateInstances()
				{
						instances = new Block[1];
			count = 1;
		/*				int id = 0;
						for (int x = 0; x < 5; x++)
						{
							for (int y = 0; y < 5; y++)
							{
								instances[id].Color = new Vector4(0.2f, 0.8f - (x * 0.03f), 0.5f + (y * 0.03f), 1); ;
								instances[id].Position = new Vector2(200 * x + 20, 200 * y + 20);
								instances[id].Size = new Vector2(180, 180);
								instances[id].Radius = 20;
								id++;
							}
						}*/

						// Set the instace data to the instanceBuffer.
						instanceBuffer = new VertexBuffer(GraphicsDevice, RectDeclaration, instances.Length, BufferUsage.WriteOnly);
						instanceBuffer.SetData(instances);

				}
		int count;
		public void SetBlocks(Block[] blocks)
		{
			count = blocks.Length;
			instanceBuffer = new VertexBuffer(GraphicsDevice, RectDeclaration, count, BufferUsage.WriteOnly);
			instanceBuffer.SetData(blocks.ToArray());
			bindings[0] = new VertexBufferBinding(geometryBuffer);
			bindings[1] = new VertexBufferBinding(instanceBuffer, 0, 1);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

		private Viewport _lastViewport;
		private Matrix _projection;
		public override void Draw(GameTime gameTime)
		{
			var vp = GraphicsDevice.Viewport;
			if ((vp.Width != _lastViewport.Width) || (vp.Height != _lastViewport.Height))
			{
				Matrix.CreateOrthographicOffCenter(0, vp.Width, vp.Height, 0, 0, 1, out _projection);
				if (GraphicsDevice.UseHalfPixelOffset)
				{
					_projection.M41 += -0.5f * _projection.M11;
					_projection.M42 += -0.5f * _projection.M22;
				}

				_lastViewport = vp;
			}

			effect.Parameters["MatrixTransform"].SetValue(_projection);
			effect.CurrentTechnique.Passes[0].Apply();

			GraphicsDevice.Indices = indexBuffer;
			GraphicsDevice.SetVertexBuffers(bindings);
			GraphicsDevice.DrawInstancedPrimitives(PrimitiveType.TriangleList, 0, 0, 2, count);

			base.Draw(gameTime);
		}

		public static readonly VertexDeclaration RectDeclaration = new VertexDeclaration
(
	new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 1),
	new VertexElement(sizeof(float) * 2, VertexElementFormat.Vector4, VertexElementUsage.Color, 0),
	new VertexElement(sizeof(float) * 6, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 2),
	new VertexElement(sizeof(float) * 8, VertexElementFormat.Single, VertexElementUsage.TextureCoordinate, 3)
);
	}
}
