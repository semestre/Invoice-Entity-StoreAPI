using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreAPI.Models;
using StoreAPI.Models.Entities;

namespace StoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly StoreDbContext _context;

        public InvoiceController(StoreDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Invoice>>> GetInvoice()
        {
            var invoices = await _context.Invoice
                .Include(i => i.Order) // Include related order data if needed
                .ToListAsync();
            return Ok(invoices);
        }
        
        
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Invoice>> GetInvoiceById(int id)
        {
            var invoice = await _context.Invoice
                .Include(i => i.Order) // Include the related order if needed
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null)
            {
                return NotFound();
            }

            return Ok(invoice);
        }
        
        

        [HttpPost]
        public async Task<ActionResult> CreateInvoice(
            [FromBody] InvoiceCDTO invoice
        )
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var newInvoice = new Invoice()
                {
                    OrderId = invoice.OrderId,
                    InvoiceNumber = invoice.InvoiceNumber,
                    IssueDate = invoice.IssueDate,
                    DueDate = invoice.DueDate,
                    Subtotal = invoice.Subtotal,
                    Tax = invoice.Tax,
                    Total = invoice.Total,
                    Currency = invoice.Currency,
                    IsPaid = invoice.IsPaid,
                    PaymentDate = invoice.PaymentDate,
                    BillingName = invoice.BillingName,
                    BillingAddress = invoice.BillingAddress,
                    BillingEmail = invoice.BillingEmail,
                    TaxId = invoice.TaxId,
                };
                
                _context.Invoice.Add(newInvoice);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            
                return Ok(newInvoice.Id); // Return the created invoice ID
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Problem(detail: ex.Message);
            }
        }
    }
}
