using Neo;
using Neo.Controls;
using Microsoft.Xna.Framework;

namespace NeoTestApp.Code
{
	class Gui : DrawableGameComponent
	{
		Neo.Neo _neo;
		public Gui(Game game) : base (game)
		{
			Style style = new Style(Game.Content);
			_neo = new Neo.Neo(game, style, Game1.CurrentPlatform);

			Row row = new Row()
			{
				Anchors = Anchors.Right,
				Size = new Size(200),
				Margins = new Margins(50, 0, 50, 0)
			};

			_neo.AddChild(row);
		}

		public override void Draw(GameTime gameTime)
		{
			_neo.Draw(gameTime);
		}

		public void Init()
		{
			_neo.Init();//Initialize()
		}
	}
}
