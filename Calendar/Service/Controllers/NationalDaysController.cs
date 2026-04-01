using ChrisKaczor.HomeMonitor.Calendar.Service.Models;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using DaysOfTheYear = ChrisKaczor.HomeMonitor.Calendar.Service.Models.DaysOfTheYear;
using HolidayCalendar = ChrisKaczor.HomeMonitor.Calendar.Service.Models.HolidayCalendar;

namespace ChrisKaczor.HomeMonitor.Calendar.Service.Controllers;

[Route("national-days")]
[ApiController]
public class NationalDaysController(IConfiguration configuration, RestClient restClient) : ControllerBase
{
    [HttpGet("today")]
    public async Task<ActionResult<IEnumerable<NationalDay>>> GetToday([FromQuery] string timezone = "Etc/UTC", [FromQuery] string provider = "")
    {
        if (string.IsNullOrEmpty(provider))
        {
            provider = configuration["Calendar:NationalDays:Provider"] ?? string.Empty;
        }

        var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timezone);
        var timeZoneOffset = timeZoneInfo.GetUtcOffset(DateTimeOffset.Now);

        if (provider == "DaysOfTheYear")
        {
            return Ok(await GetFromDaysOfTheYear(timeZoneOffset));
        }

        return Ok(await GetFromHolidayCalendar(timeZoneOffset));
    }

    private async Task<IEnumerable<NationalDay>?> GetFromDaysOfTheYear(TimeSpan timeZoneOffset)
    {
        var timeZoneOffsetHours = timeZoneOffset.TotalHours;

        var restRequest = new RestRequest(configuration["Calendar:DaysOfTheYear:Url"]);
        restRequest.AddHeader("X-Api-Key", configuration["Calendar:DaysOfTheYear:Key"] ?? string.Empty);
        restRequest.AddQueryParameter("timezone_offset", timeZoneOffsetHours);

        var response = await restClient.GetAsync<DaysOfTheYear.Response>(restRequest);

        var items = response?.Data.Where(d => d.Type == "day");

        var nationalDays = items?.Select(i => new NationalDay(i));

        return nationalDays;
    }

    private async Task<IEnumerable<NationalDay>?> GetFromHolidayCalendar(TimeSpan timeZoneOffset)
    {
        var now = DateTimeOffset.UtcNow.ToOffset(timeZoneOffset);
        var dateString = now.ToString("yyyy-MM-dd");

        var countries = configuration.GetSection("Calendar:HolidayCalendar:CountryCodes").Get<List<string>>() ?? [];

        var restRequest = new RestRequest(configuration["Calendar:HolidayCalendar:Url"]);
        restRequest.AddHeader("Authorization", $"Bearer {configuration["Calendar:HolidayCalendar:Key"] ?? string.Empty}");
        restRequest.AddQueryParameter("limit", 100);
        restRequest.AddQueryParameter("type", "Day");
        restRequest.AddQueryParameter("startDate", dateString);
        restRequest.AddQueryParameter("endDate", dateString);

        var response = await restClient.GetAsync<HolidayCalendar.Response>(restRequest);

        var nationalDays = response?.Data.Items.Where(i => countries.Contains(i.Country.Code)).Select(i => new NationalDay(i));

        return nationalDays;
    }
}