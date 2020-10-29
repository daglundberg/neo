using Microsoft.Xna.Framework; using Microsoft.Xna.Framework.Graphics; using System;

namespace NeoTestApp.Code
{
	public class RectangleMap : DrawableGameComponent
	{

		private Effect effect;

		private VertexDeclaration instanceVertexDeclarationRect;

		private VertexBuffer instanceBuffer, geometryBuffer;
		private IndexBuffer indexBuffer;

		private VertexBufferBinding[] bindings;
		private RectInfo[] instances;

		struct RectInfo
		{
			public Vector4 Position;
			public Vector4 Color;
			public Vector2 Size;
			public Vector2 UV;
		};

		public RectangleMap(Game game) : base(game)
		{

		}

		public override void Initialize()
		{
			InitializeInstanceVertexBufferRect();
			GenerateCommonGeometryRect();
			InitializeALLRects();

			// Creates the binding between the geometry and the instances.
			bindings = new VertexBufferBinding[2];
			bindings[0] = new VertexBufferBinding(geometryBuffer);
			bindings[1] = new VertexBufferBinding(instanceBuffer, 0, 1);

			base.Initialize();
		}

		protected override void LoadContent()
		{
			effect = Game.Content.Load<Effect>("instance_effect_rect");
			
			base.LoadContent();
		}

		// Initialize the VertexBuffer declaration for one rect instance.
		private void InitializeInstanceVertexBufferRect()
		{
			VertexElement[] _instanceStreamElements = new VertexElement[4];

			_instanceStreamElements[0] = new VertexElement(0, VertexElementFormat.Vector4, VertexElementUsage.Position, 1);
			_instanceStreamElements[1] = new VertexElement(sizeof(float) * 4, VertexElementFormat.Vector4, VertexElementUsage.Color, 1);
			_instanceStreamElements[2] = new VertexElement(sizeof(float) * 8, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0);
			_instanceStreamElements[3] = new VertexElement(sizeof(float) * 10, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 1);

			instanceVertexDeclarationRect = new VertexDeclaration(_instanceStreamElements);
		}

		private void GenerateCommonGeometryRect()
		{
			VertexPositionTexture[] _vertices = new VertexPositionTexture[4];
			_vertices[0].Position = new Vector3(0, -1, 0);
			_vertices[1].Position = new Vector3(0,  0, 0);
			_vertices[2].Position = new Vector3( 1, -1, 0);
			_vertices[3].Position = new Vector3( 1,  0, 0);

			_vertices[0].TextureCoordinate = new Vector2(0, 0);
			_vertices[1].TextureCoordinate = new Vector2(0, 1);
			_vertices[2].TextureCoordinate = new Vector2(1, 0);
			_vertices[3].TextureCoordinate = new Vector2(1, 1);

			geometryBuffer = new VertexBuffer(GraphicsDevice, VertexPositionTexture.VertexDeclaration, 4, BufferUsage.WriteOnly);
			geometryBuffer.SetData(_vertices);

			short[] _indices = new short[6];
			_indices[0] = 0;
			_indices[1] = 1;
			_indices[2] = 2;
			_indices[3] = 1;
			_indices[4] = 3;
			_indices[5] = 2;

			indexBuffer = new IndexBuffer(GraphicsDevice, typeof(short), 6, BufferUsage.WriteOnly);
			indexBuffer.SetData(_indices);
		}

		Vector4 AbsToScreenPos(Vector2 _position)
		{
			Vector2 _pos = new Vector2((((_position.X / (float)Game.GraphicsDevice.Viewport.Width) * 2f) - 1f), (((_position.Y / Game.GraphicsDevice.Viewport.Height) * 2f) - 1f) * -1.0f);
			return new Vector4(_pos.X, _pos.Y, 0, 0);
		}

		int count = 100;
		
		private void InitializeALLRects()
		{
			Random r = new Random();
			instances = new RectInfo[count];
			int id = 0;
			for (int x = 0; x < 10; x++)
			{
				for (int y = 0; y < 10; y++)
				{
					instances[id].Color = new Vector4((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble(), 1);
					instances[id].Position = AbsToScreenPos(new Vector2(30 * x, 20 * y));
					instances[id].Size = new Vector2(30, 20);
					id++;
				}
			}


			// Set the instace data to the instanceBuffer.
			instanceBuffer = new VertexBuffer(GraphicsDevice, instanceVertexDeclarationRect, count, BufferUsage.WriteOnly);
			instanceBuffer.SetData(instances);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			// Set the effect technique and parameters
			Vector2 scale = new Vector2(2.0f / (float)Game.GraphicsDevice.Viewport.Width, 2.0f / (float)Game.GraphicsDevice.Viewport.Height);
			effect.CurrentTechnique = effect.Techniques["Instancing"];
			effect.Parameters["Scale"].SetValue(scale);

			GraphicsDevice.Indices = indexBuffer;

			effect.CurrentTechnique.Passes[0].Apply();

			// Set the vertex buffer and draw the instanced primitives.
			GraphicsDevice.SetVertexBuffers(bindings);
			GraphicsDevice.DrawInstancedPrimitives(PrimitiveType.TriangleList, 0, 0, 2, count);

			base.Draw(gameTime);
		}
	}
}
