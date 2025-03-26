using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LekhaPana.Models.ViewModel;

public class SalesTransactionViewModel
{
    public int SalesTransactionId { get; set; }
    
    [Required(ErrorMessage = "Please select a product")]
    [Display(Name = "Product")]
    public int ProductId { get; set; }
        
    [Required(ErrorMessage = "Please select a customer")]
    [Display(Name = "Customer")]
    public int CustomerId { get; set; }
        
    [Required(ErrorMessage = "Please enter quantity")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
    public int Quantity { get; set; }
        
    [Display(Name = "Rate")]
    public decimal Rate { get; set; }
}