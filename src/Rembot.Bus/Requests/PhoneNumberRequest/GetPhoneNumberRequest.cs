using MediatR;

namespace Rembot.Bus;

public struct GetPhoneNumberRequest : IRequest
{
    public long ChatId { get; set; }
}