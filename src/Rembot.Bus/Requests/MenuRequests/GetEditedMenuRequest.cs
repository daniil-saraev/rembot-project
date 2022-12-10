using MediatR;

namespace Rembot.Bus;

public struct GetEditedMenuRequest : IRequest
{
    public long ChatId { get; set; }

    public int MessageId { get; set; }
}