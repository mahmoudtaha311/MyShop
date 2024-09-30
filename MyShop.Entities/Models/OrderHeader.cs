using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Entities.Models
{
    public class OrderHeader
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("applicationuser")]
        public string applicationuserid { get; set; }

        public ApplicationUser? applicationuser { get; set; }

        public DateTime OrderDate { get; set; }
        public DateTime ShippingDate { get; set; }
        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Carrier { get;set; }
        public DateTime PaymentDate { get; set; }
        public decimal TotalPrice { get; set; }
        //strupe properities
        public string? SessionId { get; set; }
        public string? PaymentIntentId { get; set; }
        //data of user
        public string Name { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }
        public string? City { get; set; }

        public string? Region { get; set; }


         
    }
}
