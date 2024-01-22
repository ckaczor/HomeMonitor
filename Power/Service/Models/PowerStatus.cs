using JetBrains.Annotations;
using System;

namespace ChrisKaczor.HomeMonitor.Power.Service.Models;

[PublicAPI]
public class PowerStatus
{
    public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;
    public long Generation { get; set; }
    public long Consumption { get; set; }
}