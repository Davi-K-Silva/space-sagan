import matplotlib.pyplot as plt
from mpl_toolkits.mplot3d import Axes3D
import requests

# Function to fetch ephemeris data from Horizons
def fetch_horizons_data(obj_id, start_date, end_date):
    url = 'https://ssd.jpl.nasa.gov/horizons_batch.cgi'
    params = {
        'batch': '1',
        'MAKE_EPHEM': 'YES',
        'COMMAND': f"'{obj_id}'",
        'EPHEM_TYPE': "'VECTORS'",
        'CENTER': "'500@0'",
        'START_TIME': f"'{start_date}'",
        'STOP_TIME': f"'{end_date}'",
        'STEP_SIZE': "'1 DAYS'",
        'VEC_TABLE': "'3'",
        'REF_SYSTEM': "'ICRF'",
        'REF_PLANE': "'F'",
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
        print(f"Error fetching data for object {obj_id}: {response.status_code} - {response.reason}")
        return None

def realname(id):
    name = ""
    if id == "10":
        name = "Sol"
    if id == "199":
        name = "Mercurio"
    if id == "299":
        name = "Venus"
    if id == "399":
        name = "Terra"
    if id == "499":      
        name = "Marte"
    return name


# Example usage: Fetch data for multiple objects
objects = {
    '10': ('2023-01-01', '2024-01-01'),
    '499': ('2023-01-01', '2025-01-01'),
    '399': ('2023-01-01', '2024-01-01'),
    '299': ('2023-01-01', '2024-01-01'),  # Add more objects as needed
    '199': ('2023-01-01', '2024-01-01')
}

# Initialize lists to store coordinates
fig = plt.figure()
ax = fig.add_subplot(111, projection='3d')

for obj_id, (start_date, end_date) in objects.items():
    ephemeris_text = fetch_horizons_data(obj_id, start_date, end_date)
    
    if ephemeris_text:
        # Initialize lists to store coordinates
        x_coords = []
        y_coords = []
        z_coords = []

        data = ephemeris_text
        newlines = []
        lines = data.strip().split("\n")
        for line in lines:
            if line.strip().startswith("X"):
                if "=-" in line:
                    print("here")
                    line = line.replace("=-","= -")
                    print(line)
                newlines.append(line)

        #print(newlines)
        # Splitting lines and extracting coordinates
        #lines = data.strip().split("\n")
        count = 0
        for line in newlines:
            if line.strip().startswith("X") and count ==0:
                count+=1
                parts = line.split()
                print(parts)
                x_coords.append(float(parts[2]))
                y_coords.append(float(parts[5]))
                z_coords.append(float(parts[8]))

        # Plotting orbits
        ax.scatter(x_coords, y_coords, z_coords, marker='o', label=realname(obj_id))

# Set labels and display plot
ax.set_xlabel('X')
ax.set_ylabel('Y')
ax.set_zlabel('Z')

# Fixing the scale for all axes
max_range = max(max(x_coords), max(y_coords), max(z_coords))
ax.set_xlim([-max_range, max_range])
ax.set_ylim([-max_range, max_range])
ax.set_zlim([-max_range, max_range])

ax.legend()

plt.show()
