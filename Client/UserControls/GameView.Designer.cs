namespace Client.UserControls
{
	partial class GameView
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
			playerList = new Panel();
			remainingTimeToDrawLabel = new Label();
			remainingNumOfRoundsLabel = new Label();
			scoreboardPanel = new Panel();
			scoreboardLabel = new Label();
			wordButton1 = new Button();
			wordButton2 = new Button();
			wordButton3 = new Button();
			chatPanel = new Panel();
			chatSendButton = new Button();
			chatTextBox = new TextBox();
			currentlyDrawingLabel = new Label();
			drawingCanvas = new DrawingCanvas();
			playerList.SuspendLayout();
			scoreboardPanel.SuspendLayout();
			chatPanel.SuspendLayout();
			SuspendLayout();
			// 
			// playerList
			// 
			playerList.BackColor = Color.Gray;
			playerList.Controls.Add(remainingTimeToDrawLabel);
			playerList.Controls.Add(remainingNumOfRoundsLabel);
			playerList.Location = new Point(3, 3);
			playerList.Name = "playerList";
			playerList.Size = new Size(179, 534);
			playerList.TabIndex = 0;
			// 
			// remainingTimeToDrawLabel
			// 
			remainingTimeToDrawLabel.AutoSize = true;
			remainingTimeToDrawLabel.Location = new Point(3, 511);
			remainingTimeToDrawLabel.Name = "remainingTimeToDrawLabel";
			remainingTimeToDrawLabel.Size = new Size(0, 15);
			remainingTimeToDrawLabel.TabIndex = 1;
			// 
			// remainingNumOfRoundsLabel
			// 
			remainingNumOfRoundsLabel.AutoSize = true;
			remainingNumOfRoundsLabel.Location = new Point(3, 490);
			remainingNumOfRoundsLabel.Name = "remainingNumOfRoundsLabel";
			remainingNumOfRoundsLabel.Size = new Size(0, 15);
			remainingNumOfRoundsLabel.TabIndex = 2;
			// 
			// scoreboardPanel
			// 
			scoreboardPanel.BackColor = Color.FromArgb(255, 128, 0);
			scoreboardPanel.Controls.Add(scoreboardLabel);
			scoreboardPanel.Location = new Point(757, 3);
			scoreboardPanel.Name = "scoreboardPanel";
			scoreboardPanel.Size = new Size(200, 484);
			scoreboardPanel.TabIndex = 3;
			// 
			// scoreboardLabel
			// 
			scoreboardLabel.AutoSize = true;
			scoreboardLabel.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
			scoreboardLabel.Location = new Point(46, 9);
			scoreboardLabel.Name = "scoreboardLabel";
			scoreboardLabel.Size = new Size(114, 28);
			scoreboardLabel.TabIndex = 0;
			scoreboardLabel.Text = "Scoreboard";
			// 
			// wordButton1
			// 
			wordButton1.Location = new Point(245, 239);
			wordButton1.Name = "wordButton1";
			wordButton1.Size = new Size(137, 42);
			wordButton1.TabIndex = 4;
			wordButton1.UseVisualStyleBackColor = true;
			// 
			// wordButton2
			// 
			wordButton2.Location = new Point(388, 239);
			wordButton2.Name = "wordButton2";
			wordButton2.Size = new Size(137, 42);
			wordButton2.TabIndex = 5;
			wordButton2.UseVisualStyleBackColor = true;
			// 
			// wordButton3
			// 
			wordButton3.Location = new Point(531, 239);
			wordButton3.Name = "wordButton3";
			wordButton3.Size = new Size(137, 42);
			wordButton3.TabIndex = 6;
			wordButton3.UseVisualStyleBackColor = true;
			// 
			// chatPanel
			// 
			chatPanel.BackColor = Color.Lime;
			chatPanel.Controls.Add(chatSendButton);
			chatPanel.Controls.Add(chatTextBox);
			chatPanel.Location = new Point(757, 493);
			chatPanel.Name = "chatPanel";
			chatPanel.Size = new Size(200, 44);
			chatPanel.TabIndex = 7;
			// 
			// chatSendButton
			// 
			chatSendButton.Location = new Point(150, 12);
			chatSendButton.Name = "chatSendButton";
			chatSendButton.Size = new Size(47, 23);
			chatSendButton.TabIndex = 1;
			chatSendButton.Text = "Send";
			chatSendButton.UseVisualStyleBackColor = true;
			// 
			// chatTextBox
			// 
			chatTextBox.Location = new Point(3, 13);
			chatTextBox.Name = "chatTextBox";
			chatTextBox.Size = new Size(141, 23);
			chatTextBox.TabIndex = 0;
			// 
			// currentlyDrawingLabel
			// 
			currentlyDrawingLabel.AutoSize = true;
			currentlyDrawingLabel.Location = new Point(424, 12);
			currentlyDrawingLabel.Name = "currentlyDrawingLabel";
			currentlyDrawingLabel.Size = new Size(0, 15);
			currentlyDrawingLabel.TabIndex = 8;
			// 
			// drawingCanvas
			// 
			drawingCanvas.BackColor = Color.LightGray;
			drawingCanvas.Location = new Point(188, 30);
			drawingCanvas.Name = "drawingCanvas";
			drawingCanvas.Size = new Size(563, 507);
			drawingCanvas.TabIndex = 9;
			// 
			// GameView
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			Controls.Add(currentlyDrawingLabel);
			Controls.Add(chatPanel);
			Controls.Add(wordButton3);
			Controls.Add(wordButton2);
			Controls.Add(wordButton1);
			Controls.Add(scoreboardPanel);
			Controls.Add(playerList);
			Controls.Add(drawingCanvas);
			Name = "GameView";
			Size = new Size(960, 540);
			playerList.ResumeLayout(false);
			playerList.PerformLayout();
			scoreboardPanel.ResumeLayout(false);
			scoreboardPanel.PerformLayout();
			chatPanel.ResumeLayout(false);
			chatPanel.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Panel playerList;
		private Label remainingTimeToDrawLabel;
		private Label remainingNumOfRoundsLabel;
		private Panel scoreboardPanel;
		private Label scoreboardLabel;
		private Button wordButton1;
		private Button wordButton2;
		private Button wordButton3;
		private Panel chatPanel;
		private Button chatSendButton;
		private TextBox chatTextBox;
		private Label currentlyDrawingLabel;
		private DrawingCanvas drawingCanvas;
	}
}
