using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Entities.Models
{
    public class ShoppingCart
    {
        [Key]
        public int  ShopingCardId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        [Range(1, 100)]
        public int Count { get; set; }

        [ForeignKey("applicationuser")]
        public string applicationuserId { get; set; }
        public ApplicationUser? applicationuser { get; set; }

    }
}
