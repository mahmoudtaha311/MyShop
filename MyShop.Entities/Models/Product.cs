using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyShop.Entities.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
        [Required]
        [DisplayName("Image")]
        [ValidateNever]
        public string Img {  get; set; }

        [Required]
        public decimal Price { get; set; }

        [ForeignKey("Category")]
        [DisplayName("Category")]
        public int  categoryId { get; set; }
       // [JsonIgnore]
        public Category? Category { get; set; }
    }
}
