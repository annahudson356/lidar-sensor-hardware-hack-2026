#include <Servo.h>


Servo myservo;
int pos = 0;
const int SERVO_PIN = 8;

void setup() {
  myservo.attach(SERVO_PIN);

}

void loop() {
  for(pos = 0; pos <= 180; pos ++){
    myservo.write(pos);
    delay(15);
  }
  for(pos = 180; pos >= 0; pos --){
    myservo.write(pos);
    delay(15);
  }

}
