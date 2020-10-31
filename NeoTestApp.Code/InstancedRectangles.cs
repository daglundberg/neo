using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NeoTestApp.Code
{
	public class InstancedRectangles : DrawableGameComponent
	{
		private Effect effect;
		private VertexBuffer instanceBuffer, geometryBuffer;
		private IndexBuffer indexBuffer;

		private VertexBufferBinding[] bindings;

		public InstancedRectangles(Game game) : base(game) { }

		protected override void LoadContent()
		{
			effect = Game.Content.Load<Effect>("InstancingRectangleShader");
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

		private RectangleInstance[] instances;
		private void CreateInstances()
		{
			instances = new RectangleInstance[200];

			int id = 0;
			for (int x = 0; x < 20; x++)
			{
				for (int y = 0; y < 10; y++)
				{
					instances[id].Color = new Vector4(0.2f, 0.8f - (x * 0.03f), 0.5f + (y * 0.03f), 1); ;
					instances[id].Position = new Vector2(80 * x, 80 * y);
					instances[id].Size = new Vector2(70, 70);
					instances[id].Radius = 20;
					id++;
				}
			}

			// Set the instace data to the instanceBuffer.
			instanceBuffer = new VertexBuffer(GraphicsDevice, RectangleInstance.VertexDeclaration, instances.Length, BufferUsage.WriteOnly);
			instanceBuffer.SetData(instances);
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
				// Normal 3D cameras look into the -z direction (z = 1 is in front of z = 0). The
				// sprite batch layer depth is the opposite (z = 0 is in front of z = 1).
				// --> We get the correct matrix with near plane 0 and far plane -1.
				Matrix.CreateOrthographicOffCenter(0, vp.Width, vp.Height, 0, 0, -1, out _projection);

				if (GraphicsDevice.UseHalfPixelOffset)
				{
					_projection.M41 += -0.5f * _projection.M11;
					_projection.M42 += -0.5f * _projection.M22;
				}

				_lastViewport = vp;
			}

			// Set the effect technique and parameters
			Vector2 scale = new Vector2(2.0f / (float)Game.GraphicsDevice.Viewport.Width, 2.0f / (float)Game.GraphicsDevice.Viewport.Height);
			effect.CurrentTechnique = effect.Techniques["Instancing"];
			effect.Parameters["MatrixTransform"].SetValue(_projection);

			//RasterizerState rasterizerState = new RasterizerState();
			//rasterizerState.CullMode = CullMode.None;
			//GraphicsDevice.RasterizerState = rasterizerState;
			GraphicsDevice.Indices = indexBuffer;

			effect.CurrentTechnique.Passes[0].Apply();

			// Set the vertex buffer and draw the instanced primitives.
			GraphicsDevice.SetVertexBuffers(bindings);
			GraphicsDevice.DrawInstancedPrimitives(PrimitiveType.TriangleList, 0, 0, 2, instances.Length);

			base.Draw(gameTime);
		}


		public struct RectangleInstance
		{
			public Vector2 Position;
			public Vector4 Color;
			public Vector2 Size;
			public float Radius;

			public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration
			(
				new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 1),
				new VertexElement(sizeof(float) * 2, VertexElementFormat.Vector4, VertexElementUsage.Color, 0),
				new VertexElement(sizeof(float) * 6, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 2),
				new VertexElement(sizeof(float) * 8, VertexElementFormat.Single, VertexElementUsage.TextureCoordinate, 3)
			);
		};
	}
}
