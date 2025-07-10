namespace GlobeSimulator {
	partial class Sky {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			upLabel = new Label();
			downLabel = new Label();
			leftLabel = new Label();
			rightLabel = new Label();
			((System.ComponentModel.ISupportInitialize)screen).BeginInit();
			SuspendLayout();
			// 
			// screen
			// 
			screen.Location = new Point(45, 29);
			screen.Size = new Size(298, 300);
			// 
			// upLabel
			// 
			upLabel.AutoSize = true;
			upLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 238);
			upLabel.Location = new Point(158, 11);
			upLabel.Name = "upLabel";
			upLabel.Size = new Size(40, 15);
			upLabel.TabIndex = 2;
			upLabel.Text = "South";
			// 
			// downLabel
			// 
			downLabel.AutoSize = true;
			downLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 238);
			downLabel.Location = new Point(158, 356);
			downLabel.Name = "downLabel";
			downLabel.Size = new Size(40, 15);
			downLabel.TabIndex = 3;
			downLabel.Text = "North";
			// 
			// leftLabel
			// 
			leftLabel.AutoSize = true;
			leftLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 238);
			leftLabel.Location = new Point(3, 179);
			leftLabel.Name = "leftLabel";
			leftLabel.Size = new Size(36, 15);
			leftLabel.TabIndex = 4;
			leftLabel.Text = "West";
			// 
			// rightLabel
			// 
			rightLabel.AutoSize = true;
			rightLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 238);
			rightLabel.Location = new Point(349, 179);
			rightLabel.Name = "rightLabel";
			rightLabel.Size = new Size(29, 15);
			rightLabel.TabIndex = 5;
			rightLabel.Text = "East";
			// 
			// Sky
			// 
			ClientSize = new Size(381, 380);
			Controls.Add(rightLabel);
			Controls.Add(leftLabel);
			Controls.Add(downLabel);
			Controls.Add(upLabel);
			Name = "Sky";
			Text = "Globe Simulator - Sky View";
			Controls.SetChildIndex(screen, 0);
			Controls.SetChildIndex(upLabel, 0);
			Controls.SetChildIndex(downLabel, 0);
			Controls.SetChildIndex(leftLabel, 0);
			Controls.SetChildIndex(rightLabel, 0);
			((System.ComponentModel.ISupportInitialize)screen).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Label upLabel;
		private Label downLabel;
		private Label leftLabel;
		private Label rightLabel;
	}
}
