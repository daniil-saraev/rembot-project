using Rembot.Core.Exceptions;
using Rembot.Core.Interfaces;
using Rembot.Core.Models;
using Rembot.Persistence.Data;

namespace Rembot.Persistence.Services
{
    internal class ReferalService : IReferalService
    {
        private readonly DataContext _dataContext;

        public ReferalService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<uint> CountReferals(string phoneNumber)
        {
            var user = await _dataContext.Users.FindAsync(phoneNumber);
            if (user == null) throw new UserNotFoundException();
            return (uint)user.Referals.Count;
        }

        public async Task<IEnumerable<UserDto>> GetListOfReferals(string phoneNumber)
        {
            var user = await _dataContext.Users.FindAsync(phoneNumber);
            if (user == null) throw new UserNotFoundException();
            if(user.Referals.Any())
                return user.Referals.Select(user => new UserDto { Name = user.Name, PhoneNumber = user.PhoneNumber} );
            else
                return new List<UserDto>();
        }
    }
}
