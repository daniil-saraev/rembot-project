using MediatR;

namespace Rembot.Bus;

internal struct GetPhoneNumberRequest : IRequest
{
    public long ChatId { get; set; }
}