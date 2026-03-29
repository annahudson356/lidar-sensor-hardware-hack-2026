# LiDAR Scanner
The goal of our project was to create a rotating LiDAR scanner that would take a 360 degree scan of a room.

## Components
### Firmware
In order to get the components to spin approiately, we used a 17HE15-1504S stepper motor, connected to a DRV8825 motor driver, and a servo. All of these pieces were connected
on a breadboard and controlled by an arduino (see Toolbox.ino). The Arduino takes the Pitch, Yaw, and Distance data from the LiDAR sensor and motors and outputs them through a 
Serial connection to Unity for further processing.

### Visualization
We utilized Unity to visualize the data in a meaningful way. To do this, a C# Script was used to read the data in from Serial, and another C# Script was used to render
the points on our scene. Each script was added as an asset on the main unity file. Here is an example image showing the point cloud that is created when the lidar sensor rotates around an empty area.

<img width="1565" height="885" alt="image" src="https://github.com/user-attachments/assets/74c48d45-d6ab-47bd-84a4-a90e24c303b0" />

### 3D Printed Parts
All 3D printed parts and assemblies can be found in the /3D Printed Parts directory or on Onshape at: [link]

#### Fasteners
M3-0.5x10mm (4 count)
M3-0.5 x 8mm (2 count)
M4-0.7 x 6mm (4 count)
M5-0.8 x 8mm (3 count)
M2 (2 count)

### Assembly Steps
LiDAR to Servo
Base
Using M3-0.5x10mm screws, attach Base to top of Stepper Motor
Force fit Motor Gear to axel of Stepper Motor
Attach slip ring to bottom of Base using M5-0.8 x 8mm screws
Snap fit Servo Mount Gear onto Bas

### Libraries Used
Arduino
Servo (Michael Margolis, Arduino)
AccelStepper (Michael McCauley)
LIDAR-Lite (Garmin)

## Issues Encountered

Our main issue was a tolerance defect on the slip ring. It does not hold the gear in place correctly, causing the LiDAR sensor to not move as intended.
However, the visualization works appropriately.


We ran into a few issues where the LiDAR sensor would read out garbage data if it was sitting for too long / slightly bumped. Fixing the connection of wires / limiting the amount of serial output per second fixed this issue.

# Team Members
Anna Hudson, Timothy Macias, Kalista Oberes, Ethan Toper
