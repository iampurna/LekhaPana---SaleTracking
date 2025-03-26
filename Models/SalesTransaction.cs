using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LekhaPana.Models;

public class SalesTransaction
{
    [Key]
    public int SalesTransactionId { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    public int CustomerId { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Rate { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Total { get; set; }

    public DateTime TransactionDate { get; set; } = DateTime.Now;

    public int? InvoiceId { get; set; }

    // Navigation properties
    [ForeignKey("ProductId")]
    public virtual Product Product { get; set; }

    [ForeignKey("CustomerId")]
    public virtual Customer Customer { get; set; }

    [ForeignKey("InvoiceId")]
    public virtual Invoice Invoice { get; set; }
}