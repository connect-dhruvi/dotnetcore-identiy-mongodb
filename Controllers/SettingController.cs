using Microsoft.AspNetCore.Mvc;
using slotmachine_api.Models;
using slotmachine_api.Services;
using System.Collections;
using System.Collections.Generic;

namespace slotmachine_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingController : Controller
    {
        private readonly ISettingsService _settingsService;

        public SettingController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        [HttpGet]
        public  ActionResult<List<Setting>> Get()
        {
            return _settingsService.Get();
        }

        [HttpGet("{id:length(24)}")]
        public ActionResult<Setting> Get(string id)
        {
            var settings = _settingsService.Get(id);

            if (settings == null)
            {
                return NotFound();
            }

            return settings;
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Setting settingsIn)
        {
            var settings = _settingsService.Get(id);

            if (settings == null)
                return NotFound();

            _settingsService.Update(id, settingsIn);

            return NoContent();
        }
    }
}
   