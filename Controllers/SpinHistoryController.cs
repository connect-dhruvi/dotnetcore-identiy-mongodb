using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using slotmachine_api.Helpers;
using slotmachine_api.Models;
using slotmachine_api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace slotmachine_api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class SpinHistoryController : Controller
    {
        private readonly ISpinHistoryService _spinHistoryService;

        public SpinHistoryController(ISpinHistoryService spinHistoryService)
        {
            _spinHistoryService = spinHistoryService;
        }

        [HttpGet]
        public ActionResult<List<SpinHistory>> Get() =>
            _spinHistoryService.Get();

        [HttpGet("{id:length(24)}")]
        public ActionResult<SpinHistory> Get(string id)
        {
            var book = _spinHistoryService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }
    }
}