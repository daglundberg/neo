using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Neo
{
	public class GuiBatch
	{
		readonly GuiBatcher _batcher;

		BlendState _blendState;
		SamplerState _samplerState;
		DepthStencilState _depthStencilState;
		RasterizerState _rasterizerState;
		Effect  _effectInstanced;
		Matrix _matrix;

		Rectangle _tempRect = new Rectangle(0, 0, 0, 0);
		Vector2 _texCoordTL = new Vector2(0, 0);
		Vector2 _texCoordBR = new Vector2(0, 0);
		GraphicsDevice _graphicsDevice;
		Neo _neo;

		public GuiBatch(GraphicsDevice graphicsDevice, Effect neoEffect, Neo neo)
		{
			_neo = neo;
			_graphicsDevice = graphicsDevice;

			_effectInstanced = neoEffect;

			_batcher = new GuiBatcher(graphicsDevice, neoEffect);

			//_matrix = Matrix.CreateOrthographicOffCenter(0, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height, 0, 0, 1);
		//	effectInstanced.Parameters["MatrixTransform"].SetValue(_matrix);

			_blendState = BlendState.AlphaBlend;
			_samplerState = SamplerState.LinearClamp;
			_depthStencilState = DepthStencilState.None;
			_rasterizerState = RasterizerState.CullNone ;

			_graphicsDevice.BlendState = _blendState;
			_graphicsDevice.DepthStencilState = _depthStencilState;
			_graphicsDevice.RasterizerState = _rasterizerState;
			_graphicsDevice.SamplerStates[0] = _samplerState;
		}

		public void Begin()
		{
			
		}

		float _lastScale = 0;
		private Viewport _lastViewport;
		private void CheckScreenResolution()
		{
			var vp = _graphicsDevice.Viewport;
			if ((vp.Width != _lastViewport.Width) || (vp.Height != _lastViewport.Height) || _lastScale != _neo.Scale)
			{
				Matrix.CreateOrthographicOffCenter(0, vp.Width / _neo.Scale, vp.Height / _neo.Scale, 0, 0, 1, out _matrix);
				if (_graphicsDevice.UseHalfPixelOffset)
				{
					_matrix.M41 += -0.5f * _matrix.M11;
					_matrix.M42 += -0.5f * _matrix.M22;
				}

				_lastViewport = vp;
				_lastScale = _neo.Scale;
				_effectInstanced.Parameters["MatrixTransform"].SetValue(_matrix);
			}
		}

		public void End()
		{
			CheckScreenResolution();
			_batcher.DrawBatches();
			//_batcher.DrawBatchInstanced();
		}

		public void ForceRefresh()
		{
			var vp = _graphicsDevice.Viewport;
			Matrix.CreateOrthographicOffCenter(0, vp.Width / _neo.Scale, vp.Height / _neo.Scale, 0, 0, 1, out _matrix);
			if (_graphicsDevice.UseHalfPixelOffset)
			{
				_matrix.M41 += -0.5f * _matrix.M11;
				_matrix.M42 += -0.5f * _matrix.M22;
			}

			_lastViewport = vp;
			_lastScale = _neo.Scale;
			_effectInstanced.Parameters["MatrixTransform"].SetValue(_matrix);
		}

		/// <summary>
		/// Submit a sprite for drawing in the current batch.
		/// </summary>
		/// <param name="texture">A texture.</param>
		/// <param name="destinationRectangle">The drawing bounds on screen.</param>
		/// <param name="color">A color mask.</param>
		public void Draw(Texture2D texture, Rectangle destinationRectangle, Color color)
		{
			GuiBatchItem item = _batcher.CreateBatchItem();
			item.Texture = texture;
			item.Type = GuiBatchItem.Types.Texture;

			item.Set(destinationRectangle.X,
					 destinationRectangle.Y,
					 destinationRectangle.Width,
					 destinationRectangle.Height,
					 color.ToVector4(),
					 Vector2.Zero,
					 Vector2.One,
					 0, 0);
		}

		/// <summary>
		/// Submit a sprite for drawing in the current batch.
		/// </summary>
		/// <param name="texture">A texture.</param>
		/// <param name="destinationRectangle">The drawing bounds on screen.</param>
		/// <param name="sourceRectangle">An optional region on the texture which will be rendered. If null - draws full texture.</param>
		/// <param name="color">A color mask.</param>
		public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle, Color color)
		{
			GuiBatchItem item = _batcher.CreateBatchItem();
			item.Texture = texture;
			item.Type = GuiBatchItem.Types.Texture;

			_texCoordTL.X = sourceRectangle.X * (1f / (float)texture.Width);
			_texCoordTL.Y = sourceRectangle.Y * (1f / (float)texture.Height);
			_texCoordBR.X = (sourceRectangle.X + sourceRectangle.Width) * (1f / (float)texture.Width);
			_texCoordBR.Y = (sourceRectangle.Y + sourceRectangle.Height) * (1f / (float)texture.Height);

			item.Set(destinationRectangle.X,
					 destinationRectangle.Y,
					 destinationRectangle.Width,
					 destinationRectangle.Height,
					 color.ToVector4(),
					 _texCoordTL,
					 _texCoordBR,
					 0, 0);
		}


		/// <summary>
		/// Submit a sprite for drawing in the current batch.
		/// </summary>
		/// <param name="texture">A texture.</param>
		/// <param name="destinationRectangle">The drawing bounds on screen.</param>
		/// <param name="sourceRectangle">An optional region on the texture which will be rendered. If null - draws full texture.</param>
		/// <param name="color">A color mask.</param>
		public void DrawGlyph(Texture2D atlas, Rectangle destinationRectangle, float left, float bottom, float right, float top, Color color)
		{
			var ntop = atlas.Height - top;
			var nbottom = atlas.Height - bottom;

			GuiBatchItem item = _batcher.CreateBatchItem();
			item.Texture = atlas;
			item.Type = GuiBatchItem.Types.Texture;

			_texCoordTL.X = left * (1f / (float)atlas.Width);
			_texCoordTL.Y = ntop * (1f / (float)atlas.Height);
			_texCoordBR.X = right * (1f / (float)atlas.Width);
			_texCoordBR.Y = nbottom * (1f / (float)atlas.Height);

			item.Set(destinationRectangle.X,
					 destinationRectangle.Y,
					 destinationRectangle.Width,
					 destinationRectangle.Height,
					 color.ToVector4(),
					 _texCoordTL,
					 _texCoordBR,
					 0, 0);
		}

		public void DrawString(NeoFont font, string text, Vector2 position, float scale, Color color)
		{
			if (text != null)
			{
				float advance = 0;
				float row = 0;
				for (var i = 0; i < text.Length; ++i)
				{
					char c = text[i];

					if (c == ' ')
					{
						advance += 0.35f;
						continue;
					}

					if (c == '\n')
					{
						row++;
						advance = 0;
						continue;
					}

					NeoGlyph g;
						font.Glyphs.TryGetValue(c, out g);
						Bounds gg = g.PlaneBounds * scale;
						if (g != null)
						{
							DrawGlyph(font.Atlas, new Rectangle((int)(gg.Left + (advance * scale + position.X)), (int)(scale - gg.Top + position.Y + (row * 1 * scale)), (int)(gg.Right - gg.Left), (int)(gg.Top - gg.Bottom)),
								g.AtlasBounds.Left, g.AtlasBounds.Bottom, g.AtlasBounds.Right, g.AtlasBounds.Top,
								color);
							advance += g.Advance;

						}
				}
			}
		}

		public void DrawBlock(Block block)
		{
			GuiBatchItem item = _batcher.CreateBatchItem();
			item.Type = GuiBatchItem.Types.Rectangle;

			item.Set(block.Position.X,
					 block.Position.Y,
					 block.Size.X,
					 block.Size.Y,
					 block.Color,
					 Vector2.Zero,
					 Vector2.One,
					 0, block.Radius);
		}
	}
}

