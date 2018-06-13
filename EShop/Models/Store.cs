using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Models
{
    public class Store
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }
        public string Tags { get; set; }
        public string HomeHtml { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public byte State { get; set; } = 1; //0.待审批 1.正常 2.禁用
        public Guid UserId { get; set; }

        public ICollection<Commodity> Commodities { get; set; }
    }
}
