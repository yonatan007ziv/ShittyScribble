using System.Numerics;

namespace Client.UserControls;

public partial class DrawingCanvas : UserControl
{
	private readonly Graphics graphics, bGraphics;
	private readonly Bitmap bitmap;
	private readonly Pen pen;

	private bool drawing;
	private Vector2 startingPosition;
	private bool enabled;

	public DrawingCanvas()
	{
		InitializeComponent();

		graphics = CreateGraphics();
		bitmap = new Bitmap(Width, Height, graphics);
		bGraphics = Graphics.FromImage(bitmap);
		pen = new Pen(new SolidBrush(Color.Black));

		graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
		pen.StartCap = pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;

		button1.Click += ChangeColor;
		button2.Click += ChangeColor;
		button3.Click += ChangeColor;
		button4.Click += ChangeColor;
		button5.Click += ChangeColor;
		button6.Click += ChangeColor;
		button7.Click += ChangeColor;
		button8.Click += ChangeColor;
		button9.Click += ChangeColor;
		button10.Click += ChangeColor;
		button11.Click += ChangeColor;
		button12.Click += ChangeColor;
		button13.Click += ChangeColor;
		button14.Click += ChangeColor;
		button15.Click += ChangeColor;
		button16.Click += ChangeColor;

		brushWidthTextBox.TextChanged += ChangeWidth;

		ShowEditingTools(false);
	}

	private void ShowEditingTools(bool show)
	{
		button1.Visible = show;
		button2.Visible = show;
		button3.Visible = show;
		button4.Visible = show;
		button5.Visible = show;
		button6.Visible = show;
		button7.Visible = show;
		button8.Visible = show;
		button9.Visible = show;
		button10.Visible = show;
		button11.Visible = show;
		button12.Visible = show;
		button13.Visible = show;
		button14.Visible = show;
		button15.Visible = show;
		button16.Visible = show;

		brushWidthLabel.Visible = show;
		brushWidthTextBox.Visible = show;
	}

	private void ChangeWidth(object? sender, EventArgs e)
	{
		string widthStr = ((TextBox)sender!).Text;

		int width;
		if (!int.TryParse(widthStr, out width))
			width = 1;
		ChangeBrushWidth(width);
	}

	private void ChangeColor(object? sender, EventArgs e)
	{
		ChangeBrushColor(((Button)sender!).BackColor);
	}

	public void SetCanvasEnabled(bool enabled)
	{
		ShowEditingTools(enabled);
		this.enabled = enabled;
	}

	public void ChangeBrushColor(Color color)
	{
		pen.Color = color;
	}

	public void ChangeBrushWidth(float width)
	{
		pen.Width = width;
	}

	public byte[]? GetFrame()
	{
		try
		{
			return BitmapToBytes(bitmap);
		}
		catch { return null; }
	}

	public void SetFrame(byte[] bytes)
	{
		try
		{
			using (MemoryStream stream = new MemoryStream(bytes))
			{
				using (Bitmap bitmap = new Bitmap(stream))
				{
					graphics.DrawImage(bitmap, 0, 0);
				}
			}
		}
		catch { }
	}

	static byte[] BitmapToBytes(Bitmap bitmap)
	{
		// Create memory stream to hold the bitmap data
		using (MemoryStream stream = new MemoryStream())
		{
			// Save the bitmap to the memory stream in a specific format (e.g., PNG)
			bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

			// Convert the memory stream to a byte array
			return stream.ToArray();
		}
	}

	private void DrawingCanvas_MouseDown(object sender, MouseEventArgs e)
	{
		if (!enabled)
			return;

		drawing = true;
		startingPosition = new Vector2(e.X, e.Y);
	}
	private void DrawingCanvas_MouseUp(object sender, MouseEventArgs e)
	{
		if (!enabled)
			return;

		drawing = false;
	}
	private void DrawingCanvas_MouseMove(object sender, MouseEventArgs e)
	{
		if (!enabled)
			return;

		if (drawing)
		{
			graphics.DrawLine(pen, new Point((int)startingPosition.X, (int)startingPosition.Y), e.Location);
			bGraphics.DrawLine(pen, new Point((int)startingPosition.X, (int)startingPosition.Y), e.Location);
			startingPosition.X = e.X;
			startingPosition.Y = e.Y;
		}
	}

	public void Clear()
	{
		graphics.Clear(Color.LightGray);
		bGraphics.Clear(Color.LightGray);
	}
}
