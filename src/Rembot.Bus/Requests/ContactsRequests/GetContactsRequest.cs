using MediatR;

namespace Rembot.Bus;

internal struct GetContactsRequest : IRequest
{
    public long ChatId { get; set; }

    public int MessageId { get; set; }
}