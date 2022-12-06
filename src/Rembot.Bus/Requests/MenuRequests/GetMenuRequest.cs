using MediatR;

namespace Rembot.Bus;

public struct GetMenuRequest : IRequest
{
    public long ChatId { get; set; }
}