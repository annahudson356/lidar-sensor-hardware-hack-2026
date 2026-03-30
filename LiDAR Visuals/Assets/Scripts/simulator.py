# simulator.py
import socket
import json
import math
import time
import random

UDP_IP   = '127.0.0.1'
UDP_PORT = 5005

sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

print("Simulator running — sending fake scan data to Unity...")

# Scan parameters (match your real scanner settings)
H_STEP    = 3    # horizontal degrees per step
V_STEPS   = 8    # number of vertical positions
V_RANGE   = 90   # total vertical degrees

def send_point(h_deg, v_deg, dist_m):
    h_rad = math.radians(h_deg)
    v_rad = math.radians(v_deg)

    x = dist_m * math.cos(v_rad) * math.sin(h_rad)
    y = dist_m * math.sin(v_rad)
    z = dist_m * math.cos(v_rad) * math.cos(h_rad)

    packet = json.dumps({
        "x": round(x, 4),
        "y": round(y, 4),
        "z": round(z, 4),
        "dist": round(dist_m, 4)
    })
    sock.sendto(packet.encode(), (UDP_IP, UDP_PORT))

def room_distance(h_deg, v_deg):
    """
    Simulates a simple rectangular room.
    Returns a distance in metres with a little noise added.
    """
    h_rad = math.radians(h_deg)
    v_rad = math.radians(v_deg)

    # Room half-dimensions in metres
    room_w = 3.0   # left/right walls
    room_d = 4.0   # front/back walls
    room_h = 2.5   # ceiling/floor

    dx = math.cos(v_rad) * math.sin(h_rad)
    dy = math.sin(v_rad)
    dz = math.cos(v_rad) * math.cos(h_rad)

    distances = []
    if abs(dx) > 0.001:
        distances.append(abs(room_w / dx))
    if abs(dy) > 0.001:
        distances.append(abs(room_h / dy))
    if abs(dz) > 0.001:
        distances.append(abs(room_d / dz))

    dist = min(distances) if distances else 2.0

    # Add small random noise to mimic sensor jitter
    dist += random.uniform(-0.02, 0.02)

    return max(0.1, min(dist, 40.0))

scan_count = 0

while True:
    scan_count += 1
    print(f"Scan #{scan_count} starting...")

    v_step_size = V_RANGE / V_STEPS

    for v_idx in range(V_STEPS):
        v_deg = v_idx * v_step_size

        # Boustrophedon sweep (same as real firmware)
        if v_idx % 2 == 0:
            h_range = range(0, 181, H_STEP)
        else:
            h_range = range(180, -1, -H_STEP)

        for h_deg in h_range:
            dist = room_distance(h_deg, v_deg)
            send_point(h_deg, v_deg, dist)

            # Simulate the settle delay from the real scanner
            time.sleep(0.01)

    print(f"Scan #{scan_count} complete. Starting next scan in 2 seconds...")
    time.sleep(2)
