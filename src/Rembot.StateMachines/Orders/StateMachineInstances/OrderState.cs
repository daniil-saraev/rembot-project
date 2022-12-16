using Rembot.Core.Entities;
using Rembot.StateMachines.Base;

namespace Rembot.StateMachines.Orders;

public class OrderState : StateMachineInstance
{
    public string OrderId { get; set; } = null!;
    public Order Order { get; set; } = null!;
}