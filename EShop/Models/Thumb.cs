using System;

namespace EShop.Models
{
    public class Thumb
    {
        public Guid CommodityId { get; set; }
        public string FileName { get; set; }
        public string Url => $"/assets/{CommodityId}/{FileName}";
    }
}
