weather:
	mkdir -p bin/
	arduino-cli compile --fqbn arduino:avr:uno --output-dir bin

install:
#	kubectl scale --replicas=0 --namespace=home-monitor deployment/weather-serial-reader
	arduino-cli upload --fqbn arduino:avr:uno -p /dev/ttyACM0 --input-dir bin 
#	kubectl scale --replicas=1 --namespace=home-monitor deployment/weather-serial-reader

debug:
	minicom -b 115200 -D /dev/ttyACM0
	
clean:
	rm -rf bin/
