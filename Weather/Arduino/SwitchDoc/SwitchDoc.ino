#include "elapsedMillis.h"

#define ReadInterval 1000

// Weather
#define WpinAnem 3
#define WpinRain 2

#define WintAnem 1
#define WintRain 0

#include "SDL_Weather_80422.h"
SDL_Weather_80422 weatherStation(WpinAnem, WpinRain, WintAnem, WintRain, A0, SDL_MODE_INTERNAL_AD);

// BMP280 Pressure sensor
#include <Adafruit_BMP280.h>
Adafruit_BMP280 bmp280;

// TSL2591 Light sensor
#include "Adafruit_TSL2591.h"
Adafruit_TSL2591 tsl2591 = Adafruit_TSL2591();

elapsedMillis timeElapsed;

bool bmp280_Found;
bool tsl2591_Found;

void setup()
{
    Serial.begin(115200);

    Serial.println("Board starting");

    bmp280_Found = false;
    tsl2591_Found = false;

    randomSeed(analogRead(0));

    weatherStation.setWindMode(SDL_MODE_SAMPLE, 5.0);

    timeElapsed = ReadInterval;

    char buffer[50];

    bmp280_Found = bmp280.begin();

    sprintf(buffer, "BMP280 present: %s", bmp280_Found ? "true" : "false");
    Serial.println(buffer);

    tsl2591_Found = tsl2591.begin();

    sprintf(buffer, "TSL2591 present: %s", tsl2591_Found ? "true" : "false");
    Serial.println(buffer);
}

void loop()
{
    if (timeElapsed > ReadInterval)
    {
        timeElapsed = 0;

        float currentWindSpeed = weatherStation.current_wind_speed() / 1.6;
        float currentWindGust = weatherStation.get_wind_gust() / 1.6;
        float currentRain = weatherStation.get_current_rain_total() * 0.03937;

        float bmp280_Temperature = 0.0;
        float bmp280_Pressure = 0.0;

        if (bmp280_Found)
        {
            bmp280_Pressure = bmp280.readPressure();
            bmp280_Temperature = bmp280.readTemperature();
        }

        uint32_t tsl2591_Luminosity = 0;
        uint16_t tsl2591_IR = 0;
        uint16_t tsl2591_Full = 0;

        if (tsl2591_Found)
        {
            tsl2591_Luminosity = tsl2591.getFullLuminosity();
            tsl2591_IR = tsl2591_Luminosity >> 16;
            tsl2591_Full = tsl2591_Luminosity & 0xFFFF;
        }

        char returnString[200];
        returnString[0] = '\0';

        char tempString[15];

        tempString[0] = '\0';
        strcat(returnString, "wind_speed=");
        dtostrf(currentWindSpeed, 0, 2, tempString);
        strcat(returnString, tempString);
        strcat(returnString, ",");

        tempString[0] = '\0';
        strcat(returnString, "wind_gust=");
        dtostrf(currentWindGust, 0, 2, tempString);
        strcat(returnString, tempString);
        strcat(returnString, ",");

        tempString[0] = '\0';
        strcat(returnString, "wind_dir=");
        dtostrf(weatherStation.current_wind_direction(), 0, 2, tempString);
        strcat(returnString, tempString);
        strcat(returnString, ",");

        tempString[0] = '\0';
        strcat(returnString, "rain=");
        dtostrf(currentRain, 0, 2, tempString);
        strcat(returnString, tempString);
        strcat(returnString, ",");

        tempString[0] = '\0';
        strcat(returnString, "internal_temp=");
        dtostrf(bmp280_Temperature, 0, 2, tempString);
        strcat(returnString, tempString);
        strcat(returnString, ",");

        tempString[0] = '\0';
        strcat(returnString, "pressure=");
        dtostrf(bmp280_Pressure / 100, 0, 2, tempString);
        strcat(returnString, tempString);
        strcat(returnString, ",");

        tempString[0] = '\0';
        strcat(returnString, "ir=");
        itoa(tsl2591_IR, tempString, 10);
        strcat(returnString, tempString);
        strcat(returnString, ",");

        tempString[0] = '\0';
        strcat(returnString, "visible=");
        itoa(tsl2591_Full - tsl2591_IR, tempString, 10);
        strcat(returnString, tempString);
        strcat(returnString, ",");

        tempString[0] = '\0';
        strcat(returnString, "lux=");
        dtostrf(tsl2591.calculateLux(tsl2591_Full, tsl2591_IR), 0, 2, tempString);
        strcat(returnString, tempString);

        Serial.println(returnString);
    }
}
