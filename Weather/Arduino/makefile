weather:
	mkdir -p bin/
	arduino-cli compile -b arduino:avr:uno Weather.ino -o bin/Weather

install:
	sudo service weather stop
	arduino-cli upload -b arduino:avr:uno -p /dev/ttyACM0 -i bin/Weather
	sudo service weather start

clean:
	rm -rf bin/