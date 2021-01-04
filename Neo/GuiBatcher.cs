using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Neo
{
	internal class GuiBatcher
	{
		private GuiBatchItem[] _batchItemArray;
		private Block[] _instances;

		private VertexBufferBinding[] _bindings;
		private VertexBuffer _instanceBuffer, _commonGeometry;
		private IndexBuffer _indexBuffer;

		private int _totalNumBatchItems;

		private readonly GraphicsDevice _device;
		private Effect _effectInstanced;

		public GuiBatcher(GraphicsDevice device, Effect effect, Effect effectInstanced, int capacity = 0)
		{
			_device = device;
			_effectInstanced = effectInstanced;

			if (capacity <= 0)
				capacity = 256;
			else
				capacity = (capacity + 63) & (~63); // ensure chunks of 64.

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
		}

		public GuiBatchItem CreateBatchItem()
		{
			if (_totalNumBatchItems >= _batchItemArray.Length)
			{
				int oldSize = _batchItemArray.Length;
				int newSize = oldSize + oldSize / 2; // grow by x1.5
				newSize = (newSize + 63) & (~63); // grow in chunks of 64.
				Array.Resize(ref _batchItemArray, newSize);

				//Initilize all new batch items in the array
				for (int i = oldSize; i < newSize; i++)
				{
					_batchItemArray[i] = new GuiBatchItem();
					_instances[i] = new Block();
				}
			}

			GuiBatchItem item = _batchItemArray[_totalNumBatchItems++];
			return item;
		}

		GuiBatchItem.Types _lastType = GuiBatchItem.Types.None;

		public void DrawBatches()
		{
			int i = 0;
			while(i < _totalNumBatchItems)
			{
				GuiBatchItem item = _batchItemArray[i];
				var batchType = item.Type;

				int index = 0;

				do
				{
					_instances[index].Position = item.Position;
					_instances[index].Size = item.Size;
					_instances[index].Color = item.Color;
					_instances[index].Radius = item.Radius;

					if (i < _totalNumBatchItems)
						i++;
					else
						break;

					index++;
					_lastType = item.Type;
					item = _batchItemArray[i];
				}
				while (_lastType == item.Type);

				if (batchType == GuiBatchItem.Types.Rectangle)
					DrawBlockBatch(index);
				else if (batchType == GuiBatchItem.Types.Texture)
					DrawTextureBatch(index, _batchItemArray[i - 1].Texture);

				//item.Texture = null;
				_lastType = item.Type;
			}
			_totalNumBatchItems = 0;
			_lastType = GuiBatchItem.Types.None;
		}

		private void DrawTextureBatch(int count, Texture2D texture)
		{
			_effectInstanced.Techniques[1].Passes[0].Apply();
			_effectInstanced.Parameters["tex"].SetValue(texture);
			_instanceBuffer.SetData(_instances);
			_device.DrawInstancedPrimitives(PrimitiveType.TriangleList, 0, 0, 2, count);
		}

		private void DrawBlockBatch(int count)
		{
			_effectInstanced.Techniques[0].Passes[0].Apply();
			_instanceBuffer.SetData(_instances);
			_device.DrawInstancedPrimitives(PrimitiveType.TriangleList, 0, 0, 2, count);
		}

		public static readonly VertexDeclaration RectDeclaration = new VertexDeclaration(
			new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 1),
			new VertexElement(sizeof(float) * 2, VertexElementFormat.Vector4, VertexElementUsage.Color, 0),
			new VertexElement(sizeof(float) * 6, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 2),
			new VertexElement(sizeof(float) * 8, VertexElementFormat.Single, VertexElementUsage.TextureCoordinate, 3)
		);
	}
}

