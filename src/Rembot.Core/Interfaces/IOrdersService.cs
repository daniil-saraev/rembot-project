using Rembot.Core.Entities;

namespace Rembot.Core.Interfaces;

public interface IOrdersService
{
    Task<IEnumerable<Order>> GetOrders();
}