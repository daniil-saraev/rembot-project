using MassTransit;
using Rembot.Core.Entities;
using Rembot.StateMachines.Orders;

namespace Rembot.StateMachines.Orders;

public class OrderStateMachine : MassTransitStateMachine<OrderState>
{
    public State Created { get; private set; }
    public State InProcess { get; private set; }
    public State Done { get; private set; }

    public OrderStateMachine()
    {
        InstanceState(order => order.CurrentState);

        Initially(
            When(OrderPlaced)
                .TransitionTo(Created));

        During(Created,
            When(OrderInProcess)
                .TransitionTo(InProcess));
        
        DuringAny(
            When(OrderDone)
                .TransitionTo(Done));
    }

    public Event<PlaceOrder> OrderPlaced { get; set; }
    public Event OrderInProcess { get; set; }
    public Event OrderDone { get; set; }

}

public class PlaceOrder : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; }
    public string Device { get; set; }
    public string Description { get; set; }  
    public string PhoneNumber { get; set; }
}