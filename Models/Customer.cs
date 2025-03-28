using System.ComponentModel.DataAnnotations;

namespace LekhaPana.Models;

public class Customer
{
    [Key]
    public int CustomerId { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [StringLength(100)]
    public string Email { get; set; }

    [StringLength(20)]
    public string Phone { get; set; }

    [StringLength(200)]
    public string Address { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<SalesTransaction> SalesTransactions { get; set; } = new List<SalesTransaction>();
    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}