using System.Diagnostics;
using System.Drawing.Imaging;
using System.Numerics;

namespace GlobeSimulator {
	public partial class Simulator : Form {

		// A planet struct, for loading and verifying planets
		internal struct Planet {
			internal float Tilt; // how many degrees is the globe tilted relative to the solar north?
			internal int Spins; // how many times does a planet spin in one orbit (excluding leap days)
			internal int Equinox; // which day is the fall equinox?
			internal int LeapYears; // by how many years does a leap day repeat?
			internal int LeapDay; // which day is the lead day?
			internal float Eccentricity; // The eccentricity of the orbit (how much elliptical it is instead of circular)
			internal float SkyAngle; // How wide is the angle of the sky from horizon to horizon?
			internal float MagneticLatitude; // What latitude is the magnetic north on?
			internal float MagneticLongitude; // What longitude is the magnetic north on?
			internal float EquatorialRadius; // The globe's radius on Equator
			internal float PolarRadius; // The globe's radius at a pole
			internal float SunDistance; // Average Distance to the Sun

			public Planet() {
				// Earth parameters:
				Tilt = 23.44f;
				Spins = 366;
				Equinox = 264;
				LeapYears = 4;
				LeapDay = 60;
				Eccentricity = 0.0167f;
				SkyAngle = 181.21f;
				MagneticLatitude = 85.762f;
				MagneticLongitude = 139.298f;
				EquatorialRadius = 6378.137f;
				PolarRadius = 6356.7523f;
				SunDistance = 149597870.0f;
			}
		}

		#region Variables
		// Planet Settings:
		private float Tilt; // how many degrees is the globe tilted relative to the solar north?
		internal int Spins; // how many times does a planet spin in one orbit (excluding leap days)
		internal int Equinox; // which day is the fall equinox?
		internal int LeapYears; // by how many years does a leap day repeat?
		internal int LeapDay; // which day is the lead day?
		internal float Eccentricity; // The eccentricity of the orbit (how much elliptical it is instead of circular)
		internal float SkyAngle; // How wide is the angle of the sky from horizon to horizon?
		internal float MagneticLatitude; // What latitude is the magnetic north on?
		internal float MagneticLongitude; // What longitude is the magnetic north on?
		internal float EquatorialRadius; // The globe's radius on Equator
		internal float PolarRadius; // The globe's radius at a pole
		internal float SunDistance; // Average Distance to the Sun

		// Non-planet settings:
		internal double TimeSpeed = 0;
		internal bool Rewind;
		internal float Longitude;
		internal float Latitude;
		internal float TwilightBlur;
		private int maxGenerationTasks; // Allowed threads per one window
		private int MaxGlobeRes;
		private int MaxSkyRes;
		private int MaxSkySuns;

		// Variables:
		internal double Time;
		internal int Period;
		private readonly Stopwatch stopwatch;
		private bool lockDateTime;
		internal readonly object
			timeLock = new();   // Monitor lock for atomic change in time

		// Precomputed variables:
		internal double EquinoxShift; // When is Fall Equinox in my time format?
		internal double LeapDayShift; // When is the leap day in my time format?
		internal double LeapYearMultiplier; // How long does a leap year take in my time format?
		internal double NonLeapYearMultiplier; // ...and a non-leap year?
		internal double OneDay; // How long does one day take in my time format?
		internal double OneSecond; // How long does one second take in my time format?
		internal float TiltCos, TiltSin; // A tilt vector
		internal float PolarShrink; // Oblateness (shorter polar radius)
		internal float EquatorShrink; // Anti-Oblateness (shorter equator radius?)
		internal Vector3 ObserverLatitudeVector; // Untilted observer on selected Latitude and Longitude=0
		internal Vector3 MagneticLatitudeVector; // Untilted magnetic north pole on Longitude=0
		internal float[] Nu, R; // Eccentricity samples

		// Render Windows:
		internal Chart? chart;
		internal Globe? globe;
		internal Sky? sky;

