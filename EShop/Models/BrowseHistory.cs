using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Models
{
    public class BrowseHistory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public int Count { get; set; }
        public DateTime Datetime { get; set; } = DateTime.Now;
        public Guid UserId { get; set; }
        public Guid CommodityId { get; set; }

        public User User { get; set; }
        public Commodity Commodity { get; set; }
    }
}
