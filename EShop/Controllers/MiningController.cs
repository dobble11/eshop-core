using EShop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EShop.Controllers
{
    [Route("api/mining")]
    public class MiningController : Controller
    {
        private readonly EShopContext _context;
        public MiningController(EShopContext context)
        {
            _context = context;
        }

        [HttpGet]
        public object Get()
        {
            var end = DateTime.Now;
            var start = end.AddMonths(-3);

            return new
            {
                start,
                end,
                data = GetByFilter(start, end)
            };
        }

        [HttpGet("filter")]
        public object GetByFilter(DateTime? start, DateTime? end)
        {
            start = (start ?? DateTime.MinValue);
            end = (end ?? DateTime.MaxValue);

            var data1 = _context.Orders.Include(m => m.Commodity).ThenInclude(m => m.Category).Where(m => m.State == 4 && m.DateTime >= start && m.DateTime <= end).GroupBy(m => new { m.Commodity.Category.Name, m.UserId })
                .Select(m => m.Key).GroupBy(m => m.Name).Select(m => new { name = m.Key, value = m.Count() });
            var data2 = _context.Orders.Include(m => m.Commodity).ThenInclude(m => m.Category)
                 .GroupBy(m => new { m.Commodity.Price, m.Commodity.Category.Name })
                 .Select(m => new { m.Key.Price, m.Key.Name, count = m.Sum(n => n.Count) })
                 .GroupBy(m => m.Name).Select(m => new { name = m.Key, value = decimal.Round(m.Sum(n => n.Price * n.count) / m.Sum(n => n.count), 2) });

            return new
            {
                data1,
                data2
            };
        }
    }
}
