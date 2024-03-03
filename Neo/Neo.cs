using MonoGame.Framework.Utilities;
using Neo.Controls;


namespace Neo;

public class Neo : Grid
{
	private readonly MonoGamePlatform _currentPlatform;

	private readonly Game _game;
	private NeoBatch _neoBatch;
	public float Scale;

	public Neo(Game game, MonoGamePlatform platform) : base(null)
	{
		_game = game;
		_currentPlatform = platform;

		_game.Window.ClientSizeChanged += OnResize;

		if (_currentPlatform == MonoGamePlatform.Android)
			Scale = 2.5f;
		else
			Scale = 1f;

		DefaultFont = game.Content.Load<NeoFont>("default_font_map");
		DefaultFont.Atlas = game.Content.Load<Texture2D>("default_font_atlas");
		_neo = this;
	}

	public NeoFont DefaultFont { get; }

	private void OnResize(object sender, EventArgs e)
	{
		ForceRefresh();
	}

	public void Create()
	{
		_neoBatch = new NeoBatch(_game.GraphicsDevice, _game.Content, this);
		SetBounds(new Rectangle(0, 0, (int) (_game.GraphicsDevice.Viewport.Width / Scale),
			(int) (_game.GraphicsDevice.Viewport.Height / Scale)));
	}

	public void ForceRefresh()
	{
		SetBounds(new Rectangle(0, 0, (int) (_game.GraphicsDevice.Viewport.Width / Scale),
			(int) (_game.GraphicsDevice.Viewport.Height / Scale)));
		_neoBatch.ForceRefresh();
	}

	public void Draw(GameTime gameTime)
	{
		foreach (Control child in this)
			child.Draw(gameTime, _neoBatch);

		_neoBatch.End();
	}

	public new bool ListensForMouseOrTouchAt(Point mouseOrTouchPosition)
	{
		var pos = new Point((int) (mouseOrTouchPosition.X / Scale), (int) (mouseOrTouchPosition.Y / Scale));

		foreach (Control child in this)
			if (child.ListensForMouseOrTouchAt(pos))
				return true;

		return false;
	}

	public new bool Click(Point mouseOrTouchPosition)
	{
		var pos = new Point((int) (mouseOrTouchPosition.X / Scale), (int) (mouseOrTouchPosition.Y / Scale));

		foreach (Control child in this)
			if (child.ListensForMouseOrTouchAt(pos))
				if (child.Click(pos))
					return true;

		return false;
	}

	/// <summary>
	///     Takes two sizes and returns the coordinate for centering the second size in the first one.
	///     <param name="a">Size of container.</param>
	///     <param name="b">Size of component to be centered in the container.</param>
	/// </summary>
	public static Size Center(Size a, Size b)
	{
		return new Size(
			a.Width / 2 - b.Width / 2,
			a.Height / 2 - b.Height / 2);
	}

	/// <summary>
	///     Takes two sizes and an offset. Returns the coordinate for centering the second size in the first one.
	///     <param name="a">Size of container.</param>
	///     <param name="b">Size of component to be centered in the container.</param>
	/// </summary>
	public static Size Center(Size a, Size b, int offsetHeight)
	{
		return new Size(
			a.Width / 2 - b.Width / 2,
			a.Height / 2 - b.Height / 2 + offsetHeight);
	}

	/// <summary>
	///     Takes two sizes and an offset. Returns the coordinate for centering the second size in the first one.
	///     <param name="a">Size of container.</param>
	///     <param name="b">Size of component to be centered in the container.</param>
	/// </summary>
	public static Vector2 Center(Size a, Size b, Size offset)
	{
		return offset.ToVector2() + new Size(
			a.Width / 2 - b.Width / 2,
			a.Height / 2 - b.Height / 2).ToVector2();
	}

	public static Vector2 FromBottomRight(Vector2 screenSize, int x, int y)
	{
		return new Vector2(screenSize.X - x, screenSize.Y - y);
	}
}