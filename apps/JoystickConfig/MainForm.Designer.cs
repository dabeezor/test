namespace JoystickConfig
{
	partial class MainForm
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.activeButtonTextBox = new System.Windows.Forms.TextBox();
			this.exitButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// activeButtonTextBox
			// 
			this.activeButtonTextBox.Location = new System.Drawing.Point(39, 104);
			this.activeButtonTextBox.Name = "activeButtonTextBox";
			this.activeButtonTextBox.Size = new System.Drawing.Size(154, 20);
			this.activeButtonTextBox.TabIndex = 0;
			// 
			// exitButton
			// 
			this.exitButton.Location = new System.Drawing.Point(197, 227);
			this.exitButton.Name = "exitButton";
			this.exitButton.Size = new System.Drawing.Size(75, 23);
			this.exitButton.TabIndex = 1;
			this.exitButton.Text = "EXIT";
			this.exitButton.UseVisualStyleBackColor = true;
			this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Controls.Add(this.exitButton);
			this.Controls.Add(this.activeButtonTextBox);
			this.Name = "MainForm";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox activeButtonTextBox;
		private System.Windows.Forms.Button exitButton;
	}
}

