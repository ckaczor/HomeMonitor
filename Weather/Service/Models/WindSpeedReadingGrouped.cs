using JetBrains.Annotations;
using System;

namespace ChrisKaczor.HomeMonitor.Weather.Service.Models
{
    [PublicAPI]
    public class WindSpeedReadingGrouped
    {
        public DateTimeOffset Bucket { get; set; }

        public decimal Minimum { get; set; }

        public decimal Average { get; set; }

        public decimal Maximum { get; set; }
    }
}