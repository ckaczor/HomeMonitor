weather:
	mkdir -p bin/
	arduino-cli compile --fqbn arduino:avr:uno Weather.ino --output-dir bin

install:
#	kubectl scale --replicas=0 --namespace=home-monitor deployment/weather-serial-reader
	arduino-cli upload -b arduino:avr:uno -p /dev/ttyACM0 --input-dir bin
#	kubectl scale --replicas=1 --namespace=home-monitor deployment/weather-serial-reader

clean:
	rm -rf bin/
