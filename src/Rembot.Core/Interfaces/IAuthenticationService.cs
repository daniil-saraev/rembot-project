using Rembot.Core.Models;

namespace Rembot.Core.Interfaces;

public interface IAuthenticationService
{
    Task<UserDto> Login(long chatId, CancellationToken cancellationToken);

    Task<UserDto> Register(long chatId, string phoneNumber, string name, CancellationToken cancellationToken);  
}