using JetBrains.Annotations;

namespace ChrisKaczor.HomeMonitor.Weather.Service.Models
{
    [PublicAPI]
    public class WindDirectionReadingGrouped
    {
        public int WindDirection { get; set; }

        public int Count { get; set; }
    }
}