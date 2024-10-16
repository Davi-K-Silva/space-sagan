import matplotlib.pyplot as plt
import numpy as np

# Position data
mars_data = {
    'X': 2.048987103537854E+08,
    'Y': 3.970273322261278E+07,
    'Z': -4.176915453413254E+06,
}

earth_data = {
    'X': 2.858729794957586E+07,
    'Y': -1.497930994678225E+08,
    'Z': 3.750429521718621E+04,
}

mercury_data = {
    'X': -5.889834110678695E+07,
    'Y': -1.089513901594378E+06,
    'Z': 5.297328695834260E+06,
}

venus_data = {
    'X': -5.480781628452635E+07,
    'Y': 9.236930984793840E+07,
    'Z': 4.409030327661201E+06,
}

sun_data = {
    'X': 0,
    'Y': 0,
    'Z': 0,
}

# Extract positions
mars_position = np.array([mars_data['X'], mars_data['Y'], mars_data['Z']])
earth_position = np.array([earth_data['X'], earth_data['Y'], earth_data['Z']])
mercury_position = np.array([mercury_data['X'], mercury_data['Y'], mercury_data['Z']])
venus_position = np.array([venus_data['X'], venus_data['Y'], venus_data['Z']])
sun_position = np.array([sun_data['X'], sun_data['Y'], sun_data['Z']])

# Plotting
fig = plt.figure()
ax = fig.add_subplot(111, projection='3d')

# Plot Sun
ax.scatter(sun_position[0], sun_position[1], sun_position[2], color='yellow', s=100, label='Sun')

# Plot Mars
ax.scatter(mars_position[0], mars_position[1], mars_position[2], color='red', s=50, label='Mars')

# Plot Earth
ax.scatter(earth_position[0], earth_position[1], earth_position[2], color='blue', s=50, label='Earth')

# Plot Mercury
ax.scatter(mercury_position[0], mercury_position[1], mercury_position[2], color='gray', s=50, label='Mercury')

# Plot Venus
ax.scatter(venus_position[0], venus_position[1], venus_position[2], color='orange', s=50, label='Venus')

# Labeling the plot
ax.set_xlabel('X (km)')
ax.set_ylabel('Y (km)')
ax.set_zlabel('Z (km)')
ax.set_title('Position of Sun, Mercury, Venus, Earth, and Mars')
ax.legend()

plt.show()
