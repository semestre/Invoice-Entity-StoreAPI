namespace StoreAPI.Models;

public class InvoiceCDTO
{
    public int OrderId { get; set; }
    public string InvoiceNumber { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime? DueDate { get; set; }
    public double Subtotal { get; set; }
    public double Tax { get; set; }
    public double Total { get; set; }
    public string Currency { get; set; } = "MXN";
    public bool IsPaid { get; set; }
    public DateTime? PaymentDate { get; set; }
    public string BillingName { get; set; }
    public string BillingAddress { get; set; }
    public string BillingEmail { get; set; }
    public string TaxId { get; set; }
}