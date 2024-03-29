import badger2040
import machine
import urequests
import ntptime
import datetime
import math
from machine import Pin, ADC

LOOP = True

WIDTH = 296
HEIGHT = 128

COUNTRY = "US"
STATE = "MA"
TIMEZONE = "America/New_York"

BASE_URL = f"http://172.23.10.3:8081/api/calendar/events/next?country={COUNTRY}&state={STATE}&timezone={TIMEZONE}"

badger = badger2040.Badger2040()

def get_battery_level():
    global battery_level
    
    Pin(25, Pin.OUT, value=1)       # Deselect Wi-Fi module
    Pin(29, Pin.IN, pull=None)      # Set VSYS ADC pin floating

    # VSYS measurement
    vsys_adc = ADC(29)
    vsys = (vsys_adc.read_u16() / 65535) * 3 * 3.3

    battery_level = vsys    

def get_data(url):
    global name, type, date, days_until, hours_until, minutes_until, is_today, response_time
        
    print(f"Requesting URL: {url}")
    response = urequests.get(url)

    json = response.json()
    print("Data obtained!")
    print(json)

    value = json["responseTime"];
    response_time = datetime.datetime.fromisoformat(value[0:19])
    
    event = json["event"]
    
    name = event["name"]
    type = event["type"]
    date = event["date"]
    is_today = event["isToday"]
    
    if event["durationUntil"] is None:
        days_until = 0
        hours_until = 0
        minutes_until = 0
    else:
        days_until = event["durationUntil"]["days"]
        hours_until = event["durationUntil"]["hours"]
        minutes_until = event["durationUntil"]["minutes"]

    response.close()

def update_display():
    badger.set_pen(15)
    badger.clear()

    badger.set_pen(0)
    badger.set_font("bitmap8")
    
    text = f"{name}"
    text_width = badger.measure_text(text, scale=3)
    badger.text(text, math.floor((WIDTH - text_width) / 2), 25, scale=3)
    
    if is_today:
        text = "Today"
    elif days_until == 0:
        text = "Tomorrow"
    else:
        text = f"{days_until} days after today"
    
    text_width = badger.measure_text(text, scale=2)    
    badger.text(text, math.floor((WIDTH - text_width) / 2), 65, scale=2)
    
    badger.text(f"Battery: {battery_level:.2f}V", 2, HEIGHT - 10, scale=1)
    
    text = f"{response_time.date()} {response_time.time()}"
    text_width = badger.measure_text(text, scale=1)
    badger.text(text, WIDTH - text_width - 2, HEIGHT - 10, scale=1)
    
    badger.update()
    
step = 0
    
while True:
    try:
        step = 0
        
        badger.connect(status_handler=None)
 
        step += 1
        
        try:
            if badger.isconnected():
                ntptime.settime()
        except Exception as e:
            print(f"{step} - {repr(e)}")
                        
        step += 1

        get_data(BASE_URL)

        step += 1

        get_battery_level()
        
        step += 1
        
        update_display()
        
        step += 1

        if LOOP:
            badger2040.sleep_for(30)
        else:
            break
    except Exception as e:
        badger.set_pen(15)
        badger.clear()

        badger.set_pen(0)
        badger.set_font("bitmap8")
        
        badger.text(f"{step} - {repr(e)}", 0, 0, WIDTH, scale=1)
                
        badger.update()
        
        if LOOP:
            badger2040.sleep_for(1)
        else:
            break
