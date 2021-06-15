using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using System.Reflection;

namespace ChrisKaczor.HomeMonitor.Power.Service
{
    public class TelemetryInitializer : ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            telemetry.Context.Cloud.RoleName = Assembly.GetEntryAssembly()?.GetName().Name;
        }
    }
}
