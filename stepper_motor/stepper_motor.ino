#include <AccelStepper.h>

#define STEP_PIN 3
#define DIR_PIN 2
#define M0_PIN 7
#define M1_PIN 6
#define M2_PIN 5

// Step Angle 1.8 degrees per revolution
const float stepsPerRev = 200;

AccelStepper stepper(AccelStepper::DRIVER, STEP_PIN, DIR_PIN);


void setup() {
  // put your setup code here, to run once:
  pinMode(STEP_PIN, OUTPUT);
  pinMode(DIR_PIN, OUTPUT);

}

void loop() {
  // put your main code here, to run repeatedly:
  digitalWrite(DIR_PIN, HIGH);

  for(int i = 0; i < stepPerRev; i++){
      digitalWrite(STEP_PIN, HIGH);
      delayMicroseconds(2000);
      digitalWrite(STEP_PIN, LOW);
      delayMicroseconds(2000);
  }
  delay(1000);

  digitalWrite(DIR_PIN, LOW);

}
