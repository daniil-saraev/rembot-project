using MediatR;

namespace Rembot.Bus;

internal struct GetNewMenuRequest : IRequest
{
    public long ChatId { get; set; }
}