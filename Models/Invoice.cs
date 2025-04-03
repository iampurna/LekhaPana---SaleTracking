using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LekhaPana.Models;

public class Invoice
{
    [Key]
    public int InvoiceId { get; set; }

    [Required]
    [StringLength(20)]
    public string InvoiceNumber { get; set; }

    [Required]
    public DateTime InvoiceDate { get; set; }

    [Required]
    public int CustomerId { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal InvoiceTotal { get; set; }

    public bool IsPaid { get; set; } = false;


    [ForeignKey("CustomerId")]
    public virtual Customer Customer { get; set; }
    public virtual ICollection<SalesTransaction> SalesTransactions { get; set; }
}