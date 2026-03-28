#include <Wire.h>
#include <LIDARLite.h>

LIDARLite lidar;

void setup() {
  Serial.begin(115200);
  lidar.begin(0, true); 
  lidar.configure(0);
}

void loop() {
  int distance = lidar.distance(); 
  // Read the pitch and the yaw of the servo... I think we need to integrate the other code first
  //int pitch
  //int yaw

  Serial.print(distance); Serial.print(",");
  Serial.print(pitch);    Serial.print(",");
  Serial.println(yaw);

  delay(10);
}