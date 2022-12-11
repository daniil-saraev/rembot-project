using MediatR;
using Rembot.Core.Models;

namespace Rembot.Bus;

internal struct GetUserDataRequest : IRequest<UserDto>
{
    public long ChatId { get; set; }
}