using MediatR;

namespace Rembot.Bus;

public struct GetContactsRequest : IRequest
{
    public long ChatId { get; set; }
}