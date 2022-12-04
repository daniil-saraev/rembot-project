using Rembot.Core.Exceptions;
using Rembot.Core.Interfaces;
using Rembot.Core.Models;
using Rembot.Persistence.Data;

namespace Rembot.Persistence.Services
{
    internal class OrdersService : IOrdersService
    {
        private readonly DataContext _dataContext;

        public OrdersService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IEnumerable<OrderDto>> GetOrders(string phoneNumber)
        {
            var user = await _dataContext.Users.FindAsync(phoneNumber);
            if (user == null)
                throw new UserNotFoundException();
            return user.Orders.Select(order => new OrderDto()
            {
                Cost = order.Cost,
                Description = order.Description,
                Device = order.Device,
                Status = order.Status
            });
        }
    }
}
