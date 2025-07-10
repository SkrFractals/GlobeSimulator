# GlobeSimulator
A simulator of a globe planet, that predicts/tracks the sun on the sky

The simulator has 3 graphical outputs:

1. Globe View
- Renders a globe, with a sunlit side, North Pole as red, South Pole as blue, equator as black, and observer (you) as green.
- If the green dot is at the lit side (left), it should be a day time, if in the unlit side (right) then it's nighttime.
- The camera always looks at the globe so that the sun is to the left.
- So the orbit around the sun would just look like the tilt is rotating, and the globe's spin doesn't affect camera at all.

2. Daylight Chart
- A chart plotting daylight over the year for every latitude.
- Y is the latitude, X is the time in the year from January 1 to December 31.
- So the middle would be on the equator, and you should see 365 black-white stripes representing the days and nights.
- On the pole at the top and bottom of the chart, you would see the half-year long polar days and nights.
- For example, the North Pole would be all black during the winter, and all white during the summer, and the South Pole vice versa.
- This chart also shows the green dot as the observer, which should match the daylight of the observer on the Globe View.
- When the Globe's observer is on the lit side, the Chart's observer should be at a white spot.

3. Sky View
- Renders the location of the Sun on the sky around the observer.
- It's labeled with the compass directions.
- By default, the North is on the bottom, because you are looking UP, instead of down like on a map, that flips the directions your forehead and beard are pointing.
- But if you want, you can switch it so that North is up like if you were looking down on a map.
- It should also be in sync with the other 2 renders.
- If the observer is lit, then the Sun should be on the sky, otherwise you might only see the red blur of the Sun below the horizon.
- This view can predict the exact world direction where the sun would set and rise on a globe model.

TimeSpeed:
- This slider adjusts the speed of time of the running simulation.
- All the way to the left is frozen time.
- The first step next to that is 1x, and further you go, the speed of time increases exponentially.

Rewind: 
- Will make the time rewind at the selected TimeSpeed.

Observer Latitude + Longitude:
- Where does the simulation consider you to be?
- You can either type in the precise location, or use the slider for approximate one (without a decimal point).

Wrap Longitude:
- If the Longitude is all the way to +180 or -180, it can switch it to the other.

Tilt:
- How much is the planet tilted relative to the Sun's north?

Eccentricity:
- How much elliptical is the orbit?

Spins Per Year:
- How many full spins does the planet make over one orbit (not accounting for leap years, only type in the integer full spins)?
- This is not exactly how many days are in a non-leap year, because the orbit typically subtracts one from this.

Daylight Chart, Globe Map, Sky Map:
- Opens the corresponding Render Window.

Leap Year:
- How many years are Leap Years apart?

Leap Day:
- Which day is the inserted Leap day?
- Counted from the first day in the year.

Twilight Blur:
- Blurs the sunrise and sunset, higher values will make those last longer.

Magnetic Latitude + Longitude:
- Where is the magnetic pole the compass North points to (the south magnetic pole of the planet)?
- It is drawn as blue dot on the Globe View, and the North compass direction on the Sky View points there.

Polar Radius:
- The distance from the center of the globe to a pole.

Equatorial Radius:
- The distance from the center of the globe to the equator.

Horizon Angle:
- How wide is the sky in degrees?
- To account for curvature and refraction from eye-level.

Average Distance to the Sun:
- AU for Earth.
- only has effect on Sun parallax correction on the Sky View.
