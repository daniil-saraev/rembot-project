using Rembot.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Rembot.Core.Interfaces;

public interface IAuthenticationService
{
    Task<User> Login(long chatId, CancellationToken cancellationToken);

    Task<User> Register(long chatId, string phoneNumber, CancellationToken cancellationToken);  
}