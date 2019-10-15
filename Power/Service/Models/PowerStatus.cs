using JetBrains.Annotations;

namespace ChrisKaczor.HomeMonitor.Power.Service.Models
{
    [PublicAPI]
    public class PowerStatus
    {
        public long Generation { get; set; }
        public long Consumption { get; set; }
    }
}
