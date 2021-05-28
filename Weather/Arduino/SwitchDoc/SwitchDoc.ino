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

// SHT31 Temp/Humidity sensor
#include "Adafruit_SHT31.h"
Adafruit_SHT31 sht31 = Adafruit_SHT31();

// GPS board
#include <Adafruit_GPS.h>
Adafruit_GPS GPS(&Wire);

elapsedMillis timeElapsed;

void setup()
{
    Serial.begin(115200);

    Serial.println("Board starting");

    randomSeed(analogRead(0));

    weatherStation.setWindMode(SDL_MODE_SAMPLE, 5.0);

    timeElapsed = ReadInterval;

    char buffer[50];

    bmp280.begin();

    tsl2591.begin();
    tsl2591.setGain(TSL2591_GAIN_LOW);
    tsl2591.setTiming(TSL2591_INTEGRATIONTIME_100MS);

    sht31.begin(0x44);

    GPS.begin(0x10);
    GPS.sendCommand(PMTK_SET_NMEA_OUTPUT_RMCGGA);
    GPS.sendCommand(PMTK_SET_NMEA_UPDATE_1HZ);
}

void loop()
{
    char c = GPS.read();

    if (GPS.newNMEAreceived())
        GPS.parse(GPS.lastNMEA());

    if (timeElapsed > ReadInterval)
    {
        timeElapsed = 0;

        float currentWindSpeed = weatherStation.current_wind_speed() / 1.6;
        float currentWindGust = weatherStation.get_wind_gust() / 1.6;
        float currentRain = weatherStation.get_current_rain_total() * 0.03937;

        float bmp280_Temperature = 0.0;
        float bmp280_Pressure = 0.0;

        bmp280_Pressure = bmp280.readPressure();
        bmp280_Temperature = bmp280.readTemperature();

        uint32_t tsl2591_Luminosity = 0;
        uint16_t tsl2591_IR = 0;
        uint16_t tsl2591_Full = 0;

        tsl2591_Luminosity = tsl2591.getFullLuminosity();
        tsl2591_IR = tsl2591_Luminosity >> 16;
        tsl2591_Full = tsl2591_Luminosity & 0xFFFF;
        float tsl2591_Lux = tsl2591.calculateLux(tsl2591_Full, tsl2591_IR);

        float sht31_Temperature = 0.0;
        float sht31_Humidity = 0.0;

        sht31_Temperature = sht31.readTemperature();
        sht31_Humidity = sht31.readHumidity();

        char returnString[200];
        returnString[0] = '\0';

        char tempString[15];

        tempString[0] = '\0';
        strcat(returnString, "ws=");
        dtostrf(currentWindSpeed, 0, 2, tempString);
        strcat(returnString, tempString);
        strcat(returnString, ",");

        tempString[0] = '\0';
        strcat(returnString, "wg=");
        dtostrf(currentWindGust, 0, 2, tempString);
        strcat(returnString, tempString);
        strcat(returnString, ",");

        tempString[0] = '\0';
        strcat(returnString, "wd=");
        dtostrf(weatherStation.current_wind_direction(), 0, 2, tempString);
        strcat(returnString, tempString);
        strcat(returnString, ",");

        tempString[0] = '\0';
        strcat(returnString, "r=");
        dtostrf(currentRain, 0, 2, tempString);
        strcat(returnString, tempString);
        strcat(returnString, ",");

        tempString[0] = '\0';
        strcat(returnString, "bt=");
        dtostrf(bmp280_Temperature, 0, 2, tempString);
        strcat(returnString, tempString);
        strcat(returnString, ",");

        tempString[0] = '\0';
        strcat(returnString, "bp=");
        dtostrf(bmp280_Pressure / 100, 0, 2, tempString);
        strcat(returnString, tempString);
        strcat(returnString, ",");
        
        tempString[0] = '\0';
        strcat(returnString, "tl=");
        if (isnanf(tsl2591_Lux) || isinff(tsl2591_Lux))
            strcat(returnString, "0.00");
        else
            dtostrf(tsl2591_Lux, 0, 2, tempString);
        strcat(returnString, tempString);
        strcat(returnString, ",");

        tempString[0] = '\0';
        strcat(returnString, "st=");
        dtostrf(sht31_Temperature, 0, 2, tempString);
        strcat(returnString, tempString);
        strcat(returnString, ",");

        tempString[0] = '\0';
        strcat(returnString, "sh=");
        dtostrf(sht31_Humidity, 0, 2, tempString);
        strcat(returnString, tempString);
        strcat(returnString, ",");

        tempString[0] = '\0';
        strcat(returnString, "gf=");
        itoa(GPS.fix, tempString, 10);
        strcat(returnString, tempString);
        strcat(returnString, ",");

        tempString[0] = '\0';
        strcat(returnString, "gs=");
        itoa(GPS.satellites, tempString, 10);
        strcat(returnString, tempString);
        strcat(returnString, ",");

        tempString[0] = '\0';
        strcat(returnString, "glt=");
        dtostrf(GPS.latitudeDegrees, 0, 6, tempString);
        strcat(returnString, tempString);
        strcat(returnString, ",");

        tempString[0] = '\0';
        strcat(returnString, "gln=");
        dtostrf(GPS.longitudeDegrees, 0, 6, tempString);
        strcat(returnString, tempString);
        strcat(returnString, ",");

        tempString[0] = '\0';
        strcat(returnString, "ga=");
        dtostrf(GPS.altitude, 0, 2, tempString);
        strcat(returnString, tempString);
        strcat(returnString, ",");

        tempString[0] = '\0';
        strcat(returnString, "gth=");
        itoa(GPS.hour, tempString, 10);
        strcat(returnString, tempString);
        strcat(returnString, ",");

        tempString[0] = '\0';
        strcat(returnString, "gtm=");
        itoa(GPS.minute, tempString, 10);
        strcat(returnString, tempString);
        strcat(returnString, ",");

        tempString[0] = '\0';
        strcat(returnString, "gts=");
        itoa(GPS.seconds, tempString, 10);
        strcat(returnString, tempString);
        strcat(returnString, ",");

        tempString[0] = '\0';
        strcat(returnString, "gdy=");
        itoa(GPS.year, tempString, 10);
        strcat(returnString, tempString);
        strcat(returnString, ",");

        tempString[0] = '\0';
        strcat(returnString, "gdm=");
        itoa(GPS.month, tempString, 10);
        strcat(returnString, tempString);
        strcat(returnString, ",");

        tempString[0] = '\0';
        strcat(returnString, "gdd=");
        itoa(GPS.day, tempString, 10);
        strcat(returnString, tempString);

        Serial.println(returnString);
    }
}
