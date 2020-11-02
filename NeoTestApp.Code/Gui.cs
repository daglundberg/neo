using Neo;
using Neo.Controls;
using Microsoft.Xna.Framework;
using System;

namespace NeoTestApp.Code
{
	class Gui : DrawableGameComponent
	{

		Neo.Neo _neo;
		public Gui(Game game) : base(game)
		{
			Style style = new Style(Game.Content);
			_neo = new Neo.Neo(game, style, Game1.CurrentPlatform);

			_neo.AddChildren(
				new Control[]
				{
					new Grid()
					{
						Anchors =  Anchors.Top,
						Size = new Size(600, 400),
						Margins = new Margins(0, 100, 0, 0)
					},

					new Grid()
					{
						Anchors = Anchors.Top | Anchors.Left,
						Size = new Size(100),
						Margins = new Margins(20)
					},

					new Row()
					{
						Anchors = Anchors.Right | Anchors.Left | Anchors.Bottom,
						Size = new Size(80),
						Margins = new Margins(200, 0, 200, 20),
						LayoutRule = Row.LayoutRules.LeftToRight
					}.AddChildren(
						new Control[]
						{
							new Grid()
							{
								Size = new Size(50),
								Margins = new Margins(8, 4,4,4)


							},
							new Grid()
							{
								Size = new Size(50),
								Margins = new Margins(4)
							},
							new Grid()
							{
								Size = new Size(50),
								Margins = new Margins(4)
							},

							new Grid()
							{
								Size = new Size(50),
								Margins = new Margins(4)
							},
							new Grid()
							{
								Size = new Size(50),
								Margins = new Margins(4)
							},
							new Grid()
							{
								Size = new Size(50),
								Margins = new Margins(4)
							},
							new Grid()
							{
								Size = new Size(50),
								Margins = new Margins(4)
							},
							new Grid()
							{
								Size = new Size(50),
								Margins = new Margins(4)
							},
						}),

					new Button()
					{
						Anchors = Anchors.Right | Anchors.Bottom,
						Size = new Size(110, 40),
						Margins = new Margins(40)
					},

					new Button()
					{
						Anchors = Anchors.Left | Anchors.Bottom,
						Size = new Size(110, 40),
						Margins = new Margins(40)
					},
				});
		}

		public override void Draw(GameTime gameTime)
		{
			_neo.Draw(gameTime);
		}

		public void Init()
		{
			_neo.Init();
		}
	}
}
