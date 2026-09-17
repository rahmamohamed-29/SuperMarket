using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using SmartMart.Data;
using SmartMart.Models;

namespace SmartMart.Controllers
{
    public class ProductsController : Controller
    {
                    private readonly AppDbContext context;

public ProductsController(AppDbContext context)
{
    this.context = context;
}
        IWebHostEnvironment webHostEnvironment;
        public ProductsController(IWebHostEnvironment webHost)
        {
            webHostEnvironment = webHost;
        }
        public IActionResult Index(string? search, string? sortBy, string? category)
        {
            IQueryable<Product> products = context.Products;
            if (string.IsNullOrWhiteSpace(search) == false)
            {
                products = products.Where(p => p.Name.Contains(search) ||
                                                      p.Description.Contains(search) || p.Category.Name.Contains(search));
            }
            if (string.IsNullOrWhiteSpace(category) == false)
            {
                products = products.Where(p => p.Category.Name == category);
            }
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy == "nameAsc")
                {
                    products = products.OrderBy(p => p.Name);
                }
                else if (sortBy == "nameDesc")
                {
                    products = products.OrderByDescending(p => p.Name);
                }
                else if (sortBy == "priceAsc")
                {
                    products = products.OrderBy(p => p.Price);
                }
                else if (sortBy == "priceDesc")
                {
                    products = products.OrderByDescending(p => p.Price);
                }
                else if (sortBy == "stockAsc")
                {
                    products = products.OrderBy(p =>
                                                p.StockStatus == "Out of Stock" ? 0 :
                                                p.StockStatus == "Low Stock" ? 1 :
                                                2);
                }
                else if (sortBy == "stockDesc")
                {
                    products = products.OrderByDescending(p =>
                                                p.StockStatus == "Out of Stock" ? 0 :
                                                p.StockStatus == "Low Stock" ? 1 :
                                                2);
                }

            }
            ViewBag.CurrentSearch = search;
            ViewBag.Categories = context.Categories.ToList();
            return View(products.ToList());
        }


        public IActionResult Create()
        {
            ViewBag.AllCategories = new SelectList(context.Categories.ToList(), "Id", "Name");
            return View();
        }
        [HttpPost]
        public IActionResult AddNew(Product productFromCreate)
        {
            if (productFromCreate.Name == productFromCreate.Description)
            {
                ModelState.AddModelError(string.Empty, "Description cannot be the same as Name");
            }

            if (ModelState.IsValid == true)
            {
                if (productFromCreate.ImageFile != null)
                {
                    Guid imageGuid = Guid.NewGuid();
                    string imageExtension = Path.GetExtension(productFromCreate.ImageFile.FileName);
                    string imageName = imageGuid + imageExtension;
                    productFromCreate.ImagePath = "\\images\\" + imageName;

                    string imageFullPath = webHostEnvironment.WebRootPath + productFromCreate.ImagePath;
                    FileStream imageFileStream = new FileStream(imageFullPath, FileMode.Create);
                    productFromCreate.ImageFile.CopyTo(imageFileStream);
                    imageFileStream.Dispose();
                }
                context.Products.Add(productFromCreate);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.AllCategories = new SelectList(context.Categories.ToList(), "Id", "Name");
                return View("Create", productFromCreate);
            }
        }
        public IActionResult Edit(int id)
        {
            Product product = context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                ViewBag.AllCategories = new SelectList(context.Categories.ToList(), "Id", "Name");
                return View(product );
            }

        }
        [HttpPost]
        public IActionResult EditCurrent(Product productFromEdit)
        {
            if (productFromEdit.Name == productFromEdit.Description)
            {
                ModelState.AddModelError(string.Empty, "Description cannot be the same as Name");
            }
            if (ModelState.IsValid == true)
            {
                if (productFromEdit.ImageFile != null)
                {
                    string oldImageFullPath = webHostEnvironment.WebRootPath + productFromEdit.ImagePath;

                    if (System.IO.File.Exists(oldImageFullPath))
                    {
                        System.IO.File.Delete(oldImageFullPath);
                    }

                    Guid imageGuid = Guid.NewGuid();
                    string imageExtension = Path.GetExtension(productFromEdit.ImageFile.FileName);
                    string imageName = imageGuid + imageExtension;
                    productFromEdit.ImagePath = "\\images\\" + imageName;

                    string imageFullPath = webHostEnvironment.WebRootPath + productFromEdit.ImagePath;
                    FileStream imageFileStream = new FileStream(imageFullPath, FileMode.Create);
                    productFromEdit.ImageFile.CopyTo(imageFileStream);
                    imageFileStream.Dispose();
                }
                context.Products.Update(productFromEdit);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.AllCategories = new SelectList(context.Categories.ToList(), "Id", "Name");
                return View("Edit", productFromEdit);
            }
        }

        public IActionResult Details(int id)
        {
            Product product = context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                ViewBag.AllCategories = new SelectList(context.Categories.ToList(), "Id", "Name");
                return View(product);
            }
        }
        public IActionResult Delete(int id)
        {
            Product product = context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                ViewBag.AllCategories = new SelectList(context.Categories.ToList(), "Id", "Name");
                return View(product);
            }
        }
        public IActionResult DeleteCurrent(int id)
        {
            Product product = context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                context.Products.Remove(product);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
