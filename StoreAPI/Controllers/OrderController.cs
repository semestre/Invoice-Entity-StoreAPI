
using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreAPI.Models;
using StoreAPI.Models.DTOs;
using StoreAPI.Models.Entities;

namespace StoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly StoreDbContext _context;
        public OrderController(StoreDbContext context) // WHAT was he saying about here
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Order>>> GetOrders()
        {
            var orders = await _context.Order
                .Include(o=>o.SystemUser)
                .Select(o=> new
                { 
                    Id = o.Id,
                    Total = o.Total,
                    CreatedAt = o.CreatedAt,
                    User = new UserDTO()
                    {
                        Id = o.SystemUser.Id,
                        Email = o.SystemUser.Email,
                        FirstName = o.SystemUser.FirstName,
                        LastName = o.SystemUser.LastName,
                    }
                })
                .ToListAsync();
    
            // _context.Order.FirstOrDefaultAsync(o=>o.Id == id);
            return Ok(orders);
        }


        
        // ID, Total,UserId
        [HttpPost]
        public async Task<ActionResult> CreateOrder(
            [FromBody] OrderCDTO order
        )
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var newOrder = new Order()
                {
                    SystemUserId = order.SystemUserId,
                    CreatedAt = DateTime.Now,
                    Total = order.Total,
                };
                _context.Order.Add(newOrder);
                await _context.SaveChangesAsync();

                var orderProducts = order.Products
                    .Select(x=> new OrderProduct{ OrderId = newOrder.Id, ProductId = x, Amount = 3})
                    .ToList();
                _context.OrderProduct.AddRange(orderProducts);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
        
        
                return Ok();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Problem();
            }
    
        }
    }
}
