using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartMart.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        [DisplayName("Stock Status")]
        public string StockStatus { get; set; }
        [DisplayName("Available Stock")]
        public int AvailableStock { get; set; }
        [DisplayName("Product ID")]
        public string productId { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public string? ImagePath { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public List<OrderItem>? OrderItems { get; set; }
    }
}