		// Constants:
		static Planet Earth;
		internal readonly Vector3[,] MapEarth;
		internal const float TWO_PI = 2.0f * MathF.PI;
		internal static readonly int[] Calendar = [31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
		internal static readonly string[] CalendarMonthNames = [
			"January",
			"February",
			"March",
			"April",
			"May",
			"June",
			"July",
			"August",
			"September",
			"October",
			"November",
			"December"
		];
		#endregion

		#region Events
		public Simulator() {
			lockDateTime = true;
			Time = Period = 0;
			Earth = new();
			R = new float[1025];
			Nu = new float[1025];
			stopwatch = Stopwatch.StartNew();
			// Load the PNG of the Earth's Map
			using var map = new Bitmap("MapEarth.png");
			MapEarth = new Vector3[map.Width, map.Height];
			// lock the bitmap
			var locked = map!.LockBits(
				new Rectangle(0, 0, map.Width, map.Height),
				ImageLockMode.ReadOnly,
				PixelFormat.Format24bppRgb);
			unsafe {
				// read the pixels into RAM:
				byte* bmpPtr = (byte*)(void*)locked.Scan0;
				for (var y = 0; y < map.Height; ++y)
					for (var x = 0; x < map.Width; ++x) {
						MapEarth[x, y] = new(bmpPtr[2], bmpPtr[1], bmpPtr[0]);
						bmpPtr += 3;
					}
			}
			// unlock the bitmap
			map.UnlockBits(locked);
			// initialize the form
			InitializeComponent();
		}
		private void Simulator_Load(object sender, EventArgs e) {
			maxGenerationTasks = Environment.ProcessorCount - 1;
			globeDetailBox.SelectedIndex = 2;

			// Set the minimum datetime to the start of the simulation (January 1 2000)
			timePicker.MinDate = new DateTime(2000, 1, 1);

			// Read the default settings:
			SetLatitude();
			SetLongitude();
			_ = SetAu();
			_ = SetSkyAngle();
			_ = SetTilt();
			_ = SetSpins();
			_ = SetLeapYear();
			_ = SetLeapDay();
			_ = SetYearShift();
			_ = SetEccentricity();
			_ = SetTwilight();
			_ = SetGlobeRes();
			_ = SetSkyRes();
			_ = SetSkySuns();
			_ = SetXRadius();
			_ = SetZRadius();
			_ = SetMagneticLatitude();
			_ = SetMagneticLongitude();
			for (int i = 0; i < 1024; ++i)
				R[i] = Radius(Nu[i] = TrueAnomaly(SolveEccentricAnomaly(TWO_PI * i / 1024.0f)));
			Nu[1024] = 1.0f; R[1024] = R[0];

			// Use simulation start time instead of today time
			//timePicker.Value = new(2000, 1, 1, 0, 0, 0);
			//datePicker.Value = new(2000, 1, 1, 0, 0, 0);

			SetDateTime();
			RefreshAll();
			UpdateTime();
		}
		private void Tick_Tick(object sender, EventArgs e) {
			var elapsed = stopwatch.Elapsed.TotalSeconds;
			stopwatch.Restart();
			if (TimeSpeed <= 0)
				return;
			// Animates time
			lock (timeLock) {
				UpdatePeriod(LeapYears == 0 ? 1 : LeapYears, Rewind ? Math.Max(0.0, Time - TimeSpeed * elapsed) : Time + TimeSpeed * elapsed);
			}
			RefreshAll();
			UpdateTime();
		}
		#endregion

		#region Renders
		private void ChartButton_Click(object sender, EventArgs e)
			=> OpenRenderForm(ref chart, () => new() { MyParent = this, maxResolution = 0 }, Chart_FormClosed);
		private void GlobeButton_Click(object sender, EventArgs e)
			=> OpenRenderForm(ref globe, () => new() {
				MyParent = this,
				maxResolution = MaxGlobeRes,
				TextureSmearLevel = smearBar.Value,
				detail = (Globe.Detail)globeDetailBox.SelectedIndex
			}, Globe_FormClosed);
		private void SkyButton_Click(object sender, EventArgs e)
			=> OpenRenderForm(ref sky, () => new() {
				MyParent = this,
				maxResolution = MaxSkyRes,
				MaxSuns = MaxSkySuns,
				Flip = flipSkyNorth.Checked,
				Magnetic = magneticCompassBox.Checked
			}, Sky_FormClosed);
		private void Chart_FormClosed(object? sender, FormClosedEventArgs e)
			=> chart = null;
		private void Globe_FormClosed(object? sender, FormClosedEventArgs e)
			=> globe = null;
		private void Sky_FormClosed(object? sender, FormClosedEventArgs e)
			=> sky = null;
		private static void OpenRenderForm<T>(ref T? formField, Func<T?> creator, FormClosedEventHandler closedHandler)
			where T : RenderForm {
			formField ??= creator();
			if (formField == null)
				return;
			formField.FormClosed += closedHandler;
			formField.Show();
			formField.Reset();
		}
		#endregion

		#region SettingsSimulation
		private void DatePicker_ValueChanged(object sender, EventArgs e) {
			if (lockDateTime) {
				return;
			}
			SetDateTime();
			RefreshAll(true);
			UpdateTime();
		}
		private void TimePicker_ValueChanged(object sender, EventArgs e) {
			if (lockDateTime) {
				return;
			}
			SetDateTime();
			RefreshAll(true);
			UpdateTime();
		}
		private void SetDateTime() {
			var Year = datePicker.Value.Year;
			var Month = datePicker.Value.Month - 1;
			var Day = datePicker.Value.Day - 1;
			var DTime = timePicker.Value.TimeOfDay.TotalSeconds;
			Year -= 2000;
			if (Tilt == Earth.Tilt
				&& Spins == Earth.Spins
				&& Equinox == Earth.Equinox
				&& LeapYears == Earth.LeapYears
				&& LeapDay == Earth.LeapDay
			) {
				// Add the remaining Years after 2000, accounting for different sized because of Leap Yeears:
				var Leaps = Year / LeapYears;
				double NewTime = 0; // add quadruples of years 
				Year -= Leaps * LeapYears; // subtract those used quadruples of years from the picked date time (every LeapYears years is exactly 4 in my Time format)
							   // Take a calendar with a leap year
				int[] LeapCalendar = new int[12];
				Calendar.CopyTo(LeapCalendar, 0);
				// set february to 28 days if the year is not leap
				if (Year % 4 > 0)
					LeapCalendar[1] = 28;
				// add remaining single years to my time.
				// accounting for leap years being slightly larger than 1, and non leap year slightly smaller
				if (Year > 0) {
					// First remaining year is a leap year:
					NewTime += OneDay * Spins;
					--Year;
					// The following 1-3 years are not:
					while (Year > 0) {
						NewTime += OneDay * (Spins - 1);
						--Year;
					}
				}
				// Add Months and days and Time to my time:
				while (Month > 0) {
					NewTime += OneDay * LeapCalendar[--Month];
				}
				lock (timeLock) {
					Period = Leaps;
					UpdatePeriod(LeapYears == 0 ? 1 : LeapYears, NewTime + OneDay * Day + DTime * OneSecond);
				}
			} else {
				_ = MessageBox.Show("Settings do not match Earth!", "Cannot set date-time!", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		private void RewindBox_CheckedChanged(object sender, EventArgs e) 
			=> Rewind = rewindBox.Checked;
		private void TimeBar_Scroll(object sender, EventArgs e) {
			TimeSpeed = timeBar.Value == 0 ? 0.0 : OneSecond * Math.Exp((timeBar.Value - 1) * .05);
			UpdateTime();
		}
		private void LatitudeBox_TextChanged(object sender, EventArgs e) {
			if (float.TryParse(latitudeBox.Text, out float New)) {
				Latitude = New;
				if (Latitude < -90) {
					latitudeBox.Text = "-90";
					return;
				}
				if (Latitude > 90) {
					latitudeBox.Text = "90";
					return;
				}
				UpdateLatitudeVector();
				RefreshAll();
			}
		}
		private void LongitudeBox_TextChanged(object sender, EventArgs e) {
			if (float.TryParse(longitudeBox.Text, out float New)) {
				Longitude = New;
				if (Latitude < -180) {
					longitudeBox.Text = "-180";
					return;
				}
				if (Latitude > 180) {
					longitudeBox.Text = "180";
					return;
				}
				UpdateLatitudeVector();
				RedrawAll();
			}
		}
		private void LatitudeBar_Scroll(object sender, EventArgs e) {
			SetLatitude();
			latitudeBox.Text = Latitude.ToString();
			//RefreshAll(); // it gets done in TextChanged
		}
		private void SetLatitude() {
			Latitude = latitudeBar.Value - 90;
			UpdateLatitudeVector();
		}
		private void UpdateLatitudeVector() {
			float latitudeRad = (.5f - Latitude / 180.0f) * MathF.PI;
			ObserverLatitudeVector = new(MathF.Sin(latitudeRad) * EquatorShrink, 0, MathF.Cos(latitudeRad) * PolarShrink);
		}
		private void WrapX_Click(object sender, EventArgs e) {
			longitudeBar.Value = longitudeBar.Value == 360 ? 0 : 360;
			SetLongitude();
		}
		private void LongitudeBar_Scroll(object sender, EventArgs e) {
			SetLongitude();
			longitudeBox.Text = Longitude.ToString();
			//RedrawAll(); // it gets done in TextChanged
		}
		private void SetLongitude() {
			Longitude = longitudeBar.Value - 180;
			wrapX.Enabled = longitudeBar.Value % 360 == 0;
		}
		private void TiltBox_TextChanged(object? sender, EventArgs? e) {
			if (SetTilt()) {
				RedrawAll();
			}
		}
		private bool SetTilt() {
			if (float.TryParse(tiltBox.Text, out float New)) {
				Tilt = New;
				float tiltRad = Tilt * MathF.PI / 180;
				TiltCos = MathF.Cos(tiltRad);
				TiltSin = MathF.Sin(tiltRad);
				return true;
			}
			return false;
		}
		private void EccentricityBox_TextChanged(object sender, EventArgs e) {
			if (SetEccentricity()) {
				RedrawAll();
			}
		}
		private bool SetEccentricity() {
			if (float.TryParse(eccentricityBox.Text, out float New)) {
				Eccentricity = New;
				return true;
			}
			return false;
		}
		private void SpinsBox_TextChanged(object sender, EventArgs e) {
			if (SetSpins()) {
				UpdateLeapYear();
				UpdateLeapDay();
				RedrawAll();
			}
		}
		private bool SetSpins() {
			if (int.TryParse(spinsBox.Text, out int New)) {
				Spins = New;
				return true;
			}
			return false;
		}
		private void LeapYearBox_TextChanged(object sender, EventArgs e) {
			if (SetLeapYear()) {
				RedrawAll();
			}
		}
		private bool SetLeapYear() {
			if (int.TryParse(leapYearBox.Text, out int New)) {
				LeapYears = New;
				UpdateLeapYear();
				return true;
			}
			return false;
		}
		private void UpdateLeapYear() {
			// Only Allow Leap Years if the planet actaully spins faster than it orbits:
			if (Spins <= 1)
				LeapYears = 0;
			LeapYearMultiplier = LeapYears > 0 ? Spins / (Spins - 1.0 + 1.0 / LeapYears) : 1.0;
			NonLeapYearMultiplier = LeapYears > 0 ? (Spins - 1.0) / (Spins - 1.0 + 1.0 / LeapYears) : 1.0;
			OneDay = LeapYears > 0 ? 1.0 / (Spins - 1.0 + 1.0 / LeapYears) : 1.0 / (Spins - 1.0);
			OneSecond = OneDay / (24 * 60 * 60);
		}
		private void LeapDayBox_TextChanged(object sender, EventArgs e) {
			if (SetLeapDay()) {
				RedrawAll();
			}
		}
		private bool SetLeapDay() {
			if (int.TryParse(leapDayBox.Text, out int New)) {
				LeapDay = New;
				UpdateLeapDay();
				return true;
			}
			return false;
		}
		private void UpdateLeapDay() {
			LeapDayShift = Spins != 1 ? (LeapDay - 1) * OneDay : 0.0f;
		}
		private void ShiftBox_TextChanged(object sender, EventArgs e) {
			if (SetYearShift()) {
				RedrawAll();
			}
		}
		private bool SetYearShift() {
			if (int.TryParse(shiftBox.Text, out int New)) {
				Equinox = New;
				EquinoxShift = Spins != 1 ? (float)Equinox * OneDay /* / (Spins - 1)*/ : 0.0f;
				return true;
			}
			return false;
		}
		private void TwilightBox_TextChanged(object sender, EventArgs e) {
			if (SetTwilight()) {
				RefreshAll();
			}
		}
		private bool SetTwilight() {
			if (float.TryParse(twilightBox.Text, out float New)) {
				TwilightBlur = New;
				return true;
			}
			return false;
		}
		private void MagLatitudeBox_TextChanged(object sender, EventArgs e) {
			if (SetMagneticLatitude()) {
				RefreshExceptChart();
			}
		}

		private void MagLongitudeBox_TextChanged(object sender, EventArgs e) {
			if (SetMagneticLongitude()) {
				RefreshExceptChart();
			}
		}
		private bool SetMagneticLatitude() {
			if (float.TryParse(magLatitudeBox.Text, out float New)) {
				MagneticLatitude = New;
				UpdateMagneticLatitude();
				return true;
			}
			return false;
		}
		private bool SetMagneticLongitude() {
			if (float.TryParse(magLongitudeBox.Text, out float New)) {
				MagneticLongitude = New;
				return true;
			}
			return false;
		}
		private void UpdateMagneticLatitude() {
			float latitudeRad = (.5f - MagneticLatitude / 180.0f) * MathF.PI;
			MagneticLatitudeVector = new(MathF.Sin(latitudeRad) * EquatorShrink, 0, MathF.Cos(latitudeRad) * PolarShrink);
		}
		private void XRadiusBox_TextChanged(object sender, EventArgs e) {
			if (SetXRadius()) {
				RefreshExceptChart();
			}
		}
		private bool SetXRadius() {
			if (float.TryParse(xRadiusBox.Text, out float New)) {
				EquatorialRadius = New;
				UpdateOblateness();
				UpdateLatitudeVector();
				UpdateMagneticLatitude();
				return true;
			}
			return false;
		}
		private void ZRadiusBox_TextChanged(object sender, EventArgs e) {
			if (SetZRadius()) {
				RefreshExceptChart();
			}
		}
		private bool SetZRadius() {
			if (float.TryParse(zRadiusBox.Text, out float New)) {
				PolarRadius = New;
				UpdateOblateness();
				UpdateLatitudeVector();
				UpdateMagneticLatitude();
				return true;
			}
			return false;
		}
		private void UpdateOblateness() {
			PolarShrink = MathF.Min(1.0f, PolarRadius / EquatorialRadius);
			EquatorShrink = MathF.Min(1.0f, EquatorialRadius / PolarRadius);
		}
		private void SkyFovBox_TextChanged(object sender, EventArgs e) {
			if (SetSkyAngle() && sky != null) {
				sky.CancelRenderSleep(RenderForm.RefreshType.RefreshSettings);
			}
		}
		private bool SetSkyAngle() {
			if (float.TryParse(skyFovBox.Text, out float New)) {
				SkyAngle = New;
				return true;
			}
			return false;
		}
		private void AuBox_TextChanged(object sender, EventArgs e) {
			if (SetAu() && sky != null) {
				sky.CancelRenderSleep(RenderForm.RefreshType.RefreshSettings);
			}
		}
		private bool SetAu() {
			if (float.TryParse(auBox.Text, out float New)) {
				SunDistance = New;
				return true;
			}
			return false;
		}
		#endregion

		#region SettingsRenders
		private void MaxGlobeResBox_TextChanged(object sender, EventArgs e) {
			if (SetGlobeRes() && globe != null) {
				globe.maxResolution = MaxGlobeRes;
				globe.Reset();
			}
		}
		private void GlobeDetailBox_SelectedIndexChanged(object sender, EventArgs e) {
			if (globe != null) {
				globe.detail = (Globe.Detail)globeDetailBox.SelectedIndex;
				globe.DrawBackground();
			}
		}
		private bool SetGlobeRes() {
			if (int.TryParse(maxGlobeResBox.Text, out int New)) {
				MaxGlobeRes = New;
				return true;
			}
			return false;
		}
		private void SmearBar_Scroll(object sender, EventArgs e) {
			if (globe != null) {
				globe.TextureSmearLevel = smearBar.Value;
			}
		}
		private void MagneticCompassBox_CheckedChanged(object sender, EventArgs e) {
			if (sky != null) {
				sky.Magnetic = magneticCompassBox.Checked;
				sky.CancelRenderSleep(RenderForm.RefreshType.RefreshSettings);
			}
		}
		private void FlipSkyNorth_CheckedChanged(object sender, EventArgs e) {
			if (sky != null) {
				sky.Flip = flipSkyNorth.Checked;
				sky.LabelNorth();
				sky.CancelRenderSleep(RenderForm.RefreshType.RefreshSettings);
			}
		}
		private void MaxSkyResBox_TextChanged(object sender, EventArgs e) {
			if (SetSkyRes() && sky != null) {
				sky.maxResolution = MaxGlobeRes;
				sky.Reset();
			}
		}
		private bool SetSkyRes() {
			if (int.TryParse(maxSkyResBox.Text, out int New)) {
				MaxSkyRes = New;
				return true;
			}
			return false;
		}
		private void MaxSkySunsBox_TextChanged(object sender, EventArgs e) {
			if (SetSkySuns() && sky != null) {
				sky.MaxSuns = Math.Max(1, MaxSkySuns);
				sky.Reset();
			}
		}
		private bool SetSkySuns() {
			if (int.TryParse(maxSkySunsBox.Text, out int New)) {
				MaxSkySuns = New;
				return true;
			}
			return false;
		}
		#endregion

		#region Functions
		private void UpdatePeriod(int Years, double NewTime) {
			Time = NewTime;
			while (Time >= Years) {
				Time -= Years;
				Period += Years;
			}
			while (Time < 0) {
				Time += Years;
				Period -= Years;
			}
		}
		private void UpdateTime() {
			// Earth's Date and Time only makes sense if these parameters match Earth's:
			if (Tilt == Earth.Tilt
				&& Spins == Earth.Spins
				&& Equinox == Earth.Equinox
				&& LeapYears == Earth.LeapYears
				&& LeapDay == Earth.LeapDay
			) {
				int Year = 2000 + (LeapYears == 0 ? 1 : LeapYears) * Period;
				var DayTime = Skipleaps(Time);
				int Add = (int)(DayTime / LeapYearMultiplier);
				Year += Add;
				DayTime -= Add * LeapYearMultiplier;
				// Count the days and months:
				int Month = 0, Day = (int)(DayTime / OneDay);
				foreach (int i in Calendar) {
					if (Day >= i) {
						++Month;
						Day -= i;
					} else break;
				}
				// Convert to total minutes and seconds, format as HH:mm:ss
				string DayExp, TimeExp = "██:██:██";
				var DT = TimeSpeed * tick.Interval / (1000 * OneDay);
				++Day; // count days from 1
				DayTime %= OneDay;
				DayTime /= OneDay;
				DayTime *= 24 * 60;
				int Minutes = (int)DayTime;
				int Hours = Minutes / 60;
				Minutes %= 60;
				int Seconds = (int)(DayTime * 60 % 60);

				if (DT >= 10.0f) {
					DayExp = "██";
				} else {
					if (DT >= 5.0f) {
						DayExp = (Day / 10) + "█";
					} else if (DT >= 10.0f / 24) {
						DayExp = Day.ToString();
					} else {
						DayExp = Day.ToString();
						if (DT >= 5.0f / 24) {
							TimeExp = (Hours / 10) + "█:██:██";
						} else {
							TimeExp = $"{Hours:D2}:";
							if (DT >= 20.0f / (24 * 60)) {
								TimeExp += "██:██";
							} else if (DT >= 5.0f / (24 * 60)) {
								TimeExp += (Minutes / 10) + "█:██";
							} else {
								TimeExp += $"{Minutes:D2}:";
								if (DT >= 20.0f / (24 * 60 * 60)) {
									TimeExp += "██";
								} else if (DT >= 5.0f / (24 * 60 * 60)) {
									TimeExp += (Seconds / 10) + "█";
								} else {
									TimeExp += $"{Seconds:D2}";
								}
							}
						}
					}
				}
				timeExport.Text = TimeExp + "\n" + CalendarMonthNames[Month] + " " + DayExp + "\n" + Year + "\nTime Speed = " + string.Format("{0:F1}", Math.Round(TimeSpeed * 10 / OneSecond) / 10) + "x";

				lockDateTime = true;
				timePicker.Value = new(2000, 1, 1, Hours, Minutes, Seconds);
				datePicker.Value = new(Year, Month + 1, Day, 0, 0, 0);
				datePicker.Enabled = timePicker.Enabled = true;
				lockDateTime = false;
				return;
			}
			// Fallback to a non-calendar planet date time export:
			lockDateTime = datePicker.Enabled = timePicker.Enabled = false;
			timeExport.Text =
				"Settings do not match Earth! Year: " + (int)(Time % 1 * 100)
				+ "%, Day: " + (int)(Time % OneDay * 100) + "%";
		}
		internal double Skipleaps(double Input) {
			// Add will restore the remove whole years at the end:
			int Add = (int)Input / LeapYears;
			Add *= LeapYears;
			// Remove the multiples of Leap Year Periods (4-years for Earth)
			Input -= Add;
			// Remove every next single full years, while skipping the Leap Days on the Leap Years:
			if (Input >= LeapYearMultiplier) {
				Input -= LeapYearMultiplier;
				++Add;
				while (Input >= NonLeapYearMultiplier) {
					Input -= NonLeapYearMultiplier;
					++Add;
				}
				if (Input >= LeapDayShift) {
					Input += OneDay;
				}
			}
			// Restore the removed years:
			return Input + Add * LeapYearMultiplier;
		}
		internal int GetMaxTasks() {
			int n = 0;
			if (chart != null) ++n;
			if (globe != null) ++n;
			return Math.Max(1, maxGenerationTasks / (Math.Max(1, n)));
		}
		private void RedrawAll() {
			chart?.DrawBackground();
			RefreshExceptChart();
		}
		private void RefreshAll(bool noSmear = false) {
			var NewRefreshFlag = noSmear ? RenderForm.RefreshType.RefreshReset : RenderForm.RefreshType.RefreshSettings;
			RefreshExceptChart(NewRefreshFlag);
			chart?.CancelRenderSleep(NewRefreshFlag);
		}
		private void RefreshExceptChart(RenderForm.RefreshType NewRefreshFlag = RenderForm.RefreshType.RefreshSettings) {
			globe?.CancelRenderSleep(NewRefreshFlag);
			sky?.CancelRenderSleep(NewRefreshFlag);
		}
		#endregion

		#region Eccentricity
		// Solve Kepler's equation: M = E - e*sin(E)
		// M = mean anomaly in radians, e = eccentricity
		private float SolveEccentricAnomaly(float M) {
			float E = M;  // initial guess
			const float tol = 1e-10f;
			const int maxIter = 100;

			for (int i = 0; i < maxIter; i++) {
				float f = E - Eccentricity * MathF.Sin(E) - M;
				float fPrime = 1 - Eccentricity * MathF.Cos(E);
				float delta = -f / fPrime;
				E += delta;
				if (MathF.Abs(delta) < tol)
					break;
			}
			return E;
		}
		// Compute true anomaly nu from eccentric anomaly E
		private float TrueAnomaly(float E) {
			float sqrtFactor = MathF.Sqrt((1 + Eccentricity) / (1 - Eccentricity));
			float nu = 2 * MathF.Atan2(sqrtFactor * MathF.Sin(E / 2), MathF.Cos(E / 2));
			if (nu < 0)
				nu += TWO_PI;
			return nu / TWO_PI;
		}
		// Compute distance r from true anomaly
		private float Radius(float nu)
			=> (1 - Eccentricity * Eccentricity) / (1 + Eccentricity * MathF.Cos(nu));
		private (int, double) GetEccentricLerp(double yearAlpha) {
			double yearAlphaS = yearAlpha * 1024;
			int index = (int)yearAlphaS;
			return (index % 1024, yearAlphaS - index);
		}
		internal double GetNu(double yearAlpha) {
			(int index, double a) = GetEccentricLerp(yearAlpha);
			return Nu[index] * (1 - a) + Nu[index + 1] * a;
		}
		internal double GetR(double yearAlpha) {
			(int index, double a) = GetEccentricLerp(yearAlpha);
			return R[index] * (1 - a) + R[index + 1] * a;
		}
		#endregion
	}
}
