using EShop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EShop.Controllers
{
    [Route("api/filter")]
    public class FilterController : Controller
    {
        private readonly EShopContext _context;
        public FilterController(EShopContext context)
        {
            _context = context;
        }

        [HttpGet]
        public object Get()
        {
            var categories = _context.Categories.Select(m => new { label = m.Name, value = m.Id });

            return new
            {
                categories,
                page = GetByPage(8, 1, null, null, null)
            };
        }

        [HttpGet("page/{size:int=8}/{index:int=1}")]
        public object GetByPage(int size, int index, int? categoryid, int? jg, int? xl)
        {
            var list = _context.Commodities.Include(m => m.Thumbs).Include(m => m.Orders).Include(m => m.Store).AsQueryable();

            if (categoryid != null)
            {
                list = list.Where(m => m.CategoryId == categoryid);
            }
            if (jg != null)
            {
                if (jg == 0)
                {
                    list = list.OrderBy(m => m.Price);
                }
                else
                {
                    list = list.OrderByDescending(m => m.Price);
                }
            }
            if (xl != null)
            {
                if (xl == 0)
                {
                    list = list.OrderBy(m => m.Total - m.Surplus);
                }
                else
                {
                    list = list.OrderByDescending(m => m.Total - m.Surplus);
                }
            }

            return new
            {
                items = list.Skip((index - 1) * size).Take(size).Select(m => new { id = m.Id, name = m.Name, price = m.Price, url = m.Thumbs.First().Url, storeName = m.Store.Name, storeId = m.Store.Id, sv = m.Total - m.Surplus }),
                count = list.Count(),
                size,
                curr = index
            };
        }
    }
}
