using LekhaPana.Data;
using LekhaPana.Models;
using LekhaPana.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LekhaPana.Controllers
{
    [Route("SalesTransactions")]
    [Route("SalesTransaction")]
    public class SalesTransactionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalesTransactionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SalesTransactions
        [Route("")]
        public async Task<IActionResult> Index()
        {
            var transactions = await _context.SalesTransactions
                .Include(s => s.Product)
                .Include(s => s.Customer)
                .Include(s => s.Invoice)
                .ToListAsync();

            return View(transactions);
        }


        [Route("Create")]
        public IActionResult Create()
        {
            ViewBag.Products = new SelectList(_context.Products.Where(p => p.IsActive), "ProductId", "Name");
            ViewBag.Customers = new SelectList(_context.Customers.Where(c => c.IsActive), "CustomerId", "Name");

            return View(new SalesTransactionViewModel());
        }


        [Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SalesTransactionViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Get product price
                var product = await _context.Products.FindAsync(viewModel.ProductId);

                // Calculate total
                var salesTransaction = new SalesTransaction
                {
                    ProductId = viewModel.ProductId,
                    CustomerId = viewModel.CustomerId,
                    Quantity = viewModel.Quantity,
                    Rate = product.Price, // Use product price as rate
                    Total = product.Price * viewModel.Quantity,
                    TransactionDate = DateTime.Now
                };

                _context.Add(salesTransaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Products = new SelectList(_context.Products.Where(p => p.IsActive), "ProductId", "Name",
                viewModel.ProductId);
            ViewBag.Customers = new SelectList(_context.Customers.Where(c => c.IsActive), "CustomerId", "Name",
                viewModel.CustomerId);

            return View(viewModel);
        }


        [Route("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesTransaction = await _context.SalesTransactions.FindAsync(id);
            if (salesTransaction == null || salesTransaction.InvoiceId != null)
            {
                // Cannot edit transaction that is already invoiced
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new SalesTransactionViewModel
            {
                SalesTransactionId = salesTransaction.SalesTransactionId,
                ProductId = salesTransaction.ProductId,
                CustomerId = salesTransaction.CustomerId,
                Quantity = salesTransaction.Quantity,
                Rate = salesTransaction.Rate
            };

            ViewBag.Products = new SelectList(_context.Products.Where(p => p.IsActive), "ProductId", "Name",
                salesTransaction.ProductId);
            ViewBag.Customers = new SelectList(_context.Customers.Where(c => c.IsActive), "CustomerId", "Name",
                salesTransaction.CustomerId);

            return View(viewModel);
        }


        [Route("Edit/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SalesTransactionViewModel viewModel)
        {
            if (id != viewModel.SalesTransactionId)
            {
                return NotFound();
            }

            var salesTransaction = await _context.SalesTransactions.FindAsync(id);
            if (salesTransaction == null || salesTransaction.InvoiceId != null)
            {
                // Cannot edit transaction that is already invoiced
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update transaction
                    salesTransaction.ProductId = viewModel.ProductId;
                    salesTransaction.CustomerId = viewModel.CustomerId;
                    salesTransaction.Quantity = viewModel.Quantity;

                    // Get product price
                    var product = await _context.Products.FindAsync(viewModel.ProductId);
                    salesTransaction.Rate = product.Price;
                    salesTransaction.Total = product.Price * viewModel.Quantity;

                    _context.Update(salesTransaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalesTransactionExists(viewModel.SalesTransactionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Products = new SelectList(_context.Products.Where(p => p.IsActive), "ProductId", "Name",
                viewModel.ProductId);
            ViewBag.Customers = new SelectList(_context.Customers.Where(c => c.IsActive), "CustomerId", "Name",
                viewModel.CustomerId);

            return View(viewModel);
        }

        private bool SalesTransactionExists(int id)
        {
            return _context.SalesTransactions.Any(e => e.SalesTransactionId == id);
        }
    }
}