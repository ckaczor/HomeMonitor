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
    global name, type, date, days_until, hours_until, minutes_until, is_today
        
    print(f"Requesting URL: {url}")
    response = urequests.get(url)

    json = response.json()
    print("Data obtained!")
    print(json)

    name = json["name"]
    type = json["type"]
    date = json["date"]
    is_today = json["isToday"]
    
    if json["durationUntil"] is None:
        days_until = 0
        hours_until = 0
        minutes_until = 0
    else:
        days_until = json["durationUntil"]["days"]
        hours_until = json["durationUntil"]["hours"]
        minutes_until = json["durationUntil"]["minutes"]

    response.close()

def update_display():
    badger.set_pen(15)
    badger.clear()

    badger.set_pen(0)
    badger.set_font("bitmap8")
    
    text = f"{name}"
    text_width = badger.measure_text(text, scale=2)
    badger.text(text, math.floor((WIDTH - text_width) / 2), 25, scale=2)
    
    text = "Today" if is_today else f"{days_until} days {hours_until} hours"
    text_width = badger.measure_text(text, scale=2)    
    badger.text(text, math.floor((WIDTH - text_width) / 2), 65, scale=2)
    
    badger.text(f"Battery: {battery_level:.2f}V", 2, HEIGHT - 10, scale=1)
    
    text = f"{last_updated.date()} {last_updated.time()}"
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

        last_updated = datetime.datetime.now(tz=datetime.timezone.utc)            
        year = datetime.date.today().year
            
        step += 1

        get_data(f"{BASE_URL}&year={year}")

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
        
        break