using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EShop.Models;
using EShop.Data;
using EShop.Utils;
using EShop.Filters;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace EShop.Controllers
{
    [Route("api/commodity")]
    public class CommodityController : Controller
    {
        private readonly EShopContext _context;
        private readonly IHostingEnvironment _env;

        public CommodityController(EShopContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        public object Get()
        {
            var token = Request.Cookies["token"];
            IQueryable<object> data3 = null;

            if (token != null)
            {
                var userid = Guid.Parse(Base64Helper.Decode(token));
                var data = _context.BrowseHistories.Where(m => m.UserId == userid).OrderByDescending(m => new { m.Count, m.Datetime }).Take(4);
                data3 = _context.Commodities.Where(m => data.Any(n => n.CommodityId == m.Id))
                    .Select(m => new { id = m.Id, name = m.Name, price = m.Price, url = m.Thumbs.First().Url, storeName = m.Store.Name, storeId = m.Store.Id, sv = m.Total - m.Surplus });
            }

            var data1 = _context.Commodities.Include(m => m.Thumbs).Include(m => m.Store).OrderByDescending(m => m.DateTime)
                .Take(4).Select(m => new { id = m.Id, name = m.Name, price = m.Price, url = m.Thumbs.First().Url, storeName = m.Store.Name, storeId = m.Store.Id, sv = m.Total - m.Surplus });

            var data2 = _context.Commodities.Include(m => m.Thumbs).Include(m => m.Store).Include(m => m.BrowseHistories).OrderByDescending(m => m.BrowseHistories.Sum(n => n.Count))
                .Take(4).Select(m => new { id = m.Id, name = m.Name, price = m.Price, url = m.Thumbs.First().Url, storeName = m.Store.Name, storeId = m.Store.Id, sv = m.Total - m.Surplus });

            return new { data1, data2, data3 };
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var commodity = await _context.Commodities.Include(m => m.Thumbs).Include(m => m.Orders).SingleOrDefaultAsync(m => m.Id == id);

            if (commodity == null)
            {
                return NotFound();
            }
            var values = Request.Headers["Authorization"];
            if (values.Count != 0)
            {
                var token = Base64Helper.Decode(values[0].Substring(6));
                var userid = Guid.Parse(token);

                var item = _context.BrowseHistories.SingleOrDefault(m => m.UserId == userid && m.CommodityId == commodity.Id && m.Datetime.Date == DateTime.Now.Date);
                if (item != null)
                {
                    item.Count += 1;
                }
                else
                {
                    _context.BrowseHistories.Add(new BrowseHistory { UserId = userid, CommodityId = commodity.Id, Count = 1 });
                }

                await _context.SaveChangesAsync();
            }

            decimal? rate = null;
            var orders = commodity.Orders.Where(m => m.State == 4);

            if (orders.Count() != 0)
            {
                rate = decimal.Round((decimal)orders.Sum(m => m.Rate) / orders.Count(), 1);
            }
            return Ok(new { id = commodity.Id, name = commodity.Name, description = commodity.Description, thumbs = commodity.Thumbs.Select(m => m.Url), total = commodity.Total, surplus = commodity.Surplus, price = commodity.Price, rate });
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Commodity commodity)
        {
            var target = _context.Commodities.Single(m => m.Id == commodity.Id);
            target.Name = commodity.Name;
            target.Description = commodity.Description;
            target.CategoryId = commodity.CategoryId;
            target.Price = commodity.Price;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [CustomAuthorization]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Commodity commodity)
        {
            var userid = (User as CustomPrincipal).CurrentIdentity.Id;

            commodity.Surplus = commodity.Total;
            _context.Commodities.Add(commodity);

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var commodity = await _context.Commodities.SingleOrDefaultAsync(m => m.Id == id);
            if (commodity == null)
            {
                return NotFound();
            }

            _context.Commodities.Remove(commodity);
            await _context.SaveChangesAsync();

            Directory.Delete(Path.Combine(_env.WebRootPath, "assets", commodity.Id.ToString()), true);

            return Ok(commodity);
        }
    }
}