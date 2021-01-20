using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Neo.Controls
{
	public class MessageBox : Control
	{
		public enum Result
		{
			Ok, Cancel,	Yes, No, Accept,
			Decline, Continue, Stop,
			Back, Next, Close, Done
		}

		public event EventHandler Closed;

		Row _buttonRow;
		public MessageBox(Neo neo, string message, Result[] resultButtons)
		{
			_neo = neo;
			WantsMouse = true;
			_buttonRow = new Row();
			_buttonRow.Anchors = Anchors.Bottom | Anchors.Left | Anchors.Right;
			_buttonRow.LayoutRule = Row.LayoutRules.RightToLeft;
			_buttonRow.Margins = new Margins(15);
			_buttonRow.Size = new Size(100);			

			Label _label = new Label(message);
			_label.Anchors = Anchors.Top;
			_label.Margins = new Margins(20);
			

			foreach(Result r in resultButtons)
			{
				ResultButton _btn = new ResultButton(r);
				_btn.Size = new Size(100);
				_btn.Anchors = Anchors.Bottom;
				_btn.Clicked += btnClicked;
				_buttonRow.AddChild(_btn);
			}

			AddChild(_label);
			AddChild(_buttonRow);
		}

		public MessageBox(Neo neo, string message) : this(neo, message, new Result[] { Result.Ok }) { }

		Action<Result> _method;
		public MessageBox(Neo neo, string message, Action<Result> method): this (neo, message)
		{
			_method = method;
		}

		internal override void Draw(GameTime gameTime, NeoBatch guiBatch)
		{
			if (IsClipped != true)
			{
				foreach (Block b in Blocks)
					guiBatch.Draw(b);

				foreach (Control c in this)
					c.Draw(gameTime, guiBatch);
			}
		}

		public MessageBox(Neo neo, string message, Action<Result> method,
			Result result1,
			Result result2) : this(
				neo, message,
				new Result[] { result1, result2 })
		{
			_method = method;
		}

		public MessageBox(Neo neo, string message, Action<Result> method,
			Result result1,
			Result result2,
			Result result3) : this(
				neo, message,
				new Result[] { result1, result2, result3 })
		{
			_method = method;
		}

		private void btnClicked(object sender, EventArgs e)
		{
			_neo.RemoveChild(this);
		//	_method?.Invoke(((ResultButton)sender).Result);
			Closed?.Invoke(this, e);
		}

		internal override void SetBounds(Rectangle bounds)
		{
			Bounds = bounds;
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
