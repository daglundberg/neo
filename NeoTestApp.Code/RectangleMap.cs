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

		struct RectInfo
		{
			public Vector4 Position;
			public Vector4 Color;
			public Vector2 Size;
			public float Radius;
		};

		public RectangleMap(Game game) : base(game){}
		protected override void LoadContent(){effect = Game.Content.Load<Effect>("instance_effect_rect");base.LoadContent();}

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

		// Initialize the VertexBuffer declaration for one rect instance.
		private void InitializeInstanceVertexBufferRect()
		{
			VertexElement[] _instanceStreamElements = new VertexElement[4];

			_instanceStreamElements[0] = new VertexElement(0, VertexElementFormat.Vector4, VertexElementUsage.Position, 1);
			_instanceStreamElements[1] = new VertexElement(sizeof(float) * 4, VertexElementFormat.Vector4, VertexElementUsage.Color, 1);
			_instanceStreamElements[2] = new VertexElement(sizeof(float) * 8, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 2);
			_instanceStreamElements[3] = new VertexElement(sizeof(float) * 10, VertexElementFormat.Single, VertexElementUsage.TextureCoordinate, 3);

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

		int count = 5000;
		Random r = new Random();
		private RectInfo[] instances;
		private void InitializeALLRects()
		{
			instances = new RectInfo[count];
			int id = 0;
			for (int x = 0; x < 100; x++)
			{
				for (int y = 0; y < 50; y++)
				{
					instances[id].Color = new Vector4((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble(), 1);
					instances[id].Position = AbsToScreenPos(new Vector2(20 * x, 20 * y));
					instances[id].Size = new Vector2(19, 19);
					instances[id].Radius = r.Next(85);
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
		//	int id = r.Next(10000);
		//	instances[id].Color = new Vector4((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble(), 1);
		//	instances[id].Radius = (float)r.NextDouble()* 38;

/*			instances[87].Color = new Vector4(0.5f, (float)(1 + Math.Cos(gameTime.TotalGameTime.TotalSeconds)), 0.5f, 1);
			instances[87].Radius = (float)(1 + Math.Cos(gameTime.TotalGameTime.TotalSeconds)) * 42;

			instances[34].Color = new Vector4(0.5f, 1, (float)(1 + Math.Cos(gameTime.TotalGameTime.TotalSeconds)), 1);
			instances[34].Radius = (float)(1 + Math.Sin(gameTime.TotalGameTime.TotalSeconds)) * 50;

			instances[74].Color = new Vector4((float)(1 + Math.Sin(gameTime.TotalGameTime.TotalSeconds)), 0.5f, 0.5f, 1);
			instances[74].Radius = (float)(1 + Math.Sin(gameTime.TotalGameTime.TotalSeconds)) * 38;

			instances[47].Color = new Vector4(0.5f, (float)(1 + Math.Cos(gameTime.TotalGameTime.TotalSeconds)), 0.5f, 1);
			instances[47].Radius = (float)(1 + Math.Cos(gameTime.TotalGameTime.TotalSeconds)) * 42;

			instances[94].Color = new Vector4(0.5f, 1, (float)(1 + Math.Cos(gameTime.TotalGameTime.TotalSeconds)), 1);
			instances[94].Radius = (float)(1 + Math.Sin(gameTime.TotalGameTime.TotalSeconds)) * 50;

			instances[23].Color = new Vector4((float)(1 + Math.Sin(gameTime.TotalGameTime.TotalSeconds)), 0.5f, 0.5f, 1);
			instances[23].Radius = (float)(1 + Math.Sin(gameTime.TotalGameTime.TotalSeconds)) * 38;

			instances[51].Color = new Vector4(0.5f, (float)(1 + Math.Cos(gameTime.TotalGameTime.TotalSeconds)), 0.5f, 1);
			instances[51].Radius = (float)(1 + Math.Cos(gameTime.TotalGameTime.TotalSeconds)) * 42;

			instances[42].Color = new Vector4(0.5f, 1, (float)(1 + Math.Cos(gameTime.TotalGameTime.TotalSeconds)), 1);
			instances[42].Radius = (float)(1 + Math.Sin(gameTime.TotalGameTime.TotalSeconds)) * 50;

			instances[61].Color = new Vector4((float)(1 + Math.Sin(gameTime.TotalGameTime.TotalSeconds)), 0.5f, 0.5f, 1);
			instances[61].Radius = (float)(1 + Math.Sin(gameTime.TotalGameTime.TotalSeconds)) * 38;

			instances[91].Color = new Vector4(0.5f, (float)(1 + Math.Cos(gameTime.TotalGameTime.TotalSeconds)), 0.5f, 1);
			instances[91].Radius = (float)(1 + Math.Cos(gameTime.TotalGameTime.TotalSeconds)) * 42;

			instances[99].Color = new Vector4(0.5f, 1, (float)(1 + Math.Cos(gameTime.TotalGameTime.TotalSeconds)), 1);
			instances[99].Radius = (float)(1 + Math.Sin(gameTime.TotalGameTime.TotalSeconds)) * 50;*/

		//	instanceBuffer.SetData(instances);

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
