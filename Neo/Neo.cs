﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.Utilities;
using Neo.Controls;
using System;

namespace Neo
{
	public partial class Neo : Grid
	{
		public Style Style { get; private set; }
		private MonoGamePlatform _currentPlatform;

		private Game _game;
		public float Scale;
		private GuiBatch _guiBatch;

		public NeoFont DefaultFont { get; private set; }

		public Neo(Game game, Style style, MonoGamePlatform platform)
		{
			_game = game;
			_currentPlatform = platform;

			Style = style;

			_game.Window.ClientSizeChanged += OnResize;		

			if (_currentPlatform == MonoGamePlatform.Android)
				Scale = 2.5f;
			else
				Scale = 1f;

			DefaultFont = new NeoFont(game.Content.Load<Texture2D>("atlas"), @"C:\Users\Dag\Downloads\msdf-atlas-gen-1.1-win64\msdf-atlas-gen\output.csv");
			Console.WriteLine("Loaded neo");
		}

		private void OnResize(object sender, EventArgs e) { ForceRefresh(); }

		public void Ready()
		{
			Initialize(this);
		}

		internal override void Initialize(Neo neo)
		{
			_guiBatch = new GuiBatch(_game.GraphicsDevice, _game.Content.Load<Effect>("InstancingRectangleShader"), this);
			SetBounds(new Rectangle(0, 0, (int)(_game.GraphicsDevice.Viewport.Width / Scale), (int)(_game.GraphicsDevice.Viewport.Height / Scale)));

			base.Initialize(neo);
			foreach (Control child in this)
				child.Initialize(neo);
		}

		public void ForceRefresh()
		{
			SetBounds(new Rectangle(0, 0, (int)(_game.GraphicsDevice.Viewport.Width / Scale), (int)(_game.GraphicsDevice.Viewport.Height / Scale)));
			_guiBatch.ForceRefresh();
		}

		public void Draw(GameTime gameTime)
		{
			_guiBatch.Begin();

			foreach (Control c in this)
				c.Draw(gameTime, _guiBatch);
/*
		_guiBatch.DrawString(DefaultFont,
				"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur condimentum leo lacus, quis sodales nunc congue sit amet.\n" +
				"Vivamus vehicula mi id metus scelerisque ullamcorper. Integer quis tempor est. Maecenas eu molestie enim. Morbi sodales,\n" +
								"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur condimentum leo lacus, quis sodales nunc congue sit amet.\n" +
				"Vivamus vehicula mi id metus scelerisque ullamcorper. Integer quis tempor est. Maecenas eu molestie enim. Morbi sodales,\n" +
								"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur condimentum leo lacus, quis sodales nunc congue sit amet.\n" +
				"Vivamus vehicula mi id metus scelerisque ullamcorper. Integer quis tempor est. Maecenas eu molestie enim. Morbi sodales,\n" +
								"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur condimentum leo lacus, quis sodales nunc congue sit amet.\n" +
				"Vivamus vehicula mi id metus scelerisque ullamcorper. Integer quis tempor est. Maecenas eu molestie enim. Morbi sodales,\n" +
								"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur condimentum leo lacus, quis sodales nunc congue sit amet.\n" +
				"Vivamus vehicula mi id metus scelerisque ullamcorper. Integer quis tempor est. Maecenas eu molestie enim. Morbi sodales,\n" +
								"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur condimentum leo lacus, quis sodales nunc congue sit amet.\n" +
				"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur condimentum leo lacus, quis sodales nunc congue sit amet.\n" +
				"Vivamus vehicula mi id metus scelerisque ullamcorper. Integer quis tempor est. Maecenas eu molestie enim. Morbi sodales,\n" +
								"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur condimentum leo lacus, quis sodales nunc congue sit amet.\n" +
				"Vivamus vehicula mi id metus scelerisque ullamcorper. Integer quis tempor est. Maecenas eu molestie enim. Morbi sodales,\n" +
								"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur condimentum leo lacus, quis sodales nunc congue sit amet.\n" +
				"Vivamus vehicula mi id metus scelerisque ullamcorper. Integer quis tempor est. Maecenas eu molestie enim. Morbi sodales,\n" +
								"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur condimentum leo lacus, quis sodales nunc congue sit amet.\n" +
				"Vivamus vehicula mi id metus scelerisque ullamcorper. Integer quis tempor est. Maecenas eu molestie enim. Morbi sodales,\n" +
				"Vivamus vehicula mi id metus scelerisque ullamcorper. Integer quis tempor est. Maecenas eu molestie enim. Morbi sodales,\n" +
				"elit consequat.",
							Vector2.One, 18f, Color.White) ;
*/

			_guiBatch.End();
		}

		public new bool ListensForMouseOrTouchAt(Point mouseOrTouchPosition)
		{
			Point pos = new Point((int)(mouseOrTouchPosition.X / Scale), (int)(mouseOrTouchPosition.Y / Scale));

			foreach (Control c in this)
				if (c.ListensForMouseOrTouchAt(pos))
					return true;

			return false;
		}

		public new bool Click(Point mouseOrTouchPosition)
		{
			Point pos = new Point((int)(mouseOrTouchPosition.X / Scale), (int)(mouseOrTouchPosition.Y / Scale));

			foreach (Control c in this)
				if (c.ListensForMouseOrTouchAt(pos))
				{
					if (c.Click(pos))
						return true;
				}

				return false;
		}
	}
}
