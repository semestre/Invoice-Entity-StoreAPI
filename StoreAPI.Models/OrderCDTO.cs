using StoreAPI.Models.Entities;

namespace StoreAPI.Models;
// Transacciones
public class OrderCDTO //what is DTO
{
    public double Total { get; set; }
    public int SystemUserId { get; set; }
    public List<int> Products { get; set; }
}