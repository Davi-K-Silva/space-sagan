import numpy as np
import matplotlib.pyplot as plt

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
    
    return x_ecliptic, y_ecliptic

# Orbital elements for Earth (EM Bary)
a = 1.00000261  # Semi-major axis [au]
e = 0.01671123  # Eccentricity
I = -0.00001531  # Inclination [degrees]
L = 100.46457166  # Mean longitude [degrees]
long_peri = 102.93768193  # Longitude of perihelion [degrees]
long_node = 0.0  # Longitude of the ascending node [degrees]

# Calculate orbit
x, y = calculate_orbit(a, e, I, L, long_peri, long_node)

# Plot orbit
plt.figure(figsize=(8, 8))
plt.plot(x, y, label='Earth Orbit')
plt.scatter(0, 0, color='orange', label='Sun')  # Sun at the origin
plt.xlabel('x [au]')
plt.ylabel('y [au]')
plt.title('Earth Orbit')
plt.legend()
plt.grid(True)
plt.axis('equal')
plt.show()
