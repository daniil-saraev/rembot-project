using MediatR;

namespace Rembot.Bus;

public struct GetReferalsRequest : IRequest
{
    public long ChatId { get; set; }
    public string PhoneNumber { get; set; }
}