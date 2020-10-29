using Microsoft.Xna.Framework; using Microsoft.Xna.Framework.Graphics; using System;

namespace NeoTestApp.Code
{
	public class CubeMap : DrawableGameComponent
	{
		private Texture2D texture;
		private Effect effect;

		private VertexDeclaration instanceVertexDeclarationCube;

		private VertexBuffer instanceBuffer, geometryBuffer;
		private IndexBuffer indexBuffer;

		private VertexBufferBinding[] bindings;
		private CubeInfo[] instances;

		struct CubeInfo
		{
			public Vector4 World;
			public Vector2 AtlasCoordinate;
		};

		private short _sizeX, _sizeZ;
		public int InstanceCount { get { return _sizeX * _sizeZ; } }

		public Matrix View { get; set; }
		public Matrix Projection { get; set; }

		public CubeMap(Game game, short sizeX, short sizeZ) : base(game)
		{
			_sizeX = sizeX;
			_sizeZ = sizeZ;
		}

		public override void Initialize()
		{
			InitializeInstanceVertexBufferCube();
			GenerateCommonGeometryCube();
			InitializeALLCubes();

			// Creates the binding between the geometry and the instances.
			bindings = new VertexBufferBinding[2];
			bindings[0] = new VertexBufferBinding(geometryBuffer);
			bindings[1] = new VertexBufferBinding(instanceBuffer, 0, 1);

			base.Initialize();
		}

		protected override void LoadContent()
		{
			effect = Game.Content.Load<Effect>("instance_effect");
			texture = Game.Content.Load<Texture2D>("stone");
			base.LoadContent();
		}

		// Initialize the VertexBuffer declaration for one cube instance.
		private void InitializeInstanceVertexBufferCube()
		{
			VertexElement[] _instanceStreamElements = new VertexElement[2];

			_instanceStreamElements[0] = new VertexElement(0, VertexElementFormat.Vector4,	VertexElementUsage.Position, 1);
			_instanceStreamElements[1] = new VertexElement(sizeof(Single) * 4, VertexElementFormat.Vector2,	VertexElementUsage.TextureCoordinate, 1);

			instanceVertexDeclarationCube = new VertexDeclaration(_instanceStreamElements);
		}

