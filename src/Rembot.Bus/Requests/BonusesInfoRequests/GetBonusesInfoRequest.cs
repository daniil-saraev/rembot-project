using MediatR;

namespace Rembot.Bus;

public struct GetBonusesInfoRequest : IRequest
{
    public long ChatId { get; set; }
    public string PhoneNumber { get; set; }
    public string BotUrl { get; set; }
}