using System.ComponentModel.DataAnnotations;

namespace Rembot.Core.Entities;

public class User
{
    [Key]
    public string PhoneNumber { get; private set; } 

    [Key]
    public long ChatId { get; private set; }

    [Required]
    public string Name { get; private set; }

    [Required]
    public decimal Discount { get; private set; }

    [Required]
    public decimal Cashback { get; private set; }

    public ICollection<Order> Orders { get; private set; } = new List<Order>();

    public User(string phoneNumber,
                long chatId,
                string name)           
    {
        PhoneNumber = phoneNumber;
        ChatId = chatId;
        Name = name;
        Discount = decimal.Zero;
        Cashback = decimal.Zero;
    }
}