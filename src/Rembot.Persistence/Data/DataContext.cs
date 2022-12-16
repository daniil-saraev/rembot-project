using Rembot.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Rembot.StateMachines.Users;
using Rembot.StateMachines.Orders;
using MassTransit.EntityFrameworkCoreIntegration;
using MassTransit;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Rembot.Persistence.Data;

internal class DataContext : SagaDbContext
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

        modelBuilder.Entity<User>().HasOne<UserState>()
                                    .WithOne(state => state.User)
                                    .HasForeignKey<UserState>(state => state.UserPhoneNumber)
                                    .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Order>().Property(order => order.Cost)
                                    .HasPrecision(8, 2)
                                    .HasColumnType("DECIMAL(8,2)");

        modelBuilder.Entity<Order>().HasOne<OrderState>()
                                    .WithOne(state => state.Order)
                                    .HasForeignKey<OrderState>(state => state.OrderId)
                                    .OnDelete(DeleteBehavior.Cascade);
    }

    public DbSet<User> Users { get; set; }

    public DbSet<UserState> UserStates { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<OrderState> OrderStates { get; set; }

    public DbSet<Referal> Referals { get; set; }

    protected override IEnumerable<ISagaClassMap> Configurations => new List<ISagaClassMap>() { new UserStateMap(), new OrderStateMap() };
}

internal class UserStateMap : SagaClassMap<UserState>
{
    protected override void Configure(EntityTypeBuilder<UserState> entity, ModelBuilder model)
    {
        base.Configure(entity, model);
        entity.Property(state => state.RowVersion).IsRowVersion();
    }
}

internal class OrderStateMap : SagaClassMap<OrderState>
{   
    protected override void Configure(EntityTypeBuilder<OrderState> entity, ModelBuilder model)
    {
        base.Configure(entity, model);
        entity.Property(state => state.RowVersion).IsRowVersion();
    }
}