using ChrisKaczor.HomeMonitor.Environment.Service.Data;
using ChrisKaczor.HomeMonitor.Environment.Service.Models.Device;
using Microsoft.AspNetCore.Mvc;

namespace ChrisKaczor.HomeMonitor.Environment.Service.Controllers;

[Route("[controller]")]
[ApiController]
public class DeviceController(Database database, IConfiguration configuration) : ControllerBase
{
    [HttpGet()]
    public async Task<ActionResult<List<Device>>> GetDevices()
    {
        return (await database.GetDevicesAsync()).ToList();
    }

    [HttpGet("{name}")]
    public async Task<ActionResult<Device>> GetDevice(string name)
    {
        var device = await database.GetDeviceAsync(name);

        if (device == null)
            return NotFound();

        return device;
    }

    [HttpPost()]
    public async Task<ActionResult> AddDevice(Device device)
    {
        HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader); 

        if (authorizationHeader != "Bearer " + configuration["AuthorizationToken"])
            return Unauthorized();

        var existingDevice = await database.GetDeviceAsync(device.Name);

        if (existingDevice != null)
            return BadRequest("Device already exists");

        await database.SetDeviceLastUpdatedAsync(device.Name, device.LastUpdated);

        return Ok();
    }
}