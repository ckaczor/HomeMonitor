using Microsoft.AspNetCore.Mvc;

namespace ChrisKaczor.HomeMonitor.DeviceStatus.Service.Controllers;

[Route("[controller]")]
[ApiController]
public class StatusController(DeviceRepository deviceRepository) : ControllerBase
{
    [HttpGet("recent")]
    public ActionResult<IEnumerable<Device>> GetRecent()
    {
        return deviceRepository.Values;
    }
}