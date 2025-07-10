using System.Drawing.Imaging;
using System.Numerics;

namespace GlobeSimulator {
	public partial class Chart : RenderForm {

		public Chart() {
			InitializeComponent();
		}
		protected override void Function_DrawBackground(ParallelOptions po) {
			// Prepare the squared radius vector:
			var RadiusS = new Vector3(MyParent.EquatorShrink, MyParent.EquatorShrink, MyParent.PolarShrink);
			RadiusS *= RadiusS;
			_ = Parallel.For(0, bmpH, po, (iLattitude, state) => {
				var a = pixels![iLattitude];
				var OnLatitude = GetObserverOnLattitude(90.0f - iLattitude * 180.0f / bmpH);
				for (int iTime = 0; iTime < bmpW; ++iTime) {
					if (token.IsCancellationRequested) {
						state.Stop();
						continue;
					}
					// Normalize Time Axis, Add a Leap Day
					var t = iTime * MyParent.LeapYearMultiplier / bmpW;
					// Don't need Y or Z, but computing them anyway, it's not realtime:
					a[iTime] = (byte)MathF.Max(0, MathF.Min(255, 
						(int)(255 * MathF.Max(0.0f, Math.Min(1.0f, 10 * MathF.Asin(
							-GetObserver(t, GetYearRad(t), Vector3.Normalize(OnLatitude / RadiusS), MyParent.Longitude).X
						) / MyParent.TwilightBlur)))
					));
				}
			});
		}
		protected unsafe override void Function_DrawBitmap(ParallelOptions po, BitmapData locked, byte* bmpPtr) {

			// Draw the chart bitmap
			_ = Parallel.For(0, bmpH, po, (iLattitude, state) => {
				var a = pixels![iLattitude];
				byte* row = bmpPtr + iLattitude * locked.Stride;
				//byte* row = bmpPtr + y * bmp.Height * 3;
				for (int iTime = 0; iTime < bmpW; ++iTime) {
					if (token.IsCancellationRequested) {
						state.Stop();
						continue;
					}
					// Color red if it's a leap day in a non-leap year, or blue if it's in a leap year
					if (MyParent.LeapYears > 0) {
						double t = iTime * MyParent.LeapYearMultiplier / bmpW;
						bool wasLeap = t >= MyParent.LeapDayShift;
						bool leapDay = wasLeap && t < MyParent.LeapDayShift + MyParent.OneDay;
						// 210 = RGB
						row[2] = MyParent.Time % MyParent.LeapYears < MyParent.LeapYearMultiplier && leapDay ? (byte)0 : a[iTime];
						row[1] = leapDay ? (byte)0 : a[iTime];
						row[0] = MyParent.Time % MyParent.LeapYears >= MyParent.LeapYearMultiplier && leapDay ? (byte)0 : a[iTime];
					} else row[0] = row[1] = row[2] = a[iTime];
					row += 3;
				}
			});
			// Draw the Observer Dot
			byte* dot = bmpPtr;
			// Calculate the location of the observer on the chart:
			int yc = (int)((90 - MyParent.Latitude) * bmpH / 180);
			int ys = Math.Max(yc - 4, 0);
			int ye = Math.Min(yc + 4, bmpH - 1);
			// Skip leap days on Non-Leap years:
			double currentTimeLeaped = timeB, prevTimeLeaped = timeA;
			if (MyParent.LeapYears > 0) {
				currentTimeLeaped = MyParent.Skipleaps(currentTimeLeaped);
				prevTimeLeaped = MyParent.Skipleaps(prevTimeLeaped);
			}
			// Normalize the time of year to a Lear Year length:
			currentTimeLeaped /= MyParent.LeapYearMultiplier;
			prevTimeLeaped /= MyParent.LeapYearMultiplier;
			// Stretch the observer towards previous time sample, if the time is moving fast:
			int dT = (int)((currentTimeLeaped - prevTimeLeaped) * bmpW);
			// Draw the observer rectangle:
			dot += ys * locked.Stride;
			for (int y = ys; y <= ye; ++y) {
				if (token.IsCancellationRequested) {
					return;
				}
				// Calculate the black-green coloring gradient
				int yd = (ye - ys) / 2;
				byte yColor = (byte)((y - ys) * (ye - y) * 255 / (yd * yd));
				// Actually draw the observer rectangle
				for (int x = -4 - dT; x <= 4; ++x) {
					byte* ptrX = dot + 3 * (int)((currentTimeLeaped * bmpW + x) % bmpW);
					ptrX[0] = ptrX[2] = 0; ptrX[1] = yColor;
				}
				dot += locked.Stride;
			}
		}
		
	}
}
