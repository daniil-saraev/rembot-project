using Microsoft.EntityFrameworkCore;
using Rembot.Core.Interfaces;
using Rembot.Core.Models;
using Rembot.Core.Exceptions;
using Rembot.Persistence.Data;
using Rembot.Core.Entities;

namespace Rembot.Persistence.Services
{
    internal class AuthenticationService : IAuthenticationService
    {
        private readonly DataContext _dataContext;

        public AuthenticationService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<UserDto> Login(long chatId, CancellationToken cancellationToken)
        {
            var user = await _dataContext.Users.FirstAsync(x => x.ChatId == chatId, cancellationToken);
            if (user != null)
                return new UserDto()
                {
                    PhoneNumber = user.PhoneNumber,
                    Name = user.Name
                };
            else 
                throw new UserNotFoundException();

        }

        public async Task<UserDto> Register(long chatId, string phoneNumber, string name, CancellationToken cancellationToken)
        {
            if(await _dataContext.Users.AnyAsync(x => x.ChatId == chatId, cancellationToken))
                throw new UserAlreadyExistsException();
            else
            {
                User user = new User(phoneNumber, chatId, name);
                await _dataContext.Users.AddAsync(user, cancellationToken);
                await _dataContext.SaveChangesAsync(cancellationToken);
                return new UserDto()
                {
                    PhoneNumber = user.PhoneNumber,
                    Name = user.Name
                };
            }
        }
    }
}
