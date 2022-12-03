using Rembot.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Rembot.Persistence.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(user => user.PhoneNumber);
        modelBuilder.Entity<User>().Property(user => user.Discount)
                    .HasPrecision(3, 2)
                    .HasColumnType("DECIMAL(3,2)");
        modelBuilder.Entity<User>().Property(user => user.Cashback)
                    .HasPrecision(8, 2)
                    .HasColumnType("DECIMAL(8,2)");
        modelBuilder.Entity<Order>().Property(order => order.Cost)
                    .HasPrecision(8, 2)
                    .HasColumnType("DECIMAL(8,2)");
        modelBuilder.Entity<Order>().HasOne(order => order.User)
                                    .WithMany(user => user.Orders)
                                    .HasForeignKey(order => order.UserPhoneNumber);
    }

    public DbSet<User> Users { get; set; }

    public DbSet<Order> Orders { get; set; }
}