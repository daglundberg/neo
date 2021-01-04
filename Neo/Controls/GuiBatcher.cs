/*using Microsoft.Xna.Framework.Graphics;
using System;

namespace Neo
{
	internal class GuiBatcher
	{
		private VertexPositionColorTexture[] _vertexArray;
		private short[] _indices;

		private GuiBatchItem[] _batchItemList;
		private int _batchItemCount;

		private const int _maxBatchSize = short.MaxValue / 6;

		private readonly GraphicsDevice _device;
		private Effect _effect;

		public GuiBatcher(GraphicsDevice device, Effect effect, int capacity = 0)
		{
			_device = device;
			_effect = effect;

			if (capacity <= 0)
				capacity = 256;
			else
				capacity = (capacity + 63) & (~63); // ensure chunks of 64.

			_batchItemList = new GuiBatchItem[capacity];
			_batchItemCount = 0;

			for (int i = 0; i < capacity; i++)
				_batchItemList[i] = new GuiBatchItem();

			EnsureArrayCapacity(capacity);
		}

		// Reuse a previously allocated SpriteBatchItem from the item pool. 
		// if there is none available grow the pool and initialize new items.
		public GuiBatchItem CreateBatchItem()
		{
			if (_batchItemCount >= _batchItemList.Length)
			{
				var oldSize = _batchItemList.Length;
				var newSize = oldSize + oldSize / 2; // grow by x1.5
				newSize = (newSize + 63) & (~63); // grow in chunks of 64.
				Array.Resize(ref _batchItemList, newSize);

				//Initilize all new batch items in the array
				for (int i = oldSize; i < newSize; i++)
					_batchItemList[i] = new GuiBatchItem();

				//..and create indices for all the new ones
				EnsureArrayCapacity(Math.Min(newSize, _maxBatchSize));
			}

			GuiBatchItem item = _batchItemList[_batchItemCount++];
			return item;
		}

		// Resize and recreate the missing indices for the index and vertex position color buffers.
		private unsafe void EnsureArrayCapacity(int numBatchItems)
		{
			int neededCapacity = 6 * numBatchItems;
			if (_indices != null && neededCapacity <= _indices.Length)
				return;

			short[] newIndices = new short[6 * numBatchItems];

			int start = 0;
			if (_indices != null)
			{
				_indices.CopyTo(newIndices, 0);
				start = _indices.Length / 6;
			}

			int q = 0 + (start * 6);
			for (int i = start; i < numBatchItems; i++, q += 6)
			{
				// Triangle 1
				newIndices[q + 0] = (short)(i * 4 + 0);
				newIndices[q + 1] = (short)(i * 4 + 1);
				newIndices[q + 2] = (short)(i * 4 + 2);
				// Triangle 2
				newIndices[q + 3] = (short)(i * 4 + 1);
				newIndices[q + 4] = (short)(i * 4 + 3);
				newIndices[q + 5] = (short)(i * 4 + 2);
			}

			_indices = newIndices;

			_vertexArray = new VertexPositionColorTexture[4 * numBatchItems];
		}

		// Sorts the batch items and then groups batch drawing into maximal allowed batch sets that do not
		// overflow the 16 bit array indices for vertices.
		public unsafe void DrawBatch()
		{
			// Determine how many iterations through the drawing code we need to make
			int batchIndex = 0;
			int batchCount = _batchItemCount;

			// Iterate through the batches, doing short.MaxValue sets of vertices only.
			while (batchCount > 0)
			{
				// setup the vertexArray array
				var startIndex = 0;
				var index = 0;
				Texture2D tex = null;

				int numBatchesToProcess = batchCount;
				if (numBatchesToProcess > _maxBatchSize)
					numBatchesToProcess = _maxBatchSize;


				for (int i = 0; i < numBatchesToProcess; i++, batchIndex++, index += 4)
				{
					GuiBatchItem item = _batchItemList[batchIndex];

					// if the texture changed, we need to flush and bind the new texture
					bool shouldFlush = !ReferenceEquals(item.Texture, tex) && item.Texture != null;
					if (shouldFlush)
					{
						FlushVertexArray(startIndex, index, tex);

						tex = item.Texture;
						startIndex = index = 0;
					}

					_vertexArray[index + 0] = item.vertexTL;
					_vertexArray[index + 1] = item.vertexTR;
					_vertexArray[index + 2] = item.vertexBL;
					_vertexArray[index + 3] = item.vertexBR;

					// Release the texture.
					item.Texture = null;
				}

				// flush the remaining vertexArray data
				FlushVertexArray(startIndex, index, tex);
				// Update our batch count to continue the process of culling down
				// large batches
				batchCount -= numBatchesToProcess;
			}
			// return items to the pool.  
			_batchItemCount = 0;
		}

		// Sends the triangle list to the graphics device. Here is where the actual drawing starts.
		private void FlushVertexArray(int start, int end, Texture texture)
		{
			if (start == end)
				return;


			var vertexCount = end - start;

*//*			if (texture != null)
				_effect.Parameters["Texture"].SetValue(texture);*//*

			_effect.Techniques[0].Passes[0].Apply();
			_device.DrawUserIndexedPrimitives(
					PrimitiveType.TriangleList,
					_vertexArray,
					0,
					vertexCount,
					_indices,
					0,
					(vertexCount / 4) * 2,
					VertexPositionColorTexture.VertexDeclaration);
		}
	}
}

*/