using MediatR;

namespace Rembot.Bus;

public struct GetNewMenuRequest : IRequest
{
    public long ChatId { get; set; }
}