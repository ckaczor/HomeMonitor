class Device:
    last_status = 0

    def __init__(self, name, pin):
        self.name = name
        self.pin = pin