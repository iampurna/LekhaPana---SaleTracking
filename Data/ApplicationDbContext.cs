using LekhaPana.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace LekhaPana.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Product> Products { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<SalesTransaction> SalesTransactions { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Invoice
        modelBuilder.Entity<Invoice>()
            .HasOne(i => i.Customer)
            .WithMany(c => c.Invoices)
            .HasForeignKey(i => i.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure SalesTransaction
        modelBuilder.Entity<SalesTransaction>()
            .HasOne(s => s.Product)
            .WithMany(p => p.SalesTransactions)
            .HasForeignKey(s => s.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SalesTransaction>()
            .HasOne(s => s.Customer)
            .WithMany(c => c.SalesTransactions)
            .HasForeignKey(s => s.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SalesTransaction>()
            .HasOne(s => s.Invoice)
            .WithMany(i => i.SalesTransactions)
            .HasForeignKey(s => s.InvoiceId)
            .OnDelete(DeleteBehavior.Restrict);

            // Seed data
            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = 1, Name = "Laptop", Description = "High-performance laptop", Price = 1200.00M, IsActive = true },
                new Product { ProductId = 2, Name = "Smartphone", Description = "Latest smartphone model", Price = 800.00M, IsActive = true },
                new Product { ProductId = 3, Name = "Headphones", Description = "Noise-cancelling headphones", Price = 250.00M, IsActive = true }
            );

            modelBuilder.Entity<Customer>().HasData(
                new Customer { CustomerId = 1, Name = "Purna Lungeli", Email = "linkwithpurna@example.com", Phone = "9812345678", Address = "New Road", IsActive = true },
                new Customer { CustomerId = 2, Name = "Rajendra Lungeli", Email = "abc@example.com", Phone = "9809876540", Address = "PutaliSadak", IsActive = true }
            );
        }
}