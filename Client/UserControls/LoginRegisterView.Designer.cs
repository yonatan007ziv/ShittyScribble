namespace Client.UserControls
{
	partial class LoginRegisterUserControl
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
			loginUsernameLabel = new Label();
			loginPasswordLabel = new Label();
			loginButton = new Button();
			loginUsernameField = new TextBox();
			loginPasswordField = new TextBox();
			registerPasswordField = new TextBox();
			registerUsernameField = new TextBox();
			registerButton = new Button();
			registerPasswordLabel = new Label();
			registerUsernameLabel = new Label();
			registerEmailField = new TextBox();
			registerEmailLabel = new Label();
			register2FAField = new TextBox();
			register2FAButton = new Button();
			SuspendLayout();
			// 
			// loginUsernameLabel
			// 
			loginUsernameLabel.AutoSize = true;
			loginUsernameLabel.Location = new Point(40, 68);
			loginUsernameLabel.Name = "loginUsernameLabel";
			loginUsernameLabel.Size = new Size(63, 15);
			loginUsernameLabel.TabIndex = 0;
			loginUsernameLabel.Text = "Username:";
			// 
			// loginPasswordLabel
			// 
			loginPasswordLabel.AutoSize = true;
			loginPasswordLabel.Location = new Point(43, 110);
			loginPasswordLabel.Name = "loginPasswordLabel";
			loginPasswordLabel.Size = new Size(60, 15);
			loginPasswordLabel.TabIndex = 1;
			loginPasswordLabel.Text = "Password:";
			// 
			// loginButton
			// 
			loginButton.Location = new Point(125, 140);
			loginButton.Name = "loginButton";
			loginButton.Size = new Size(75, 23);
			loginButton.TabIndex = 2;
			loginButton.Text = "Login";
			loginButton.UseVisualStyleBackColor = true;
			loginButton.Click += LoginButton;
			// 
			// loginUsernameField
			// 
			loginUsernameField.Location = new Point(109, 65);
			loginUsernameField.Name = "loginUsernameField";
			loginUsernameField.Size = new Size(100, 23);
			loginUsernameField.TabIndex = 3;
			// 
			// loginPasswordField
			// 
			loginPasswordField.Location = new Point(109, 102);
			loginPasswordField.Name = "loginPasswordField";
			loginPasswordField.Size = new Size(100, 23);
			loginPasswordField.TabIndex = 4;
			// 
			// registerPasswordField
			// 
			registerPasswordField.Location = new Point(405, 105);
			registerPasswordField.Name = "registerPasswordField";
			registerPasswordField.Size = new Size(100, 23);
			registerPasswordField.TabIndex = 9;
			// 
			// registerUsernameField
			// 
			registerUsernameField.Location = new Point(405, 68);
			registerUsernameField.Name = "registerUsernameField";
			registerUsernameField.Size = new Size(100, 23);
			registerUsernameField.TabIndex = 8;
			// 
			// registerButton
			// 
			registerButton.Location = new Point(405, 183);
			registerButton.Name = "registerButton";
			registerButton.Size = new Size(75, 23);
			registerButton.TabIndex = 7;
			registerButton.Text = "Register";
			registerButton.UseVisualStyleBackColor = true;
			registerButton.Click += RegisterButton;
			// 
			// registerPasswordLabel
			// 
			registerPasswordLabel.AutoSize = true;
			registerPasswordLabel.Location = new Point(328, 110);
			registerPasswordLabel.Name = "registerPasswordLabel";
			registerPasswordLabel.Size = new Size(60, 15);
			registerPasswordLabel.TabIndex = 6;
			registerPasswordLabel.Text = "Password:";
			// 
			// registerUsernameLabel
			// 
			registerUsernameLabel.AutoSize = true;
			registerUsernameLabel.Location = new Point(325, 68);
			registerUsernameLabel.Name = "registerUsernameLabel";
			registerUsernameLabel.Size = new Size(63, 15);
			registerUsernameLabel.TabIndex = 5;
			registerUsernameLabel.Text = "Username:";
			// 
			// registerEmailField
			// 
			registerEmailField.Location = new Point(396, 134);
			registerEmailField.Name = "registerEmailField";
			registerEmailField.Size = new Size(100, 23);
			registerEmailField.TabIndex = 11;
			// 
			// registerEmailLabel
			// 
			registerEmailLabel.AutoSize = true;
			registerEmailLabel.Location = new Point(339, 137);
			registerEmailLabel.Name = "registerEmailLabel";
			registerEmailLabel.Size = new Size(39, 15);
			registerEmailLabel.TabIndex = 10;
			registerEmailLabel.Text = "Email:";
			// 
			// register2FAField
			// 
			register2FAField.Location = new Point(600, 94);
			register2FAField.Name = "register2FAField";
			register2FAField.Size = new Size(100, 23);
			register2FAField.TabIndex = 12;
			// 
			// register2FAButton
			// 
			register2FAButton.Location = new Point(613, 134);
			register2FAButton.Name = "register2FAButton";
			register2FAButton.Size = new Size(75, 23);
			register2FAButton.TabIndex = 13;
			register2FAButton.Text = "Send 2FA";
			register2FAButton.UseVisualStyleBackColor = true;
			register2FAButton.Click += Register2FAButton;
			// 
			// LoginRegisterUserControl
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			Controls.Add(register2FAButton);
			Controls.Add(register2FAField);
			Controls.Add(registerEmailField);
			Controls.Add(registerEmailLabel);
			Controls.Add(registerPasswordField);
			Controls.Add(registerUsernameField);
			Controls.Add(registerButton);
			Controls.Add(registerPasswordLabel);
			Controls.Add(registerUsernameLabel);
			Controls.Add(loginPasswordField);
			Controls.Add(loginUsernameField);
			Controls.Add(loginButton);
			Controls.Add(loginPasswordLabel);
			Controls.Add(loginUsernameLabel);
			Name = "LoginRegisterUserControl";
			Size = new Size(815, 479);
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Label loginUsernameLabel;
		private Label loginPasswordLabel;
		private Button loginButton;
		private TextBox loginUsernameField;
		private TextBox loginPasswordField;
		private TextBox registerPasswordField;
		private TextBox registerUsernameField;
		private Button registerButton;
		private Label registerPasswordLabel;
		private Label registerUsernameLabel;
		private TextBox registerEmailField;
		private Label registerEmailLabel;
		private TextBox register2FAField;
		private Button register2FAButton;
	}
}
