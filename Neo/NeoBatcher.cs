namespace Neo;

internal class NeoBatcher
{
	private const int InitialBatchSize = 256;
	private const int MaxBatchSize = short.MaxValue / 6; // 6 = 4 vertices unique and 2 shared, per quad
	private const int InitialVertexArraySize = 256;

	public static readonly VertexDeclaration BlockDeclaration = new(
		new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
		new VertexElement(sizeof(float) * 2, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
		new VertexElement(sizeof(float) * 4, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 1),
		new VertexElement(sizeof(float) * 6, VertexElementFormat.Vector4, VertexElementUsage.Color, 0),
		new VertexElement(sizeof(float) * 10, VertexElementFormat.Single, VertexElementUsage.TextureCoordinate, 2)
	);

	private readonly GraphicsDevice _device;

	private readonly Effect _effect;

	// Index pointer to the next available SpriteBatchItem in _batchItemList.
	private int _batchItemCount;

	private NeoBatchItem[] _batchItemList;

	// Vertex index array. The values in this array never change.
	private short[] _index;

	private BlockVertex[] _vertexArray;

	public NeoBatcher(GraphicsDevice device, Effect effect, int capacity = 0)
	{
		_device = device;
		_effect = effect;

		if (capacity <= 0)
			capacity = InitialBatchSize;
		else
			capacity = (capacity + 63) & ~63; // ensure chunks of 64.

		_batchItemList = new NeoBatchItem[capacity];
		_batchItemCount = 0;

		for (var i = 0; i < capacity; i++)
			_batchItemList[i] = new NeoBatchItem();

		EnsureArrayCapacity(capacity);
	}

	/// <summary>
	///     Reuse a previously allocated SpriteBatchItem from the item pool.
	///     if there is none available grow the pool and initialize new items.
	/// </summary>
	public NeoBatchItem CreateBatchItem()
	{
		if (_batchItemCount >= _batchItemList.Length)
		{
			var oldSize = _batchItemList.Length;
			var newSize = oldSize + oldSize / 2; // grow by x1.5
			newSize = (newSize + 63) & ~63; // grow in chunks of 64.
			Array.Resize(ref _batchItemList, newSize);
			for (var i = oldSize; i < newSize; i++)
				_batchItemList[i] = new NeoBatchItem();

			EnsureArrayCapacity(Math.Min(newSize, MaxBatchSize));
		}

		var item = _batchItemList[_batchItemCount++];
		return item;
	}

	/// <summary>
	///     Resize and recreate the missing indices for the index and vertex position color buffers.
	/// </summary>
	/// <param name="numBatchItems"></param>
	private unsafe void EnsureArrayCapacity(int numBatchItems)
	{
		var neededCapacity = 6 * numBatchItems;
		if (_index != null && neededCapacity <= _index.Length)
			// Short circuit out of here because we have enough capacity.
			return;
		var newIndex = new short[6 * numBatchItems];
		var start = 0;
		if (_index != null)
		{
			_index.CopyTo(newIndex, 0);
			start = _index.Length / 6;
		}

		fixed (short* indexFixedPtr = newIndex)
		{
			var indexPtr = indexFixedPtr + start * 6;
			for (var i = start; i < numBatchItems; i++, indexPtr += 6)
			{
				/*
                 *  TL    TR
                 *   0----1 0,1,2,3 = index offsets for vertex indices
                 *   |   /| TL,TR,BL,BR are vertex references in SpriteBatchItem.
                 *   |  / |
                 *   | /  |
                 *   |/   |
                 *   2----3
                 *  BL    BR
                 */
				// Triangle 1
				*(indexPtr + 0) = (short) (i * 4);
				*(indexPtr + 1) = (short) (i * 4 + 1);
				*(indexPtr + 2) = (short) (i * 4 + 2);
				// Triangle 2
				*(indexPtr + 3) = (short) (i * 4 + 1);
				*(indexPtr + 4) = (short) (i * 4 + 3);
				*(indexPtr + 5) = (short) (i * 4 + 2);
			}
		}

		_index = newIndex;

		_vertexArray = new BlockVertex[4 * numBatchItems];
	}

	/// <summary>
	///     Sorts the batch items and then groups batch drawing into maximal allowed batch sets that do not
	///     overflow the 16 bit array indices for vertices.
	/// </summary>
	public unsafe void DrawBatch()
	{
		// Determine how many iterations through the drawing code we need to make
		var batchIndex = 0;
		var batchCount = _batchItemCount;

		// Iterate through the batches, doing short.MaxValue sets of vertices only.
		while (batchCount > 0)
		{
			// setup the vertexArray array
			var startIndex = 0;
			var index = 0;
			Texture2D tex = null;

			var numBatchesToProcess = batchCount;
			if (numBatchesToProcess > MaxBatchSize) numBatchesToProcess = MaxBatchSize;
			NeoBatchItem.ItemType lastType;
			// Avoid the array checking overhead by using pointer indexing!
			fixed (BlockVertex* vertexArrayFixedPtr = _vertexArray)
			{
				var vertexArrayPtr = vertexArrayFixedPtr;


				if (_batchItemList[0] != null)
					lastType = _batchItemList[0].Type;
				else
					lastType = NeoBatchItem.ItemType.Texture;

				// Draw the batches
				for (var i = 0; i < numBatchesToProcess; i++, batchIndex++, index += 4, vertexArrayPtr += 4)
				{
					var item = _batchItemList[batchIndex];
					// if the texture changed, we need to flush and bind the new texture
					var shouldFlush = !(ReferenceEquals(item.Texture, tex) && lastType == item.Type);
					if (shouldFlush)
					{
						FlushVertexArray(startIndex, index, tex, lastType);

						tex = item.Texture;
						startIndex = index = 0;
						vertexArrayPtr = vertexArrayFixedPtr;
						_device.Textures[0] = tex;
					}

					// store the SpriteBatchItem data in our vertexArray
					*(vertexArrayPtr + 0) = item.vertexTL;
					*(vertexArrayPtr + 1) = item.vertexTR;
					*(vertexArrayPtr + 2) = item.vertexBL;
					*(vertexArrayPtr + 3) = item.vertexBR;

					// Release the texture.
					item.Texture = null;
					lastType = item.Type;
				}
			}

			// flush the remaining vertexArray data
			FlushVertexArray(startIndex, index, tex, lastType);
			// Update our batch count to continue the process of culling down
			// large batches
			batchCount -= numBatchesToProcess;
		}

		// return items to the pool.  
		_batchItemCount = 0;
	}

	/// <summary>
	///     Sends the triangle list to the graphics device. Here is where the actual drawing starts.
	/// </summary>
	/// <param name="start">Start index of vertices to draw. Not used except to compute the count of vertices to draw.</param>
	/// <param name="end">End index of vertices to draw. Not used except to compute the count of vertices to draw.</param>
	/// <param name="texture">The texture to draw.</param>
	private void FlushVertexArray(int start, int end, Texture texture, NeoBatchItem.ItemType type)
	{
		if (start == end)
			return;

		var vertexCount = end - start;

		_effect.Parameters["Texture"].SetValue(texture);

		if (type == NeoBatchItem.ItemType.Glyph)
			_effect.Techniques["Glyphs"].Passes[0].Apply();
		else if (type == NeoBatchItem.ItemType.Block)
			_effect.Techniques["Block"].Passes[0].Apply();
		else
			_effect.Techniques["BasicTexture"].Passes[0].Apply();

		_device.Textures[0] = texture;

		_device.DrawUserIndexedPrimitives(
			PrimitiveType.TriangleList,
			_vertexArray,
			0,
			vertexCount,
			_index,
			0,
			vertexCount / 4 * 2,
			BlockDeclaration);
	}
}