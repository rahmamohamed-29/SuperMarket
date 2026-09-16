using SmartMart.Models;

namespace SmartMart.ViewModel
{
    public class DetailsCustomer
    {
        public Customer Customer { get; set; }

        public List<Order> Orders { get; set; }
    }
}
