#include <AccelStepper.h>
#include <Servo.h>
#include <LIDARLite.h>
#include <Wire.h>   // required to use LIDARLite with I2C

// Note: LIDARLite uses SCL and SDA pins
#include <string>
#define STEP_PIN 3
#define DIR_PIN 2
#define M0_PIN 7
#define M1_PIN 6
#define M2_PIN 5

#define SERVO_PIN 8

// Step Angle 1.8 degrees per revolution
const float stepsPerRev = 200;
const float rpm = 100;
const float microSetting = 4;
// Data To Report to Unity
double yaw;
double pitch;
long long distance;


int  lidarCount = 0;
int loopCount = 0;
int servoDirection = 10;
unsigned long lastServoUpdate = 0;
const int SERVO_DELAY = 15;

LIDARLite lidar;
Servo myservo;
AccelStepper stepper(AccelStepper::DRIVER, STEP_PIN, DIR_PIN);


void setup() {
    // LiDAR + Serial setup
    Serial.begin(115200);
    delay(100);
    lidar.begin(0, true);
    lidar.configure(0);

    // Stepper motor setup
    pinMode(STEP_PIN, OUTPUT);
    pinMode(DIR_PIN, OUTPUT);

    // Set up microstepping
    pinMode(M0_PIN, OUTPUT);
    pinMode(M1_PIN, OUTPUT);
    pinMode(M2_PIN, OUTPUT);

    digitalWrite(M0_PIN, LOW);
    digitalWrite(M1_PIN, HIGH);
    digitalWrite(M2_PIN, LOW);

    float speedSps = (microSetting * stepsPerRev * rpm) / 60;
    stepper.setMaxSpeed(speedSps);
    stepper.setSpeed(speedSps);

    // Servo setup
    myservo.attach(SERVO_PIN);

}

void loop() {

    // Repeatedly poll the stepper motor
    stepper.runSpeed();
    pitch = fmod(((float)stepper.currentPosition() / (stepsPerRev * microSetting)) * 360.0, 360.0);

    // Rotate servo after 3 revs
    if(pitch < 0.01){
        loopCount ++;
    }
    else if(loopCount >= 3){
        yaw += servoDirection;
        myservo.write(yaw);
        // Reverse the direction of the servo motor every 180 degrees
        if (yaw >= 180 || yaw <= 0) {
            servoDirection = -servoDirection;
        }
        loopCount = 0;

    }
    // Read lidar and bias correct every 100 readings
    bool biasCorrect = (lidarCount % 100 == 0);
    distance = lidar.distance(biasCorrect);
    lidarCount++;
    
    
    Serial.print(pitch, 2);
    Serial.print(", ");
    Serial.print(yaw, 2);
    Serial.print(", ");
    Serial.println(distance);

}
