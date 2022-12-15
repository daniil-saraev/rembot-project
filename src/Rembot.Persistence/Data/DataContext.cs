using Rembot.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Rembot.StateMachines.Users;
using Rembot.StateMachines.Orders;

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
        modelBuilder.Entity<User>().HasMany<Order>(user => user.Orders)
                                    .WithOne(order => order.User)
                                    .HasForeignKey(order => order.UserPhoneNumber)
                                    .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<UserState>().HasOne<User>(state => state.User)
                                        .WithOne()
                                        .HasForeignKey<User>(user => user.PhoneNumber);

        modelBuilder.Entity<Order>().Property(order => order.Cost)
                                    .HasPrecision(8, 2)
                                    .HasColumnType("DECIMAL(8,2)");
        modelBuilder.Entity<OrderState>().HasOne<Order>(state => state.Order)
                                        .WithOne()
                                        .HasForeignKey<Order>(order => order.Id);

        modelBuilder.Entity<Referal>().HasKey(referal => referal.GuestPhoneNumber);
    }

    public DbSet<User> Users { get; set; }

    public DbSet<UserState> UserStates { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<OrderState> OrderStates { get; set; }

    public DbSet<Referal> Referals { get; set; }
}