using JetBrains.Annotations;
using System;

namespace ChrisKaczor.HomeMonitor.Power.Service.Models;

[PublicAPI]
public class PowerStatusGrouped
{
    public DateTimeOffset Bucket { get; set; }
    public long AverageGeneration { get; set; }
    public long AverageConsumption { get; set; }
}