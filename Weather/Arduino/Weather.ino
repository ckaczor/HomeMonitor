#include <Wire.h>                               // I2C needed for sensors
#include "SparkFunMPL3115A2.h"                  // Pressure sensor - Search "SparkFun MPL3115" and install from Library Manager
#include "SparkFun_Si7021_Breakout_Library.h"   // Humidity sensor - Search "SparkFun Si7021" and install from Library Manager
#include <SoftwareSerial.h>                     // Needed for GPS
#include <TinyGPS++.h>                          // GPS parsing - Available from https://github.com/mikalhart/TinyGPSPlus

static const int RXPin = 5;                     // GPS is attached to pin 5 (RX into GPS)
static const int TXPin = 4;                     // GPS is attached to pin 4 (TX from GPS)

TinyGPSPlus     gps;                            // GPS module
SoftwareSerial  ss(RXPin, TXPin);               // Software serial port for GPS
MPL3115A2       pressureSensor;                 // Instance of the pressure sensor
Weather         humiditySensor;                 // Instance of the humidity sensor

// Digital I/O pins
const byte WSPEED = 3;                          // Wind speed switch
const byte RAIN = 2;                            // Rain switch
const byte STAT1 = 7;                           // Blue status light
const byte STAT2 = 8;                           // Green status light
const byte GPS_PWRCTL = 6;                      // Pulling this pin low puts GPS to sleep but maintains RTC and RAM

// Analog I/O pins
const byte REFERENCE_3V3 = A3;                  // 3.3V reference
const byte LIGHT = A1;                          // Light level
const byte BATT = A2;                           // Battery level
const byte WDIR = A0;                           // Wind direction

//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
// Global Variables
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

long    lastSecond;                 // The millis counter to see when a second rolls by
long    lastWindCheck = 0;          // Time of the last wind check
int     winddir = 0;                // Instantaneous wind direction [0-360]
float   windspeedmph = 0;           // Instantaneous wind speed [mph]
float   humidity = 0;               // Instantaneous humidity [%]
float   tempH = 0;                  // Instantaneous temperature from humidity sensor [F]
float   tempP = 0;                  // Instantaneous temperature from pressure sensor [F]
float   pressure = 0;               // Instantaneous pressure [pascals]
float   rain = 0;                   // Instantaneous rain [inches]

float   batt_lvl = 0;               // Battery level [Analog value from 0 to 1023]
float   light_lvl = 0;              // Light level [Analog value from 0 to 1023]

// volatiles are subject to modification by IRQs
volatile long lastRainIRQ = 0;
volatile byte rainClicks = 0;

volatile long lastWindIRQ = 0;
volatile byte windClicks = 0;

//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
//Interrupt routines (these are called by the hardware interrupts, not by the main code)
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

void rainIRQ()
{
  // Count rain gauge bucket tips as they occur
  // Activated by the magnet and reed switch in the rain gauge, attached to input D2

  long timeNow = millis();

  if (timeNow - lastRainIRQ > 10)   // Ignore switch-bounce glitches less than 10ms after the reed switch closes
  {
    lastRainIRQ = timeNow;          // Update to the current time
    rainClicks++;                   // Each click is 0.011" of water
  }
}

void wspeedIRQ()
{
  // Activated by the magnet in the anemometer (2 ticks per rotation), attached to input D3

  long timeNow = millis();

  if (timeNow - lastWindIRQ > 10)   // Ignore switch-bounce glitches less than 10ms (142MPH max reading) after the reed switch closes
  {
    lastWindIRQ = timeNow;          // Update to the current time
    windClicks++;                   // There is 1.492MPH for each click per second
  }
}

void setup()
{
  Serial.begin(9600);

  Serial.println("Board starting");

  ss.begin(9600); //Begin listening to GPS over software serial at 9600. This should be the default baud of the module.

  pinMode(STAT1, OUTPUT); //Status LED Blue
  pinMode(STAT2, OUTPUT); //Status LED Green

  pinMode(GPS_PWRCTL, OUTPUT);
  digitalWrite(GPS_PWRCTL, HIGH); //Pulling this pin low puts GPS to sleep but maintains RTC and RAM

  pinMode(WSPEED, INPUT_PULLUP); // input from wind meters windspeed sensor
  pinMode(RAIN, INPUT_PULLUP); // input from wind meters rain gauge sensor

  pinMode(REFERENCE_3V3, INPUT);
  pinMode(LIGHT, INPUT);

  //Configure the pressure sensor
  pressureSensor.begin(); // Get sensor online
  pressureSensor.setModeBarometer(); // Measure pressure in Pascals from 20 to 110 kPa
  pressureSensor.setOversampleRate(7); // Set Oversample to the recommended 128
  pressureSensor.enableEventFlags(); // Enable all three pressure and temp event flags

  //Configure the humidity sensor
  humiditySensor.begin();

  lastSecond = millis();

  // attach external interrupt pins to IRQ functions
  attachInterrupt(0, rainIRQ, FALLING);
  attachInterrupt(1, wspeedIRQ, FALLING);

  // turn on interrupts
  interrupts();

  Serial.println("Board ready");
}

void loop()
{
  //Keep track of which minute it is
  if (millis() - lastSecond >= 1000)
  {
    digitalWrite(STAT1, HIGH); //Blink stat LED

    lastSecond += 1000;

    //Go calc all the various sensors
    calcWeather(); 

    //Report all readings every second
    printWeather();

    digitalWrite(STAT1, LOW); //Turn off stat LED
  }

  smartdelay(800); //Wait 1 second, and gather GPS data
}

