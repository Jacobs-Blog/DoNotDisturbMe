#include <Servo.h>

String inputString = "";         // a String to hold incoming data
boolean stringComplete = false;  // whether the string is complete
int lastAngle;

Servo myServo;

void setup() 
{
  myServo.attach(9);
  myServo.write(0);
  Serial.begin(9600);
  inputString.reserve(200);
}

void loop() 
{
  if (stringComplete) 
  {
    if(inputString == "angle")
    {
      Serial.print(lastAngle);
    }
    else
    {
      Serial.print(inputString);
      
      lastAngle = inputString.toInt();
      myServo.write(lastAngle);
    }
    
    
    // clear the string:
    inputString = "";
    stringComplete = false;
  }
}

void serialEvent() 
{
  while (Serial.available()) 
  {
    char inChar = (char)Serial.read();
    inputString += inChar;
    if (inChar == '\n') 
    {
      stringComplete = true;
    }
  }
}
