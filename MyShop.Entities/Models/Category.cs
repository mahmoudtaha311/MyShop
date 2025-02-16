﻿using System.ComponentModel.DataAnnotations;

namespace MyShop.Entities.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public List<Product>? Products { get; set; }
    }
}
