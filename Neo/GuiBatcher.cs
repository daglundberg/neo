using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Neo
{
	internal partial class GuiBatcher
	{
		private GuiBatchItem[] _batchItemArray;
		private Block[] _instances;

		private VertexBufferBinding[] _bindings;
		private VertexBuffer _instanceBuffer, _commonGeometry;

		private IndexBuffer _indexBuffer;

		private int _totalNumBatchItems;

		private readonly GraphicsDevice _device;
		private Effect _neoEffect;
		private Neo _neo;

		public GuiBatcher(GraphicsDevice device, Effect neoEffect, Neo neo)
		{
			_neo = neo;
			_device = device;
			_neoEffect = neoEffect;

			int capacity = 256; // should be chunks of 64.


			_batchItemArray = new GuiBatchItem[capacity];
			_instances = new Block[capacity];

			_totalNumBatchItems = 0;

			for (int i = 0; i < capacity; i++)
				_batchItemArray[i] = new GuiBatchItem();

			VertexPositionTexture[] _vertices = new VertexPositionTexture[4];
			_vertices[0].Position = new Vector3(0, 0, 0);
			_vertices[1].Position = new Vector3(0, 1, 0);
			_vertices[2].Position = new Vector3(1, 0, 0);
			_vertices[3].Position = new Vector3(1, 1, 0);

			_vertices[0].TextureCoordinate = new Vector2(0, 0);
			_vertices[1].TextureCoordinate = new Vector2(0, 1);
			_vertices[2].TextureCoordinate = new Vector2(1, 0);
			_vertices[3].TextureCoordinate = new Vector2(1, 1);

			_commonGeometry = new VertexBuffer(_device, VertexPositionTexture.VertexDeclaration, 4, BufferUsage.WriteOnly);
			_commonGeometry.SetData(_vertices);

			_indexBuffer = new IndexBuffer(_device, typeof(short), 6, BufferUsage.WriteOnly);
			_indexBuffer.SetData(new short[] { 2, 3, 1, 2, 1, 0 });

			_instanceBuffer = new VertexBuffer(_device, RectDeclaration, _instances.Length, BufferUsage.WriteOnly);

			// Creates the binding between the geometry and the instances.
			_bindings = new VertexBufferBinding[2];
			_bindings[0] = new VertexBufferBinding(_commonGeometry);
			_bindings[1] = new VertexBufferBinding(_instanceBuffer, 0, 1);
			_device.Indices = _indexBuffer;
			_device.SetVertexBuffers(_bindings);

			//---
			EnsureArrayCapacity(capacity);
			//---
		}

		public GuiBatchItem CreateBatchItem()
		{
			if (_totalNumBatchItems >= _batchItemArray.Length)
			{
				int oldSize = _batchItemArray.Length;
				int newSize = oldSize + oldSize / 2; // grow by x1.5
				newSize = (newSize + 63) & (~63); // grow in chunks of 64.
				Array.Resize(ref _batchItemArray, newSize);
				Array.Resize(ref _instances, newSize);

				_instanceBuffer = new VertexBuffer(_device, RectDeclaration, _instances.Length, BufferUsage.WriteOnly);

				// Creates the binding between the geometry and the instances.
				_bindings = new VertexBufferBinding[2];
				_bindings[0] = new VertexBufferBinding(_commonGeometry);
				_bindings[1] = new VertexBufferBinding(_instanceBuffer, 0, 1);
				_device.Indices = _indexBuffer;
				_device.SetVertexBuffers(_bindings);

				//Initilize all new batch items in the array
				for (int i = oldSize; i < newSize; i++)
				{
					_batchItemArray[i] = new GuiBatchItem();
					_instances[i] = new Block();
				}

				EnsureArrayCapacity(Math.Min(newSize, MaxBatchSize));
			}

			GuiBatchItem item = _batchItemArray[_totalNumBatchItems++];
			return item;
		}

		GuiBatchItem.Types _lastType = GuiBatchItem.Types.None;

		public unsafe void DrawBatches()
		{
			int i = 0;
			while (i < _totalNumBatchItems)
			{
				GuiBatchItem item = _batchItemArray[i];

				int index = 0;

				if (item.Type == GuiBatchItem.Types.Rectangle)
				{
					do
					{
						if (i < _totalNumBatchItems)
							i++;
						else
							break;

						_instances[index].Position = item.Position;
						_instances[index].Size = item.Size;
						_instances[index].Color = item.Color;
						_instances[index].Radius = item.Radius;

						index++;
						_lastType = item.Type;
						item = _batchItemArray[i];
					}
					while (_lastType == item.Type);

					DrawBlockBatch(index);
				}
				else if (item.Type == GuiBatchItem.Types.Texture)
				{
					int indic;
					Texture2D tex = item.Texture;
					do
					{
						indic = index * 4;

						if (i < _totalNumBatchItems)
							i++;
						else
							break;

						_vertexArray[indic] = item.vertexTL;
						_vertexArray[indic + 1] = item.vertexTR;
						_vertexArray[indic + 2] = item.vertexBL;
						_vertexArray[indic + 3] = item.vertexBR;


						index++;
						_lastType = item.Type;
						item = _batchItemArray[i];
						
					}
					while (_lastType == item.Type);

					FlushVertexArray(0, indic+4, tex);
				}					
			}
			_totalNumBatchItems = 0;
			_lastType = GuiBatchItem.Types.None;
		}

		private void DrawBlockBatch(int count)
		{
			_instanceBuffer.SetData(_instances);
			_neoEffect.Parameters["Scale"].SetValue(_neo.Scale);
			_neoEffect.Techniques[0].Passes[0].Apply();
			_device.DrawInstancedPrimitives(PrimitiveType.TriangleList, 0, 0, 2, count);
		}

		private void FlushVertexArray(int start, int end, Texture texture)
		{
			if (start == end)
				return;

			var vertexCount = end - start;

			_neoEffect.Techniques[2].Passes[0].Apply();
			_neoEffect.Parameters["tex"].SetValue(texture);
			_device.Textures[0] = texture;

			_device.DrawUserIndexedPrimitives(
						PrimitiveType.TriangleList,
						_vertexArray,
						0,
						vertexCount,
						_index,
						0,
						(vertexCount / 4) * 2,
						VertexPositionTexture.VertexDeclaration);
		}

		public static readonly VertexDeclaration RectDeclaration = new VertexDeclaration(
			new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 1),
			new VertexElement(sizeof(float) * 2, VertexElementFormat.Vector4, VertexElementUsage.Color, 0),
			new VertexElement(sizeof(float) * 6, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 2),
			new VertexElement(sizeof(float) * 8, VertexElementFormat.Single, VertexElementUsage.TextureCoordinate, 3)
		);
	}
}

