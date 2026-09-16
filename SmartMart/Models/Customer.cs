using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartMart.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public string Email { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        [NotMapped]
        
        public IFormFile? ImageFile { get; set; } 
        public string? ImagePath { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();

    }
}
