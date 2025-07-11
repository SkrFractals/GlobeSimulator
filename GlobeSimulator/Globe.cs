using System.Drawing.Imaging;
using System.Numerics;

namespace GlobeSimulator {
	public partial class Globe : RenderForm {

		public enum Detail : int {
			PreDrawSimpleSphereNoOblateness = 0,
			SimpleOblateSpheroid = 1,
			TexturedOblateSpheroid = 2
		}

		public required Detail detail;
		public required int TextureSmearLevel;

		private float radius;
		private int w2, h2;

		public Globe() {
			Square = true;
			InitializeComponent();
		}
		protected override void Function_Reset() {
			w2 = bmpW / 2;
			h2 = bmpH / 2;
			radius = MathF.Min(bmpW, bmpH) * .4f - 4;
		}
		protected override void Function_DrawBackground(ParallelOptions po) {
			// TODO also draw a Sun on the left?
			if (detail > Detail.PreDrawSimpleSphereNoOblateness)
				return; // This is only for the pre-drawn detail level, other levels can't be predrawn, so skip this
			// Draws a bare Globe with a lit side on the left:
			_ = Parallel.For(0, bmpW, po, (x, state) => {
				float dx = x - w2, dx2 = dx * dx;
				// Calculates how much the pixel is lit by the Sun:
				float litBySun = MathF.Max(0.5f, MathF.Min(1.0f, .75f - .25f * dx * MyParent.TwilightBlur / radius));
				for (int y = 0; y < bmpH; ++y) {
					if (token.IsCancellationRequested) {
						state.Stop();
						continue;
					}
					// Calculates the distance of the pixel from the center and compares to radius, to make a circle:
					float dy = y - h2,
						centerDistance = MathF.Sqrt(dx2 + dy * dy),
						inCircle = MathF.Min(1, MathF.Max(0, radius - centerDistance));
					// Colors the pixel based on whether it is inside the circle and if it's lit:
					pixels![y][x] = (byte)(inCircle * litBySun * 255);
				}
			});
		}
		protected unsafe override void Function_DrawBitmap(ParallelOptions po, BitmapData locked, byte* bmpPtr) {
			// Orbit Radians (Z rotation)
			var yearRad = GetYearRad(timeB);
			float cosZ = (float)Math.Cos(yearRad), sinZ = (float)Math.Sin(yearRad);
			var mapScale = Math.Min(MyParent.MapEarth.GetLength(0) / 360, MyParent.MapEarth.GetLength(1) / 180);
			
			if (detail >= Detail.SimpleOblateSpheroid) {
				// Radius constants:
				float PolarRadiusN = radius * MyParent.PolarShrink,
					PolarRadius = S(PolarRadiusN),
					EquatorRadius = S(radius * MyParent.EquatorShrink),
					ByteRadius = 255 * radius;
				var RadiusS = Vector3.One / new Vector3(EquatorRadius, EquatorRadius, PolarRadius);
				// a transposed rotation matrix for inverse rotation from screen space to globe space (XZ to tilt, XY to orbit)
				Vector3[] toGlobe = [
					new(sinZ * MyParent.TiltCos, cosZ * MyParent.TiltCos, -MyParent.TiltSin),
					new(-cosZ, sinZ, 0),
					new(sinZ *  MyParent.TiltSin, cosZ * MyParent.TiltSin, MyParent.TiltCos)
				], toScreen = [
					new(toGlobe[0].X, toGlobe[1].X, toGlobe[2].X),
					new(toGlobe[0].Y, toGlobe[1].Y, toGlobe[2].Y),
					new(toGlobe[0].Z, toGlobe[1].Z, toGlobe[2].Z)
				];
				_ = Parallel.For(0, bmpH, po, (y, state) => {
					// Get a row of pixels:
					var a = pixels![y];
					byte* row = bmpPtr + y * locked.Stride;
					// Proccess all pixels in this row:
					for (int x = 0; x < bmpW; ++x) {
						if (token.IsCancellationRequested) {
							state.Stop();
							continue;
						}
						// Convert pixel to normalized screen coordinates
						Vector3 pix_screen = new(x - w2, 0, y - h2),
							pix_globe = new( // Rotate to Globe space
								Vector3.Dot(toGlobe[0], pix_screen),
								Vector3.Dot(toGlobe[1], pix_screen),
								Vector3.Dot(toGlobe[2], pix_screen)
							),
							pix_globeS = pix_globe * pix_globe;
						// Check if inside oblate spheroid
						float d = Vector3.Dot(pix_globeS, RadiusS);
						if (d <= 1.0f) {
							// project the Earth slice to the surface in the direction of depth, for texture sampling:
							// float[] depthVector = toGlobe * { 0, 1, 0 } = toScreen[1]; 
							// Find T where pix_globe + T*depthVector is on the surface
							float A = Vector3.Dot(toScreen[1] * toScreen[1], RadiusS),
								B = 2 * Vector3.Dot(pix_globe * toScreen[1], RadiusS),
								C = Vector3.Dot(pix_globeS, RadiusS) - 1,
								T = -.5f * (B + MathF.Sqrt(S(B) - 4 * A * C)) / A;
							// Project the pix_globe towards me to the surface with the found T
							pix_globe += T * new Vector3(toGlobe[0].Y, toGlobe[1].Y, toGlobe[2].Y);

							// get a normal vector to the surface:
							var normalEarth = Vector3.Normalize(pix_globe / RadiusS);
							normalEarth = new( // Rotate to Screen space
								Vector3.Dot(toScreen[0], normalEarth),
								Vector3.Dot(toScreen[1], normalEarth),
								Vector3.Dot(toScreen[2], normalEarth)
							);
							// is the pixel on the globe? also get the daylight brightness from normal.X, and put it into the alpha as well:
							// TODO apply Twilight
							var alpha = MathF.Min(1.0f, (1.0f - d) * radius) * MathF.Max(0.5f, Math.Min(1.0f, 0.5f + 20 * MathF.Asin(-normalEarth.X) / MyParent.TwilightBlur));

							if (detail >= Detail.TexturedOblateSpheroid) {
								
								// Get latitude and longitude:
								float lat = mapScale * (180 - MathF.Acos(pix_globe.Z / MathF.Sqrt(S(pix_globe.X) + S(pix_globe.Y) + S(pix_globe.Z))) * (180 / MathF.PI)),
									lon = MathF.Atan2(-pix_globe.X, pix_globe.Y) * (180 / MathF.PI);
								// Spin Days:
								double s = MyParent.LeapYears > 0 ? MyParent.Spins + 1.0 / MyParent.LeapYears : MyParent.Spins,
									timeS = Math.Min(1, (timeB - timeA) * s) * 360 * mapScale;
								lon += (2 - (float)(timeB * s + MyParent.EquinoxShift) % 1) * 360;
								lon = (lon + 360) % 360;
								lon *= mapScale;
								int ilat = (int)lat, samples = 0;
								var RGB = Vector3.Zero;
								// Sample UV from an Earth map
								var DT = mapScale * MathF.Pow(.5f, TextureSmearLevel - 5);
								for (var i = 0.0f; i <= timeS; i += DT) {
									var tex = MyParent.MapEarth[(int)((lon + i) % (360 * mapScale)), ilat];
									RGB += tex;
									++samples;
								}
								RGB *= alpha / samples;
								// draw the pixel of the earth, or black outside:
								row[2] = (byte)RGB.X;
								row[1] = (byte)RGB.Y;
								row[0] = (byte)RGB.Z;
							} else {
								// Just an oblate spheroid without a texture
								row[2] = row[1] = row[0] = (byte)(alpha * 255);
							}

						} else row[0] = row[1] = row[2] = 0;

						row += 3;
					}
				});


			} else {
				_ = Parallel.For(0, bmpH, po, (y, state) => {
					var a = pixels![y];
					byte* row = bmpPtr + y * locked.Stride;
					for (int x = 0; x < bmpW; ++x) {
						if (token.IsCancellationRequested) {
							state.Stop();
							continue;
						}
						// Draw the precalculated globe:
						row[0] = row[1] = row[2] = a[x];
						row += 3;
					}
				});
			}
			if (token.IsCancellationRequested) {
				return;
			}
			byte* dotp = bmpPtr;
			// Draw axial North and South Poles:
			var N = MyParent.PolarShrink * GetNorth(cosZ, sinZ);
			for (float p = 1.0f; p < 1.1f; p += 0.02f) {
				DrawPoint(bmpPtr, locked.Stride, p * N, 0, 0, 0); // axial north pole
				DrawPoint(bmpPtr, locked.Stride, -p * N, 0, 0, 0); // axial south pole
			}
			// Draw Equator:
			for (int t = 0; t < 360; ++t) {
				DrawPoint(bmpPtr, locked.Stride, GetObserver(0, cosZ, sinZ, new(MyParent.EquatorShrink, 0, 0), t), 0, 0, 0);
			}
			for (var t = timeB; t >= timeA; t -= .00002f) {
				if (token.IsCancellationRequested) {
					return;
				}
				double yr = GetYearRad(t);
				// Calculate the location of the magnetic North, from its Latitude and Longitude
				N = GetObserver(t, yr, MyParent.MagneticLatitudeVector, MyParent.MagneticLongitude);
				// Draw magnetic North and South:
				DrawPoint(bmpPtr, locked.Stride, N, 0, 0, 255); // blue north
				DrawPoint(bmpPtr, locked.Stride, -N, 255, 0, 0); // red south
				// Draw Observer Dot
				DrawPoint(bmpPtr, locked.Stride, GetObserver(t, yr, MyParent.ObserverLatitudeVector, MyParent.Longitude), 0, 255, 0);
			}
			
			//Draws Observer's Local Axes (Red-East, Green-North, Blue-Up)
			/*var Up_Local = GetObserver(timeB, yearRad, OnLatitude);
			(var North_Local, var East_Local) = GetNorthEast(yearRad, Up_Local);
			for (float a = 0.01f; a < 0.1f; a += 0.01f) {
				DrawPoint(bmpPtr, locked.Stride, Up_Local + a * East_Local, 255, 0, 0);
				DrawPoint(bmpPtr, locked.Stride, Up_Local + a * North_Local, 0, 255, 0);
				DrawPoint(bmpPtr, locked.Stride, (1 + a) * Up_Local, 0, 0, 255);
			}*/
		}
		private unsafe void DrawPoint(byte* bmpPtr, int stride, Vector3 Observer, int R, int G, int B) {
			byte* dot = bmpPtr;
			// If on the back of the globe - make the point 2x smaller
			int r = Observer.Y > 0 ? 2 : 4;
			int xs = (int)(w2 + Observer.X * radius) - r;
			int zs = (int)(h2 - Observer.Z * radius) - r;
			r *= 2;
			int xe = xs + r;
			int ze = zs + r;
			dot += zs * stride;
			// If on the back of the globe - make the point 4x darker
			if (Observer.Y > 0) {
				R /= 4;
				G /= 4;
				B /= 4;
			}
			if (R == 0) R = 256;
			if (G == 0) G = 256;
			if (B == 0) B = 256;
			// Draw a small rectangle at the point:
			for (int iy = zs; iy <= ze; ++iy) {
				for (int ix = xs; ix <= xe; ++ix) {
					byte* ptrX = dot + 3 * ix;
					ptrX[2] = (byte)(MathF.Max(R, ptrX[2]) % 256);
					ptrX[1] = (byte)(MathF.Max(G, ptrX[1]) % 256);
					ptrX[0] = (byte)(MathF.Max(B, ptrX[0]) % 256);
				}
				dot += stride;
			}
		}

		/*static double[,] BuildRotationMatrix(double tilt, double orbit) {
			
		}
		static double[,] MultiplyMatrices(double[,] A, double[,] B) {
			double[,] result = new double[3, 3];
			for (int i = 0; i < 3; i++)
				for (int j = 0; j < 3; j++)
					for (int k = 0; k < 3; k++)
						result[i, j] += A[i, k] * B[k, j];
			return result;
		}*/
	}
}
