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

# Convert the data to numpy arrays for easier plotting
earth_positions = np.array(earth_orbit_data)

# Extract the X, Y, and Z coordinates
X = earth_positions[:, 0]
Y = earth_positions[:, 1]
Z = earth_positions[:, 2]

# Sun position (at the center of the Solar System Barycenter)
sun_position = np.array([0, 0, 0])

# Plotting
fig = plt.figure()
ax = fig.add_subplot(111, projection='3d')

# Plot Sun
ax.scatter(sun_position[0], sun_position[1], sun_position[2], color='yellow', s=100, label='Sun')

# Plot Earth's orbit
ax.plot(X, Y, Z, color='blue', label='Earth Orbit')
ax.scatter(X, Y, Z, color='blue', s=20)  # Plot each point on the orbit

# Labeling the plot
ax.set_xlabel('X (km)')
ax.set_ylabel('Y (km)')
ax.set_zlabel('Z (km)')
ax.set_title('Earth Orbit Around the Sun')
ax.legend()

plt.show()
