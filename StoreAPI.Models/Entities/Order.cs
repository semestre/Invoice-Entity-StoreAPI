namespace StoreAPI.Models.Entities;
// EntityFramework Sequelize
public class Order
{
    public int Id { get; set; }
    public double Total { get; set; }
    public DateTime CreatedAt { get; set; }
    public int SystemUserId { get; set; } // shows the relation of one
    public SystemUser SystemUser { get; set; } // this is how u get it
    
    public List<OrderProduct> OrderProducts { get; set; }
    
    public List<Invoice> Invoices { get; set; } // relation to many 
}