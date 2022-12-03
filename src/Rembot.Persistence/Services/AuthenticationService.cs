using Microsoft.EntityFrameworkCore;
using Rembot.Core.Entities;
using Rembot.Core.Interfaces;
using Rembot.Persistence.Data;

namespace Rembot.Persistence.Services
{
    internal class AuthenticationService : IAuthenticationService
    {
        private readonly DataContext _dataContext;

        public AuthenticationService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public Task<User> Login(long chatId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<User> Register(long chatId, string phoneNumber, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
