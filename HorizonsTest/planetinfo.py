import os
import matplotlib.pyplot as plt
from mpl_toolkits.mplot3d import Axes3D
import numpy as np

def load_planet_data(file_path):
    dates = []
    positions = []
    
    with open(file_path, 'r') as file:
        lines = file.readlines()
        for line in lines:
            # Skip header and empty lines
            if line.startswith("Date") or not line.strip():
                continue

            # Parse each line (Date, X, Y, Z)
            parts = line.strip().split(',')
            date = parts[0].strip()
            x = float(parts[1].strip())  # No conversion, keep in kilometers
            y = float(parts[2].strip())  # No conversion, keep in kilometers
            z = float(parts[3].strip())  # No conversion, keep in kilometers
            
            dates.append(date)
            positions.append((x, y, z))
    
    return dates, np.array(positions)

def plot_orbits(data_folder):
    fig = plt.figure()
    ax = fig.add_subplot(111, projection='3d')
    
    # List all planet data files in the directory
    for filename in os.listdir(data_folder):
        if filename.endswith(".txt"):
            planet_name = filename.split('_')[1]  # Extract planet name from filename
            file_path = os.path.join(data_folder, filename)

            # Load data and unpack positions
            dates, positions = load_planet_data(file_path)
            x, y, z = positions[:, 0], positions[:, 1], positions[:, 2]

            # Plot the orbit
            ax.plot(x, y, z, label=planet_name)
    
    # Set axis labels in kilometers
    ax.set_xlabel("X (km)")
    ax.set_ylabel("Y (km)")
    ax.set_zlabel("Z (km)")
    ax.set_title("Planetary Orbits in Kilometers")
    
    # Show legend
    ax.legend()
    plt.show()

# Set the path to your folder containing the planet data files
data_folder = '../Assets/Resources/PlanetData'
plot_orbits(data_folder)
