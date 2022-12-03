using Rembot.Core.Entities;

namespace Rembot.Core.Models
{
    public struct UserDto
    {
        public long ChatId { get; set; }

        public string Name { get; set; }

        public decimal Discount { get; set; }

        public decimal Cashback { get; set; }

        public ICollection<Order> Orders { get; set; }

        public ICollection<User> Referals { get; set; }
    }
}
