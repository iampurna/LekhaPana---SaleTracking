using LekhaPana.Data;
using LekhaPana.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LekhaPana.Controllers
{
    [Route("Customer")]
    [Route("Customers")]
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Customers
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var customers = await _context.Customers.ToListAsync();
            return View(customers);
        }

        // GET: Customers/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Email,Phone,Address,IsActive")] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return View(customer);
            }

            try
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Customer created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"An error occurred: {ex.Message}";
                return View(customer);
            }
        }

        // GET: Customers/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return NotFound();

            return View(customer);
        }

        // POST: Customers/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,Name,Email,Phone,Address,IsActive")] Customer customer)
        {
            if (id != customer.CustomerId) return NotFound();

            if (!ModelState.IsValid) return View(customer);

            try
            {
                _context.Update(customer);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Customer updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(customer.CustomerId)) return NotFound();
                throw;
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"An error occurred: {ex.Message}";
                return View(customer);
            }
        }

        // GET: Customers/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return NotFound();

            return View(customer);
        }
        
        // POST: Customers/Delete/5
        [HttpPost("Delete/{id}"), ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(id);
                if (customer == null) return NotFound();

                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Customer deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Failed to delete customer: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool CustomerExists(int id) => _context.Customers.Any(e => e.CustomerId == id);
    }
}
