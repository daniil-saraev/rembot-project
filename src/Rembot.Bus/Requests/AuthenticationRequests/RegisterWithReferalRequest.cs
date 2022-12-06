using MediatR;
using Rembot.Core.Models;

namespace Rembot.Bus;

public struct RegisterWithReferalRequest : IRequest
{
    public long ChatId { get; set; }
    public string Name { get; set; }
    public string UserPhoneNumber { get; set; }
    public string LinkOwnerPhoneNumber { get; set; }
}