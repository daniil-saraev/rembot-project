using MediatR;
using Rembot.Core.Models;

namespace Rembot.Bus;

public struct LoginRequest : IRequest
{
    public long ChatId { get; set; }
}