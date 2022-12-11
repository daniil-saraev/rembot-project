using MediatR;

namespace Rembot.Bus;

internal struct GetBonusesInfoRequest : IRequest
{
    public long ChatId { get; set; }
    public int MessageId { get; set; }
    public string PhoneNumber { get; set; }
    public string BotUrl { get; set; }
}