using LekhaPana.Data;
using LekhaPana.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LekhaPana.Controllers
{
    [Route("Product")]
    [Route("Products")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        // GET: Products/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Price,IsActive")] Product product)
        {
            if (!ModelState.IsValid)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new
                    {
                        success = false,
                        message = string.Join("; ", ModelState.Values
                            .SelectMany(x => x.Errors)
                            .Select(x => x.ErrorMessage))
                    });
                }
                return View(product);
            }

            try
            {
                _context.Add(product);
                await _context.SaveChangesAsync();

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true });
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Error saving product" });
                }
                ModelState.AddModelError("", "Error saving product");
                return View(product);
            }
        }

        // GET: Products/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Description,Price,IsActive")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new
                    {
                        success = false,
                        message = string.Join("; ", ModelState.Values
                            .SelectMany(x => x.Errors)
                            .Select(x => x.ErrorMessage))
                    });
                }
                return View(product);
            }

            try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true });
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Error updating product" });
                }
                ModelState.AddModelError("", "Error updating product");
                return View(product);
            }
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
