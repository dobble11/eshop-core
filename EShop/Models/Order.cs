using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Models
{
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Tel { get; set; }
        public string Address { get; set; }
        public int Count { get; set; }
        public byte Rate { get; set; }
        public byte State { get; set; } //0.未付款 1.已付款 2.已发货 3.已签收 4.交易成功 5.已撤销 6.已退货
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string Note { get; set; }
        public Guid CommodityId { get; set; }
        public Guid UserId { get; set; }

        public Commodity Commodity { get; set; }
        public User User { get; set; }
    }
}
