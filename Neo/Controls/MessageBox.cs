using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Neo.Components;
using System;

namespace Neo.Controls
{
	public class MessageBox : Control
	{
		public enum Result
		{
			Ok,
			Cancel,
			Yes,
			No
		}

		public event EventHandler Closed;
		public MessageBox(string message)
		{
			Flow = Flow.Center;
			WantsMouse = true;
			Container _box = new Container(new PixelUnit(0, 0), new PixelUnit(500, 300));
			_box.Flow = Flow.Center;
			_box.WantsMouse = true;

			Label _label = new Label(message);
			_label.Flow = Flow.Center;

			Button _btnOk = new Button("Ok");
			_btnOk.Flow = Flow.Bottom;
			_btnOk.Clicked += btnOk_Clicked;

			_box.AddChild(_btnOk);
			_box.AddChild(_label);
			AddChild(_box) ;
		}

		Action<Result> _method;
		public MessageBox(string message, Action<Result> method): this (message)
		{
			_method = method;
		}

		private void btnOk_Clicked(object sender, EventArgs e)
		{
			_neo.RemoveControl(this);
			if (_method != null)
				_method(Result.Ok);
			Closed?.Invoke(this, e);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			//throw new NotImplementedException();
		}

		Neo _neo;
		public override void Initialize(Neo neo, GraphicsDeviceManager graphics)
		{
			_neo = neo;
		}
	}
}
