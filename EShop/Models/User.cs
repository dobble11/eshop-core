using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Models
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime Datetime { get; set; } = DateTime.Now;
        public byte Role { get; set; }  //0.管理员，1.普通用户，2.商家
        public byte State { get; set; }


        public ICollection<BrowseHistory> BrowseHistories { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<ShopCart> ShopCarts { get; set; }
        public ICollection<Address> Addresses { get; set; }
    }

    public class SignInViewModel
    {
        public string Email { get; set; }
        public string Pass { get; set; }
    }

    public class SignUpViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Pass { get; set; }
    }
}
