using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SmartMart.Data;
using SmartMart.Models;
using SmartMart.ViewModel;

namespace SmartMart.Controllers
{
    public class CustomersController : Controller
    {
       private readonly AppDbContext context;

public CustomersController(AppDbContext context)
{
    this.context = context;
}
        IWebHostEnvironment webHostEnvironment;
        public CustomersController(IWebHostEnvironment webHost)
        {
            webHostEnvironment = webHost;
        }
        public IActionResult Index(string? search, string? sortBy)
        {
            IQueryable<Customer> customers = context.Customers.Include(c => c.Orders);
            if (string.IsNullOrWhiteSpace(search) == false)
            {
                customers = customers.Where(c => c.Name.Contains(search) ||
                                                      c.Address.Contains(search) || c.City.Contains(search));
            }
           

            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy == "nameAsc")
                {
                    customers = customers.OrderBy(c => c.Name);
                }
                else if (sortBy == "nameDesc")
                {
                    customers = customers.OrderByDescending(c => c.Name);
                }
                
               

            }
            ViewBag.CurrentSearch = search;
            ViewBag.Categories = context.Categories.ToList();
            return View(customers.ToList());
        }


        public IActionResult Create()
        {
            ViewBag.Categories = context.Categories.ToList();
            ViewBag.Products = context.Products.ToList();
            CreateNewCustomers createNewCustomers = new CreateNewCustomers
            {
                Order = new Order
                {
                    OrderDate = DateTime.Now,
                    Status = "Pending"
                },
                OrderItem = new List<OrderItem>
                {
                   new OrderItem()
                }
            };
            return View(createNewCustomers);
        }
        [HttpPost]
        public IActionResult AddNew(CreateNewCustomers CustomerFromCreate)
        {

            if (ModelState.IsValid)
            {
                if (CustomerFromCreate.Customer.ImageFile != null)
                {
                    Guid imageGuid = Guid.NewGuid();
                    string imageExtension = Path.GetExtension(CustomerFromCreate.Customer.ImageFile.FileName);
                    string imageName = imageGuid + imageExtension;
                    CustomerFromCreate.Customer.ImagePath = "\\images\\" + imageName;

                    string imageFullPath = webHostEnvironment.WebRootPath + CustomerFromCreate.Customer.ImagePath;
                    FileStream imageFileStream = new FileStream(imageFullPath, FileMode.Create);
                    CustomerFromCreate.Customer.ImageFile.CopyTo(imageFileStream);
                    imageFileStream.Dispose();
                }
                context.Customers.Add(CustomerFromCreate.Customer);
                context.SaveChanges();
                CustomerFromCreate.Order.CustomerId = CustomerFromCreate.Customer.Id;
                context.Orders.Add(CustomerFromCreate.Order);
                context.SaveChanges();
                foreach (var orderItem in CustomerFromCreate.OrderItem)
                {
                    orderItem.OrderId = CustomerFromCreate.Order.Id;
                    context.OrderItems.Add(orderItem);
                }
                context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.Categories = context.Categories.ToList();
                ViewBag.Products = context.Products.ToList();
                return View("Create", CustomerFromCreate);
            }
        }
        public IActionResult Edit(int id)
        {
            Customer customer = context.Customers.FirstOrDefault(c => c.Id == id);
            if (customer == null)
            {
                return NotFound();
            }
            else
            {
                
                return View(customer);
            }

        }
        [HttpPost]
        public IActionResult EditCurrent(Customer customerFromEdit)
        {
            if (ModelState.IsValid == true)
            {
                if (customerFromEdit.ImageFile != null)
                {
                    string oldImageFullPath = webHostEnvironment.WebRootPath + customerFromEdit.ImagePath;

                    if (System.IO.File.Exists(oldImageFullPath))
                    {
                        System.IO.File.Delete(oldImageFullPath);
                    }

                    Guid imageGuid = Guid.NewGuid();
                    string imageExtension = Path.GetExtension(customerFromEdit.ImageFile.FileName);
                    string imageName = imageGuid + imageExtension;
                    customerFromEdit.ImagePath = "\\images\\" + imageName;

                    string imageFullPath = webHostEnvironment.WebRootPath + customerFromEdit.ImagePath;
                    FileStream imageFileStream = new FileStream(imageFullPath, FileMode.Create);
                    customerFromEdit.ImageFile.CopyTo(imageFileStream);
                    imageFileStream.Dispose();
                }
                context.Customers.Update(customerFromEdit);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.AllCategories = new SelectList(context.Categories.ToList(), "Id", "Name");
                return View("Edit", customerFromEdit);
            }
        }

        public IActionResult Details(int id)
        {

            Customer customer = context.Customers.FirstOrDefault(c => c.Id == id);
            DetailsCustomer detailsCustomer = new DetailsCustomer
            {
                Customer = customer,
                Orders = context.Orders.Where(o => o.CustomerId == id).Include(o => o.OrderItems).ThenInclude(oi => oi.Product).ToList()
            };

            if (customer == null)
            {
                return NotFound();
            }
            else
            {
                return View(detailsCustomer);
            }


        }




        //***************************************
        public IActionResult Delete(int id)
        {
            Customer customer = context.Customers.FirstOrDefault(c => c.Id == id);
            DetailsCustomer detailsCustomer = new DetailsCustomer
            {
                Customer = customer,
                Orders = context.Orders.Where(o => o.CustomerId == id).Include(o => o.OrderItems).ThenInclude(oi => oi.Product).ToList()
            };

            if (customer == null)
            {
                return NotFound();
            }
            else
            {
                return View(detailsCustomer);
            }
        }
        [HttpPost]
        public IActionResult DeleteCurrent(int id)
        {
            Customer customer = context.Customers.FirstOrDefault(c => c.Id == id);
            List<Order> orders = context.Orders.Where(o => o.CustomerId == id).ToList();
            foreach (var order in orders)
            {
                List<OrderItem> orderItems = context.OrderItems.Where(oi => oi.OrderId == order.Id).ToList();
                context.OrderItems.RemoveRange(orderItems);
            }

            if (customer == null)
            {
                return NotFound();
            }
            else
            {
                context.Orders.RemoveRange(orders);
                context.Customers.Remove(customer);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
