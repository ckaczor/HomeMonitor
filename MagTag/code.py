import alarm
import board
import time
from secrets import secrets
from adafruit_datetime import datetime
from adafruit_magtag.magtag import MagTag

TIME_BETWEEN_REFRESHES = 60 * 15        # Seconds

DATA_SOURCE = "http://172.23.10.3/api/weather/readings/recent?tz=%s" % secrets["timezone"].replace("/", "%2F")
TEMPERATURE_LOCATION = ["temperature"]
HUMIDITY_LOCATION = ["humidity"]
PRESSURE_LOCATION = ["pressure"]
LIGHT_LOCATION = ["lightLevel"]
TIMESTAMP_LOCATION = ["timestamp"]

DISPLAY_WIDTH = 296
DISPLAY_HEIGHT = 128


def temperature_transform(temperatureValue):
    return "%d°" % temperatureValue


def humidity_icon_transform(dummyValue):
    return "\uf07a"


def humidity_transform(humidityValue):
    return "%d%%" % humidityValue


def pressure_icon_transform(dummyValue):
    return "\uf079"


def pressure_transform(pressureValue):
    return "%.2f\"" % (pressureValue / 3386)


def light_icon_transform(dummyValue):
    return "\uf00d"


def light_transform(lightValue):
    return "%d lx" % lightValue


def timestamp_transform(timestampValue):
    date = datetime.fromisoformat(timestampValue[:19])

    return "%04d-%02d-%02d %02d:%02d" % (date.year, date.month, date.day, date.hour, date.minute)


def battery_transform(dummyValue):
    return "Battery: %.2fV" % magtag.peripherals.battery


magtag = MagTag(
    url=DATA_SOURCE,
    json_path=(
        TEMPERATURE_LOCATION,
        HUMIDITY_LOCATION,
        HUMIDITY_LOCATION,
        PRESSURE_LOCATION,
        PRESSURE_LOCATION,
        LIGHT_LOCATION,
        LIGHT_LOCATION,
        TIMESTAMP_LOCATION,
        TIMESTAMP_LOCATION,
    ),
)

magtag.add_text(
    text_font="/fonts/SourceSansPro-Regular-80.bdf",
    text_anchor_point=(0.5, 0.5),
    text_position=(DISPLAY_WIDTH / 4 + 10, DISPLAY_HEIGHT / 2 - 15),
    text_transform=temperature_transform,
)

magtag.add_text(
    text_font="/fonts/WeatherIcons-Regular-25.bdf",
    text_anchor_point=(0.0, 0.0),
    text_position=(DISPLAY_WIDTH / 2 + 20, 10),
    text_transform=humidity_icon_transform,
)

magtag.add_text(
    text_font="/fonts/Lato-Bold-ltd-25.bdf",
    text_anchor_point=(0.0, 0.0),
    text_position=(DISPLAY_WIDTH / 2 + 50, 12),
    text_transform=humidity_transform,
)

magtag.add_text(
    text_font="/fonts/WeatherIcons-Regular-25.bdf",
    text_anchor_point=(0.0, 0.0),
    text_position=(DISPLAY_WIDTH / 2 + 20, 43),
    text_transform=pressure_icon_transform,
)

magtag.add_text(
    text_font="/fonts/Lato-Bold-ltd-25.bdf",
    text_anchor_point=(0.0, 0.0),
    text_position=(DISPLAY_WIDTH / 2 + 50, 43),
    text_transform=pressure_transform,
)

magtag.add_text(
    text_font="/fonts/WeatherIcons-Regular-25.bdf",
    text_anchor_point=(0.0, 0.0),
    text_position=(DISPLAY_WIDTH / 2 + 16, 75),
    text_transform=light_icon_transform,
)

magtag.add_text(
    text_font="/fonts/Lato-Bold-ltd-25.bdf",
    text_anchor_point=(0.0, 0.0),
    text_position=(DISPLAY_WIDTH / 2 + 50, 75),
    text_transform=light_transform,
)

magtag.add_text(
    text_position=(4, DISPLAY_HEIGHT - 4),
    text_anchor_point=(0.0, 1.0),
    text_transform=battery_transform,
)

magtag.add_text(
    text_position=(DISPLAY_WIDTH - 4, DISPLAY_HEIGHT - 4),
    text_anchor_point=(1.0, 1.0),
    text_transform=timestamp_transform,
)

try:
    magtag.network.connect()

    value = magtag.fetch()

    print("Response is", value)
except (ValueError, RuntimeError, ConnectionError, OSError) as e:
    print("Some error occurred, retrying! -", e)

# Wait for display to refresh
time.sleep(2)

# Setup timer alarm
time_alarm = alarm.time.TimeAlarm(monotonic_time=time.monotonic() + TIME_BETWEEN_REFRESHES)

# Setup button alarms
magtag.peripherals.buttons[0].deinit()

pin_alarm_a = alarm.pin.PinAlarm(pin=board.BUTTON_A, value=False, pull=True)

# Deep sleep on all alarms
alarm.exit_and_deep_sleep_until_alarms(time_alarm, pin_alarm_a)
