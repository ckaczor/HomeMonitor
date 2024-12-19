using ChrisKaczor.HomeMonitor.Calendar.Service.Models.NationalDays;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace ChrisKaczor.HomeMonitor.Calendar.Service.Controllers;

[Route("national-days")]
[ApiController]
public class NationalDaysController(IConfiguration configuration, RestClient restClient) : ControllerBase
{
    [HttpGet("today")]
    public async Task<ActionResult<Response>> GetToday()
    {
        var restRequest = new RestRequest(configuration["Calendar:NationalDays:Url"]);
        restRequest.AddHeader("X-Api-Key", configuration["Calendar:NationalDays:Key"] ?? string.Empty);

        var response = await restClient.GetAsync<Response>(restRequest);

        return Ok(response?.Data.Where(d => d.Type == "day"));
    }
}