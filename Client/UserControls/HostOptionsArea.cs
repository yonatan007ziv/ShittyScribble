namespace Client.UserControls
{
	internal partial class HostOptionsArea : UserControl
	{
		private readonly GameView parentGameView;

		public HostOptionsArea(GameView parentGameView)
		{
			InitializeComponent();
			this.parentGameView = parentGameView;
		}

		private void startGameButton_Click(object sender, EventArgs e)
		{
			if(!int.TryParse(timeToDrawTextBox.Text, out int timeToDraw))
			{
				MessageBox.Show("Invalid time to draw!");
				return;
			}
			if (!int.TryParse(numberOfRoundsTextBox.Text, out int numOfRounds))
			{
				MessageBox.Show("Invalid number of rounds!");
				return;
			}

			parentGameView.HostSendStartGame(timeToDraw, numOfRounds);
		}
	}
}
