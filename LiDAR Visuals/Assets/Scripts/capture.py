# capture.py
import serial
import time
import os

SERIAL_PORT = 'COM3'    # change to your port
BAUD        = 115200
OUTPUT_DIR  = 'scans'   # folder where scan files get saved

# Make sure the output folder exists
os.makedirs(OUTPUT_DIR, exist_ok=True)

ser = serial.Serial(SERIAL_PORT, BAUD, timeout=1)
print("Waiting for data...")

def new_filename():
    timestamp = time.strftime("%Y%m%d_%H%M%S")
    return os.path.join(OUTPUT_DIR, f"scan_{timestamp}.csv")

def parse_line(line):
    """Parse 'pitch, yaw, distance' from serial line. Returns (pitch, yaw, dist) or None."""
    try:
        parts = line.strip().split(',')
        if len(parts) != 3:
            return None
        pitch = float(parts[0])
        yaw   = float(parts[1])
        dist  = int(parts[2])
        return pitch, yaw, dist
    except ValueError:
        return None

current_file    = None
current_writer  = None
points_in_scan  = 0
last_yaw        = None
cycle_complete  = False

def open_new_scan():
    global current_file, points_in_scan
    filename = new_filename()
    current_file = open(filename, 'w')
    current_file.write("pitch,yaw,distance_cm\n")  # CSV header
    points_in_scan = 0
    print(f"New scan started → {filename}")
    return filename

def close_scan():
    global current_file
    if current_file:
        current_file.flush()
        current_file.close()
        print(f"Scan saved! ({points_in_scan} points)")
        current_file = None

# Open the first scan file immediately
current_filename = open_new_scan()

try:
    while True:
        raw = ser.readline().decode('utf-8', errors='ignore').strip()
        if not raw:
            continue

        parsed = parse_line(raw)
        if parsed is None:
            continue

        pitch, yaw, dist = parsed

        # Detect cycle completion:
        # yaw hits 180 then comes back toward 0 — when it crosses back below 5
        # after having been above 170, that's one full sweep done
        if last_yaw is not None:
            if last_yaw >= 170 and yaw < 170:
                # yaw just reversed from near 180 — halfway point
                cycle_complete = False
            if last_yaw > 5 and yaw <= 5 and not cycle_complete:
                # yaw just returned to 0 — full cycle done
                cycle_complete = True
                close_scan()
                current_filename = open_new_scan()

        last_yaw = yaw

        # Write point to current file
        if current_file:
            current_file.write(f"{pitch},{yaw},{dist}\n")
            points_in_scan += 1

        # Live feedback every 500 points
        if points_in_scan % 500 == 0:
            print(f"  {points_in_scan} points captured... (yaw={yaw:.1f}°)")

except KeyboardInterrupt:
    print("\nStopped by user.")
    close_scan()
    ser.close()
