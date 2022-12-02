using Rembot.Core.Entities;

namespace Rembot.Core.Interfaces;

public interface IAuthenticationService
{
    Task<User> Authenticate(long chatId);
}