using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Neo.Components;
using System;

namespace Neo.Component
{
	public class MessageBox : Control
	{
		public enum Result
		{
			Ok,
			Cancel,
			Yes,
			No,
			Accept,
			Decline,
			Continue,
			Stop,
			Back,
			Next,
			Close,
			Done
		}

		public event EventHandler Closed;

		public MessageBox(string message, Result[] resultButtons)
		{
			Flow = Flow.Center;
			WantsMouse = true;
			Container _box = new Container(new ScreenUnit(0, 0), new ScreenUnit(600, 300));
			_box.Flow = Flow.Center;
			_box.WantsMouse = true;

			Label _label = new Label(message);
			_label.Flow = Flow.Center;

			foreach(Result r in resultButtons)
			{
				ResultButton _btn = new ResultButton(r);
				_btn.Flow = resultButtons.Length > 1 ? Flow.HorizontalSharing : Flow.Bottom;
				_btn.Clicked += btnClicked;
				_btn.PositionOffset = new ScreenUnit(0, 20);
				_box.AddChild(_btn);
			}

			_box.AddChild(_label);
			AddChild(_box) ;
		}

		public MessageBox(string message) : this(message, new Result[] { Result.Ok })
		{
		}

		Action<Result> _method;
		public MessageBox(string message, Action<Result> method): this (message)
		{
			_method = method;
		}

		public MessageBox(string message, Action<Result> method, Result result1, Result result2) : this(message, new Result[] { result1, result2 })
		{
			_method = method;
		}

		public MessageBox(string message, Action<Result> method, Result result1, Result result2, Result result3) : this(message, new Result[] { result1, result2, result3 })
		{
			_method = method;
		}

		private void btnClicked(object sender, EventArgs e)
		{
			_neo.RemoveControl(this);
			_method?.Invoke(((ResultButton)sender).Result);
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

	public class ResultButton : Button
	{
		public MessageBox.Result Result { get; private set; }
		public ResultButton(MessageBox.Result result)
		{
			Result = result;
			Text = result.ToString();
		}
	}
}
