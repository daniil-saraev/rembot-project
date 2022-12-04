using Rembot.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Rembot.Core.Models
{
    public struct OrderDto
    {
        public string Device { get; set; }

        public string Description { get; set; }

        public Status Status { get; set; }

        public decimal Cost { get; set; }
    }
}
