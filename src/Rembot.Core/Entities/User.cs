using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rembot.Core.Entities;

[Table("Users")]
public class User
{
    [Key]
    public string Id { get; private set; }

    [Key]
    public long ChatId { get; private set; }

    [Required]
    public string FirstName { get; private set; }

    public string? MiddleName { get; private set; }

    public string? LastName { get; private set; }

    [Required]
    public decimal Discount { get; private set; }

    public ICollection<Order> Orders { get; private set; }

    public ICollection<User> Referals { get; private set; }
}