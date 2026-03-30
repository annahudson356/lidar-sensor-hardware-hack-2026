# bridge.py
import serial
import socket
import json
import math

SERIAL_PORT = 'COM3'   # or /dev/ttyUSB0 on Linux/Mac
BAUD = 115200
UDP_IP = '127.0.0.1'
UDP_PORT = 5005

ser = serial.Serial(SERIAL_PORT, BAUD, timeout=1)
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

print("Bridge running — streaming to Unity...")

while True:
    line = ser.readline().decode('utf-8', errors='ignore').strip()
    if not line:
        continue
    try:
        h_deg, v_deg, dist_cm = map(float, line.split(','))
        if dist_cm <= 0 or dist_cm > 1200:  # filter bad readings
            continue

        # Spherical → Cartesian
        h_rad = math.radians(h_deg)
        v_rad = math.radians(v_deg)
        dist_m = dist_cm / 100.0

        x = dist_m * math.cos(v_rad) * math.sin(h_rad)
        y = dist_m * math.sin(v_rad)
        z = dist_m * math.cos(v_rad) * math.cos(h_rad)

        packet = json.dumps({"x": round(x, 4),
                              "y": round(y, 4),
                              "z": round(z, 4),
                              "dist": round(dist_m, 4)})
        sock.sendto(packet.encode(), (UDP_IP, UDP_PORT))

    except ValueError:
        pass