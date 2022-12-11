using MediatR;

namespace Rembot.Bus;

internal struct GetDeviceRequest : IRequest
{
    public long ChatId { get; set; }
}