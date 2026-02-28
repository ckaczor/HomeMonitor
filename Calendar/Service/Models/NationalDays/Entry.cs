using JetBrains.Annotations;
using System.Text.Json.Serialization;

namespace ChrisKaczor.HomeMonitor.Calendar.Service.Models.NationalDays;

[PublicAPI]
public class Entry
{
    public string Name { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    [JsonConverter(typeof(StringOrBooleanConverter))]
    public string Excerpt { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;
}