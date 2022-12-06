using MediatR;
using Rembot.Core.Models;

namespace Rembot.Bus;

public struct RegisterRequest : IRequest
{
    public long ChatId { get; set; }
    public string PhoneNumber { get; set; }
    public string Name { get; set; }
}