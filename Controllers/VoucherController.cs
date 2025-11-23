using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsStore.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SportsStore.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly StoreDbContext _ctx;
        public VoucherController(StoreDbContext ctx) { _ctx = ctx; }

        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var userId = User.Identity?.IsAuthenticated == true ? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value : null;
            var today = System.DateTime.UtcNow.Date;
            int diff = (7 + (today.DayOfWeek - System.DayOfWeek.Monday)) % 7;
            var weekStart = today.AddDays(-diff);

            var vouchers = await _ctx.Vouchers.Where(v => v.IsActive).OrderBy(v => v.Code).ToListAsync();
            var usageQuery = _ctx.VoucherUserUsages.AsQueryable();

            var list = new List<VoucherDto>();
            foreach (var v in vouchers)
            {
                int userDailyUsed = 0;
                int userWeeklyUsed = 0;
                int userDailyRemaining = v.PerUserDailyLimit > 0 ? v.PerUserDailyLimit : -1;
                int userWeeklyRemaining = v.PerUserWeeklyLimit > 0 ? v.PerUserWeeklyLimit : -1;
                if (userId != null)
                {
                    if (v.PerUserDailyLimit > 0)
                    {
                        userDailyUsed = await usageQuery.Where(u => u.UserId == userId && u.VoucherId == v.Id && u.PeriodType == "Daily" && u.PeriodStartDate == today).Select(u => u.UsageCount).FirstOrDefaultAsync();
                        userDailyRemaining = v.PerUserDailyLimit - userDailyUsed;
                    }
                    if (v.PerUserWeeklyLimit > 0)
                    {
                        userWeeklyUsed = await usageQuery.Where(u => u.UserId == userId && u.VoucherId == v.Id && u.PeriodType == "Weekly" && u.PeriodStartDate == weekStart).Select(u => u.UsageCount).FirstOrDefaultAsync();
                        userWeeklyRemaining = v.PerUserWeeklyLimit - userWeeklyUsed;
                    }
                }
                list.Add(new VoucherDto
                {
                    Code = v.Code,
                    Type = v.DiscountType.ToString(),
                    Value = v.Value,
                    Min = v.MinOrderTotal ?? 0m,
                    DailyLimit = v.DailyLimit,
                    DailyUsed = v.DailyUsedCount,
                    Remaining = v.DailyLimit > 0 ? (v.DailyLimit - v.DailyUsedCount) : -1,
                    PerUserDailyLimit = v.PerUserDailyLimit,
                    PerUserWeeklyLimit = v.PerUserWeeklyLimit,
                    UserDailyUsed = userDailyUsed,
                    UserWeeklyUsed = userWeeklyUsed,
                    UserDailyRemaining = userDailyRemaining,
                    UserWeeklyRemaining = userWeeklyRemaining
                });
            }
            return Ok(list);
        }

        public class VoucherDto
        {
            public string Code { get; set; } = string.Empty;
            public string Type { get; set; } = string.Empty; // Percentage / FixedAmount
            public decimal Value { get; set; }
            public decimal Min { get; set; }
            public int DailyLimit { get; set; }
            public int DailyUsed { get; set; }
            public int Remaining { get; set; }
            public int PerUserDailyLimit { get; set; }
            public int PerUserWeeklyLimit { get; set; }
            public int UserDailyUsed { get; set; }
            public int UserWeeklyUsed { get; set; }
            public int UserDailyRemaining { get; set; }
            public int UserWeeklyRemaining { get; set; }
        }
    }
}
