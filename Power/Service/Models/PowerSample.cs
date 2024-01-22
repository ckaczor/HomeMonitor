using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ChrisKaczor.HomeMonitor.Power.Service.Models;

[PublicAPI]
public class PowerSample
{
    [JsonPropertyName("sensorId")]
    public string SensorId { get; set; }

    [JsonPropertyName("timestamp")]
    public DateTimeOffset Timestamp { get; set; }

    [JsonPropertyName("channels")]
    public PowerChannel[] Channels { get; set; }

    [JsonPropertyName("cts")]
    public Dictionary<string, double>[] CurrentTransformers { get; set; }
}