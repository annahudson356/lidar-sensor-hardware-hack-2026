#include <AccelStepper.h>
#include <Servo.h>

#define STEP_PIN 3
#define DIR_PIN 2
#define M0_PIN 7
#define M1_PIN 6
#define M2_PIN 5

#define SERVO_PIN = 8;

// Step Angle 1.8 degrees per revolution
const float stepsPerRev = 200;
const float rpm = 1000;

AccelStepper stepper(AccelStepper::DRIVER, STEP_PIN, DIR_PIN);

Servo myservo;
int pos = 0;
int servoDirection = 1;
unsigned long lastServoUpdate = 0;
const int SERVO_DELAY = 15;


void setup() {
    // put your setup code here, to run once:
    pinMode(STEP_PIN, OUTPUT);
    pinMode(DIR_PIN, OUTPUT);

    // Set up microstepping
    pinMode(M0_PIN, OUTPUT);
    pinMode(M1_PIN, OUTPUT);
    pinMode(M2_PIN, OUTPUT);

    digitalWrite(M0_PIN, HIGH);
    digitalWrite(M1_PIN, HIGH);
    digitalWrite(M2_PIN, HIGH);

    float microSetting = 1;
    float speedSps = (microSetting * stepsPerRev * rpm) / 60;
    float max = (microSetting * stepsPerRev * 300) / 60;
    stepper.setMaxSpeed(max);
    stepper.setSpeed(speedSps);

    // Set up servo
    myservo.attach(SERVO_PIN);

}

void loop() {
    // put your main code here, to run repeatedly:
    stepper.runSpeed();

    unsigned long now = millis();
    if (now - lastServoUpdate >= SERVO_DELAY) {
        lastServoUpdate = now;
        pos += servoDirection;
        myservo.write(pos);
        if (pos >= 180 || pos <= 0) {
            servoDirection = -servoDirection;
        }
    }

}
