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
    [Route("api/cart")]
    [CustomAuthorization]
    public class CartController : Controller
    {
        private readonly EShopContext _context;
        public CartController(EShopContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userid = (User as CustomPrincipal).CurrentIdentity.Id;

            var user = await _context.Users.Include(m => m.ShopCarts)
                .ThenInclude(m => m.Commodity)
                .ThenInclude(m => m.Thumbs)
                .SingleOrDefaultAsync(m => m.Id == userid);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user.ShopCarts.Select(m => new { commodityId = m.CommodityId, userId = m.UserId, commodityName = m.Commodity.Name, price = m.Commodity.Price, count = m.Count, url = m.Commodity.Thumbs.First().Url }));
        }

        [HttpPut("{id}/{count}")]
        public async Task<IActionResult> UpdateCart([FromRoute]Guid id, [FromRoute]int count)
        {
            var userid = (User as CustomPrincipal).CurrentIdentity.Id;

            var item = _context.ShopCarts.SingleOrDefault(m => m.UserId == userid && m.CommodityId == id);
            if (item == null)
            {
                return NotFound();
            }
            item.Count = count;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddCart([FromBody]ShopCart cart)
        {
            var userid = (User as CustomPrincipal).CurrentIdentity.Id;

            var user = await _context.Users.Include(m => m.ShopCarts).SingleOrDefaultAsync(m => m.Id == userid);

            if (user == null)
            {
                return NotFound();
            }

            var item = user.ShopCarts.SingleOrDefault(m => m.CommodityId == cart.CommodityId);
            if (item == null)
            {
                user.ShopCarts.Add(cart);
            }
            else
            {
                item.Count += cart.Count;
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart([FromRoute]Guid id)
        {
            var userid = (User as CustomPrincipal).CurrentIdentity.Id;

            var item = _context.ShopCarts.SingleOrDefault(m => m.UserId == userid && m.CommodityId == id);
            if (item == null)
            {
                return NotFound();
            }
            _context.ShopCarts.Remove(item);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
