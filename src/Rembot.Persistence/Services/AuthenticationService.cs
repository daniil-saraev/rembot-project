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

        public async Task<UserDto> Login(long chatId)
        {
            if(!(await _dataContext.Users.AnyAsync()))
                throw new UserNotFoundException();
            var user = await _dataContext.Users.FirstAsync(x => x.ChatId == chatId);
            if (user == null)
                throw new UserNotFoundException();           
            else 
                return new UserDto()
                {
                    PhoneNumber = user.PhoneNumber,
                    Name = user.Name
                };
        }

        public async Task<UserDto> Register(long chatId, string phoneNumber, string name)
        {
            User user = await CreateUser(chatId, phoneNumber, name);
            return new UserDto()
            {
                PhoneNumber = user.PhoneNumber,
                Name = user.Name
            };
        }

        public async Task<UserDto> RegisterWithReferal(long chatId, string name, string userPhoneNumber, string linkOwnerPhoneNumber)
        {
            User user = await CreateUser(chatId, userPhoneNumber, name);
            var linkOwner = await _dataContext.Users.FindAsync(linkOwnerPhoneNumber);
            if (linkOwner != null && !linkOwner.Referals.Contains(user))
            {                        
                linkOwner.Referals.Add(user);
                _dataContext.Users.Update(linkOwner);
                await _dataContext.SaveChangesAsync();
            }
            return new UserDto()
            {
                PhoneNumber = user.PhoneNumber,
                Name = user.Name
            };
        }

        private async Task<User> CreateUser(long chatId, string phoneNumber, string name)
        {
            if(await _dataContext.Users.AnyAsync(x => x.ChatId == chatId))
                throw new UserAlreadyExistsException();
            else
            {
                User user = new User(phoneNumber, chatId, name);
                await _dataContext.Users.AddAsync(user);
                await _dataContext.SaveChangesAsync();
                return user;
            }            
        }
    }
}
