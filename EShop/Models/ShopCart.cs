using System;

namespace EShop.Models
{
    public class ShopCart
    {
        public Guid CommodityId { get; set; }
        public Guid UserId { get; set; }
        public int Count { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;

        public Commodity Commodity { get; set; }
        public User User { get; set; }
    }
}
