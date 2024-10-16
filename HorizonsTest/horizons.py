import matplotlib.pyplot as plt
import numpy as np

# Position data for Earth at different times
earth_orbit_data = [
    (-2.682456095823074E+07, 1.448975704171931E+08, 2.413001115289330E+04),
    (-9.906509850050257E+07, 1.103153188907793E+08, 2.617342884978652E+04),
    (-1.404339080974078E+08, 5.107836133237942E+07, 2.956162461347878E+04),
    (-1.481869068936424E+08, -2.782315857156947E+07, 3.374689879771136E+04),
    (-1.166540904570800E+08, -9.713263871932262E+07, 3.739045974498242E+04),
    (-5.328792362541436E+07, -1.426707827423932E+08, 3.981048565176874E+04),
    (2.149984051511574E+07, -1.505859051110129E+08, 4.010841811215132E+04),
    (9.262412495155303E+07, -1.195864841245933E+08, 3.826323086332530E+04),
    (1.387359789097274E+08, -5.686478782224681E+07, 3.442701386049762E+04),
    (1.473588784571390E+08, 1.854315256927273E+07, 2.990429803438578E+04),
    (1.158358813063503E+08, 9.100471765086839E+07, 2.566139368866757E+04),
    (5.361245933506583E+07, 1.365637821399999E+08, 2.328046844377369E+04),
    (-2.600298246493929E+07, 1.445600958366426E+08, 2.324604087693989E+04),
]

# Position data for Mars at different times
mars_orbit_data = [
    (7.752583519268439E+06, 2.337987557943932E+08, 4.707746519684419E+06),
    (-5.421594226480182E+07, 2.332648776601892E+08, 6.217823880848512E+06),
    (-1.071858196169919E+08, 2.188565196665210E+08, 7.216257314635783E+06),
    (-1.591163034225527E+08, 1.892356715610578E+08, 7.870476085229695E+06),
    (-1.999096486412697E+08, 1.491275591185231E+08, 8.031667295731254E+06),
    (-2.299762717931310E+08, 9.863481268283156E+07, 7.712126942588501E+06),
    (-2.457436473113893E+08, 4.397122361626422E+07, 6.954375422742983E+06),
    (-2.471586096907884E+08, -1.515428052341948E+07, 5.751081904794994E+06),
    (-2.329856254452827E+08, -7.331379880561344E+07, 4.185658780860338E+06),
    (-2.046893400400904E+08, -1.250136923437167E+08, 2.409131185058415E+06),
    (-1.615268519074686E+08, -1.698005851804723E+08, 4.128486091508865E+05),
    (-1.083732613923766E+08, -2.011037351357945E+08, -1.545970729562879E+06),
    (-4.507776377907523E+07, -2.175196919117816E+08, -3.441546040035352E+06),
]

# Position data for Mercury at different times
mercury_orbit_data = [
    (1.764026173635770E+07, 4.205586451549117E+07, 1.724774348507849E+06),
    (-5.493676219688971E+07, -3.734257162876161E+07, 1.897549535243489E+06),
    (2.640589897984096E+07, -5.977029884361363E+07, -7.392621933381952E+06),
    (7.833268439239624E+06, 4.488594937039087E+07, 2.867693200543825E+06),
    (-5.322329560728410E+07, -4.075518687469741E+07, 1.473592839186937E+06),
    (3.751665506724610E+07, -5.026474988766800E+07, -7.622307037168078E+06),
    (-7.382443005816810E+06, 4.567062241792723E+07, 4.340161558077861E+06),
    (-4.485537457813899E+07, -5.226041071368721E+07, -2.211942246950418E+05),
    (4.622784275065255E+07, -3.767338503281131E+07, -7.379197579694102E+06),
    (-2.658235940349510E+07, 4.047607508223532E+07, 5.690109263829736E+06),
    (-3.422519097171840E+07, -6.108847168990459E+07, -1.904244720509995E+06),
    (5.062442664700754E+07, -2.645566424402387E+07, -6.852359506108861E+06),
    (-4.227610797568034E+07, 2.953899410451555E+07, 6.249351652770508E+06),
]

# Convert the data to numpy arrays for easier plotting
earth_positions = np.array(earth_orbit_data)
mars_positions = np.array(mars_orbit_data)
mercury_positions = np.array(mercury_orbit_data)

# Extract the X, Y, and Z coordinates
X_earth = earth_positions[:, 0]
Y_earth = earth_positions[:, 1]
Z_earth = earth_positions[:, 2]

X_mars = mars_positions[:, 0]
Y_mars = mars_positions[:, 1]
Z_mars = mars_positions[:, 2]

X_mercury = mercury_positions[:, 0]
Y_mercury = mercury_positions[:, 1]
Z_mercury = mercury_positions[:, 2]

# Sun position (at the center of the Solar System Barycenter)
sun_position = np.array([0, 0, 0])

# Plotting
fig = plt.figure()
ax = fig.add_subplot(111, projection='3d')

# Plot Sun
ax.scatter(sun_position[0], sun_position[1], sun_position[2], color='yellow', s=100, label='Sun')

# Plot Earth's orbit
ax.plot(X_earth, Y_earth, Z_earth, color='blue', label='Earth Orbit')
ax.scatter(X_earth, Y_earth, Z_earth, color='blue', s=20)  # Plot each point on the orbit

# Plot Mars' orbit
ax.plot(X_mars, Y_mars, Z_mars, color='red', label='Mars Orbit')
ax.scatter(X_mars, Y_mars, Z_mars, color='red', s=20)  # Plot each point on the orbit

# Plot Mercury's orbit
ax.plot(X_mercury, Y_mercury, Z_mercury, color='orange', label='Mercury Orbit')
ax.scatter(X_mercury, Y_mercury, Z_mercury, color='orange', s=20)  # Plot each point on the orbit

# Labeling the plot
ax.set_xlabel('X (km)')
ax.set_ylabel('Y (km)')
ax.set_zlabel('Z (km)')
ax.set_title('Mercury, Earth, and Mars Orbits Around the Sun')
ax.legend()

plt.show()
