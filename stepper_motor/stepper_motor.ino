#include <AccelStepper.h>

#define STEP_PIN 3
#define DIR_PIN 2
#define M0_PIN 7
#define M1_PIN 6
#define M2_PIN 5

// Step Angle 1.8 degrees per revolution
const float stepsPerRev = 200;
const float rpm = 50;

AccelStepper stepper(AccelStepper::DRIVER, STEP_PIN, DIR_PIN);


void setup() {
  // put your setup code here, to run once:
  pinMode(STEP_PIN, OUTPUT);
  pinMode(DIR_PIN, OUTPUT);

  // Set up microstepping
  pinMode(M0_PIN, HIGH);
  pinMode(M1_PIN, HIGH);
  pinMode(M2_PIN, HIGH);

  float microSetting = 1;
  float speedSps = (microSetting * stepsPerRev * rpm) / 60;
  float max = (microSetting * stepsPerRev * 300) / 60;
  stepper.setMaxSpeed(max);
  stepper.setSpeed(speedSps);

}

void loop() {
  // put your main code here, to run repeatedly:
  stepper.runSpeed();

}
