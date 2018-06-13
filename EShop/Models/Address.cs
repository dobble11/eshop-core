using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShop.Models
{
    public class Address
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Tel { get; set; }
        public string Area { get; set; }
        public bool IsDefault { get; set; }
        public Guid UserId { get; set; }

        public User User { get; set; }
    }
}
