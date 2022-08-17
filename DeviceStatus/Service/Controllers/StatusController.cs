using Microsoft.AspNetCore.Mvc;

namespace Service.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly DeviceRepository _deviceRepository;
        public StatusController(DeviceRepository deviceRepository)
        {
            _deviceRepository = deviceRepository;
        }

        [HttpGet("recent")]
        public ActionResult<IEnumerable<Device>> GetRecent()
        {
            return _deviceRepository.Values;
        }
    }
}
