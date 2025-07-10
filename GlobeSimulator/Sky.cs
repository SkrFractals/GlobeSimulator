using System.Drawing.Imaging;
using System.Numerics;

namespace GlobeSimulator {
	public partial class Sky : RenderForm {

		public required bool Flip, Magnetic;
		public required int MaxSuns;

		private float radius;
		private int w2, h2;

		public Sky() {
			Square = true;
			screenX = 64; screenY = 32; screenW = 64; screenH = 32;
			InitializeComponent();
		}
		protected override void Function_Reset() {
			w2 = bmpW / 2;
			h2 = bmpH / 2;
			radius = MathF.Min(bmpW, bmpH) * .4f - 4f;
			leftLabel.Location = new Point(12, screenY + h2 - leftLabel.Height / 2);
			rightLabel.Location = new Point(12 + bmpW+screenX, screenY + h2 - rightLabel.Height / 2);
			upLabel.Location = new Point(screenX + w2 - upLabel.Width / 2, 12);
			downLabel.Location = new Point(screenX + w2 - downLabel.Width / 2, 12 + bmpH + screenY);
			LabelNorth();
		}
		internal void LabelNorth() {
			if (Flip) {
				upLabel.Text = "North";
				downLabel.Text = "South";
			} else {
				downLabel.Text = "North";
				upLabel.Text = "South";
			}
		}
		protected override void Function_DrawBackground(ParallelOptions po) {
			_ = Parallel.For(0, bmpW, po, (x, state) => {
				float dx = x - w2, dx2 = dx * dx;
				for (int y = 0; y < bmpH; ++y) {
					if (token.IsCancellationRequested) {
						state.Stop();
						continue;
					}
					// Precompute the ground pixels:
					float dy = y - h2, d = MathF.Sqrt(dx2 + dy * dy), a = MathF.Min(1, MathF.Max(0, radius - d));
					pixels![y][x] = (byte)(a * 255);
				}
			});
		}
		protected unsafe override void Function_DrawBitmap(ParallelOptions po, BitmapData locked, byte* bmpPtr) {
			// Prepare Sun Samples Count
			var max = timeB - Math.Min(2.0 / MyParent.Spins, timeB - timeA);
			var dt = Math.Max(0.00001, MyParent.TimeSpeed * .00002f);
			int I = Math.Max(1, (int)((timeB - max) / dt));
			if (I > MaxSuns) {
				I = MaxSuns;
				dt = (timeB - max) / I;
			}
			// Prepare the squared radius vector:
			var RadiusS = new Vector3(MyParent.EquatorShrink, MyParent.EquatorShrink, MyParent.PolarShrink);
			RadiusS *= RadiusS;
			// Sun Projecton
			float minX = 1;
			float maxX = -1;
			(float, float)[] sun = new (float, float)[I + 1];
			void AddSun(int i, double t) {
				var yearRad = GetYearRad(t);
				// Observer's location relative to center:
				var Observer = GetObserver(t, yearRad, MyParent.ObserverLatitudeVector * MathF.Max(MyParent.EquatorialRadius, MyParent.PolarRadius), MyParent.Longitude);
				// Observer's up is the normal on the surface:
				var Up_Local = GetObserver(t, yearRad, Vector3.Normalize(MyParent.ObserverLatitudeVector / RadiusS), MyParent.Longitude);
				// Remember the maximum and minimum brightness for averaging the daylight
				minX = MathF.Min(minX, Up_Local.X);
				maxX = MathF.Max(maxX, Up_Local.X);
				// The Earth's North direction (should be equal to North_Local when on Equator)
				(var North_Local, var East_Local) = GetNorthEast(Magnetic, t, yearRad, Up_Local);
				// Sun is always exactly to the -X in this coordinate system, Y and Z are for the paralax based on the location of the observer
				var SunDistance = MyParent.SunDistance * MyParent.GetR(yearRad);
				Vector3 sunDir = Vector3.Normalize(new(-1, -Observer.Y / (float)SunDistance, -Observer.Z / (float)SunDistance));
				float dot = Vector3.Dot(sunDir, Up_Local);
				// Skip if the sun is perfectly on the opposite side, that would project it to infinity
				if (dot > -.99) {
					// Stereographically project from –Z
					var scale = (180.0f / MyParent.SkyAngle) * radius / (1.0f + Vector3.Dot(sunDir, Up_Local));
					float x_proj = Vector3.Dot(sunDir, East_Local) * scale;
					float y_proj = Vector3.Dot(sunDir, North_Local) * scale;
					// Add the projected Sun sample to the list, flip north if the checkbox is checked in the settings
					sun[i] = (w2 + x_proj, Flip ? h2 - y_proj : h2 + y_proj);
				}
			}
			// Project the Sun Samples
			if (I == 0) {
				// Only One
				AddSun(0, timeB);
			} else {
				// Multiple samples making a smear, when speedy time:
				_ = Parallel.For(0, I + 1, po, (it, state) => {
					if (token.IsCancellationRequested) {
						state.Stop();
					} else {
						var t = timeB - it * dt;
						if (t >= 0) {
							AddSun(it, t);
						}
					}
				});
			}
			if (token.IsCancellationRequested) {
				return;
			}
			// Calculate the daylight brightness as an average of sun samples:
			float day = MathF.Max(0.0f, Math.Min(1.0f, 1.0f + 10 * MathF.Asin(-(minX + maxX) / 2) / MyParent.TwilightBlur));
			// Sky colors:
			float skyG = day * 128, skyB = day * 255;
			// Ground colors:
			float groundR = (day*.75f+.25f)*255, groundG = (day * .75f + .25f) * 128;
			// Precalculate dx
			float[][] dx = new float[bmpW][];
			try {
				// Precalculate horizontal distance to the suns for every screen pixel collumn, for performance
				_ = Parallel.For(0, bmpW, po, (x, state) => {
					var dxi = dx[x] = new float[sun.Length];
					for (int i = sun.Length; 0 <= --i; dxi[i] = S(sun[i].Item1 - x))
						if (token.IsCancellationRequested) {
							state.Stop();
							break;
						}
				});
				_ = Parallel.For(0, bmpH, po, (y, state) => {

					// Precalculate horizontal distance to the suns for this screen pixel row, for performance
					float[] dy = new float[sun.Length];
					for (int i = sun.Length; 0 <= --i; dy[i] = S(sun[i].Item2 - y)) ;
					// Draw this row:
					var a = pixels![y];
					byte* row = bmpPtr + y * locked.Stride;
					for (int x = 0; x < bmpW; ++x) {
						var dxi = dx[x];
						if (token.IsCancellationRequested) {
							state.Stop();
							break;
						}
						// calculate distance to the nearest sun
						float distance = 1000000000.0f;
						for (int i = sun.Length; 0 <= --i; distance = Math.Min(distance, dxi[i] + dy[i])) ;
						distance = MathF.Sqrt(distance);
						// Calculate the Sun radius based on the screen size and Sun distance
						float sunradius = MathF.Max(2, radius * (32.0f / (60 * MyParent.SkyAngle)));
						// Caclulate the Sun intensity at this pixel (inside Sun, or bloom):
						float alphaR = Math.Min(1, Math.Max(0, Math.Max(sunradius - distance, .5f - (distance - sunradius) / (2 * radius))));
						// Red bloom on Twilight:
						float alphaNR = day * alphaR;
						// Place the pixel:
						float s = a[x] / 255.0f, g = 1 - s;
						row[2] = (byte)(s * alphaR * 255 + g * groundR);
						row[1] = (byte)(s * (skyG * (1 - alphaNR) + alphaNR * 255) + g * groundG);
						row[0] = (byte)(s * (skyB * (1 - alphaNR) + alphaNR * 255));
						row += 3;
					}
				});
			} catch (Exception) { return; }
			if (token.IsCancellationRequested) {
				return;
			}
		}
		
	}
}
