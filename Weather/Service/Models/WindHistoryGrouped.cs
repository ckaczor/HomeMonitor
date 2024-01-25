using JetBrains.Annotations;
using System;

namespace ChrisKaczor.HomeMonitor.Weather.Service.Models;

[PublicAPI]
public class WindHistoryGrouped
{
    public DateTimeOffset Bucket { get; set; }

    public decimal MinimumSpeed { get; set; }

    public decimal AverageSpeed { get; set; }

    public decimal MaximumSpeed { get; set; }

    public decimal AverageDirection { get; set; }
}