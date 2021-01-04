using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Neo
{
	public class InstancedRectangles : DrawableGameComponent
	{
		private Effect _effectInstanced;
		private VertexBuffer _instanceBuffer, _commonGeometry;
		private IndexBuffer _indexBuffer;

		private VertexBufferBinding[] _bindings;
		private Neo _neo;

		public InstancedRectangles(Game game, Neo neo) : base(game)
		{
			_neo = neo;
			_effectInstanced = Game.Content.Load<Effect>("InstancingRectangleShader");
			Initialize();
		}

		protected override void LoadContent()
		{
			base.LoadContent();
		}

		public override void Initialize()
		{
			GenerateCommonGeometryRect();
			CreateInstances();

			// Creates the binding between the geometry and the instances.
			_bindings = new VertexBufferBinding[2];
			_bindings[0] = new VertexBufferBinding(_commonGeometry);
			_bindings[1] = new VertexBufferBinding(_instanceBuffer, 0, 1);

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

			_commonGeometry = new VertexBuffer(GraphicsDevice, VertexPositionTexture.VertexDeclaration, 4, BufferUsage.WriteOnly);
			_commonGeometry.SetData(_vertices);

			short[] _indices = new short[6];
			_indices[5] = 0;
			_indices[4] = 1;
			_indices[3] = 2;
			_indices[2] = 1;
			_indices[1] = 3;
			_indices[0] = 2;

			_indexBuffer = new IndexBuffer(GraphicsDevice, typeof(short), 6, BufferUsage.WriteOnly);
			_indexBuffer.SetData(_indices);
		}

		private Block[] _instances;
		private void CreateInstances()
		{
			_instances = new Block[1];
			_count = 1;
			// Set the instance data to the instanceBuffer.
			_instanceBuffer = new VertexBuffer(GraphicsDevice, RectDeclaration, _instances.Length, BufferUsage.WriteOnly);
			_instanceBuffer.SetData(_instances);
		}

		int _count;
		public void SetBlocks(Block[] blocks)
		{
			_count = blocks.Length;
			_instanceBuffer = new VertexBuffer(GraphicsDevice, RectDeclaration, _count, BufferUsage.WriteOnly);
			_instanceBuffer.SetData(blocks.ToArray());
			_bindings[0] = new VertexBufferBinding(_commonGeometry);
			_bindings[1] = new VertexBufferBinding(_instanceBuffer, 0, 1);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

		private Viewport _lastViewport;
		private Matrix _projection;
		float _lastScale = 0;
		public override void Draw(GameTime gameTime)
		{
			CheckScreenResolution();

			_effectInstanced.Parameters["MatrixTransform"].SetValue(_projection);
			_effectInstanced.CurrentTechnique.Passes[0].Apply();

			GraphicsDevice.Indices = _indexBuffer;
			GraphicsDevice.SetVertexBuffers(_bindings);
			GraphicsDevice.DrawInstancedPrimitives(PrimitiveType.TriangleList, 0, 0, 2, _count);

			base.Draw(gameTime);
		}

		private void CheckScreenResolution()
		{
			var vp = GraphicsDevice.Viewport;
			if ((vp.Width != _lastViewport.Width) || (vp.Height != _lastViewport.Height) || _lastScale != _neo.Scale)
			{
				Matrix.CreateOrthographicOffCenter(0, vp.Width / _neo.Scale, vp.Height / _neo.Scale, 0, 0, 1, out _projection);
				if (GraphicsDevice.UseHalfPixelOffset)
				{
					_projection.M41 += -0.5f * _projection.M11;
					_projection.M42 += -0.5f * _projection.M22;
				}

				_lastViewport = vp;
				_lastScale = _neo.Scale;
			}
		}

		public static readonly VertexDeclaration RectDeclaration = new VertexDeclaration(
			new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 1),
			new VertexElement(sizeof(float) * 2, VertexElementFormat.Vector4, VertexElementUsage.Color, 0),
			new VertexElement(sizeof(float) * 6, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 2),
			new VertexElement(sizeof(float) * 8, VertexElementFormat.Single, VertexElementUsage.TextureCoordinate, 3)
		);
	}
}
