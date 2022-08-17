#include <WiFiNINA.h>
#include <ArduinoMqttClient.h>

#include "arduino_secrets.h"

char ssid[] = SECRET_SSID;
char pass[] = SECRET_PASS;

WiFiClient wifiClient;
MqttClient mqttClient(wifiClient);

const char broker[] = "172.23.10.3";
int port = 1883;

const long interval = 500;  // Interval for sending messages (milliseconds)
unsigned long previousMilliseconds = 0;

int washerPin = 0;
int dryerPin = 1;

int lastWasherValue = -1;
int lastDryerValue = -1;

const char washerTopic[] = "washer";
const char dryerTopic[] = "dryer";

void connectNetwork() {
  Serial.print("Attempting to connect to WPA SSID: ");
  Serial.println(ssid);

  while (WiFi.begin(ssid, pass) != WL_CONNECTED) {
    Serial.print(".");
    delay(5000);
  }

  Serial.println("Connected to the network");
  Serial.println();
}

bool checkNetwork() {
  if (!WiFi.status() == WL_DISCONNECTED) {
    WiFi.disconnect();

    Serial.print("Attempting to reconnect to WPA SSID: ");
    Serial.println(ssid);

    if (!WiFi.begin(ssid, pass) != WL_CONNECTED) {
      Serial.println("Network reconnection failed");
      return false;
    }

    Serial.println("Network reconnected");
    return true;
  }

  return true;
}

void connectBroker() {
  Serial.print("Attempting to connect to the MQTT broker: ");
  Serial.println(broker);

  while (!mqttClient.connect(broker, port)) {
    Serial.print(".");
    delay(5000);
  }

  Serial.print("Connected to the MQTT broker: ");
  Serial.println(broker);
}

bool checkBroker() {
  if (!mqttClient.connected()) {
    Serial.print("Attempting to reconnect to the MQTT broker: ");
    Serial.println(broker);

    if (!mqttClient.connect(broker, port)) {
      Serial.println("Broker reconnection failed");
      return false;
    }

    Serial.println("Broker reconnected");

    outputValue(washerTopic, lastWasherValue);
    outputValue(dryerTopic, lastDryerValue);

    return true;
  }

  return true;
}

void setupDevices() {
  pinMode(washerPin, INPUT_PULLUP);
  pinMode(dryerPin, INPUT_PULLUP);

  lastWasherValue = digitalRead(washerPin);
  lastDryerValue = digitalRead(dryerPin);

  outputValue(washerTopic, lastWasherValue);
  outputValue(dryerTopic, lastDryerValue);
}

void outputValue(char topic[], int value) {
  Serial.print("Sending message to topic: ");
  Serial.print(topic);
  Serial.print(" ");
  Serial.println(value == 1 ? 0 : 1);

  mqttClient.beginMessage(topic);
  mqttClient.print(value == 1 ? 0 : 1);
  mqttClient.endMessage();
}

void setup() {
  Serial.begin(9600);

  while (!Serial) {
    ;  // wait for serial port to connect. Needed for native USB port only
  }

  connectNetwork();
  connectBroker();

  setupDevices();

  Serial.println();
}

void loop() {
  mqttClient.poll();

  unsigned long currentMilliseconds = millis();

  if (currentMilliseconds - previousMilliseconds >= interval) {
    previousMilliseconds = currentMilliseconds;

    if (!checkNetwork()) {
      return;
    }

    if (!checkBroker()) {
      return;
    }

    int washerValue = digitalRead(washerPin);

    if (washerValue != lastWasherValue) {
      lastWasherValue = washerValue;

      outputValue(washerTopic, washerValue);
    }

    int dryerValue = digitalRead(dryerPin);

    if (dryerValue != lastDryerValue) {
      lastDryerValue = dryerValue;

      outputValue(dryerTopic, dryerValue);
    }
  }
}