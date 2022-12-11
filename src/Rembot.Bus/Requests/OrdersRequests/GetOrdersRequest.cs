using MediatR;

namespace Rembot.Bus;

internal struct GetOrdersRequest : IRequest
{
    public long ChatId { get; set; }
    public int MessageId { get; set; }
    public string PhoneNumber { get; set; }
}