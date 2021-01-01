using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using slotmachine_api.Helpers;
using slotmachine_api.Models;
using slotmachine_api.Models.Identity;
using slotmachine_api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace slotmachine_api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class SlotMachineController : Controller
    {
        private readonly ISpinHistoryService _spinHistoryService;
        private readonly ISettingsService _settingService;
        private readonly UserManager<ApplicationUser> _userManager;

        public SlotMachineController(ISpinHistoryService spinHistoryService,
            ISettingsService settingService,
            UserManager<ApplicationUser> userManager)
        {
            _spinHistoryService = spinHistoryService;
            _settingService = settingService;
            _userManager = userManager;
        }

        private (int, int[]) SpinAndGetWinningAmount(int bet)
        {
            var setting = _settingService.GetByKey(ConstantHelper.SlotMachineArraySizeKey);
            var arraySize = Convert.ToInt32(setting.Value);

            var slotValue = new int[arraySize];

            for (int i = 0; i < arraySize; i++)
                slotValue[i] = new Random().Next(0, 9);

            var points = 0;
            int win = 0;
            int? previousNumber = null;

            for (int i = 0; i < arraySize; i++)
            {
                if (previousNumber == slotValue[i] || !previousNumber.HasValue)
                    points += slotValue[i];
                else
                    break;

                previousNumber = slotValue[i];
            }
            win = points * bet;

            return (win, slotValue);
        }

        private void SaveSpinHitory(int win, int[] slotValue)
        {
            _spinHistoryService.Create(new SpinHistory
            {
                Points = win,
                SpinValue = slotValue,
                UserId = User.Identity.Name.ToString(),
                TimeStamp = DateTime.Now
            });
        }

        [HttpGet("Spin/{bet}")]
        public IActionResult Spin(int bet)
        {
            if (bet == 0)
                return BadRequest("Bet can not be 0.");

            var (win, slotValue) = SpinAndGetWinningAmount(bet);

            SaveSpinHitory(win, slotValue);

            var user = _userManager.Users.FirstOrDefault(x => x.Id == new Guid(User.Identity.Name));
            user.Points += win - bet;
            _userManager.UpdateAsync(user).Wait();

            return Ok(new { CurrentPoints = user.Points, Win = win, SpinValue = slotValue });
        }
    }
}