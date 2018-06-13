using EShop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using EShop.Models;
using EShop.Utils;

namespace EShop.Controllers
{
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly EShopContext _context;
        public AccountController(EShopContext context)
        {
            _context = context;
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody]SignInViewModel vm)
        {
            var user = await _context.Users.SingleOrDefaultAsync(m => m.Email == vm.Email && m.Password == vm.Pass);

            if (user == null)
            {
                return NotFound();
            }
            Response.Cookies.Append("token", Base64Helper.Encode(user.Id.ToString()));

            return Ok();
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody]SignUpViewModel vm)
        {
            var exist = await _context.Users.AnyAsync(m => m.Email == vm.Email);

            if (exist)
            {
                return BadRequest();
            }

            var user = new User { Name = vm.Name, Email = vm.Email, Password = vm.Pass };
            _context.Users.Add(user);

            await _context.SaveChangesAsync();
            Response.Cookies.Append("token", Base64Helper.Encode(user.Id.ToString()));

            return Ok();
        }

        [HttpPost("signout")]
        public IActionResult SignOut()
        {
            Response.Cookies.Delete("token");

            return NoContent();
        }
    }
}
