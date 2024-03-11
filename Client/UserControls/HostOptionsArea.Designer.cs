namespace Client.UserControls
{
	partial class HostOptionsArea
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			startGameButton = new Button();
			timeToDrawLabel = new Label();
			timeToDrawTextBox = new TextBox();
			numberOfRoundsTextBox = new TextBox();
			numberOfRoundsLabel = new Label();
			secondsLabel1 = new Label();
			secondsLabel2 = new Label();
			SuspendLayout();
			// 
			// startGameButton
			// 
			startGameButton.Location = new Point(120, 61);
			startGameButton.Name = "startGameButton";
			startGameButton.Size = new Size(75, 23);
			startGameButton.TabIndex = 0;
			startGameButton.Text = "Start Game";
			startGameButton.UseVisualStyleBackColor = true;
			startGameButton.Click += startGameButton_Click;
			// 
			// timeToDrawLabel
			// 
			timeToDrawLabel.AutoSize = true;
			timeToDrawLabel.Location = new Point(24, 0);
			timeToDrawLabel.Name = "timeToDrawLabel";
			timeToDrawLabel.Size = new Size(79, 15);
			timeToDrawLabel.TabIndex = 1;
			timeToDrawLabel.Text = "Time to draw:";
			// 
			// timeToDrawTextBox
			// 
			timeToDrawTextBox.Location = new Point(109, -3);
			timeToDrawTextBox.Name = "timeToDrawTextBox";
			timeToDrawTextBox.Size = new Size(100, 23);
			timeToDrawTextBox.TabIndex = 2;
			// 
			// numberOfRoundsTextBox
			// 
			numberOfRoundsTextBox.Location = new Point(109, 32);
			numberOfRoundsTextBox.Name = "numberOfRoundsTextBox";
			numberOfRoundsTextBox.Size = new Size(100, 23);
			numberOfRoundsTextBox.TabIndex = 4;
			// 
			// numberOfRoundsLabel
			// 
			numberOfRoundsLabel.AutoSize = true;
			numberOfRoundsLabel.Location = new Point(-5, 35);
			numberOfRoundsLabel.Name = "numberOfRoundsLabel";
			numberOfRoundsLabel.Size = new Size(108, 15);
			numberOfRoundsLabel.TabIndex = 3;
			numberOfRoundsLabel.Text = "Number of rounds:";
			// 
			// secondsLabel1
			// 
			secondsLabel1.AutoSize = true;
			secondsLabel1.Location = new Point(215, 0);
			secondsLabel1.Name = "secondsLabel1";
			secondsLabel1.Size = new Size(50, 15);
			secondsLabel1.TabIndex = 5;
			secondsLabel1.Text = "seconds";
			// 
			// secondsLabel2
			// 
			secondsLabel2.AutoSize = true;
			secondsLabel2.Location = new Point(215, 35);
			secondsLabel2.Name = "secondsLabel2";
			secondsLabel2.Size = new Size(50, 15);
			secondsLabel2.TabIndex = 7;
			secondsLabel2.Text = "seconds";
			// 
			// HostOptionsArea
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = Color.Transparent;
			Controls.Add(secondsLabel2);
			Controls.Add(secondsLabel1);
			Controls.Add(numberOfRoundsTextBox);
			Controls.Add(numberOfRoundsLabel);
			Controls.Add(timeToDrawTextBox);
			Controls.Add(timeToDrawLabel);
			Controls.Add(startGameButton);
			Name = "HostOptionsArea";
			Size = new Size(290, 106);
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Button startGameButton;
		private Label timeToDrawLabel;
		private TextBox timeToDrawTextBox;
		private TextBox numberOfRoundsTextBox;
		private Label numberOfRoundsLabel;
		private Label secondsLabel1;
		private Label secondsLabel2;
	}
}
