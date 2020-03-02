(function () {
    var ADDRESS = "http://home.kaczorzoo.net/api/weather/readings/recent";

    function getDataFromXML() {
        document.getElementById("temperature-value").textContent = "----";
        document.getElementById("humidity-value").textContent = "----";
        document.getElementById("pressure-value").textContent = "----";
        document.getElementById("light-value").textContent = "----";

        setTimeout(function () {
            try {
                var xmlhttp = new XMLHttpRequest();

                xmlhttp.open("GET", ADDRESS, true);
                xmlhttp.onreadystatechange = function () {
                    if (xmlhttp.readyState === 4 && xmlhttp.status === 200) {
                        var xmlDoc = xmlhttp.response;

                        var data = JSON.parse(xmlDoc);

                        document.getElementById("temperature-value").textContent = data.humidityTemperature.toFixed(1) + '°F';
                        document.getElementById("humidity-value").textContent = data.humidity.toFixed(1) + '%';
                        document.getElementById("pressure-value").textContent = (data.pressure / 33.864 / 100).toFixed(1) + '"';
                        document.getElementById("light-value").textContent = (data.lightLevel / data.batteryLevel).toFixed(1) + '%';

                        xmlhttp = null;
                    } else {
                        document.getElementById("temperature-value").textContent = "Error";
                        document.getElementById("humidity-value").textContent = "Error";
                        document.getElementById("pressure-value").textContent = "Error";
                        document.getElementById("light-value").textContent = "Error";
                    }
                };

                xmlhttp.send();
            }
            catch (e) {
                document.getElementById("temperature-value").textContent = "Error";
                document.getElementById("humidity-value").textContent = "Error";
                document.getElementById("pressure-value").textContent = "Error";
                document.getElementById("light-value").textContent = "Error";
            }
        }, 100);
    }

    function handleVisibilityChange() {
        if (document.visibilityState === 'visible') {
            getDataFromXML();
        }
    }

    function init() {
        document.getElementById("body").addEventListener("click", getDataFromXML);

        document.addEventListener("visibilitychange", handleVisibilityChange);

        getDataFromXML();
    }

    window.onload = init;
}());