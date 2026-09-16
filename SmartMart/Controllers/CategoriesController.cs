using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SmartMart.Data;
using SmartMart.Models;

namespace SmartMart.Controllers
{
    public class CategoriesController : Controller
    {
        AppDbContext context = new AppDbContext();
        public IActionResult Index(string? search, string? sortType, string? sortOrder)
        {
            IQueryable<Category> categories = context.Categories;
            if (string.IsNullOrWhiteSpace(search) == false)
            {
                categories = categories.Where(c => c.Name.Contains(search) ||
                                                           c.Description.Contains(search));
            }
            if (string.IsNullOrWhiteSpace(sortType) == false)
            {
                if (sortType == "Name" && sortOrder == "asc")
                {
                    categories = categories.OrderBy(c => c.Name);
                }
                else if (sortType == "Name" && sortOrder == "desc")
                {
                    categories = categories.OrderByDescending(c => c.Name);
                }
                  
            }
            ViewBag.CurrentSearch = search;
            return View(categories.ToList());
        }


        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddNew(Category categoryFromCreate)
        {
            if (categoryFromCreate.Name == categoryFromCreate.Description)
            {
                ModelState.AddModelError(string.Empty, "Description cannot be the same as Name");
            }

            foreach (var item in ModelState)
            {
                foreach (var error in item.Value.Errors)
                {
                    Console.WriteLine($"Property: {item.Key}");
                    Console.WriteLine($"Error: {error.ErrorMessage}");
                }
            }

            if (ModelState.IsValid == true)
            {
                context.Categories.Add(categoryFromCreate);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View("Create", categoryFromCreate);
            }
        }
        public IActionResult Edit(int id) 
        { 
            Category ctgry = context.Categories.FirstOrDefault(c => c.Id == id);
            if (ctgry == null) 
            {
                return NotFound();
            }
            else
            {
                return View(ctgry);
            }
            
        }
        [HttpPost]
        public IActionResult EditCurrent(Category categoryFromEdit)
        {
            if (categoryFromEdit.Name == categoryFromEdit.Description)
            {
                ModelState.AddModelError(string.Empty, "Description cannot be the same as Name");
            }
            if (ModelState.IsValid == true)
            {
                context.Categories.Update(categoryFromEdit);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View("Edit", categoryFromEdit);
            }
        }
        public IActionResult Delete(int id)
        {
            Category ctgry = context.Categories.FirstOrDefault(c => c.Id == id);
            if (ctgry == null)
            {
                return NotFound();
            }
            else
            {
                return View(ctgry);
            }
        }
        public IActionResult DeleteCurrent(int id)
        {
            Category ctgry = context.Categories.FirstOrDefault(c => c.Id == id);
            if (ctgry == null)
            {
                return NotFound();
            }
            else
            {
                context.Categories.Remove(ctgry);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
