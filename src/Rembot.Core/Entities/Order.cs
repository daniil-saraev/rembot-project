using System.ComponentModel.DataAnnotations;

namespace Rembot.Core.Entities;

public class Order
{
    [Key]
    public string Id { get; private set; }

    [Required]
    public string Device { get; private set; }

    [Required]
    public string Description { get; private set; }

    [Required]
    public Status Status { get; private set; }

    [Required]
    public decimal Cost { get; private set; }

    [Required]
    public string UserPhoneNumber { get; private set; }

    public User User { get; private set; }

    public Order(string device,
                string description,
                decimal cost,
                string userPhoneNumber)
    {
        Id = Guid.NewGuid().ToString();
        Device = device;
        Description = description;
        Status = Status.Created;
        Cost = cost;
        UserPhoneNumber = userPhoneNumber;
    }
}

public enum Status
{
    Created,
    InProccess,
    Done
}