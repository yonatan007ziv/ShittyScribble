using Client.UserControls;

namespace Client
{
	public partial class MainForm : Form
	{
		public static MainForm Instance = null!;

		public MainForm()
		{
			Instance = this;

			InitializeComponent();
			Controls.Add(new LoginRegisterUserControl());
		}

		public void NavigateTo(UserControl control)
		{
			Controls.Clear();
			Controls.Add(control);
		}
	}
}