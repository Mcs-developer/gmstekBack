using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GMSTEK.Models
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Item> Items { get; set; }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options){}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<InvoiceItem>()
                   .HasKey(x => new { x.InvoiceId, x.ItemId });

            builder.Entity<InvoiceItem>()
                   .HasOne(x => x.Invoice)
                   .WithMany(i => i.InvoiceItems)
                   .HasForeignKey(x => x.InvoiceId);

            builder.Entity<InvoiceItem>()
                   .HasOne(x => x.Item)
                   .WithMany(i => i.InvoiceItems)
                   .HasForeignKey(x => x.ItemId);
        }

    }
}
