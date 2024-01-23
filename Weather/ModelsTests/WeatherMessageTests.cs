using ChrisKaczor.HomeMonitor.Weather.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ModelsTests;

[TestClass]
public class WeatherMessageTests
{
    [TestMethod]
    public void ParseTest()
    {
        var weatherMessage = WeatherMessage.Parse("$,ws=1.80,wg=1.15,wd=180.00,r=0.01,bt=14.44,bp=1016.76,tl=597.83,st=9.68,sh=78.95,gf=1,gs=13,glt=42.764725,gln=-71.042038,ga=20.70,gth=22,gtm=20,gts=11,gdy=21,gdm=5,gdd=28,#");

        Assert.AreEqual(MessageType.Data, weatherMessage.Type);

        Assert.AreEqual(WindDirection.South, weatherMessage.WindDirection);
        Assert.AreEqual(1.80M, weatherMessage.WindSpeed);
        Assert.AreEqual(42.764725M, weatherMessage.Latitude);
        Assert.AreEqual(-71.042038M, weatherMessage.Longitude);
        Assert.AreEqual(1016.76M, weatherMessage.Pressure);
        Assert.AreEqual(14.44M, weatherMessage.PressureTemperature);
        Assert.AreEqual(0.01M, weatherMessage.Rain);
        Assert.AreEqual(9.68M, weatherMessage.HumidityTemperature);
        Assert.AreEqual(78.95M, weatherMessage.Humidity);
        Assert.AreEqual(13, weatherMessage.SatelliteCount);
        Assert.AreEqual(20.70M, weatherMessage.Altitude);
        Assert.AreEqual(1.80M, weatherMessage.WindSpeed);
        Assert.AreEqual(597.83M, weatherMessage.LightLevel);
        Assert.AreEqual(DateTimeOffset.Parse("2021-05-28 22:20:11 +00:00"), weatherMessage.GpsTimestamp);
    }
}