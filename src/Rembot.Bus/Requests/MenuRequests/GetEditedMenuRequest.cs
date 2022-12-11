using MediatR;

namespace Rembot.Bus;

internal struct GetEditedMenuRequest : IRequest
{
    public long ChatId { get; set; }

    public int MessageId { get; set; }
}