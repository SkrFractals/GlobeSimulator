namespace GlobeSimulator {
	partial class Simulator {
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
			components = new System.ComponentModel.Container();
			tick = new System.Windows.Forms.Timer(components);
			tiltBox = new TextBox();
			tiltLabel = new Label();
			label1 = new Label();
			chartButton = new Button();
			globeButton = new Button();
			skyButton = new Button();
			timeBar = new TrackBar();
			latitudeLabel = new Label();
			latitudeBar = new TrackBar();
			wrapX = new Button();
			longitudeLabel = new Label();
			longitudeBar = new TrackBar();
			spinsLabel = new Label();
			spinsBox = new TextBox();
			twilightLabel = new Label();
			twilightBox = new TextBox();
			timeExport = new Label();
			shiftBox = new TextBox();
			label2 = new Label();
			eccentricityBox = new TextBox();
			label3 = new Label();
			leapYearBox = new TextBox();
			leapYearLabel = new Label();
			leapDayBox = new TextBox();
			label4 = new Label();
			rewindBox = new CheckBox();
			maxGlobeResBox = new TextBox();
			globeResLabel = new Label();
			magneticCompassBox = new CheckBox();
			flipSkyNorth = new CheckBox();
			maxSkyResLabel = new Label();
			maxSkyResBox = new TextBox();
			maxSkySunsLabel = new Label();
			maxSkySunsBox = new TextBox();
			magLatitudeBox = new TextBox();
			magLongitudeBox = new TextBox();
			zRadiusBox = new TextBox();
			xRadiusBox = new TextBox();
			skyFovBox = new TextBox();
			magLatitudeLabel = new Label();
			magLongitudeLabel = new Label();
			xRadiusLabel = new Label();
			zRadiusLabel = new Label();
			skyAngleLabel = new Label();
			latitudeBox = new TextBox();
			longitudeBox = new TextBox();
			smearLabel = new Label();
			smearBar = new TrackBar();
			globeDetailLabel = new Label();
			globeDetailBox = new ComboBox();
			auBox = new TextBox();
			label5 = new Label();
			dateLabel = new Label();
			timePicker = new DateTimePicker();
			datePicker = new DateTimePicker();
			((System.ComponentModel.ISupportInitialize)timeBar).BeginInit();
			((System.ComponentModel.ISupportInitialize)latitudeBar).BeginInit();
			((System.ComponentModel.ISupportInitialize)longitudeBar).BeginInit();
			((System.ComponentModel.ISupportInitialize)smearBar).BeginInit();
			SuspendLayout();
			// 
			// tick
			// 
			tick.Enabled = true;
			tick.Interval = 50;
			tick.Tick += Tick_Tick;
			// 
			// tiltBox
			// 
			tiltBox.Location = new Point(815, 272);
			tiltBox.Margin = new Padding(4, 3, 4, 3);
			tiltBox.Name = "tiltBox";
			tiltBox.Size = new Size(75, 23);
			tiltBox.TabIndex = 18;
			tiltBox.Text = "23.44";
			tiltBox.TextChanged += TiltBox_TextChanged;
			// 
			// tiltLabel
			// 
			tiltLabel.AutoSize = true;
			tiltLabel.Location = new Point(702, 275);
			tiltLabel.Margin = new Padding(4, 0, 4, 0);
			tiltLabel.Name = "tiltLabel";
			tiltLabel.Size = new Size(105, 15);
			tiltLabel.TabIndex = 1;
			tiltLabel.Text = "Tilt (Earth = 23.44):";
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point(10, 18);
			label1.Margin = new Padding(4, 0, 4, 0);
			label1.Name = "label1";
			label1.Size = new Size(71, 15);
			label1.TabIndex = 4;
			label1.Text = "Time Speed:";
			// 
			// chartButton
			// 
			chartButton.Location = new Point(10, 272);
			chartButton.Margin = new Padding(4, 3, 4, 3);
			chartButton.Name = "chartButton";
			chartButton.Size = new Size(98, 27);
			chartButton.TabIndex = 8;
			chartButton.Text = "Daylight Chart";
			chartButton.UseVisualStyleBackColor = true;
			chartButton.Click += ChartButton_Click;
			// 
			// globeButton
			// 
			globeButton.Location = new Point(10, 305);
			globeButton.Margin = new Padding(4, 3, 4, 3);
			globeButton.Name = "globeButton";
			globeButton.Size = new Size(98, 27);
			globeButton.TabIndex = 9;
			globeButton.Text = "Globe Map";
			globeButton.UseVisualStyleBackColor = true;
			globeButton.Click += GlobeButton_Click;
			// 
			// skyButton
			// 
			skyButton.Location = new Point(10, 438);
			skyButton.Margin = new Padding(4, 3, 4, 3);
			skyButton.Name = "skyButton";
			skyButton.Size = new Size(98, 27);
			skyButton.TabIndex = 13;
			skyButton.Text = "Sky Map";
			skyButton.UseVisualStyleBackColor = true;
			skyButton.Click += SkyButton_Click;
			// 
			// timeBar
			// 
			timeBar.Location = new Point(14, 37);
			timeBar.Margin = new Padding(4, 3, 4, 3);
			timeBar.Maximum = 360;
			timeBar.Name = "timeBar";
			timeBar.Size = new Size(876, 45);
			timeBar.TabIndex = 4;
			timeBar.Scroll += TimeBar_Scroll;
			// 
			// latitudeLabel
			// 
			latitudeLabel.AutoSize = true;
			latitudeLabel.Location = new Point(10, 101);
			latitudeLabel.Margin = new Padding(4, 0, 4, 0);
			latitudeLabel.Name = "latitudeLabel";
			latitudeLabel.Size = new Size(107, 15);
			latitudeLabel.TabIndex = 5;
			latitudeLabel.Text = "Observer Lattitude:";
			// 
			// latitudeBar
			// 
			latitudeBar.LargeChange = 15;
			latitudeBar.Location = new Point(14, 129);
			latitudeBar.Margin = new Padding(4, 3, 4, 3);
			latitudeBar.Maximum = 180;
			latitudeBar.Name = "latitudeBar";
			latitudeBar.Size = new Size(876, 45);
			latitudeBar.TabIndex = 4;
			latitudeBar.Value = 90;
			latitudeBar.Scroll += LatitudeBar_Scroll;
			// 
			// wrapX
			// 
			wrapX.Enabled = false;
			wrapX.Location = new Point(768, 188);
			wrapX.Margin = new Padding(4, 3, 4, 3);
			wrapX.Name = "wrapX";
			wrapX.Size = new Size(122, 27);
			wrapX.TabIndex = 6;
			wrapX.Text = "Wrap Longitude";
			wrapX.UseVisualStyleBackColor = true;
			wrapX.Click += WrapX_Click;
			// 
			// longitudeLabel
			// 
			longitudeLabel.AutoSize = true;
			longitudeLabel.Location = new Point(10, 194);
			longitudeLabel.Margin = new Padding(4, 0, 4, 0);
			longitudeLabel.Name = "longitudeLabel";
			longitudeLabel.Size = new Size(114, 15);
			longitudeLabel.TabIndex = 9;
			longitudeLabel.Text = "Observer Longitude:";
			// 
			// longitudeBar
			// 
			longitudeBar.LargeChange = 45;
			longitudeBar.Location = new Point(14, 221);
			longitudeBar.Margin = new Padding(4, 3, 4, 3);
			longitudeBar.Maximum = 360;
			longitudeBar.Name = "longitudeBar";
			longitudeBar.Size = new Size(876, 45);
			longitudeBar.TabIndex = 7;
			longitudeBar.Value = 180;
			longitudeBar.Scroll += LongitudeBar_Scroll;
			// 
			// spinsLabel
			// 
			spinsLabel.AutoSize = true;
			spinsLabel.Location = new Point(654, 333);
			spinsLabel.Margin = new Padding(4, 0, 4, 0);
			spinsLabel.Name = "spinsLabel";
			spinsLabel.Size = new Size(153, 15);
			spinsLabel.TabIndex = 11;
			spinsLabel.Text = "Spins Per Year (Earth = 366):";
			// 
			// spinsBox
			// 
			spinsBox.Location = new Point(815, 330);
			spinsBox.Margin = new Padding(4, 3, 4, 3);
			spinsBox.Name = "spinsBox";
			spinsBox.Size = new Size(75, 23);
			spinsBox.TabIndex = 20;
			spinsBox.Text = "366";
			spinsBox.TextChanged += SpinsBox_TextChanged;
			// 
			// twilightLabel
			// 
			twilightLabel.AutoSize = true;
			twilightLabel.Location = new Point(730, 450);
			twilightLabel.Margin = new Padding(4, 0, 4, 0);
			twilightLabel.Name = "twilightLabel";
			twilightLabel.Size = new Size(75, 15);
			twilightLabel.TabIndex = 13;
			twilightLabel.Text = "Twilight Blur:";
			// 
			// twilightBox
			// 
			twilightBox.Location = new Point(815, 446);
			twilightBox.Margin = new Padding(4, 3, 4, 3);
			twilightBox.Name = "twilightBox";
			twilightBox.Size = new Size(75, 23);
			twilightBox.TabIndex = 24;
			twilightBox.Text = "5";
			twilightBox.TextChanged += TwilightBox_TextChanged;
			// 
			// timeExport
			// 
			timeExport.AutoSize = true;
			timeExport.Font = new Font("Consolas", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 238);
			timeExport.Location = new Point(14, 583);
			timeExport.Margin = new Padding(4, 0, 4, 0);
			timeExport.Name = "timeExport";
			timeExport.Size = new Size(118, 24);
			timeExport.TabIndex = 15;
			timeExport.Text = "Date/Time";
			// 
			// shiftBox
			// 
			shiftBox.Location = new Point(815, 417);
			shiftBox.Margin = new Padding(4, 3, 4, 3);
			shiftBox.Name = "shiftBox";
			shiftBox.Size = new Size(75, 23);
			shiftBox.TabIndex = 23;
			shiftBox.Text = "264";
			shiftBox.TextChanged += ShiftBox_TextChanged;
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new Point(627, 420);
			label2.Margin = new Padding(4, 0, 4, 0);
			label2.Name = "label2";
			label2.Size = new Size(180, 15);
			label2.TabIndex = 17;
			label2.Text = "Day Of Fall Equinox (Earth = 264)";
			// 
			// eccentricityBox
			// 
			eccentricityBox.Location = new Point(815, 301);
			eccentricityBox.Margin = new Padding(4, 3, 4, 3);
			eccentricityBox.Name = "eccentricityBox";
			eccentricityBox.Size = new Size(75, 23);
			eccentricityBox.TabIndex = 19;
			eccentricityBox.Text = "0.0167";
			eccentricityBox.TextChanged += EccentricityBox_TextChanged;
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new Point(651, 304);
			label3.Margin = new Padding(4, 0, 4, 0);
			label3.Name = "label3";
			label3.Size = new Size(156, 15);
			label3.TabIndex = 19;
			label3.Text = "Eccentricity (Earth = 0.0167):";
			// 
			// leapYearBox
			// 
			leapYearBox.Location = new Point(815, 359);
			leapYearBox.Margin = new Padding(4, 3, 4, 3);
			leapYearBox.Name = "leapYearBox";
			leapYearBox.Size = new Size(75, 23);
			leapYearBox.TabIndex = 21;
			leapYearBox.Text = "4";
			leapYearBox.TextChanged += LeapYearBox_TextChanged;
			// 
			// leapYearLabel
			// 
			leapYearLabel.AutoSize = true;
			leapYearLabel.Location = new Point(689, 362);
			leapYearLabel.Margin = new Padding(4, 0, 4, 0);
			leapYearLabel.Name = "leapYearLabel";
			leapYearLabel.Size = new Size(118, 15);
			leapYearLabel.TabIndex = 21;
			leapYearLabel.Text = "Leap Year (Earth = 4):";
			// 
			// leapDayBox
			// 
			leapDayBox.Location = new Point(815, 388);
			leapDayBox.Margin = new Padding(4, 3, 4, 3);
			leapDayBox.Name = "leapDayBox";
			leapDayBox.Size = new Size(75, 23);
			leapDayBox.TabIndex = 22;
			leapDayBox.Text = "60";
			leapDayBox.TextChanged += LeapDayBox_TextChanged;
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Location = new Point(559, 391);
			label4.Margin = new Padding(4, 0, 4, 0);
			label4.Name = "label4";
			label4.Size = new Size(248, 15);
			label4.TabIndex = 23;
			label4.Text = "Which is the Leap Day in the Year (Earth = 59):";
			// 
			// rewindBox
			// 
			rewindBox.AutoSize = true;
			rewindBox.Location = new Point(88, 18);
			rewindBox.Name = "rewindBox";
			rewindBox.Size = new Size(65, 19);
			rewindBox.TabIndex = 1;
			rewindBox.Text = "Rewind";
			rewindBox.UseVisualStyleBackColor = true;
			rewindBox.CheckedChanged += RewindBox_CheckedChanged;
			// 
			// maxGlobeResBox
			// 
			maxGlobeResBox.Location = new Point(150, 367);
			maxGlobeResBox.Name = "maxGlobeResBox";
			maxGlobeResBox.Size = new Size(159, 23);
			maxGlobeResBox.TabIndex = 11;
			maxGlobeResBox.Text = "640";
			maxGlobeResBox.TextChanged += MaxGlobeResBox_TextChanged;
			// 
			// globeResLabel
			// 
			globeResLabel.AutoSize = true;
			globeResLabel.Location = new Point(14, 370);
			globeResLabel.Name = "globeResLabel";
			globeResLabel.Size = new Size(126, 15);
			globeResLabel.TabIndex = 28;
			globeResLabel.Text = "Max Globe Resolution:";
			// 
			// magneticCompassBox
			// 
			magneticCompassBox.AutoSize = true;
			magneticCompassBox.Checked = true;
			magneticCompassBox.CheckState = CheckState.Checked;
			magneticCompassBox.Location = new Point(14, 471);
			magneticCompassBox.Name = "magneticCompassBox";
			magneticCompassBox.Size = new Size(149, 19);
			magneticCompassBox.TabIndex = 14;
			magneticCompassBox.Text = "Magnetic Sky Compass";
			magneticCompassBox.UseVisualStyleBackColor = true;
			magneticCompassBox.CheckedChanged += MagneticCompassBox_CheckedChanged;
			// 
			// flipSkyNorth
			// 
			flipSkyNorth.AutoSize = true;
			flipSkyNorth.Location = new Point(14, 496);
			flipSkyNorth.Name = "flipSkyNorth";
			flipSkyNorth.Size = new Size(100, 19);
			flipSkyNorth.TabIndex = 15;
			flipSkyNorth.Text = "Flip Sky North";
			flipSkyNorth.UseVisualStyleBackColor = true;
			flipSkyNorth.CheckedChanged += FlipSkyNorth_CheckedChanged;
			// 
			// maxSkyResLabel
			// 
			maxSkyResLabel.AutoSize = true;
			maxSkyResLabel.Location = new Point(14, 522);
			maxSkyResLabel.Name = "maxSkyResLabel";
			maxSkyResLabel.Size = new Size(113, 15);
			maxSkyResLabel.TabIndex = 32;
			maxSkyResLabel.Text = "Max Sky Resolution:";
			// 
			// maxSkyResBox
			// 
			maxSkyResBox.Location = new Point(146, 519);
			maxSkyResBox.Name = "maxSkyResBox";
			maxSkyResBox.Size = new Size(100, 23);
			maxSkyResBox.TabIndex = 16;
			maxSkyResBox.Text = "640";
			maxSkyResBox.TextChanged += MaxSkyResBox_TextChanged;
			// 
			// maxSkySunsLabel
			// 
			maxSkySunsLabel.AutoSize = true;
			maxSkySunsLabel.Location = new Point(14, 551);
			maxSkySunsLabel.Name = "maxSkySunsLabel";
			maxSkySunsLabel.Size = new Size(124, 15);
			maxSkySunsLabel.TabIndex = 34;
			maxSkySunsLabel.Text = "Max Sky Sun Samples:";
			// 
			// maxSkySunsBox
			// 
			maxSkySunsBox.Location = new Point(146, 548);
			maxSkySunsBox.Name = "maxSkySunsBox";
			maxSkySunsBox.Size = new Size(100, 23);
			maxSkySunsBox.TabIndex = 17;
			maxSkySunsBox.Text = "640";
			maxSkySunsBox.TextChanged += MaxSkySunsBox_TextChanged;
			// 
			// magLatitudeBox
			// 
			magLatitudeBox.Location = new Point(815, 475);
			magLatitudeBox.Margin = new Padding(4, 3, 4, 3);
			magLatitudeBox.Name = "magLatitudeBox";
			magLatitudeBox.Size = new Size(75, 23);
			magLatitudeBox.TabIndex = 25;
			magLatitudeBox.Text = "85.762";
			magLatitudeBox.TextChanged += MagLatitudeBox_TextChanged;
			// 
			// magLongitudeBox
			// 
			magLongitudeBox.Location = new Point(815, 504);
			magLongitudeBox.Margin = new Padding(4, 3, 4, 3);
			magLongitudeBox.Name = "magLongitudeBox";
			magLongitudeBox.Size = new Size(75, 23);
			magLongitudeBox.TabIndex = 26;
			magLongitudeBox.Text = "139.298";
			magLongitudeBox.TextChanged += MagLongitudeBox_TextChanged;
			// 
			// zRadiusBox
			// 
			zRadiusBox.Location = new Point(815, 562);
			zRadiusBox.Margin = new Padding(4, 3, 4, 3);
			zRadiusBox.Name = "zRadiusBox";
			zRadiusBox.Size = new Size(75, 23);
			zRadiusBox.TabIndex = 28;
			zRadiusBox.Text = "6356.7523";
			zRadiusBox.TextChanged += ZRadiusBox_TextChanged;
			// 
			// xRadiusBox
			// 
			xRadiusBox.Location = new Point(815, 533);
			xRadiusBox.Margin = new Padding(4, 3, 4, 3);
			xRadiusBox.Name = "xRadiusBox";
			xRadiusBox.Size = new Size(75, 23);
			xRadiusBox.TabIndex = 27;
			xRadiusBox.Text = "6378.137";
			xRadiusBox.TextChanged += XRadiusBox_TextChanged;
			// 
			// skyFovBox
			// 
			skyFovBox.Location = new Point(815, 591);
			skyFovBox.Margin = new Padding(4, 3, 4, 3);
			skyFovBox.Name = "skyFovBox";
			skyFovBox.Size = new Size(75, 23);
			skyFovBox.TabIndex = 29;
			skyFovBox.Text = "181.21";
			skyFovBox.TextChanged += SkyFovBox_TextChanged;
			// 
			// magLatitudeLabel
			// 
			magLatitudeLabel.AutoSize = true;
			magLatitudeLabel.Location = new Point(697, 478);
			magLatitudeLabel.Margin = new Padding(4, 0, 4, 0);
			magLatitudeLabel.Name = "magLatitudeLabel";
			magLatitudeLabel.Size = new Size(110, 15);
			magLatitudeLabel.TabIndex = 40;
			magLatitudeLabel.Text = "Magnetic Lattitude:";
			// 
			// magLongitudeLabel
			// 
			magLongitudeLabel.AutoSize = true;
			magLongitudeLabel.Location = new Point(690, 507);
			magLongitudeLabel.Margin = new Padding(4, 0, 4, 0);
			magLongitudeLabel.Name = "magLongitudeLabel";
			magLongitudeLabel.Size = new Size(117, 15);
			magLongitudeLabel.TabIndex = 41;
			magLongitudeLabel.Text = "Magnetic Longitude:";
			// 
			// xRadiusLabel
			// 
			xRadiusLabel.AutoSize = true;
			xRadiusLabel.Location = new Point(706, 536);
			xRadiusLabel.Margin = new Padding(4, 0, 4, 0);
			xRadiusLabel.Name = "xRadiusLabel";
			xRadiusLabel.Size = new Size(101, 15);
			xRadiusLabel.TabIndex = 42;
			xRadiusLabel.Text = "Equatorial Radius:";
			// 
			// zRadiusLabel
			// 
			zRadiusLabel.AutoSize = true;
			zRadiusLabel.Location = new Point(732, 565);
			zRadiusLabel.Margin = new Padding(4, 0, 4, 0);
			zRadiusLabel.Name = "zRadiusLabel";
			zRadiusLabel.Size = new Size(75, 15);
			zRadiusLabel.TabIndex = 43;
			zRadiusLabel.Text = "Polar Radius:";
			// 
			// skyAngleLabel
			// 
			skyAngleLabel.AutoSize = true;
			skyAngleLabel.Location = new Point(688, 594);
			skyAngleLabel.Margin = new Padding(4, 0, 4, 0);
			skyAngleLabel.Name = "skyAngleLabel";
			skyAngleLabel.Size = new Size(119, 15);
			skyAngleLabel.TabIndex = 44;
			skyAngleLabel.Text = "Horizon Angle (FOV):";
			// 
			// latitudeBox
			// 
			latitudeBox.Location = new Point(131, 100);
			latitudeBox.Name = "latitudeBox";
			latitudeBox.Size = new Size(100, 23);
			latitudeBox.TabIndex = 3;
			latitudeBox.Text = "0";
			latitudeBox.TextChanged += LatitudeBox_TextChanged;
			// 
			// longitudeBox
			// 
			longitudeBox.Location = new Point(131, 192);
			longitudeBox.Name = "longitudeBox";
			longitudeBox.Size = new Size(100, 23);
			longitudeBox.TabIndex = 5;
			longitudeBox.Text = "0";
			longitudeBox.TextChanged += LongitudeBox_TextChanged;
			// 
			// smearLabel
			// 
			smearLabel.AutoSize = true;
			smearLabel.Location = new Point(14, 396);
			smearLabel.Name = "smearLabel";
			smearLabel.Size = new Size(114, 15);
			smearLabel.TabIndex = 47;
			smearLabel.Text = "Texture Smear Level:";
			// 
			// smearBar
			// 
			smearBar.LargeChange = 45;
			smearBar.Location = new Point(150, 396);
			smearBar.Margin = new Padding(4, 3, 4, 3);
			smearBar.Maximum = 5;
			smearBar.Name = "smearBar";
			smearBar.Size = new Size(159, 45);
			smearBar.TabIndex = 12;
			smearBar.Value = 4;
			smearBar.Scroll += SmearBar_Scroll;
			// 
			// globeDetailLabel
			// 
			globeDetailLabel.AutoSize = true;
			globeDetailLabel.Location = new Point(14, 341);
			globeDetailLabel.Name = "globeDetailLabel";
			globeDetailLabel.Size = new Size(80, 15);
			globeDetailLabel.TabIndex = 49;
			globeDetailLabel.Text = "Render Detail:";
			// 
			// globeDetailBox
			// 
			globeDetailBox.FormattingEnabled = true;
			globeDetailBox.Items.AddRange(new object[] { "PreDrawSimpleSphereNoOblateness", "SimpleOblateSpheroid", "TexturedOblateSpheroid" });
			globeDetailBox.Location = new Point(150, 338);
			globeDetailBox.Name = "globeDetailBox";
			globeDetailBox.Size = new Size(159, 23);
			globeDetailBox.TabIndex = 10;
			globeDetailBox.SelectedIndexChanged += GlobeDetailBox_SelectedIndexChanged;
			// 
			// auBox
			// 
			auBox.Location = new Point(815, 620);
			auBox.Margin = new Padding(4, 3, 4, 3);
			auBox.Name = "auBox";
			auBox.Size = new Size(75, 23);
			auBox.TabIndex = 30;
			auBox.Text = "149597870";
			auBox.TextChanged += AuBox_TextChanged;
			// 
			// label5
			// 
			label5.AutoSize = true;
			label5.Location = new Point(650, 623);
			label5.Margin = new Padding(4, 0, 4, 0);
			label5.Name = "label5";
			label5.Size = new Size(157, 15);
			label5.TabIndex = 52;
			label5.Text = "Average distance to the Sun:";
			// 
			// dateLabel
			// 
			dateLabel.AutoSize = true;
			dateLabel.Location = new Point(502, 14);
			dateLabel.Name = "dateLabel";
			dateLabel.Size = new Size(65, 15);
			dateLabel.TabIndex = 53;
			dateLabel.Text = "Date-Time:";
			// 
			// timePicker
			// 
			timePicker.Format = DateTimePickerFormat.Time;
			timePicker.Location = new Point(786, 8);
			timePicker.MinDate = new DateTime(2000, 1, 1, 0, 0, 0, 0);
			timePicker.Name = "timePicker";
			timePicker.ShowUpDown = true;
			timePicker.Size = new Size(104, 23);
			timePicker.TabIndex = 2;
			timePicker.ValueChanged += TimePicker_ValueChanged;
			// 
			// datePicker
			// 
			datePicker.CustomFormat = "MM/dd/yyyy hh:mm tt";
			datePicker.Location = new Point(573, 8);
			datePicker.MinDate = new DateTime(2000, 1, 1, 0, 0, 0, 0);
			datePicker.Name = "datePicker";
			datePicker.Size = new Size(207, 23);
			datePicker.TabIndex = 54;
			datePicker.ValueChanged += DatePicker_ValueChanged;
			// 
			// Simulator
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(912, 689);
			Controls.Add(datePicker);
			Controls.Add(timePicker);
			Controls.Add(dateLabel);
			Controls.Add(label5);
			Controls.Add(auBox);
			Controls.Add(globeDetailBox);
			Controls.Add(globeDetailLabel);
			Controls.Add(smearBar);
			Controls.Add(smearLabel);
			Controls.Add(longitudeBox);
			Controls.Add(latitudeBox);
			Controls.Add(skyAngleLabel);
			Controls.Add(zRadiusLabel);
			Controls.Add(xRadiusLabel);
			Controls.Add(magLongitudeLabel);
			Controls.Add(magLatitudeLabel);
			Controls.Add(skyFovBox);
			Controls.Add(zRadiusBox);
			Controls.Add(xRadiusBox);
			Controls.Add(magLongitudeBox);
			Controls.Add(magLatitudeBox);
			Controls.Add(maxSkySunsLabel);
			Controls.Add(maxSkySunsBox);
			Controls.Add(maxSkyResLabel);
			Controls.Add(maxSkyResBox);
			Controls.Add(flipSkyNorth);
			Controls.Add(magneticCompassBox);
			Controls.Add(globeResLabel);
			Controls.Add(maxGlobeResBox);
			Controls.Add(rewindBox);
			Controls.Add(label4);
			Controls.Add(leapDayBox);
			Controls.Add(leapYearLabel);
			Controls.Add(leapYearBox);
			Controls.Add(label3);
			Controls.Add(eccentricityBox);
			Controls.Add(label2);
			Controls.Add(shiftBox);
			Controls.Add(timeExport);
			Controls.Add(twilightBox);
			Controls.Add(twilightLabel);
			Controls.Add(spinsBox);
			Controls.Add(spinsLabel);
			Controls.Add(longitudeBar);
			Controls.Add(longitudeLabel);
			Controls.Add(wrapX);
			Controls.Add(latitudeBar);
			Controls.Add(latitudeLabel);
			Controls.Add(timeBar);
			Controls.Add(skyButton);
			Controls.Add(globeButton);
			Controls.Add(chartButton);
			Controls.Add(label1);
			Controls.Add(tiltLabel);
			Controls.Add(tiltBox);
			Margin = new Padding(4, 3, 4, 3);
			MaximumSize = new Size(928, 728);
			MinimumSize = new Size(928, 728);
			Name = "Simulator";
			Text = "Globe Simulator";
			Load += Simulator_Load;
			((System.ComponentModel.ISupportInitialize)timeBar).EndInit();
			((System.ComponentModel.ISupportInitialize)latitudeBar).EndInit();
			((System.ComponentModel.ISupportInitialize)longitudeBar).EndInit();
			((System.ComponentModel.ISupportInitialize)smearBar).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Timer tick;
		private System.Windows.Forms.TextBox tiltBox;
		private System.Windows.Forms.Label tiltLabel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button chartButton;
		private System.Windows.Forms.Button globeButton;
		private System.Windows.Forms.Button skyButton;
		private System.Windows.Forms.TrackBar timeBar;
		private System.Windows.Forms.Label latitudeLabel;
		private System.Windows.Forms.TrackBar latitudeBar;
		private System.Windows.Forms.Button wrapX;
		private System.Windows.Forms.Label longitudeLabel;
		private System.Windows.Forms.TrackBar longitudeBar;
		private System.Windows.Forms.Label spinsLabel;
		private System.Windows.Forms.TextBox spinsBox;
		private System.Windows.Forms.Label twilightLabel;
		private System.Windows.Forms.TextBox twilightBox;
		private System.Windows.Forms.Label timeExport;
		private TextBox shiftBox;
		private Label label2;
		private TextBox eccentricityBox;
		private Label label3;
		private TextBox leapYearBox;
		private Label leapYearLabel;
		private TextBox leapDayBox;
		private Label label4;
		private CheckBox rewindBox;
		private TextBox maxGlobeResBox;
		private Label globeResLabel;
		private CheckBox magneticCompassBox;
		private CheckBox flipSkyNorth;
		private Label maxSkyResLabel;
		private TextBox maxSkyResBox;
		private Label maxSkySunsLabel;
		private TextBox maxSkySunsBox;
		private TextBox magLatitudeBox;
		private TextBox magLongitudeBox;
		private TextBox zRadiusBox;
		private TextBox xRadiusBox;
		private TextBox skyFovBox;
		private Label magLatitudeLabel;
		private Label magLongitudeLabel;
		private Label xRadiusLabel;
		private Label zRadiusLabel;
		private Label skyAngleLabel;
		private TextBox latitudeBox;
		private TextBox longitudeBox;
		private Label smearLabel;
		private TrackBar smearBar;
		private Label globeDetailLabel;
		private ComboBox globeDetailBox;
		private TextBox auBox;
		private Label label5;
		private Label dateLabel;
		private DateTimePicker timePicker;
		private DateTimePicker datePicker;
	}
}

