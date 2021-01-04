/*using Microsoft.Xna.Framework;
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
		Effect _effect;
		Matrix _matrix;

		Rectangle _tempRect = new Rectangle(0, 0, 0, 0);
		Vector2 _texCoordTL = new Vector2(0, 0);
		Vector2 _texCoordBR = new Vector2(0, 0);
		GraphicsDevice _graphicsDevice;

		public GuiBatch(GraphicsDevice graphicsDevice, Effect effect, int capacity = 0)
		{
			_graphicsDevice = graphicsDevice;
			_effect = effect;

			_batcher = new GuiBatcher(graphicsDevice, effect,  capacity);

			_matrix = Matrix.CreateOrthographicOffCenter(0, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height, 0, 0, 1);

			_effect.Parameters["MatrixTransform"].SetValue(_matrix);
			_blendState = BlendState.AlphaBlend;
			_samplerState = SamplerState.LinearClamp;
			_depthStencilState = DepthStencilState.None;
			_rasterizerState = RasterizerState.CullCounterClockwise;

			_graphicsDevice.BlendState = _blendState;
			_graphicsDevice.DepthStencilState = _depthStencilState;
			_graphicsDevice.RasterizerState = _rasterizerState;
			_graphicsDevice.SamplerStates[0] = _samplerState;
		}

		public void Begin()
		{

		}

		public void End()
		{
			_batcher.DrawBatch();
		}

		public void ForceRefresh()
		{
			_matrix = Matrix.CreateOrthographicOffCenter(0, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height, 0, 0, 1);
			_effect.Parameters["MatrixTransform"].SetValue(_matrix);
		}

		void CheckValid(SpriteFont spriteFont, string text)
		{
			if (spriteFont == null)
				throw new ArgumentNullException("spriteFont");
			if (text == null)
				throw new ArgumentNullException("text");
		}
		void CheckValid(SpriteFont spriteFont, StringBuilder text)
		{
			if (spriteFont == null)
				throw new ArgumentNullException("spriteFont");
			if (text == null)
				throw new ArgumentNullException("text");
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

			item.Set(destinationRectangle.X,
					 destinationRectangle.Y,
					 destinationRectangle.Width,
					 destinationRectangle.Height,
					 color,
					 Vector2.Zero,
					 Vector2.One,
					 0);
		}

		public void DrawBlock(Block block)
		{
			GuiBatchItem item = _batcher.CreateBatchItem();
			item.Texture = null;

			item.Set(block.Position.X,
					 block.Position.Y,
					 block.Size.X,
					 block.Size.Y,
					 Color.Red,
					 Vector2.Zero,
					 Vector2.One,
					 0);
		}

		*//*		/// <summary>
				/// Submit a text string of sprites for drawing in the current batch.
				/// </summary>
				/// <param name="spriteFont">A font.</param>
				/// <param name="text">The text which will be drawn.</param>
				/// <param name="position">The drawing location on screen.</param>
				/// <param name="color">A color mask.</param>
				public unsafe void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color)
				{
					CheckValid(spriteFont, text);

					float sortKey = 0;

					var offset = Vector2.Zero;
					var firstGlyphOfLine = true;

					fixed (SpriteFont.Glyph* pGlyphs = spriteFont.Glyphs)
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
							item.SortKey = sortKey;

							_texCoordTL.X = pCurrentGlyph->BoundsInTexture.X * (1f/(float)spriteFont.Texture.Width);
							_texCoordTL.Y = pCurrentGlyph->BoundsInTexture.Y * (1f / (float)spriteFont.Texture.Height);
							_texCoordBR.X = (pCurrentGlyph->BoundsInTexture.X + pCurrentGlyph->BoundsInTexture.Width) * (1f / (float)spriteFont.Texture.Width);
							_texCoordBR.Y = (pCurrentGlyph->BoundsInTexture.Y + pCurrentGlyph->BoundsInTexture.Height) * (1f / (float)spriteFont.Texture.Height);

							item.Set(p.X,
									 p.Y,
									 pCurrentGlyph->BoundsInTexture.Width,
									 pCurrentGlyph->BoundsInTexture.Height,
									 color,
									 _texCoordTL,
									 _texCoordBR,
									 0);

							offset.X += pCurrentGlyph->Width + pCurrentGlyph->RightSideBearing;
						}

					// We need to flush if we're using Immediate sort mode.
					FlushIfNeeded();
				}

				/// <summary>
				/// Submit a text string of sprites for drawing in the current batch.
				/// </summary>
				/// <param name="spriteFont">A font.</param>
				/// <param name="text">The text which will be drawn.</param>
				/// <param name="position">The drawing location on screen.</param>
				/// <param name="color">A color mask.</param>
				/// <param name="rotation">A rotation of this string.</param>
				/// <param name="origin">Center of the rotation. 0,0 by default.</param>
				/// <param name="scale">A scaling of this string.</param>
				/// <param name="effects">Modificators for drawing. Can be combined.</param>
				/// <param name="layerDepth">A depth of the layer of this string.</param>
				public void DrawString(
					SpriteFont spriteFont, string text, Vector2 position, Color color,
					float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
				{
					var scaleVec = new Vector2(scale, scale);
					DrawString(spriteFont, text, position, color, rotation, origin, scaleVec, effects, layerDepth);
				}

				/// <summary>
				/// Submit a text string of sprites for drawing in the current batch.
				/// </summary>
				/// <param name="spriteFont">A font.</param>
				/// <param name="text">The text which will be drawn.</param>
				/// <param name="position">The drawing location on screen.</param>
				/// <param name="color">A color mask.</param>
				/// <param name="rotation">A rotation of this string.</param>
				/// <param name="origin">Center of the rotation. 0,0 by default.</param>
				/// <param name="scale">A scaling of this string.</param>
				/// <param name="effects">Modificators for drawing. Can be combined.</param>
				/// <param name="layerDepth">A depth of the layer of this string.</param>
				public unsafe void DrawString(
					SpriteFont spriteFont, string text, Vector2 position, Color color,
					float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
				{
					CheckValid(spriteFont, text);

					float sortKey = 0;
					// set SortKey based on SpriteSortMode.
					switch (_sortMode)
					{
						// Comparison of Depth
						case SpriteSortMode.FrontToBack:
							sortKey = layerDepth;
							break;
						// Comparison of Depth in reverse
						case SpriteSortMode.BackToFront:
							sortKey = -layerDepth;
							break;
					}

					var flipAdjustment = Vector2.Zero;

					var flippedVert = (effects & SpriteEffects.FlipVertically) == SpriteEffects.FlipVertically;
					var flippedHorz = (effects & SpriteEffects.FlipHorizontally) == SpriteEffects.FlipHorizontally;

					if (flippedVert || flippedHorz)
					{
						Vector2 size;

						var source = new SpriteFont.CharacterSource(text);
						spriteFont.MeasureString(ref source, out size);

						if (flippedHorz)
						{
							origin.X *= -1;
							flipAdjustment.X = -size.X;
						}

						if (flippedVert)
						{
							origin.Y *= -1;
							flipAdjustment.Y = spriteFont.LineSpacing - size.Y;
						}
					}

					Matrix transformation = Matrix.Identity;
					float cos = 0, sin = 0;
					if (rotation == 0)
					{
						transformation.M11 = (flippedHorz ? -scale.X : scale.X);
						transformation.M22 = (flippedVert ? -scale.Y : scale.Y);
						transformation.M41 = ((flipAdjustment.X - origin.X) * transformation.M11) + position.X;
						transformation.M42 = ((flipAdjustment.Y - origin.Y) * transformation.M22) + position.Y;
					}
					else
					{
						cos = (float)Math.Cos(rotation);
						sin = (float)Math.Sin(rotation);
						transformation.M11 = (flippedHorz ? -scale.X : scale.X) * cos;
						transformation.M12 = (flippedHorz ? -scale.X : scale.X) * sin;
						transformation.M21 = (flippedVert ? -scale.Y : scale.Y) * (-sin);
						transformation.M22 = (flippedVert ? -scale.Y : scale.Y) * cos;
						transformation.M41 = (((flipAdjustment.X - origin.X) * transformation.M11) + (flipAdjustment.Y - origin.Y) * transformation.M21) + position.X;
						transformation.M42 = (((flipAdjustment.X - origin.X) * transformation.M12) + (flipAdjustment.Y - origin.Y) * transformation.M22) + position.Y;
					}

					var offset = Vector2.Zero;
					var firstGlyphOfLine = true;

					fixed (SpriteFont.Glyph* pGlyphs = spriteFont.Glyphs)
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

							if (flippedHorz)
								p.X += pCurrentGlyph->BoundsInTexture.Width;
							p.X += pCurrentGlyph->Cropping.X;

							if (flippedVert)
								p.Y += pCurrentGlyph->BoundsInTexture.Height - spriteFont.LineSpacing;
							p.Y += pCurrentGlyph->Cropping.Y;

							Vector2.Transform(ref p, ref transformation, out p);

							var item = _batcher.CreateBatchItem();
							item.Texture = spriteFont.Texture;
							item.SortKey = sortKey;

							_texCoordTL.X = pCurrentGlyph->BoundsInTexture.X * (1f / (float)spriteFont.Texture.Width);
							_texCoordTL.Y = pCurrentGlyph->BoundsInTexture.Y * (1f / (float)spriteFont.Texture.Height);
							_texCoordBR.X = (pCurrentGlyph->BoundsInTexture.X + pCurrentGlyph->BoundsInTexture.Width) * (1f / (float)spriteFont.Texture.Width);
							_texCoordBR.Y = (pCurrentGlyph->BoundsInTexture.Y + pCurrentGlyph->BoundsInTexture.Height) * (1f / (float)spriteFont.Texture.Height);

							if ((effects & SpriteEffects.FlipVertically) != 0)
							{
								var temp = _texCoordBR.Y;
								_texCoordBR.Y = _texCoordTL.Y;
								_texCoordTL.Y = temp;
							}
							if ((effects & SpriteEffects.FlipHorizontally) != 0)
							{
								var temp = _texCoordBR.X;
								_texCoordBR.X = _texCoordTL.X;
								_texCoordTL.X = temp;
							}

							if (rotation == 0f)
							{
								item.Set(p.X,
										p.Y,
										pCurrentGlyph->BoundsInTexture.Width * scale.X,
										pCurrentGlyph->BoundsInTexture.Height * scale.Y,
										color,
										_texCoordTL,
										_texCoordBR,
										layerDepth);
							}
							else
							{
								item.Set(p.X,
										p.Y,
										0,
										0,
										pCurrentGlyph->BoundsInTexture.Width * scale.X,
										pCurrentGlyph->BoundsInTexture.Height * scale.Y,
										sin,
										cos,
										color,
										_texCoordTL,
										_texCoordBR,
										layerDepth);
							}

							offset.X += pCurrentGlyph->Width + pCurrentGlyph->RightSideBearing;
						}

					// We need to flush if we're using Immediate sort mode.
					FlushIfNeeded();
				}

				/// <summary>
				/// Submit a text string of sprites for drawing in the current batch.
				/// </summary>
				/// <param name="spriteFont">A font.</param>
				/// <param name="text">The text which will be drawn.</param>
				/// <param name="position">The drawing location on screen.</param>
				/// <param name="color">A color mask.</param>
				public unsafe void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color)
				{
					CheckValid(spriteFont, text);

					float sortKey = 0;

					var offset = Vector2.Zero;
					var firstGlyphOfLine = true;

					fixed (SpriteFont.Glyph* pGlyphs = spriteFont.Glyphs)
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
							item.SortKey = sortKey;

							_texCoordTL.X = pCurrentGlyph->BoundsInTexture.X * (1f / (float)spriteFont.Texture.Width);
							_texCoordTL.Y = pCurrentGlyph->BoundsInTexture.Y * (1f / (float)spriteFont.Texture.Height);
							_texCoordBR.X = (pCurrentGlyph->BoundsInTexture.X + pCurrentGlyph->BoundsInTexture.Width) *(1f/(float)spriteFont.Texture.Width);
							_texCoordBR.Y = (pCurrentGlyph->BoundsInTexture.Y + pCurrentGlyph->BoundsInTexture.Height) * (1f / (float)spriteFont.Texture.Height);

							item.Set(p.X,
									 p.Y,
									 pCurrentGlyph->BoundsInTexture.Width,
									 pCurrentGlyph->BoundsInTexture.Height,
									 color,
									 _texCoordTL,
									 _texCoordBR,
									 0);

							offset.X += pCurrentGlyph->Width + pCurrentGlyph->RightSideBearing;
						}

					// We need to flush if we're using Immediate sort mode.
					FlushIfNeeded();
				}

				/// <summary>
				/// Submit a text string of sprites for drawing in the current batch.
				/// </summary>
				/// <param name="spriteFont">A font.</param>
				/// <param name="text">The text which will be drawn.</param>
				/// <param name="position">The drawing location on screen.</param>
				/// <param name="color">A color mask.</param>
				/// <param name="rotation">A rotation of this string.</param>
				/// <param name="origin">Center of the rotation. 0,0 by default.</param>
				/// <param name="scale">A scaling of this string.</param>
				/// <param name="effects">Modificators for drawing. Can be combined.</param>
				/// <param name="layerDepth">A depth of the layer of this string.</param>
				public void DrawString(
					SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color,
					float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
				{
					var scaleVec = new Vector2(scale, scale);
					DrawString(spriteFont, text, position, color, rotation, origin, scaleVec, effects, layerDepth);
				}

				/// <summary>
				/// Submit a text string of sprites for drawing in the current batch.
				/// </summary>
				/// <param name="spriteFont">A font.</param>
				/// <param name="text">The text which will be drawn.</param>
				/// <param name="position">The drawing location on screen.</param>
				/// <param name="color">A color mask.</param>
				/// <param name="rotation">A rotation of this string.</param>
				/// <param name="origin">Center of the rotation. 0,0 by default.</param>
				/// <param name="scale">A scaling of this string.</param>
				/// <param name="effects">Modificators for drawing. Can be combined.</param>
				/// <param name="layerDepth">A depth of the layer of this string.</param>
				public unsafe void DrawString(
					SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color,
					float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
				{
					CheckValid(spriteFont, text);

					float sortKey = 0;
					// set SortKey based on SpriteSortMode.
					switch (_sortMode)
					{
						// Comparison of Depth
						case SpriteSortMode.FrontToBack:
							sortKey = layerDepth;
							break;
						// Comparison of Depth in reverse
						case SpriteSortMode.BackToFront:
							sortKey = -layerDepth;
							break;
					}

					var flipAdjustment = Vector2.Zero;

					var flippedVert = (effects & SpriteEffects.FlipVertically) == SpriteEffects.FlipVertically;
					var flippedHorz = (effects & SpriteEffects.FlipHorizontally) == SpriteEffects.FlipHorizontally;

					if (flippedVert || flippedHorz)
					{
						var source = new SpriteFont.CharacterSource(text);
						Vector2 size;
						spriteFont.MeasureString(ref source, out size);

						if (flippedHorz)
						{
							origin.X *= -1;
							flipAdjustment.X = -size.X;
						}

						if (flippedVert)
						{
							origin.Y *= -1;
							flipAdjustment.Y = spriteFont.LineSpacing - size.Y;
						}
					}

					Matrix transformation = Matrix.Identity;
					float cos = 0, sin = 0;
					if (rotation == 0)
					{
						transformation.M11 = (flippedHorz ? -scale.X : scale.X);
						transformation.M22 = (flippedVert ? -scale.Y : scale.Y);
						transformation.M41 = ((flipAdjustment.X - origin.X) * transformation.M11) + position.X;
						transformation.M42 = ((flipAdjustment.Y - origin.Y) * transformation.M22) + position.Y;
					}
					else
					{
						cos = (float)Math.Cos(rotation);
						sin = (float)Math.Sin(rotation);
						transformation.M11 = (flippedHorz ? -scale.X : scale.X) * cos;
						transformation.M12 = (flippedHorz ? -scale.X : scale.X) * sin;
						transformation.M21 = (flippedVert ? -scale.Y : scale.Y) * (-sin);
						transformation.M22 = (flippedVert ? -scale.Y : scale.Y) * cos;
						transformation.M41 = (((flipAdjustment.X - origin.X) * transformation.M11) + (flipAdjustment.Y - origin.Y) * transformation.M21) + position.X;
						transformation.M42 = (((flipAdjustment.X - origin.X) * transformation.M12) + (flipAdjustment.Y - origin.Y) * transformation.M22) + position.Y;
					}

					var offset = Vector2.Zero;
					var firstGlyphOfLine = true;

					fixed (SpriteFont.Glyph* pGlyphs = spriteFont.Glyphs)
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

							if (flippedHorz)
								p.X += pCurrentGlyph->BoundsInTexture.Width;
							p.X += pCurrentGlyph->Cropping.X;

							if (flippedVert)
								p.Y += pCurrentGlyph->BoundsInTexture.Height - spriteFont.LineSpacing;
							p.Y += pCurrentGlyph->Cropping.Y;

							Vector2.Transform(ref p, ref transformation, out p);

							var item = _batcher.CreateBatchItem();
							item.Texture = spriteFont.Texture;
							item.SortKey = sortKey;

							_texCoordTL.X = pCurrentGlyph->BoundsInTexture.X * (float)(1f / (float)spriteFont.Texture.Width);
							_texCoordTL.Y = pCurrentGlyph->BoundsInTexture.Y * (float)(1f / (float)spriteFont.Texture.Height);
							_texCoordBR.X = (pCurrentGlyph->BoundsInTexture.X + pCurrentGlyph->BoundsInTexture.Width) * (float)(1f / (float)spriteFont.Texture.Width);
							_texCoordBR.Y = (pCurrentGlyph->BoundsInTexture.Y + pCurrentGlyph->BoundsInTexture.Height) * (float)(1f / (float)spriteFont.Texture.Height);

							if ((effects & SpriteEffects.FlipVertically) != 0)
							{
								var temp = _texCoordBR.Y;
								_texCoordBR.Y = _texCoordTL.Y;
								_texCoordTL.Y = temp;
							}
							if ((effects & SpriteEffects.FlipHorizontally) != 0)
							{
								var temp = _texCoordBR.X;
								_texCoordBR.X = _texCoordTL.X;
								_texCoordTL.X = temp;
							}

							if (rotation == 0f)
							{
								item.Set(p.X,
										p.Y,
										pCurrentGlyph->BoundsInTexture.Width * scale.X,
										pCurrentGlyph->BoundsInTexture.Height * scale.Y,
										color,
										_texCoordTL,
										_texCoordBR,
										layerDepth);
							}
							else
							{
								item.Set(p.X,
										p.Y,
										0,
										0,
										pCurrentGlyph->BoundsInTexture.Width * scale.X,
										pCurrentGlyph->BoundsInTexture.Height * scale.Y,
										sin,
										cos,
										color,
										_texCoordTL,
										_texCoordBR,
										layerDepth);
							}

							offset.X += pCurrentGlyph->Width + pCurrentGlyph->RightSideBearing;
						}

					// We need to flush if we're using Immediate sort mode.
					FlushIfNeeded();
				}*//*
	}
}

*/