//While we delay for a given amount of time, gather GPS data
static void smartdelay(unsigned long ms)
{
  unsigned long start = millis();
  do
  {
    while (ss.available())
      gps.encode(ss.read());
  } while (millis() - start < ms);
}


//Calculates each of the variables that wunderground is expecting
void calcWeather()
{
    //Calc the wind speed and direction every second for 120 second to get 2 minute average
    float currentSpeed = get_wind_speed();
    windspeedmph = currentSpeed; //update global variable for windspeed when using the printWeather() function
    //float currentSpeed = random(5); //For testing
    int currentDirection = get_wind_direction();

  //Calc winddir
  winddir = get_wind_direction();

  //Calc windspeed
  //windspeedmph = get_wind_speed(); //This is calculated in the main loop on line 196

  //Calc humidity
  humidity = humiditySensor.getRH();
  tempH = humiditySensor.readTempF();

  //Calc tempf from pressure sensor
  tempP = pressureSensor.readTempF();

  //Calc pressure
  pressure = pressureSensor.readPressure();

  //Calc light level
  light_lvl = get_light_level();

  //Calc battery level
  batt_lvl = get_battery_level();

  rain = rainClicks * 0.011;
  rainClicks = 0;
}

//Returns the voltage of the light sensor based on the 3.3V rail
//This allows us to ignore what VCC might be (an Arduino plugged into USB has VCC of 4.5 to 5.2V)
float get_light_level()
{
  float operatingVoltage = analogRead(REFERENCE_3V3);

  float lightSensor = analogRead(LIGHT);

  operatingVoltage = 3.3 / operatingVoltage; //The reference voltage is 3.3V

  lightSensor = operatingVoltage * lightSensor;

  return (lightSensor);
}

//Returns the voltage of the raw pin based on the 3.3V rail
//This allows us to ignore what VCC might be (an Arduino plugged into USB has VCC of 4.5 to 5.2V)
//Battery level is connected to the RAW pin on Arduino and is fed through two 5% resistors:
//3.9K on the high side (R1), and 1K on the low side (R2)
float get_battery_level()
{
  float operatingVoltage = analogRead(REFERENCE_3V3);

  float rawVoltage = analogRead(BATT);

  operatingVoltage = 3.30 / operatingVoltage; //The reference voltage is 3.3V

  rawVoltage = operatingVoltage * rawVoltage; //Convert the 0 to 1023 int to actual voltage on BATT pin

  rawVoltage *= 4.90; //(3.9k+1k)/1k - multiple BATT voltage by the voltage divider to get actual system voltage

  return (rawVoltage);
}

//Returns the instataneous wind speed
float get_wind_speed()
{
  float deltaTime = millis() - lastWindCheck; //750ms

  deltaTime /= 1000.0; //Covert to seconds

  float windSpeed = (float)windClicks / deltaTime; //3 / 0.750s = 4

  windClicks = 0; //Reset and start watching for new wind
  lastWindCheck = millis();

  windSpeed *= 1.492; //4 * 1.492 = 5.968MPH

  /* Serial.println();
    Serial.print("Windspeed:");
    Serial.println(windSpeed);*/

  return (windSpeed);
}

//Read the wind direction sensor, return heading in degrees
int get_wind_direction()
{
  unsigned int adc;

  adc = analogRead(WDIR); // get the current reading from the sensor

  // The following table is ADC readings for the wind direction sensor output, sorted from low to high.
  // Each threshold is the midpoint between adjacent headings. The output is degrees for that ADC reading.
  // Note that these are not in compass degree order! See Weather Meters datasheet for more information.

  if (adc < 380) return (113);
  if (adc < 393) return (68);
  if (adc < 414) return (90);
  if (adc < 456) return (158);
  if (adc < 508) return (135);
  if (adc < 551) return (203);
  if (adc < 615) return (180);
  if (adc < 680) return (23);
  if (adc < 746) return (45);
  if (adc < 801) return (248);
  if (adc < 833) return (225);
  if (adc < 878) return (338);
  if (adc < 913) return (0);
  if (adc < 940) return (293);
  if (adc < 967) return (315);
  if (adc < 990) return (270);
  return (-1); // error, disconnected?
}


//Prints the various variables directly to the port
//I don't like the way this function is written but Arduino doesn't support floats under sprintf
void printWeather()
{
  //Serial.println();
  Serial.print("$,winddir=");
  Serial.print(winddir);
  Serial.print(",windspeedmph=");
  Serial.print(windspeedmph, 1);
  Serial.print(",humidity=");
  Serial.print(humidity, 1);
  Serial.print(",tempH=");
  Serial.print(tempH, 1);
  Serial.print(",tempP=");
  Serial.print(tempP, 1);
  Serial.print(",rain=");
  Serial.print(rain, 3);
  Serial.print(",pressure=");
  Serial.print(pressure, 2);
  Serial.print(",batt_lvl=");
  Serial.print(batt_lvl, 2);
  Serial.print(",light_lvl=");
  Serial.print(light_lvl, 2);

  Serial.print(",lat=");
  Serial.print(gps.location.lat(), 6);
  Serial.print(",lng=");
  Serial.print(gps.location.lng(), 6);
  Serial.print(",altitude=");
  Serial.print(gps.altitude.meters());
  Serial.print(",sats=");
  Serial.print(gps.satellites.value());

  char sz[32];
  Serial.print(",date=");
  sprintf(sz, "%02d/%02d/%02d", gps.date.month(), gps.date.day(), gps.date.year());
  Serial.print(sz);

  Serial.print(",time=");
  sprintf(sz, "%02d:%02d:%02d", gps.time.hour(), gps.time.minute(), gps.time.second());
  Serial.print(sz);

  Serial.print(",");
  Serial.println("#");

}


