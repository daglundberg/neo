using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Neo.Controls
{
	public class MessageBox : Control
	{
		public event EventHandler Closed;

		Row _buttonRow;
		public MessageBox(Neo neo, string message, Result[] resultButtons) : base (neo, true)
		{
			WantsMouse = true;
			_buttonRow = new Row(neo);
			_buttonRow.Anchors = Anchors.Bottom | Anchors.Left | Anchors.Right;
			
			_buttonRow.LayoutRule = Row.LayoutRules.RightToLeft;
			_buttonRow.Margins = new Margins(15);
			_buttonRow.Size = new Size(50);			

			Label _label = new Label(neo, message);
			_label.Anchors = Anchors.Left | Anchors.Top;
			_label.Margins = new Margins(20);			

			foreach(Result r in resultButtons)
			{
				ResultButton _btn = new ResultButton(neo, r);
				_btn.Size = new Size(27 + 10 * r.ToString().Length, 38);
				//_btn.Anchors = Anchors.Bottom;
				_btn.Color = Color.Aqua;
				_btn.Margins = new Margins(5, 0, 5, 0);
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
			//_method?.Invoke(((ResultButton)sender).Result);
			Closed?.Invoke(((ResultButton)sender).Result, e);
		}

		internal override void SetBounds(Rectangle bounds)
		{
			Bounds = bounds;
			foreach (Control child in this)
				child.SetBounds(CalculateChildBounds(Bounds, child, true, true));
		}

		public enum Result { Ok, Cancel, Yes, No, Close, Continue, Stop, Back, Next, Done, Accept, Decline }
	}

	public class ResultButton : Button
	{
		public MessageBox.Result Result { get; private set; }
		public ResultButton(Neo neo, MessageBox.Result result) : base(neo)
		{
			Result = result;
			Text = result.ToString();
		}
	}
}
