using Rembot.Core.Exceptions;
using Rembot.Core.Interfaces;
using Rembot.Persistence.Data;

namespace Rembot.Persistence.Services
{
    internal class DiscountService : IDiscountService
    {
        private readonly DataContext _dataContext;

        public DiscountService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<decimal> CalculateDiscount(string phoneNumber)
        {
            var user = await _dataContext.Users.FindAsync(phoneNumber);
            if (user == null) throw new UserNotFoundException();

            return user.Discount;
        }
    }
}
