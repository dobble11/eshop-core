using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Models
{
    public class Commodity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public Guid StoreId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Total { get; set; }
        public int Surplus { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public byte State { get; set; }
        public string Tags { get; set; }

        public Store Store { get; set; }
        public Category Category { get; set; }
        public ICollection<BrowseHistory> BrowseHistories { get; set; }
        public ICollection<ShopCart> ShopCarts { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Thumb> Thumbs { get; set; }
    }
}
