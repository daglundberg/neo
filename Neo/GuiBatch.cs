using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Text;

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
				for (var i = 0; i < text.Length; ++i)
				{
					NeoGlyph g;
					char c = text[i];
					font.Glyphs.TryGetValue(c, out g);
					Bounds gg = g.PlaneBounds * scale;
					if (g != null)
					{
						DrawGlyph(font.Atlas, new Rectangle((int)(gg.Left + (advance * scale + position.X)), (int)(scale - gg.Top + position.Y), (int)(gg.Right - gg.Left), (int)(gg.Top - gg.Bottom)),
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

			/// <summary>
				/// Submit a text string of sprites for drawing in the current batch.
				/// </summary>
				/// <param name="spriteFont">A font.</param>
				/// <param name="text">The text which will be drawn.</param>
				/// <param name="position">The drawing location on screen.</param>
				/// <param name="color">A color mask.</param>
				public unsafe void DrawString(GuiFont spriteFont, string text, Vector2 position, Color color)
				{

					var offset = Vector2.Zero;
					var firstGlyphOfLine = true;

					fixed (GuiFont.Glyph* pGlyphs = spriteFont.Glyphs)
						for (var i = 0; i < text.Length; ++i)
						{
							var c = text[i];

							if (c == '\r')
								continue;

							if (c == '\n')
							{
								offset.X = 0;
								offset.Y += spriteFont.LineSpacing;
								firstGlyphOfLine = true;
								continue;
							}

							var currentGlyphIndex = spriteFont.GetGlyphIndexOrDefault(c);
							var pCurrentGlyph = pGlyphs + currentGlyphIndex;

							// The first character on a line might have a negative left side bearing.
							// In this scenario, SpriteBatch/SpriteFont normally offset the text to the right,
							//  so that text does not hang off the left side of its rectangle.
							if (firstGlyphOfLine)
							{
								offset.X = Math.Max(pCurrentGlyph->LeftSideBearing, 0);
								firstGlyphOfLine = false;
							}
							else
							{
								offset.X += spriteFont.Spacing + pCurrentGlyph->LeftSideBearing;
							}

							var p = offset;
							p.X += pCurrentGlyph->Cropping.X;
							p.Y += pCurrentGlyph->Cropping.Y;
							p += position;

							var item = _batcher.CreateBatchItem();
							item.Texture = spriteFont.Texture;

							_texCoordTL.X = pCurrentGlyph->BoundsInTexture.X * (1f/(float)spriteFont.Texture.Width);
							_texCoordTL.Y = pCurrentGlyph->BoundsInTexture.Y * (1f / (float)spriteFont.Texture.Height);
							_texCoordBR.X = (pCurrentGlyph->BoundsInTexture.X + pCurrentGlyph->BoundsInTexture.Width) * (1f / (float)spriteFont.Texture.Width);
							_texCoordBR.Y = (pCurrentGlyph->BoundsInTexture.Y + pCurrentGlyph->BoundsInTexture.Height) * (1f / (float)spriteFont.Texture.Height);

							item.Set(p.X,
									 p.Y,
									 pCurrentGlyph->BoundsInTexture.Width,
									 pCurrentGlyph->BoundsInTexture.Height,
									 color.ToVector4(),
									 _texCoordTL,
									 _texCoordBR,
									 0, 0);

							offset.X += pCurrentGlyph->Width + pCurrentGlyph->RightSideBearing;
						}
				}
	}
}

