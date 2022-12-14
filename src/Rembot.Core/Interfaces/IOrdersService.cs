using Rembot.Core.Models;

namespace Rembot.Core.Interfaces;

public interface IOrdersService
{
    Task<IEnumerable<OrderDto>> GetOrders(string phoneNumber);
    Task PlaceOrder(string device, string description, string phoneNumber);
}