using Rembot.Core.Interfaces;
using Rembot.Core.Models;
using Rembot.Persistence.Data;
using Microsoft.EntityFrameworkCore;

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
            var count = await _dataContext.Referals.Where(referal => referal.HostPhoneNumber == phoneNumber).CountAsync();
            return (uint)count;
        }

        public async Task<IEnumerable<UserDto>> GetListOfReferals(string phoneNumber)
        {
            var referals =  _dataContext.Referals.Where(referal => referal.HostPhoneNumber == phoneNumber);
            if(referals.Any())
                return await _dataContext.Users.Join(
                    referals, 
                    user => user.PhoneNumber, 
                    referal => referal.GuestPhoneNumber, 
                    (user, referals) => new UserDto {Name = user.Name, PhoneNumber = user.PhoneNumber})
                    .ToListAsync();
            else
                return new List<UserDto>();
        }
    }
}
