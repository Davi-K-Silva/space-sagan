import numpy as np
import matplotlib.pyplot as plt
from mpl_toolkits.mplot3d import Axes3D

def kepler_equation(E, M, e):
    return E - e * np.sin(E) - M

def solve_kepler(M, e, tol=1e-6):
    E = M  # Initial guess: mean anomaly
    while True:
        delta = kepler_equation(E, M, e) / (1 - e * np.cos(E))
        E -= delta
        if np.abs(delta) < tol:
            break
    return E

def calculate_orbit(a, e, I, L, long_peri, long_node):
    # Number of points to plot
    num_points = 500

    # Mean anomalies from 0 to 2pi
    M = np.linspace(0, 2 * np.pi, num_points)
    
    # Solve Kepler's equation for each mean anomaly
    E = np.array([solve_kepler(m, e) for m in M])
    
    # True anomaly
    theta = 2 * np.arctan2(np.sqrt(1 + e) * np.sin(E / 2), np.sqrt(1 - e) * np.cos(E / 2))
    
    # Distance r at each true anomaly
    r = a * (1 - e**2) / (1 + e * np.cos(theta))
    
    # Position in the orbital plane
    x_orbit = r * np.cos(theta)
    y_orbit = r * np.sin(theta)
    
    # Rotation to the ecliptic plane
    cos_I = np.cos(np.radians(I))
    sin_I = np.sin(np.radians(I))
    cos_long_node = np.cos(np.radians(long_node))
    sin_long_node = np.sin(np.radians(long_node))
    cos_long_peri = np.cos(np.radians(long_peri))
    sin_long_peri = np.sin(np.radians(long_peri))
    
    x_ecliptic = (cos_long_node * cos_long_peri - sin_long_node * sin_long_peri * cos_I) * x_orbit + (-cos_long_node * sin_long_peri - sin_long_node * cos_long_peri * cos_I) * y_orbit
    y_ecliptic = (sin_long_node * cos_long_peri + cos_long_node * sin_long_peri * cos_I) * x_orbit + (-sin_long_node * sin_long_peri + cos_long_node * cos_long_peri * cos_I) * y_orbit
    
    z_ecliptic = sin_I * x_orbit * np.sin(np.radians(long_node)) + sin_I * y_orbit * np.cos(np.radians(long_node))
    
    return x_ecliptic, y_ecliptic, z_ecliptic

# Orbital elements for planets
# Orbital elements extracted from the table you provided

# Mercury
a_mercury = 0.38709927
e_mercury = 0.20563593
I_mercury = 7.00497902
L_mercury = 252.25032350
long_peri_mercury = 77.45779628
long_node_mercury = 48.33076593

# Venus
a_venus = 0.72333566
e_venus = 0.00677672
I_venus = 3.39467605
L_venus = 181.97909950
long_peri_venus = 131.60246718
long_node_venus = 76.67984255

# Mars
a_mars = 1.52371034
e_mars = 0.09339410
I_mars = 1.84969142
L_mars = -4.55343205
long_peri_mars = -23.94362959
long_node_mars = 49.55953891

# Jupiter
a_jupiter = 5.20288700
e_jupiter = 0.04838624
I_jupiter = 1.30439695
L_jupiter = 34.39644051
long_peri_jupiter = 14.72847983
long_node_jupiter = 100.47390909

# Saturn
a_saturn = 9.53667594
e_saturn = 0.05386179
I_saturn = 2.48599187
L_saturn = 49.95424423
long_peri_saturn = 92.59887831
long_node_saturn = 113.66242448

# Uranus
a_uranus = 19.18916464
e_uranus = 0.04725744
I_uranus = 0.77263783
L_uranus = 313.23810451
long_peri_uranus = 170.95427630
long_node_uranus = 74.01692503

# Neptune
a_neptune = 30.06992276
e_neptune = 0.00859048
I_neptune = 1.77004347
L_neptune = -55.12002969
long_peri_neptune = 44.96476227
long_node_neptune = 131.78422574

# Earth (EM Barycenter)
a_earth = 1.00000261
e_earth = 0.01671123
I_earth = -0.00001531
L_earth = 100.46457166
long_peri_earth = 102.93768193
long_node_earth = 0.0

# Calculate orbits
orbits = {
    'Mercury': calculate_orbit(a_mercury, e_mercury, I_mercury, L_mercury, long_peri_mercury, long_node_mercury),
    'Venus': calculate_orbit(a_venus, e_venus, I_venus, L_venus, long_peri_venus, long_node_venus),
    'Mars': calculate_orbit(a_mars, e_mars, I_mars, L_mars, long_peri_mars, long_node_mars),
    'Jupiter': calculate_orbit(a_jupiter, e_jupiter, I_jupiter, L_jupiter, long_peri_jupiter, long_node_jupiter),
    'Saturn': calculate_orbit(a_saturn, e_saturn, I_saturn, L_saturn, long_peri_saturn, long_node_saturn),
    'Uranus': calculate_orbit(a_uranus, e_uranus, I_uranus, L_uranus, long_peri_uranus, long_node_uranus),
    'Neptune': calculate_orbit(a_neptune, e_neptune, I_neptune, L_neptune, long_peri_neptune, long_node_neptune),
    'Earth': calculate_orbit(a_earth, e_earth, I_earth, L_earth, long_peri_earth, long_node_earth),
}

# Plot orbits in 3D
fig = plt.figure(figsize=(12, 12))
ax = fig.add_subplot(111, projection='3d')
ax.set_title('Orbits of Planets in the Solar System')

# Plot each orbit
for planet, (x, y, z) in orbits.items():
    ax.plot(x, y, z, label=planet)

# Plot the Sun at the origin
ax.scatter(0, 0, 0, color='orange', label='Sun')

ax.set_xlabel('X [au]')
ax.set_ylabel('Y [au]')
ax.set_zlabel('Z [au]')
ax.legend()
plt.show()
