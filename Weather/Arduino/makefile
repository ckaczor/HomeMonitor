weather:
	mkdir -p bin/
	arduino-cli compile -b arduino:avr:uno Weather.ino -o bin/Weather

install:
	kubectl scale --replicas=0 --namespace=home-monitor deployment/weather-serial-reader
	arduino-cli upload -b arduino:avr:uno -p /dev/ttyACM0 -i bin/Weather
	kubectl scale --replicas=1 --namespace=home-monitor deployment/weather-serial-reader

clean:
	rm -rf bin/