# GlobeSimulator
A simulator of a globe planet, that predicts/tracks the sun on the sky

The simulator has 3 graphical outputs:

1. Globe View
- Renders a globe, with a sunlit side, north pole as red, south pole as blue, equator as black, and observer (you) as green.
- If the green dot is at the lit side (left), it should be a day time, if in the unlit side (right) then it's night time.
- The camera always look at the globe so that the sun is to the left.
- So the orbit around the sun would just look like the tilt is rotating, and the globe's spin doesn't affect camera at all.

2. Daylight Chart
- A chart plotting daylight over the year for every latitude.
- Y is the latitude, X is the time in the year from January 1 to December 31.
- So the middle would be on the equator and you should see 365 black-white stripes representing the days and nights.
- On the pole at the top and bottom of the chart, you would see the half-year long polar days and nights.
- For example the north pole would be all black during the winter, and all white during the summer, and the south pole vice-versa.
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
- This slider adjust the speed of time of the running simulation.
- All the way to left left is frozen time.
- The first step next to that is 1x, and further you go, the speed of time increases exponentially.

Rewind: 
- Will make the time rewind at the selected TimeSpeed.

Observer Latitude + Longitude:
- Where does the simulation consider you to be?
- You can either type in the precise location, or use the slider for approximate one (without a decimal point).

Wrap Longitude:
- If the Longitude is all the way to to +180 or -180, it can switch it to the other.

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

---------------------------------------------------------

Precision errors already accounted for:

-Earth's orbit is elliptical instead or a perfect circle (implemented elliptical Sun's angle and distance correction)
ERROR SIZE: Sun size ~0.9%, Sun Location on the sky - up to 2 degrees orbital angle in between aphelion and perihelion

-Leap days (the year doesn't have exactly 365 days, but it does in this simulation)
ERROR SIZE: missing February 29, up to 1 degree of orbital angle 

-Horizon curvature and atmospheric refraction letting you see slightly more than 180 degrees of sky
ERROR SIZE: 1.21 degrees below horizon

-Magnetic north is not exactly at axial North
ERROR SIZE: up to 5 degrees of compass North offset

-Globe's oblateness (the simulation works with a perfectly spherical Globe)
ERROR SIZE: up to 0.3% in observer location (but negligible for this simulation)

-Since the Sun isn't infinitely far away, it should move in a paralax slightly based on where on the Globe the observer is. (the simulation pretends the Sun is infinitely far away and makes perfectly parallel rays)
ERROR SIZE: up to 9 arcseconds (very small)

---------------------------------------------------------

Precision errors not accounted for (yet):

-No mountains and other surface irregularities.
ERROR SIZE: None with Sea on Horizon
IMPLEMENTATION DIFFICULTY: 10/10 (would have to implement all the mountains on Earth, not going to do that)

-The simulation aligns the compass precisely to the magnetic north, but i real life, there can be a lot of secondary things that can slightly nudge your compass needle, like nearby iron and magnets.
ERROR SIZE: Unknown
IMPLEMENTATION DIFFICULTY: 11/10 No idea how I could easily account for this, I would need to know the insides of the Earth perfectly and your real compass would have stay away from any magnetic objects that I have no control over.
