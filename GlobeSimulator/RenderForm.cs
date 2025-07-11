using System.Drawing.Imaging;
using System.Numerics;

namespace GlobeSimulator {
	public partial class RenderForm : Form {
		public enum RefreshType : byte {
			DontRefresh = 0,
			RefreshTime = 1,
			RefreshSettings = 2,
			RefreshReset = 3
		}
		public required Simulator 
			MyParent;			// pointer to the parent Simulation window
		public required int
			maxResolution;		// resolution cap, if less than 20 then unlimited
		private Bitmap? 
			bmpFront, bmpBack;	// double buffered bitmaps, back is being drawn to, front is being displayed
		protected byte[][]? 
			pixels;				// precomputed bitmap data (that donesn't change in time)
		protected double 
			prevTime,			// latest drawn time 
			currentTime,		// latest fetched time
			timeA,				// min(current,prev)
			timeB;				// max(current,prev)
		protected int 
			screenX = 12,		// left padding
			screenY = 12,		// top padding
			screenW = 12,		// right padding
			screenH = 12,		// bottom padding
			bmpW, bmpH;
		protected bool
			Square = false;		// are dimension forced to be square?

		private Task? task;
		private readonly object	
			taskLock = new();   // Monitor lock
		private CancellationTokenSource?
			cancel;             // Cancellation Token Source
		protected CancellationToken
			token;              // Cancellation token
		private CancellationTokenSource?
			cancelRefresh;             // Cancellation Token Source
		private CancellationToken
			tokenRefresh;              // Cancellation token
		internal RefreshType RefreshFlag;

