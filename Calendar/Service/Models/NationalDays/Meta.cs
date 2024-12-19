using JetBrains.Annotations;
using System.Text.Json.Serialization;

namespace ChrisKaczor.HomeMonitor.Calendar.Service.Models.NationalDays;

[PublicAPI]
public class Meta
{
    [JsonPropertyName("cache_status")]
    public string CacheStatus { get; set; } = string.Empty;

    [JsonPropertyName("response_count")]
    public int ResponseCount { get; set; }

    [JsonPropertyName("request_type")]
    public string RequestType { get; set; } = string.Empty;
}