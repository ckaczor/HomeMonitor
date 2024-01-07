#!/usr/bin/python
# -*- coding: utf-8 -*-

import config
import RPi.GPIO as GPIO
import time
import paho.mqtt.client as mqtt

from datetime import datetime as dt
from paho.mqtt.client import connack_string as ack
from paho.mqtt.properties import Properties
from paho.mqtt.packettypes import PacketTypes


def log(message=""):
    print(dt.now().strftime("%H:%M:%S.%f")[:-2] + " " + message)


def on_connect(client, userdata, flags, rc, v5config=None):
    log("Connection returned result: " + ack(rc))


def on_publish(client, userdata, mid, tmp=None):
    log("Published message id: " + str(mid))


log("Config:")

for device in config.devices:
    log("  pin %s: %s" % (device.pin, device.name))

log()

client = mqtt.Client(client_id="device_status_sender",
                     transport="tcp",
                     protocol=mqtt.MQTTv5)


properties = Properties(PacketTypes.CONNECT)
properties.SessionExpiryInterval = 30 * 60

client.connect("172.23.10.3", 1883, 60,
               clean_start=mqtt.MQTT_CLEAN_START_FIRST_ONLY, properties=properties)

client.on_connect = on_connect
client.on_publish = on_publish

client.loop_start()

GPIO.setmode(GPIO.BCM)

for device in config.devices:
    log("Setting up pin %s" % (device.pin))
    GPIO.setup(device.pin, GPIO.IN, pull_up_down=GPIO.PUD_DOWN)

log()

while True:
    for device in config.devices:
        pin_status = GPIO.input(device.pin)

        if pin_status != device.last_status:
            log("pin %s: %s" % (device.pin, pin_status))

            device.last_status = pin_status

            info = client.publish("device-status/" % (device.name), str(pin_status), 1)

            info.wait_for_publish()

    time.sleep(0.25)

GPIO.cleanup()
