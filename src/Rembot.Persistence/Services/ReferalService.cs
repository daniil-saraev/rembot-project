using Rembot.Core.Exceptions;
using Rembot.Core.Interfaces;
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

        public async Task AddReferal(string thisUserPhoneNumber, string linkOwnerPhoneNumber)
        {
            var user = await _dataContext.Users.FindAsync(thisUserPhoneNumber);
            if (user == null) throw new UserNotFoundException();
            var linkOwner = await _dataContext.Users.FindAsync(linkOwnerPhoneNumber);
            if (linkOwner == null) throw new UserNotFoundException();

            linkOwner.Referals.Add(user);
            _dataContext.Users.Update(linkOwner);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<uint> CountReferals(string phoneNumber)
        {
            var user = await _dataContext.Users.FindAsync(phoneNumber);
            if (user == null) throw new UserNotFoundException();
            return (uint)user.Referals.Count;
        }
    }
}
