using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.Utilities;
using Neo.Controls;
using System;

namespace Neo
{
	public partial class Neo : Grid
	{
		private MonoGamePlatform _currentPlatform;

		private Game _game;
		public float Scale;
		private NeoBatch _neoBatch;

		public NeoFont DefaultFont { get; private set; }

		public Neo(Game game, MonoGamePlatform platform) : base(null)
		{
			_game = game;
			_currentPlatform = platform;

			_game.Window.ClientSizeChanged += OnResize;		

			if (_currentPlatform == MonoGamePlatform.Android)
				Scale = 2.5f;
			else
				Scale = 1f;

			DefaultFont = game.Content.Load<NeoFont>("output");
			DefaultFont.Atlas = game.Content.Load<Texture2D>("atlas");
			_neo = this;
		}

		private void OnResize(object sender, EventArgs e) { ForceRefresh(); }

		public void Create()
		{
			_neoBatch = new NeoBatch(_game.GraphicsDevice, _game.Content, this);
			SetBounds(new Rectangle(0, 0, (int)(_game.GraphicsDevice.Viewport.Width / Scale), (int)(_game.GraphicsDevice.Viewport.Height / Scale)));
		}

		public void ForceRefresh()
		{
			SetBounds(new Rectangle(0, 0, (int)(_game.GraphicsDevice.Viewport.Width / Scale), (int)(_game.GraphicsDevice.Viewport.Height / Scale)));
			_neoBatch.ForceRefresh();
		}

		public void Draw(GameTime gameTime)
		{
			foreach (Control child in this)
				child.Draw(gameTime, _neoBatch);

			_neoBatch.DrawString(_neo.DefaultFont, "Test", Vector2.One, 21f, Color.Blue);

			_neoBatch.End();
		}

		public new bool ListensForMouseOrTouchAt(Point mouseOrTouchPosition)
		{
			Point pos = new Point((int)(mouseOrTouchPosition.X / Scale), (int)(mouseOrTouchPosition.Y / Scale));

			foreach (Control child in this)
				if (child.ListensForMouseOrTouchAt(pos))
					return true;

			return false;
		}

		public new bool Click(Point mouseOrTouchPosition)
		{
			Point pos = new Point((int)(mouseOrTouchPosition.X / Scale), (int)(mouseOrTouchPosition.Y / Scale));

			foreach (Control child in this)
				if (child.ListensForMouseOrTouchAt(pos))
				{
					if (child.Click(pos))
						return true;
				}

				return false;
		}
	}
}
