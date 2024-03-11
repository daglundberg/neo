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

	public MessageBox(Neo neo, string title, string message, Result[] resultButtons) : base(neo, true)
	{
		WantsMouse = true;
		_buttonRow = new Row(neo);
		_buttonRow.Anchors = Anchors.Bottom | Anchors.Left | Anchors.Right;

		_buttonRow.LayoutRule = Row.LayoutRules.RightToLeft;
		_buttonRow.Margins = new Margins(15);
		_buttonRow.Size = new Size(50);

		var _label = new Label(neo, title);
		_label.Anchors = Anchors.Left | Anchors.Top;
		_label.Margins = new Margins(10,2,0,0);
		
		var _label2 = new Label(neo, message);
		_label2.FontSize = _label.FontSize * 0.6f;
		_label2.Anchors = Anchors.Left | Anchors.Top;
		_label2.Margins = new Margins(12, 62, 0, 0);

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
		AddChild(_label2);
		AddChild(_buttonRow);
	}

	public MessageBox(Neo neo, string title, string msg) : this(neo, title, msg, new[] {Result.Ok})
	{
	}

	public MessageBox(Neo neo, string title,  string msg, Action<Result> method) : this(neo, title,msg )
	{
		_method = method;
	}

	public MessageBox(Neo neo, string title, string msg, Action<Result> method,
		Result result1,
		Result result2) : this(
		neo, title, msg,
		new[] {result1, result2})
	{
		_method = method;
	}

	public MessageBox(Neo neo, string title, string msg, Action<Result> method,
		Result result1,
		Result result2,
		Result result3) : this(
		neo, title, msg,
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
				Color = new Color(44,44,44), Position = Bounds.Location.ToVector2()+new Vector2(0,30), Radius = 5,
				Size = Bounds.Size.ToVector2() - new Vector2(0, 30)
			});
			guiBatch.Draw(new CustomBlock
			{
				Color = new Color(77,77,77), Position = Bounds.Location.ToVector2(), RadiusTL = 20, RadiusBR = 3f, RadiusBL = 3f, RadiusTR = 20f, 
				Size = new Vector2(Bounds.Size.X, 60)
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