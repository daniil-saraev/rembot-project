using Rembot.Core.Exceptions;
using Rembot.Core.Interfaces;
using Rembot.Persistence.Data;

namespace Rembot.Persistence.Services
{
    internal class CashbackService : ICashbackService
    {
        private readonly DataContext _dataContext;

        public CashbackService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<decimal> CalculateCashback(string phoneNumber)
        {
            var user = await _dataContext.Users.FindAsync(phoneNumber);
            if (user == null) throw new UserNotFoundException();

            return user.Cashback;
        }
    }
}
