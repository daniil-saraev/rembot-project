using MediatR;

namespace Rembot.Bus;

internal struct GetDescriptionRequest : IRequest
{
    public long ChatId { get; set; }
}