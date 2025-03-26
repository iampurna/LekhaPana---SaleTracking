using LekhaPana.Data;
using LekhaPana.Models;
using LekhaPana.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace LekhaPana.Controllers
{
    [Route("Invoice")]
    [Route("Invoices")]

    public class InvoicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InvoicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Invoices
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Invoices
                .Include(i => i.Customer)
                .ToListAsync());
        }

        // GET: Invoices/Details/5
        [HttpGet("Details/{id:int}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.SalesTransactions)
                .ThenInclude(s => s.Product)
                .FirstOrDefaultAsync(m => m.InvoiceId == id);

            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // GET: Invoices/Generate
        [HttpGet("Generate")]
        public IActionResult Generate()
        {
            ViewBag.Customers = new SelectList(_context.Customers.Where(c => c.IsActive), "CustomerId", "Name");
            return View(new GenerateInvoiceViewModel());
        }

        // POST: Invoices/Generate
        [HttpPost("Generate")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Generate(GenerateInvoiceViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Find all uninvoiced transactions for selected customer on selected date
                var transactions = await _context.SalesTransactions
                    .Where(s => s.CustomerId == viewModel.CustomerId &&
                                s.TransactionDate.Date == viewModel.InvoiceDate.Date &&
                                s.InvoiceId == null)
                    .ToListAsync();

                if (transactions.Count == 0)
                {
                    ModelState.AddModelError("", "No uninvoiced transactions found for this customer on the selected date.");
                    ViewBag.Customers = new SelectList(_context.Customers.Where(c => c.IsActive), "CustomerId", "Name", viewModel.CustomerId);
                    return View(viewModel);
                }

                // Generate invoice number (YYYYMMDD-CUST-###)
                string dateStr = viewModel.InvoiceDate.ToString("yyyyMMdd");
                string custStr = viewModel.CustomerId.ToString("D3");
                string invoiceNumber = $"{dateStr}-{custStr}";

                // Calculate invoice total
                decimal invoiceTotal = transactions.Sum(t => t.Total);

                // Create invoice
                var invoice = new Invoice
                {
                    InvoiceNumber = invoiceNumber,
                    InvoiceDate = viewModel.InvoiceDate,
                    CustomerId = viewModel.CustomerId,
                    InvoiceTotal = invoiceTotal,
                    IsPaid = false
                };

                _context.Add(invoice);
                await _context.SaveChangesAsync();

                // Update transactions with invoice id
                foreach (var transaction in transactions)
                {
                    transaction.InvoiceId = invoice.InvoiceId;
                    _context.Update(transaction);
                }
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", new { id = invoice.InvoiceId });
            }

            ViewBag.Customers = new SelectList(_context.Customers.Where(c => c.IsActive), "CustomerId", "Name", viewModel.CustomerId);
            return View(viewModel);
        }

        // POST: Invoices/MarkAsPaid/5
        [HttpPost("MarkAsPaid/{id:int}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsPaid(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }

            invoice.IsPaid = true;
            _context.Update(invoice);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = invoice.InvoiceId });
        }
    }
}