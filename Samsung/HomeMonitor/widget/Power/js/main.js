(function () {
    var ADDRESS = "http://home.kaczorzoo.net/api/power/status/recent";

    function getDataFromXML() {
        document.getElementById("generation-value").textContent = "----";
        document.getElementById("consumption-value").textContent = "----";

        setTimeout(function () {
            try {
                var xmlhttp = new XMLHttpRequest();

                xmlhttp.open("GET", ADDRESS, true);
                xmlhttp.onreadystatechange = function () {
                    if (xmlhttp.readyState === 4 && xmlhttp.status === 200) {
                        var xmlDoc = xmlhttp.response;

                        var data = JSON.parse(xmlDoc);

                        document.getElementById("generation-value").textContent = (data.generation < 0 ? 0 : data.generation) + ' W';
                        document.getElementById("consumption-value").textContent = data.consumption + ' W';

                        xmlhttp = null;
                    } else {
                        document.getElementById("generation-value").textContent = "Error";
                        document.getElementById("consumption-value").textContent = "Error";
                    }
                };

                xmlhttp.send();
            }
            catch (e) {
                document.getElementById("generation-value").textContent = "Error";
                document.getElementById("consumption-value").textContent = "Error";
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