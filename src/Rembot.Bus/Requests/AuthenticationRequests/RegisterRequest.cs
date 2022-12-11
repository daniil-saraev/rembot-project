using MediatR;
using Rembot.Core.Models;

namespace Rembot.Bus;

internal struct RegisterRequest : IRequest<UserDto>
{
    public long ChatId { get; set; }
    public string PhoneNumber { get; set; }
    public string Name { get; set; }
}