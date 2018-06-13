using EShop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using EShop.Models;
using EShop.Filters;
using System.Linq;
using EShop.Utils;
using System;

namespace EShop.Controllers
{
    [Route("api/index")]
    public class IndexController : Controller
    {
        private readonly EShopContext _context;
        public IndexController(EShopContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            User user = null;
            var token = Request.Cookies["token"];

            if (token != null)
            {
                var userid = Guid.Parse(Base64Helper.Decode(token));
                user = await _context.Users.SingleOrDefaultAsync(m => m.Id == userid);
            }

            return Ok(user);
        }

        [HttpGet("suggest")]
        public IActionResult GetSuggest(int cate, string qs)
        {
            var num = 20;
            var n1 = 0;
            var n2 = 0;

            if (cate == 0)
            {
                var count1 = _context.Commodities.Count(m => m.Name.ToLower().Contains(qs));
                var count2 = _context.Stores.Count(m => m.Name.ToLower().Contains(qs));

                if (count1 + count2 > num)
                {
                    if (count1 <= num / 2)
                    {
                        n1 = count1;
                        n2 = num - n1;
                    }
                    else if (count2 <= num / 2)
                    {
                        n2 = count2;
                        n1 = num - n2;
                    }
                    else
                    {
                        n1 = 10;
                        n2 = 10;
                    }
                }
                else
                {
                    n1 = count1;
                    n2 = count2;
                }
            }
            else if (cate == 1)
            {
                n1 = num;
            }
            else
            {
                n2 = num;
            }

            var r1 = _context.Commodities.Where(m => m.Name.ToLower().Contains(qs))
                .OrderByDescending(m => m.Total - m.Surplus)
                .Take(n1).Select(m => new { value = m.Name, type = 1 });

            var r2 = _context.Commodities.Where(m => m.Name.ToLower().Contains(qs))
                .OrderByDescending(m => m.Total - m.Surplus)
                .Take(n1).Select(m => new { value = m.Name, type = 1 });

            var r = r1.Union(r2);

            return Ok(r);
        }

        [HttpGet("category")]
        public IActionResult GetCategory()
        {
            return Ok(_context.Categories.Select(m => new { id = m.Id, name = m.Name }));
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchAsync(int cate, string qs)
        {
            var result = await _context.Commodities.Include(m => m.Thumbs).Where(m => m.Name.Contains(qs)).Select(m => new { id = m.Id, name = m.Name, price = m.Price, url = m.Thumbs.First().Url, storeName = m.Store.Name, storeId = m.Store.Id, sv = m.Total - m.Surplus }).ToArrayAsync();

            return Ok(result);
        }
    }
}
