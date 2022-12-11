using Rembot.Core.Models;

namespace Rembot.Bus;

public class StateContext
{
    public UserDto User { get; set; }
    public State State { get; set; }

    public StateContext()
    {
        State = State.Authentication;
    }
}