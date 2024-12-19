using JetBrains.Annotations;

namespace ChrisKaczor.HomeMonitor.Calendar.Service.Models.NationalDays;

[PublicAPI]
public class Response
{
    public int Code { get; set; }
    public Meta Meta { get; set; } = new();
    public IEnumerable<Entry> Data { get; set; } = Array.Empty<Entry>();
}