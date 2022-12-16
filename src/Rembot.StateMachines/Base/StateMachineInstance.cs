using System.ComponentModel.DataAnnotations;
using MassTransit;

namespace Rembot.StateMachines.Base;

public abstract class StateMachineInstance : SagaStateMachineInstance
{
    [Key]
    public Guid CorrelationId { get; set; }

    public string CurrentState { get; set; } = null!;

    public byte[] RowVersion { get; set; }
}