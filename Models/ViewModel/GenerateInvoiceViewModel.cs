using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace LekhaPana.Models.ViewModel;

public class GenerateInvoiceViewModel
{
    [Required(ErrorMessage = "Please select a customer")]
    [Display(Name = "Customer")]
    public int CustomerId { get; set; }
        
    [Required(ErrorMessage = "Please enter invoice date")]
    [Display(Name = "Invoice Date")]
    [DataType(DataType.Date)]
    public DateTime InvoiceDate { get; set; } = DateTime.Today;
}
