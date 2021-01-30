using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Neo
{
    /// <summary>
    /// Helper class for drawing text strings and sprites in one or more optimized batches.
    /// </summary>
	public class NeoBatch
    {
        readonly NeoBatcher _batcher;
        Effect _effect;
        Matrix _matrix;
        Vector2 _texCoordTL = new Vector2(0, 0);
        Vector2 _texCoordBR = new Vector2(0, 0);
        Neo _neo;

        GraphicsDevice _graphicsDevice;

        /// <param name="graphicsDevice">The <see cref="GraphicsDevice"/>, which will be used for sprite rendering.</param>
        /// <param name="capacity">The initial capacity of the internal array holding batch items (the value will be rounded to the next multiple of 64).</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="graphicsDevice"/> is null.</exception>
        public NeoBatch(GraphicsDevice graphicsDevice, ContentManager content, Neo neo)
        {
            _neo = neo;
            if (graphicsDevice == null)
            {
                throw new ArgumentNullException("graphicsDevice");
            }

            _graphicsDevice = graphicsDevice;

            _effect = content.Load<Effect>("neo_shader");

            _batcher = new NeoBatcher(graphicsDevice, _effect, 0);

            CheckScreenResolution();

            _graphicsDevice.BlendState = BlendState.AlphaBlend;
            _graphicsDevice.DepthStencilState = DepthStencilState.None;
            _graphicsDevice.RasterizerState = RasterizerState.CullNone;
            _graphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
        }

        float _lastScale = 0;
        private Viewport _lastViewport;

        private void CheckScreenResolution()
        {
            var vp = _graphicsDevice.Viewport;
            if ((vp.Width != _lastViewport.Width) || (vp.Height != _lastViewport.Height) || _lastScale != _neo.Scale)
                SetMatrix(vp);
        }

        public void ForceRefresh()
        {
            SetMatrix(_graphicsDevice.Viewport);
        }

        private void SetMatrix(Viewport vp)
        {
            Matrix.CreateOrthographicOffCenter(0, vp.Width / _neo.Scale, vp.Height / _neo.Scale, 0, 0, 1, out _matrix);
            if (_graphicsDevice.UseHalfPixelOffset)
            {
                _matrix.M41 += -0.5f * _matrix.M11;
                _matrix.M42 += -0.5f * _matrix.M22;
            }

            _lastViewport = vp;
            _lastScale = _neo.Scale;
            _effect.Parameters["MatrixTransform"].SetValue(_matrix);
        }

        public void DrawGlyph(Texture2D atlas, Rectangle destinationRectangle, float left, float bottom, float right, float top, Color color)
        {
            var ntop = atlas.Height - top;
            var nbottom = atlas.Height - bottom;

            NeoBatchItem item = _batcher.CreateBatchItem();
            item.Texture = atlas;

            _texCoordTL.X = left * (1f / (float)atlas.Width);
            _texCoordTL.Y = ntop * (1f / (float)atlas.Height);
            _texCoordBR.X = right * (1f / (float)atlas.Width);
            _texCoordBR.Y = nbottom * (1f / (float)atlas.Height);

            item.Set(destinationRectangle.X,
                     destinationRectangle.Y,
                     destinationRectangle.Width,
                     destinationRectangle.Height,
                     color,
                     _texCoordTL,
                     _texCoordBR);
            item.Type = NeoBatchItem.ItemType.Glyph;

        }

        public void DrawString(string text, Vector2 position, float scale, Color color)
        {
            NeoFont font = _neo.DefaultFont;

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

        public void End()
        {
            _batcher.DrawBatch();
        }

        void CheckValid(Texture2D texture)
        {
            if (texture == null)
                throw new ArgumentNullException("texture");
        }

        /*        void CheckValid(SpriteFont spriteFont, string text)
                {
                    if (spriteFont == null)
                        throw new ArgumentNullException("spriteFont");
                    if (text == null)
                        throw new ArgumentNullException("text");
                    if (!_beginCalled)
                        throw new InvalidOperationException("DrawString was called, but Begin has not yet been called. Begin must be called successfully before you can call DrawString.");
                }

                void CheckValid(SpriteFont spriteFont, StringBuilder text)
                {
                    if (spriteFont == null)
                        throw new ArgumentNullException("spriteFont");
                    if (text == null)
                        throw new ArgumentNullException("text");
                    if (!_beginCalled)
                        throw new InvalidOperationException("DrawString was called, but Begin has not yet been called. Begin must be called successfully before you can call DrawString.");
                }*/

        /// <summary>
        /// Submit a sprite for drawing in the current batch.
        /// </summary>
        /// <param name="texture">A texture.</param>
        /// <param name="position">The drawing location on screen.</param>
        /// <param name="sourceRectangle">An optional region on the texture which will be rendered. If null - draws full texture.</param>
        /// <param name="color">A color mask.</param>
        /// <param name="rotation">A rotation of this sprite.</param>
        /// <param name="origin">Center of the rotation. 0,0 by default.</param>
        /// <param name="scale">A scaling of this sprite.</param>
        /// <param name="effects">Modificators for drawing. Can be combined.</param>
        /// <param name="layerDepth">A depth of the layer of this sprite.</param>
        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            CheckValid(texture);

            var item = _batcher.CreateBatchItem();
            item.Texture = texture;

            origin = origin * scale;

            float w, h;
            if (sourceRectangle.HasValue)
            {
                var srcRect = sourceRectangle.GetValueOrDefault();
                w = srcRect.Width * scale.X;
                h = srcRect.Height * scale.Y;
                _texCoordTL.X = srcRect.X * (1f / (float)texture.Width);
                _texCoordTL.Y = srcRect.Y * (1f / (float)texture.Height);
                _texCoordBR.X = (srcRect.X + srcRect.Width) * (1f / (float)texture.Width);
                _texCoordBR.Y = (srcRect.Y + srcRect.Height) * (1f / (float)texture.Height);
            }
            else
            {
                w = texture.Width * scale.X;
                h = texture.Height * scale.Y;
                _texCoordTL = Vector2.Zero;
                _texCoordBR = Vector2.One;
            }

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
                item.Set(position.X - origin.X,
                        position.Y - origin.Y,
                        w,
                        h,
                        color,
                        _texCoordTL,
                        _texCoordBR);
            }
            else
            {
                item.Set(position.X,
                        position.Y,
                        -origin.X,
                        -origin.Y,
                        w,
                        h,
                        (float)Math.Sin(rotation),
                        (float)Math.Cos(rotation),
                        color,
                        _texCoordTL,
                        _texCoordBR);
            }
        }

        /// <summary>
        /// Submit a sprite for drawing in the current batch.
        /// </summary>
        /// <param name="texture">A texture.</param>
        /// <param name="position">The drawing location on screen.</param>
        /// <param name="sourceRectangle">An optional region on the texture which will be rendered. If null - draws full texture.</param>
        /// <param name="color">A color mask.</param>
        /// <param name="rotation">A rotation of this sprite.</param>
        /// <param name="origin">Center of the rotation. 0,0 by default.</param>
        /// <param name="scale">A scaling of this sprite.</param>
        /// <param name="effects">Modificators for drawing. Can be combined.</param>
        /// <param name="layerDepth">A depth of the layer of this sprite.</param>
		public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            var scaleVec = new Vector2(scale, scale);
            Draw(texture, position, sourceRectangle, color, rotation, origin, scaleVec, effects, layerDepth);
        }

        /// <summary>
        /// Submit a sprite for drawing in the current batch.
        /// </summary>
        /// <param name="texture">A texture.</param>
        /// <param name="destinationRectangle">The drawing bounds on screen.</param>
        /// <param name="sourceRectangle">An optional region on the texture which will be rendered. If null - draws full texture.</param>
        /// <param name="color">A color mask.</param>
        /// <param name="rotation">A rotation of this sprite.</param>
        /// <param name="origin">Center of the rotation. 0,0 by default.</param>
        /// <param name="effects">Modificators for drawing. Can be combined.</param>
        public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects)
        {
            CheckValid(texture);

            var item = _batcher.CreateBatchItem();
            item.Texture = texture;

            if (sourceRectangle.HasValue)
            {
                var srcRect = sourceRectangle.GetValueOrDefault();
                _texCoordTL.X = srcRect.X * (1f / (float)texture.Width);
                _texCoordTL.Y = srcRect.Y * (1f / (float)texture.Height);
                _texCoordBR.X = (srcRect.X + srcRect.Width) * (1f / (float)texture.Width);
                _texCoordBR.Y = (srcRect.Y + srcRect.Height) * (1f / (float)texture.Height);

                if (srcRect.Width != 0)
                    origin.X = origin.X * (float)destinationRectangle.Width / (float)srcRect.Width;
                else
                    origin.X = origin.X * (float)destinationRectangle.Width * (1f / (float)texture.Width);
                if (srcRect.Height != 0)
                    origin.Y = origin.Y * (float)destinationRectangle.Height / (float)srcRect.Height;
                else
                    origin.Y = origin.Y * (float)destinationRectangle.Height * (1f / (float)texture.Height);
            }
            else
            {
                _texCoordTL = Vector2.Zero;
                _texCoordBR = Vector2.One;

                origin.X = origin.X * (float)destinationRectangle.Width * (1f / (float)texture.Width);
                origin.Y = origin.Y * (float)destinationRectangle.Height * (1f / (float)texture.Width);
            }

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
                item.Set(destinationRectangle.X - origin.X,
                        destinationRectangle.Y - origin.Y,
                        destinationRectangle.Width,
                        destinationRectangle.Height,
                        color,
                        _texCoordTL,
                        _texCoordBR);
            }
            else
            {
                item.Set(destinationRectangle.X,
                        destinationRectangle.Y,
                        -origin.X,
                        -origin.Y,
                        destinationRectangle.Width,
                        destinationRectangle.Height,
                        (float)Math.Sin(rotation),
                        (float)Math.Cos(rotation),
                        color,
                        _texCoordTL,
                        _texCoordBR);
            }
        }

        /// <summary>
        /// Submit a sprite for drawing in the current batch.
        /// </summary>
        /// <param name="texture">A texture.</param>
        /// <param name="position">The drawing location on screen.</param>
        /// <param name="sourceRectangle">An optional region on the texture which will be rendered. If null - draws full texture.</param>
        /// <param name="color">A color mask.</param>
		public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color)
        {
            CheckValid(texture);

            var item = _batcher.CreateBatchItem();
            item.Texture = texture;

            Vector2 size;

            if (sourceRectangle.HasValue)
            {
                var srcRect = sourceRectangle.GetValueOrDefault();
                size = new Vector2(srcRect.Width, srcRect.Height);
                _texCoordTL.X = srcRect.X * (1f / (float)texture.Width);
                _texCoordTL.Y = srcRect.Y * (1f / (float)texture.Height);
                _texCoordBR.X = (srcRect.X + srcRect.Width) * (1f / (float)texture.Width);
                _texCoordBR.Y = (srcRect.Y + srcRect.Height) * (1f / (float)texture.Height);
            }
            else
            {
                size = new Vector2(texture.Width, texture.Height);
                _texCoordTL = Vector2.Zero;
                _texCoordBR = Vector2.One;
            }

            item.Set(position.X,
                     position.Y,
                     size.X,
                     size.Y,
                     color,
                     _texCoordTL,
                     _texCoordBR);
        }

        /// <summary>
        /// Submit a sprite for drawing in the current batch.
        /// </summary>
        /// <param name="texture">A texture.</param>
        /// <param name="destinationRectangle">The drawing bounds on screen.</param>
        /// <param name="sourceRectangle">An optional region on the texture which will be rendered. If null - draws full texture.</param>
        /// <param name="color">A color mask.</param>
		public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color)
        {
            CheckValid(texture);

            var item = _batcher.CreateBatchItem();
            item.Texture = texture;

            if (sourceRectangle.HasValue)
            {
                var srcRect = sourceRectangle.GetValueOrDefault();
                _texCoordTL.X = srcRect.X * (1f / (float)texture.Width);
                _texCoordTL.Y = srcRect.Y * (1f / (float)texture.Height);
                _texCoordBR.X = (srcRect.X + srcRect.Width) * (1f / (float)texture.Width);
                _texCoordBR.Y = (srcRect.Y + srcRect.Height) * (1f / (float)texture.Height);
            }
            else
            {
                _texCoordTL = Vector2.Zero;
                _texCoordBR = Vector2.One;
            }

            item.Set(destinationRectangle.X,
                     destinationRectangle.Y,
                     destinationRectangle.Width,
                     destinationRectangle.Height,
                     color,
                     _texCoordTL,
                     _texCoordBR);
        }

        /// <summary>
        /// Submit a sprite for drawing in the current batch.
        /// </summary>
        /// <param name="texture">A texture.</param>
        /// <param name="position">The drawing location on screen.</param>
        /// <param name="color">A color mask.</param>
		public void Draw(Texture2D texture, Vector2 position, Color color)
        {
            CheckValid(texture);

            var item = _batcher.CreateBatchItem();
            item.Texture = texture;

            item.Set(position.X,
                     position.Y,
                     texture.Width,
                     texture.Height,
                     color,
                     Vector2.Zero,
                     Vector2.One);
        }

        /// <summary>
        /// Submit a sprite for drawing in the current batch.
        /// </summary>
        /// <param name="texture">A texture.</param>
        /// <param name="destinationRectangle">The drawing bounds on screen.</param>
        /// <param name="color">A color mask.</param>
        public void Draw(Texture2D texture, Rectangle destinationRectangle, Color color)
        {
            CheckValid(texture);

            var item = _batcher.CreateBatchItem();
            item.Texture = texture;

            item.Set(destinationRectangle.X,
                     destinationRectangle.Y,
                     destinationRectangle.Width,
                     destinationRectangle.Height,
                     color,
                     Vector2.Zero,
                     Vector2.One);
        }

        /// <summary>
        /// Submit a block for drawing in the current batch.
        /// </summary>
        public void Draw(Vector2 position, Vector2 size, Color color, float radius)
        {
            var item = _batcher.CreateBatchItem();

            item.SetBlock(position.X,
                     position.Y,
                     size.X,
                     size.Y,
                     color,
                     Vector2.Zero,
                     Vector2.One,
                     radius);
        }

        /// <summary>
        /// Submit a block for drawing in the current batch.
        /// </summary>
        public void Draw(Block block)
        {
            var item = _batcher.CreateBatchItem();

            item.SetBlock(block.Position.X,
                     block.Position.Y,
                     block.Size.X,
                     block.Size.Y,
                     block.Color,
                     Vector2.Zero,
                     Vector2.One,
                     block.Radius);
        }

        /*        /// <summary>
                /// Submit a text string of sprites for drawing in the current batch.
                /// </summary>
                /// <param name="spriteFont">A font.</param>
                /// <param name="text">The text which will be drawn.</param>
                /// <param name="position">The drawing location on screen.</param>
                /// <param name="color">A color mask.</param>
                public unsafe void DrawString (SpriteFont spriteFont, string text, Vector2 position, Color color)
                {
                    CheckValid(spriteFont, text);

                    // float sortKey = (_sortMode == SpriteSortMode.Texture) ? spriteFont.Texture.SortingKey : 0;

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

                        _texCoordTL.X = pCurrentGlyph->BoundsInTexture.X * spriteFont.Texture.TexelWidth;
                        _texCoordTL.Y = pCurrentGlyph->BoundsInTexture.Y * spriteFont.Texture.TexelHeight;
                        _texCoordBR.X = (pCurrentGlyph->BoundsInTexture.X + pCurrentGlyph->BoundsInTexture.Width) * spriteFont.Texture.TexelWidth;
                        _texCoordBR.Y = (pCurrentGlyph->BoundsInTexture.Y + pCurrentGlyph->BoundsInTexture.Height) * spriteFont.Texture.TexelHeight;

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
                public void DrawString (
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
                public unsafe void DrawString (
                    SpriteFont spriteFont, string text, Vector2 position, Color color,
                    float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
                {
                    CheckValid(spriteFont, text);

                    float sortKey = 0;
                    // set SortKey based on SpriteSortMode.
                    switch (_sortMode)
                    {
                            // Comparison of Texture objects.
                            case SpriteSortMode.Texture:
                                sortKey = spriteFont.Texture.SortingKey;
                                break;
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

                        _texCoordTL.X = pCurrentGlyph->BoundsInTexture.X * spriteFont.Texture.TexelWidth;
                        _texCoordTL.Y = pCurrentGlyph->BoundsInTexture.Y * spriteFont.Texture.TexelHeight;
                        _texCoordBR.X = (pCurrentGlyph->BoundsInTexture.X + pCurrentGlyph->BoundsInTexture.Width) * spriteFont.Texture.TexelWidth;
                        _texCoordBR.Y = (pCurrentGlyph->BoundsInTexture.Y + pCurrentGlyph->BoundsInTexture.Height) * spriteFont.Texture.TexelHeight;

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
                public unsafe void DrawString (SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color)
                {
                    CheckValid(spriteFont, text);

                    float sortKey =  (_sortMode == SpriteSortMode.Texture) ? spriteFont.Texture.SortingKey : 0;

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

                        _texCoordTL.X = pCurrentGlyph->BoundsInTexture.X * spriteFont.Texture.TexelWidth;
                        _texCoordTL.Y = pCurrentGlyph->BoundsInTexture.Y * spriteFont.Texture.TexelHeight;
                        _texCoordBR.X = (pCurrentGlyph->BoundsInTexture.X + pCurrentGlyph->BoundsInTexture.Width) * spriteFont.Texture.TexelWidth;
                        _texCoordBR.Y = (pCurrentGlyph->BoundsInTexture.Y + pCurrentGlyph->BoundsInTexture.Height) * spriteFont.Texture.TexelHeight;

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
                public void DrawString (
                    SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color,
                    float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
                {
                    var scaleVec = new Vector2 (scale, scale);
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
                public unsafe void DrawString (
                    SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color,
                    float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
                {
                    CheckValid(spriteFont, text);

                    float sortKey = 0;
                    // set SortKey based on SpriteSortMode.
                    switch (_sortMode)
                    {
                            // Comparison of Texture objects.
                            case SpriteSortMode.Texture:
                                sortKey = spriteFont.Texture.SortingKey;
                                break;
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

                        _texCoordTL.X = pCurrentGlyph->BoundsInTexture.X * (float)spriteFont.Texture.TexelWidth;
                        _texCoordTL.Y = pCurrentGlyph->BoundsInTexture.Y * (float)spriteFont.Texture.TexelHeight;
                        _texCoordBR.X = (pCurrentGlyph->BoundsInTexture.X + pCurrentGlyph->BoundsInTexture.Width) * (float)spriteFont.Texture.TexelWidth;
                        _texCoordBR.Y = (pCurrentGlyph->BoundsInTexture.Y + pCurrentGlyph->BoundsInTexture.Height) * (float)spriteFont.Texture.TexelHeight;

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
                }*/

    }
}

