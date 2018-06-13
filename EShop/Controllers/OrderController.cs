using EShop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using EShop.Models;
using System;
using EShop.Filters;
using System.Linq;

namespace EShop.Controllers
{
    [Route("api/order")]
    [CustomAuthorization]
    public class OrderController : Controller
    {
        private readonly EShopContext _context;
        public OrderController(EShopContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var userid = (User as CustomPrincipal).CurrentIdentity.Id;
            var orders = _context.Orders.Include(m => m.Commodity).ThenInclude(m => m.Thumbs).Where(m => m.UserId == userid).OrderByDescending(m => m.DateTime);

            return Ok(orders.Select(m => new { id = m.Id, count = m.Count, address = m.Address, name = m.Name, note = m.Note, state = m.State, price = m.Commodity.Price, commodityName = m.Commodity.Name, url = m.Commodity.Thumbs.First().Url, datetime = m.DateTime }));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]Order[] orders, [FromQuery]string from)
        {
            var userid = (User as CustomPrincipal).CurrentIdentity.Id;

            foreach (var order in orders)
            {
                order.UserId = userid;
                order.State = 1;
            }

            var results = orders.Where(m =>
            {
                var commodity = _context.Commodities.Single(n => n.Id == m.CommodityId);
                if (commodity.Surplus < m.Count)
                {
                    return false;
                }
                return true;
            });    //判断订单中的商品数量是否小于库存

            foreach (var order in results)
            {
                var commodity = _context.Commodities.Single(m => m.Id == order.CommodityId);
                commodity.Surplus -= order.Count;
            }

            if (from == "cart") //来自购物车的订单，需要先移除购物车
            {
                _context.ShopCarts.RemoveRange(results.Select(m => new ShopCart { CommodityId = m.CommodityId, UserId = m.UserId }));
            }

            _context.Orders.AddRange(results);

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Order order)
        {
            var target = _context.Orders.Single(m => m.Id == order.Id);
            target.State = order.State;
            target.Rate = order.Rate;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
