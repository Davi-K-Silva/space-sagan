import matplotlib.pyplot as plt
from mpl_toolkits.mplot3d import Axes3D
import requests

# Example data (replace with your actual API response parsing logic)
# Function to fetch ephemeris data from Horizons
def fetch_horizons_data():
    url = 'https://ssd.jpl.nasa.gov/horizons_batch.cgi'
    params = {
        'batch': '1',
        'MAKE_EPHEM': 'YES',
        'COMMAND': "'199'",
        'EPHEM_TYPE': "'VECTORS'",
        'CENTER': "'500@0'",
        'START_TIME': "'2023-01-01'",
        'STOP_TIME': "'2024-01-01'",
        'STEP_SIZE': "'1 DAYS'",
        'VEC_TABLE': "'3'",
        'REF_SYSTEM': "'ICRF'",
        'REF_PLANE': "'ECLIPTIC'",
        'VEC_CORR': "'NONE'",
        'CAL_TYPE': "'M'",
        'OUT_UNITS': "'KM-S'",
        'VEC_LABELS': "'YES'",
        'VEC_DELTA_T': "'NO'",
        'CSV_FORMAT': "'NO'",  # Request data in plain text format
        'OBJ_DATA': "'YES'"
    }
    
    response = requests.get(url, params=params)
    if response.status_code == 200:
        # Find the start and end markers
        start_marker = "$$SOE"
        end_marker = "$$EOE"
        
        # Extract the ephemeris data between markers
        start_index = response.text.find(start_marker)
        end_index = response.text.find(end_marker)
        
        if start_index != -1 and end_index != -1:
            ephemeris_data = response.text[start_index + len(start_marker):end_index].strip()
            return ephemeris_data
        else:
            print("Markers not found in the response.")
            return None
    else:
        print(f"Error fetching data: {response.status_code} - {response.reason}")
        return None

# Fetch ephemeris data from Horizons
ephemeris_text = fetch_horizons_data()

data = ephemeris_text
# Initialize lists to store coordinates
x_coords = []
y_coords = []
z_coords = []

newlines = []

lines = data.strip().split("\n")
for line in lines:
    if line.strip().startswith("X"):
        if "=-" in line:
            print("here")
            line = line.replace("=-","= -")
            print(line)
        newlines.append(line)

print(newlines)
# Splitting lines and extracting coordinates
#lines = data.strip().split("\n")
for line in newlines:
    if line.strip().startswith("X"):
        parts = line.split()
        print(parts)
        x_coords.append(float(parts[2]))
        y_coords.append(float(parts[5]))
        z_coords.append(float(parts[8]))

# Plotting
fig = plt.figure()
ax = fig.add_subplot(111, projection='3d')

ax.scatter(x_coords, y_coords, z_coords, c='r', marker='o')

ax.set_xlabel('X')
ax.set_ylabel('Y')
ax.set_zlabel('Z')

plt.show()