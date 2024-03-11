namespace Client.UserControls
{
	partial class LobbySelector
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
			lobbyNameLabel = new Label();
			lobbyDescriptionLabel = new Label();
			maxPlayersLabel = new Label();
			nameField = new TextBox();
			descriptionField = new TextBox();
			maxPlayersField = new TextBox();
			createLobbyButton = new Button();
			lobbiesList = new Panel();
			SuspendLayout();
			// 
			// lobbyNameLabel
			// 
			lobbyNameLabel.AutoSize = true;
			lobbyNameLabel.Location = new Point(44, 341);
			lobbyNameLabel.Name = "lobbyNameLabel";
			lobbyNameLabel.Size = new Size(42, 15);
			lobbyNameLabel.TabIndex = 1;
			lobbyNameLabel.Text = "Name:";
			// 
			// lobbyDescriptionLabel
			// 
			lobbyDescriptionLabel.AutoSize = true;
			lobbyDescriptionLabel.Location = new Point(150, 341);
			lobbyDescriptionLabel.Name = "lobbyDescriptionLabel";
			lobbyDescriptionLabel.Size = new Size(70, 15);
			lobbyDescriptionLabel.TabIndex = 2;
			lobbyDescriptionLabel.Text = "Description:";
			// 
			// maxPlayersLabel
			// 
			maxPlayersLabel.AutoSize = true;
			maxPlayersLabel.Location = new Point(315, 341);
			maxPlayersLabel.Name = "maxPlayersLabel";
			maxPlayersLabel.Size = new Size(73, 15);
			maxPlayersLabel.TabIndex = 4;
			maxPlayersLabel.Text = "Max Players:";
			// 
			// nameField
			// 
			nameField.Location = new Point(44, 359);
			nameField.Name = "nameField";
			nameField.Size = new Size(100, 23);
			nameField.TabIndex = 5;
			// 
			// descriptionField
			// 
			descriptionField.Location = new Point(150, 359);
			descriptionField.Name = "descriptionField";
			descriptionField.Size = new Size(159, 23);
			descriptionField.TabIndex = 6;
			// 
			// maxPlayersField
			// 
			maxPlayersField.Location = new Point(315, 358);
			maxPlayersField.Name = "maxPlayersField";
			maxPlayersField.Size = new Size(100, 23);
			maxPlayersField.TabIndex = 7;
			// 
			// createLobbyButton
			// 
			createLobbyButton.Location = new Point(421, 357);
			createLobbyButton.Name = "createLobbyButton";
			createLobbyButton.Size = new Size(106, 23);
			createLobbyButton.TabIndex = 8;
			createLobbyButton.Text = "Create Lobby";
			createLobbyButton.UseVisualStyleBackColor = true;
			createLobbyButton.Click += CreateLobbyButton;
			// 
			// lobbiesList
			// 
			lobbiesList.AutoScroll = true;
			lobbiesList.Location = new Point(3, 3);
			lobbiesList.Name = "lobbiesList";
			lobbiesList.Size = new Size(802, 400);
			lobbiesList.TabIndex = 9;
			// 
			// LobbySelector
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			Controls.Add(createLobbyButton);
			Controls.Add(maxPlayersField);
			Controls.Add(descriptionField);
			Controls.Add(nameField);
			Controls.Add(maxPlayersLabel);
			Controls.Add(lobbyDescriptionLabel);
			Controls.Add(lobbyNameLabel);
			Controls.Add(lobbiesList);
			Name = "LobbySelector";
			Size = new Size(808, 406);
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion
		private Label lobbyNameLabel;
		private Label lobbyDescriptionLabel;
		private Label maxPlayersLabel;
		private TextBox nameField;
		private TextBox descriptionField;
		private TextBox maxPlayersField;
		private Button createLobbyButton;
		private Panel lobbiesList;
	}
}
