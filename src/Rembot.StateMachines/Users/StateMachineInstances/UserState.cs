using Rembot.Core.Entities;
using Rembot.StateMachines.Base;

namespace Rembot.StateMachines.Users;

public class UserState : StateMachineInstance
{
    public User? User { get; set; }
}