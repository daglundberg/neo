namespace Neo.Controls;

public class MessageBox : Control
{
	public enum Result
	{
		Ok,
		Cancel,
		Yes,
		No,
		Close,
		Continue,
		Stop,
		Back,
		Next,
		Done,
		Accept,
		Decline
	}

	private readonly Row _buttonRow;

	private Action<Result> _method;

	public MessageBox(Neo neo, string message, Result[] resultButtons) : base(neo, true)
	{
		WantsMouse = true;
		_buttonRow = new Row(neo);
		_buttonRow.Anchors = Anchors.Bottom | Anchors.Left | Anchors.Right;

		_buttonRow.LayoutRule = Row.LayoutRules.RightToLeft;
		_buttonRow.Margins = new Margins(15);
		_buttonRow.Size = new Size(50);

		var _label = new Label(neo, message);
		_label.Anchors = Anchors.Left | Anchors.Top;
		_label.Margins = new Margins(20);

		foreach (var r in resultButtons)
		{
			var _btn = new ResultButton(neo, r);
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

	public MessageBox(Neo neo, string message) : this(neo, message, new[] {Result.Ok})
	{
	}

	public MessageBox(Neo neo, string message, Action<Result> method) : this(neo, message)
	{
		_method = method;
	}

	public MessageBox(Neo neo, string message, Action<Result> method,
		Result result1,
		Result result2) : this(
		neo, message,
		new[] {result1, result2})
	{
		_method = method;
	}

	public MessageBox(Neo neo, string message, Action<Result> method,
		Result result1,
		Result result2,
		Result result3) : this(
		neo, message,
		new[] {result1, result2, result3})
	{
		_method = method;
	}

	public event EventHandler Closed;

	internal override void Draw(GameTime gameTime, NeoBatch guiBatch)
	{
		if (IsClipped != true)
		{
			guiBatch.Draw(new Block
			{
				Color = Color.Aquamarine, Position = Bounds.Location.ToVector2(), Radius = 5,
				Size = Bounds.Size.ToVector2()
			});

			foreach (Control c in this)
				c.Draw(gameTime, guiBatch);
		}
	}

	private void btnClicked(object sender, EventArgs e)
	{
		_neo.RemoveChild(this);
		//_method?.Invoke(((ResultButton)sender).Result);
		Closed?.Invoke(((ResultButton) sender).Result, e);
	}

	internal override void SetBounds(Rectangle bounds)
	{
		Bounds = bounds;
		foreach (Control child in this)
			child.SetBounds(CalculateChildBounds(Bounds, child, true, true));
	}
}

public class ResultButton : Button
{
	public ResultButton(Neo neo, MessageBox.Result result) : base(neo)
	{
		Result = result;
		Text = result.ToString();
	}

	public MessageBox.Result Result { get; }
}