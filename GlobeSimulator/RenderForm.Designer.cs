namespace GlobeSimulator {
	partial class RenderForm {
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
			screen = new PictureBox();
			((System.ComponentModel.ISupportInitialize)screen).BeginInit();
			SuspendLayout();
			// 
			// screen
			// 
			screen.Location = new Point(12, 12);
			screen.Name = "screen";
			screen.Size = new Size(100, 100);
			screen.SizeMode = PictureBoxSizeMode.StretchImage;
			screen.Paint += Screen_Paint;
			screen.TabIndex = 1;
			screen.TabStop = false;
			// 
			// RenderForm
			// 
			ClientSize = new Size(160, 160);
			Controls.Add(screen);
			Name = "RenderForm";
			Text = "Globe Simulator - Render Form";
			Load += RenderForm_Load;
			SizeChanged += RenderForm_SizeChanged;
			((System.ComponentModel.ISupportInitialize)screen).EndInit();
			ResumeLayout(false);
		}

		#endregion

		protected System.Windows.Forms.PictureBox screen;
	}
}
