using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rembot.Core.Entities;

[Table("Orders")]
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

    [ForeignKey("User")]
    [Required]
    public string UserId { get; private set; }

    public User User { get; private set; }
}

public enum Status
{
    InProccess,
    Done
}