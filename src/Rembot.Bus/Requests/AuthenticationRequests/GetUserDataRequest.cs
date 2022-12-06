using MediatR;
using Rembot.Core.Models;

namespace Rembot.Bus;

public struct GetUserDataRequest : IRequest<UserDto>
{
    public long ChatId { get; set; }
}