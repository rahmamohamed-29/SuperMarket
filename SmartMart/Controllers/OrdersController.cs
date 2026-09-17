using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartMart.Data;
using SmartMart.Models;
using SmartMart.ViewModel;


namespace SmartMart.Controllers
{
    public class OrdersController : Controller
    {
              private readonly AppDbContext context;

public OrdersController(AppDbContext context)
{
    this.context = context;
}
        public IActionResult Create(int id)
        {

            var customer = context.Customers.Find(id);

            if (customer == null)
            {
                return NotFound();
            }

            ViewBag.Categories = context.Categories.ToList();
            ViewBag.Products = context.Products.ToList();
            CreateNewOrder createNewOrder = new CreateNewOrder
            {

                Order = new Order
                {
                    CustomerId = id,
                    OrderDate = DateTime.Now,
                    Status = "Pending"
                },
                OrderItem = new List<OrderItem>
                {
                   new OrderItem()
                }
            };
            return View(createNewOrder);
        }

        public IActionResult AddNew(CreateNewOrder OrderFromCreate)
        {

            if (ModelState.IsValid)
            {

                context.Orders.Add(OrderFromCreate.Order);
                context.SaveChanges();

                foreach (var orderItem in OrderFromCreate.OrderItem)
                {
                    orderItem.OrderId = OrderFromCreate.Order.Id;
                    context.OrderItems.Add(orderItem);
                }
                context.SaveChanges();

                return RedirectToAction(nameof(Index), "Customers");
            }
            else
            {
                ViewBag.Categories = context.Categories.ToList();
                ViewBag.Products = context.Products.ToList();
                return View("Create", OrderFromCreate);
            }
        }

        public IActionResult Details(int id)
        {

            Order order = context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Product).ThenInclude(p => p.Category).FirstOrDefault(o => o.Id == id); ;
            

            if (order == null)
            {
                return NotFound();
            }
            else
            {
                DetailsOrder detailsOrder = new DetailsOrder
                {
                    order = order,
                    OrderItems = order.OrderItems
                };
                return View(detailsOrder);
            }


        }

        public IActionResult Edit(int id)
        {
            Order order = context.Orders.FirstOrDefault(o => o.Id == id);
            List<OrderItem> orderItems = context.OrderItems.Where(oi => oi.OrderId == id).Include(oi => oi.Product).ThenInclude(p => p.Category).ToList();
            if (order == null)
            {
                return NotFound();
            }
            else
            {
                ViewBag.Categories = context.Categories.ToList();
                ViewBag.Products = context.Products.ToList();
              DetailsOrder editOrder = new DetailsOrder
                {
                    order = order,
                    OrderItems = orderItems
                };
                return View(editOrder);
            }
        }

        public IActionResult EditCurrent(DetailsOrder orderFromEdit)
        {
            if (ModelState.IsValid)
            {
                context.Orders.Update(orderFromEdit.order);
                foreach (var orderItem in orderFromEdit.OrderItems)
                {
                    if (orderItem.Id == 0)
                    {                   
                        orderItem.OrderId = orderFromEdit.order.Id;
                        context.OrderItems.Add(orderItem);
                    }
                    else
                    {
                        context.OrderItems.Update(orderItem);
                    }
                }
                context.SaveChanges();
                return RedirectToAction(nameof(Details), new { id = orderFromEdit.order.Id });
            }
            else
            {
                ViewBag.Categories = context.Categories.ToList();
                ViewBag.Products = context.Products.ToList();
                return View("Edit", orderFromEdit);
            }
        }

        public IActionResult Delete(int id)
        {
            Order order = context.Orders.FirstOrDefault(o => o.Id == id);
            List<Order> orders = context.Orders.Where(o => o.CustomerId == id).ToList();
            List<OrderItem> orderItems = context.OrderItems.Where(oi => oi.OrderId == id).Include(oi => oi.Product).ThenInclude(p => p.Category).ToList();

            DetailsOrder deleteOrder = new DetailsOrder
            {
                order = order,
                OrderItems = orderItems
            };

           
            if (order == null)
            {
                return NotFound();
            }
            else
            {
                return View(deleteOrder);
            }
        }

        public IActionResult DeleteCurrent(int id)
        {
            Order order = context.Orders.FirstOrDefault(o => o.Id == id);
            List<Order> orders = context.Orders.Where(o => o.CustomerId == id).ToList();
            List<OrderItem> orderItems = context.OrderItems.Where(oi => oi.OrderId == id).ToList();
           


            if (order == null)
            {
                return NotFound();
            }
            else
            {
                var customerId = order.CustomerId;
                context.OrderItems.RemoveRange(orderItems);
                context.Orders.Remove(order);
                context.SaveChanges();
                return RedirectToAction(nameof(Details), "Customers", new { id = customerId });
            }
        }

    }


}