		private void GenerateCommonGeometryCube()
		{
			VertexPositionTexture[] _vertices = new VertexPositionTexture[24];

			#region filling vertices
			_vertices[0].Position = new Vector3(-1, 1, -1);
			_vertices[0].TextureCoordinate = new Vector2(0, 0);
			_vertices[1].Position = new Vector3(1, 1, -1);
			_vertices[1].TextureCoordinate = new Vector2(1, 0);
			_vertices[2].Position = new Vector3(-1, 1, 1);
			_vertices[2].TextureCoordinate = new Vector2(0, 1);
			_vertices[3].Position = new Vector3(1, 1, 1);
			_vertices[3].TextureCoordinate = new Vector2(1, 1);

			_vertices[4].Position = new Vector3(-1, -1, 1);
			_vertices[4].TextureCoordinate = new Vector2(0, 0);
			_vertices[5].Position = new Vector3(1, -1, 1);
			_vertices[5].TextureCoordinate = new Vector2(1, 0);
			_vertices[6].Position = new Vector3(-1, -1, -1);
			_vertices[6].TextureCoordinate = new Vector2(0, 1);
			_vertices[7].Position = new Vector3(1, -1, -1);
			_vertices[7].TextureCoordinate = new Vector2(1, 1);

			_vertices[8].Position = new Vector3(-1, 1, -1);
			_vertices[8].TextureCoordinate = new Vector2(0, 0);
			_vertices[9].Position = new Vector3(-1, 1, 1);
			_vertices[9].TextureCoordinate = new Vector2(1, 0);
			_vertices[10].Position = new Vector3(-1, -1, -1);
			_vertices[10].TextureCoordinate = new Vector2(0, 1);
			_vertices[11].Position = new Vector3(-1, -1, 1);
			_vertices[11].TextureCoordinate = new Vector2(1, 1);

			_vertices[12].Position = new Vector3(-1, 1, 1);
			_vertices[12].TextureCoordinate = new Vector2(0, 0);
			_vertices[13].Position = new Vector3(1, 1, 1);
			_vertices[13].TextureCoordinate = new Vector2(1, 0);
			_vertices[14].Position = new Vector3(-1, -1, 1);
			_vertices[14].TextureCoordinate = new Vector2(0, 1);
			_vertices[15].Position = new Vector3(1, -1, 1);
			_vertices[15].TextureCoordinate = new Vector2(1, 1);

			_vertices[16].Position = new Vector3(1, 1, 1);
			_vertices[16].TextureCoordinate = new Vector2(0, 0);
			_vertices[17].Position = new Vector3(1, 1, -1);
			_vertices[17].TextureCoordinate = new Vector2(1, 0);
			_vertices[18].Position = new Vector3(1, -1, 1);
			_vertices[18].TextureCoordinate = new Vector2(0, 1);
			_vertices[19].Position = new Vector3(1, -1, -1);
			_vertices[19].TextureCoordinate = new Vector2(1, 1);

			_vertices[20].Position = new Vector3(1, 1, -1);
			_vertices[20].TextureCoordinate = new Vector2(0, 0);
			_vertices[21].Position = new Vector3(-1, 1, -1);
			_vertices[21].TextureCoordinate = new Vector2(1, 0);
			_vertices[22].Position = new Vector3(1, -1, -1);
			_vertices[22].TextureCoordinate = new Vector2(0, 1);
			_vertices[23].Position = new Vector3(-1, -1, -1);
			_vertices[23].TextureCoordinate = new Vector2(1, 1);
			#endregion

			geometryBuffer = new VertexBuffer(GraphicsDevice, VertexPositionTexture.VertexDeclaration, 24, BufferUsage.WriteOnly);
			geometryBuffer.SetData(_vertices);

			#region filling indices

			Int16[] _indices = new Int16[36];
			_indices[0] = 0; _indices[1] = 1; _indices[2] = 2;
			_indices[3] = 1; _indices[4] = 3; _indices[5] = 2;

			_indices[6] = 4; _indices[7] = 5; _indices[8] = 6;
			_indices[9] = 5; _indices[10] = 7; _indices[11] = 6;

			_indices[12] = 8; _indices[13] = 9; _indices[14] = 10;
			_indices[15] = 9; _indices[16] = 11; _indices[17] = 10;

			_indices[18] = 12; _indices[19] = 13; _indices[20] = 14;
			_indices[21] = 13; _indices[22] = 15; _indices[23] = 14;

			_indices[24] = 16; _indices[25] = 17; _indices[26] = 18;
			_indices[27] = 17; _indices[28] = 19; _indices[29] = 18;

			_indices[30] = 20; _indices[31] = 21; _indices[32] = 22;
			_indices[33] = 21; _indices[34] = 23; _indices[35] = 22;

			#endregion

			indexBuffer = new IndexBuffer(GraphicsDevice, typeof(Int16), 36, BufferUsage.WriteOnly);
			indexBuffer.SetData(_indices);
		}

		// Initialize all the cube instance. (sizeX * sizeZ)
		private void InitializeALLCubes()
		{
			Random _randomHeight = new Random();
			instances = new CubeInfo[InstanceCount];

			// Set the position for each cube.
			for (Int32 i = 0; i < _sizeX; ++i)
			{
				for (Int32 j = 0; j < _sizeZ; ++j)
				{
					instances[i * _sizeX + j].World = new Vector4(i * 2, _randomHeight.Next(0, 2), j * 2, 1);
					instances[i * _sizeX + j].AtlasCoordinate = new Vector2(_randomHeight.Next(0, 2), _randomHeight.Next(0, 2));
				}
			}

			// Set the instace data to the instanceBuffer.
			instanceBuffer = new VertexBuffer(GraphicsDevice, instanceVertexDeclarationCube, InstanceCount, BufferUsage.WriteOnly);
			instanceBuffer.SetData(instances);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			// Set the effect technique and parameters
			effect.CurrentTechnique = effect.Techniques["Instancing"];
			effect.Parameters["WorldViewProjection"].SetValue(View * Projection);
			effect.Parameters["cubeTexture"].SetValue(texture);

			// Set the indices in the graphics device.
			GraphicsDevice.Indices = indexBuffer;

			// Apply the current technique pass.
			effect.CurrentTechnique.Passes[0].Apply();

			// Set the vertex buffer and draw the instanced primitives.
			GraphicsDevice.SetVertexBuffers(bindings);
			GraphicsDevice.DrawInstancedPrimitives(PrimitiveType.TriangleList, 0, 0, 12, InstanceCount);

			base.Draw(gameTime);
		}
	}
}
