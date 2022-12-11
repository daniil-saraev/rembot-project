using MediatR;

namespace Rembot.Bus;

internal struct PlaceOrderRequest : IRequest
{
    public string Device { get; set; }
    public string Description { get; set; }  
    public long ChatId { get; set; }
    public string PhoneNumber { get; set; }
}