using Rembot.Core.Models;

namespace Rembot.Core.Interfaces;

public interface IAuthenticationService
{
    Task<UserDto> Login(long chatId);

    Task<UserDto> Register(long chatId, string phoneNumber, string name);  

    Task<UserDto> RegisterWithReferal(long chatId, string name, string userPhoneNumber, string linkOwnerPhoneNumber);
}