		public RenderForm()
			=> InitializeComponent();
		private void RenderForm_Load(object sender, EventArgs e) { }
		private void RenderForm_SizeChanged(object sender, EventArgs e)
			=> Reset();
		private void Screen_Paint(object sender, PaintEventArgs e) {
			lock (taskLock) {
				if (screen.Image == null)
					return;
				var g = e.Graphics;
				// Set nearest neighbor interpolation
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
				// Draw the image scaled to the PictureBox size
				g.DrawImage(
				screen.Image,
				new Rectangle(0, 0, screen.Width, screen.Height),
				new Rectangle(0, 0, screen.Image.Width, screen.Image.Height),
					GraphicsUnit.Pixel
				);
			}
		}
		internal void CancelRenderSleep(RefreshType NewRefreshFlag) {
			RefreshFlag = NewRefreshFlag;
			cancelRefresh?.Cancel();
		}
		internal void Reset() {
			if (MyParent == null)
				return;
			// cancel the image binding:
			lock (taskLock) {
				screen.Image = null;
			}
			// stop the previous drawing thread:
			cancel?.Cancel();
			cancelRefresh?.Cancel();
			task?.Wait();
			// handle resolution constraints:
			int w = bmpW = Math.Max(1, ClientSize.Width - screenW - screenX),
				h = bmpH = Math.Max(1, ClientSize.Height - screenH - screenY);
			if (Square) {
				int min = w = h = Math.Min(w, h);
				if (maxResolution > 20 && min > maxResolution)
					min = maxResolution;
				bmpW = bmpH = min;
			} else {
				if (maxResolution > 20) {
					if (w > maxResolution) {
						float s = (float)maxResolution / w;
						bmpW = maxResolution;
						bmpH = (int)(s * h);
					}
					if (h > maxResolution) {
						float s = (float)maxResolution / h;
						bmpH = maxResolution;
						bmpW = (int)(s * w);
					}
				}
			}
			// resize the screen pictureBox
			screen.SetBounds(screenX, screenY, w, h);
			// virtual reset call:
			Function_Reset();
			// start the new drawing thread:
			task = Task.Run(Task_Reset, token = (cancel = new()).Token);
		}
		internal void DrawBackground() {
			lock (taskLock) {
				screen.Image = null;
			}
			cancel?.Cancel();
			cancelRefresh?.Cancel();
			task?.Wait();
			task = Task.Run(Task_DrawBackground, token = (cancel = new()).Token);
		}
		internal void Task_Reset() {
			// resize the double buffered bitmaps
			lock (taskLock) {
				if (bmpFront == null || bmpW != bmpFront.Width || bmpH != bmpFront.Height) {
					bmpFront?.Dispose();
					bmpFront = new(bmpW, bmpH);
				}
				if (bmpBack == null || bmpW != bmpBack.Width || bmpH != bmpBack.Height) {
					bmpBack?.Dispose();
					bmpBack = new(bmpW, bmpH);
				}
			}
			// resize the pixels array
			if (pixels == null || pixels.Length != bmpH) {
				pixels = new byte[bmpH][];
				for (int i = 0; i < bmpH; ++i)
					pixels[i] = new byte[bmpW];
			} else if (pixels[0].Length != bmpW) {
				for (int i = 0; i < bmpH; ++i)
					pixels[i] = new byte[bmpW];
			}
			Task_DrawBackground();
		}
		// (Re)Draws Chart, call on Load or parameter change
		internal async void Task_DrawBackground() {
			var po = new ParallelOptions {
				MaxDegreeOfParallelism = MyParent.GetMaxTasks(),
				CancellationToken = token
			};
			try {
				Function_DrawBackground(po);
			} catch {
				return;
			}
			RefreshFlag = RefreshType.RefreshReset;
			while (true) {
				tokenRefresh = (cancelRefresh = new()).Token;
				if (token.IsCancellationRequested) {
					return;
				}
				// sample the time, update the map if the time is different and frame is requested, or it a refresh is requested
				currentTime = MyParent.Time;
				if (RefreshFlag >= RefreshType.RefreshSettings || currentTime != prevTime && RefreshFlag > RefreshType.DontRefresh) {
					if(RefreshFlag == RefreshType.RefreshReset)
						prevTime = currentTime;
					RefreshFlag = RefreshType.DontRefresh; // just refreshed, so reset the flag to not refresh again until 
					Task_DrawBitmap();
					prevTime = currentTime;
				}
				try {
					
					await Task.Delay(TimeSpan.FromSeconds(1), tokenRefresh);
				} catch (TaskCanceledException) { }
			}
		}
		// Draws Bitmap, call on Load or Time increment
		internal void Task_DrawBitmap() {
			// lock the bitmap:
			var locked = bmpBack!.LockBits(
				new Rectangle(0, 0, bmpBack.Width, bmpBack.Height),
				ImageLockMode.WriteOnly,
				PixelFormat.Format24bppRgb);
			// prepare parallel options for threading and cancelling
			var po = new ParallelOptions {
				MaxDegreeOfParallelism = MyParent.GetMaxTasks(),
				CancellationToken = token
			};
			unsafe {
				byte* bmpPtr = (byte*)(void*)locked.Scan0;
				try {
					// sort the time interval to handle time running in both directions:
					timeA = Math.Min(prevTime, currentTime);
					timeB = Math.Max(prevTime, currentTime);
					// draw the bitmap:
					Function_DrawBitmap(po, locked, bmpPtr);

				} catch { }

			}
			// unlock the bitmap:
			bmpBack.UnlockBits(locked);
			// Swap front/back safely (double buffering)
			lock (taskLock) {
				(bmpBack, bmpFront) = (bmpFront, bmpBack);
				screen.Image = bmpFront;
			}
			/*var image = bmpFront;
			if (screen.InvokeRequired) {
				screen.Invoke(() => screen.Image = image);
			} else {
				screen.Image = image;
			}*/
		}
		protected virtual void Function_Reset() { }
		protected virtual void Function_DrawBackground(ParallelOptions po) { }
		protected unsafe virtual void Function_DrawBitmap(ParallelOptions po, BitmapData locked, byte* bmpPtr) { }
		protected Vector3 GetObserver(double t, float yearRadCos, float yearRadSin, Vector3 OnLatitude, float Longitude) {
			// How many times does the globe spin per orbit?
			var s = MyParent.LeapYears > 0 ? MyParent.Spins + 1.0 / MyParent.LeapYears : MyParent.Spins;
			// Longitude + Time Of Day:
			var dayRad = (t * s + MyParent.EquinoxShift + Longitude / 360.0) * Simulator.TWO_PI;
			// Lattitude + Tilt + Rotate by dayRad
			float y_time = OnLatitude.X * (float)Math.Sin(dayRad);
			float x_tilt = OnLatitude.X * (float)Math.Cos(dayRad);
			float y_tilt = y_time * MyParent.TiltCos - OnLatitude.Z * MyParent.TiltSin;
			// The observer's location on the Earth, so the observer's Z (up) would be the same direction:
			return new(
				y_tilt * yearRadSin + x_tilt * yearRadCos,
				y_tilt * yearRadCos - x_tilt * yearRadSin,
				y_time * MyParent.TiltSin + OnLatitude.Z * MyParent.TiltCos
			);
		}
		protected Vector3 GetObserver(double t, double yearRad, Vector3 OnLatitude, float Longitude) 
			=> GetObserver(t, (float)Math.Cos(yearRad), (float)Math.Sin(yearRad), OnLatitude, Longitude);
		protected static Vector3 GetObserverOnLattitude(float Latitude) {
			float latitudeRad = (.5f - Latitude / 180.0f) * MathF.PI;
			return new(MathF.Sin(latitudeRad), 0, MathF.Cos(latitudeRad));
		}
		protected double GetYearRad(double t) => (MyParent.GetNu(t) + MyParent.EquinoxShift) * Simulator.TWO_PI; 
		protected Vector3 GetNorth(double yearRad) 
			=> GetNorth((float)Math.Cos(yearRad), (float)Math.Sin(yearRad));
		protected Vector3 GetNorth(float cosZ, float sinZ)
			=> new(-sinZ * MyParent.TiltSin, -cosZ * MyParent.TiltSin, MyParent.TiltCos);
		protected (Vector3, Vector3) GetNorthEast(bool Magnetic, double t, double yearRad, Vector3 Up_Local) {
			// Observer's East direction should be perpendicular to their's Up and the Earth's North Axis
			Vector3[] Norths = Magnetic
				? [GetObserver(t, yearRad, MyParent.MagneticLatitudeVector, MyParent.MagneticLongitude), GetNorth(yearRad), Vector3.UnitZ, new Vector3(0, 0.1f, 1)]
				: [GetNorth(yearRad), GetObserver(t, yearRad, MyParent.MagneticLatitudeVector, MyParent.MagneticLongitude),  Vector3.UnitZ, new Vector3(0, 0.1f, 1)];
			var East_Local = Vector3.Zero;
			foreach (var North in Norths) {
				if ((East_Local = Vector3.Cross(North, Up_Local)) != Vector3.Zero) {
					East_Local /= MathF.Sqrt(S(East_Local.X) + S(East_Local.Y) + S(East_Local.Z)); // normalize
					break;
				}
			}
			// Observer's North direction should be perpendicular to their's Up and East
			var North_Local = Vector3.Cross(Up_Local, East_Local);
			return (
				North_Local / MathF.Sqrt(S(North_Local.X) + S(North_Local.Y) + S(North_Local.Z)),
				East_Local
			);
		}
		//square
		protected static float S(float x) => x * x;
	}
}
