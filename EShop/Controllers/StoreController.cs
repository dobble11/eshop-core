using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EShop.Data;
using EShop.Models;
using EShop.Filters;

namespace EShop.Controllers
{
    [Route("api/store")]
    [CustomAuthorization]
    public class StoreController : Controller
    {
        private readonly EShopContext _context;

        public StoreController(EShopContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetStore()
        {
            var userid = (User as CustomPrincipal).CurrentIdentity.Id;

            var store = await _context.Stores.SingleOrDefaultAsync(m => m.UserId == userid);
            var categories = _context.Categories.Select(m => new { id = m.Id, name = m.Name });

            if (store == null)
            {
                return NotFound();
            }

            return Ok(new { store = new { id = store.Id, name = store.Name, imgUrl = store.ImgUrl, description = store.Description }, page1 = GetCommodityByPage(10, 1, store.Id), page2 = GetOrderByPage(10, 1, store.Id), categories });
        }

        [HttpPost]
        public async Task<IActionResult> PostStore([FromBody] Store store)
        {
            var userid = (User as CustomPrincipal).CurrentIdentity.Id;
            var user = _context.Users.SingleOrDefault(m => m.Id == userid);

            user.Role = 2;
            store.UserId = userid;
            _context.Stores.Add(store);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("commodity/page/{size:int=10}/{index:int=1}")]
        public object GetCommodityByPage(int size, int index, Guid storeid)
        {
            return new
            {
                items = _context.Commodities.Include(m => m.Category).Where(m => m.StoreId == storeid).OrderByDescending(m => m.DateTime).Skip((index - 1) * size).Take(size).Select(m => new { id = m.Id, name = m.Name, description = m.Description, categoryName = m.Category.Name, categoryId = m.Category.Id, price = m.Price, total = m.Total, surplus = m.Surplus, datetime = m.DateTime }),
                count = _context.Commodities.Where(m => m.StoreId == storeid).Count(),
                size,
                index
            };

        }

        [HttpGet("order/page/{size:int=10}/{index:int=1}")]
        public object GetOrderByPage(int size, int index, Guid storeid)
        {
            return new
            {
                items = _context.Orders.Include(m => m.Commodity).Where(m => m.Commodity.StoreId == storeid).OrderBy(m => m.State).Skip((index - 1) * size).Take(size).Select(m => new { id = m.Id, name = m.Name, price = m.Commodity.Price, state = m.State, datetime = m.DateTime, rate = m.Rate, count = m.Count, address = m.Address, commodityName = m.Commodity.Name }),
                count = _context.Orders.Include(m => m.Commodity).Where(m => m.Commodity.StoreId == storeid).Count(),
                size,
                index
            };

        }
    }
}