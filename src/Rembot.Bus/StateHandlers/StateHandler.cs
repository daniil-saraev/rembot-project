using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types.Enums;
using static Rembot.Bus.Buttons;

namespace Rembot.Bus;

public abstract class StateHandler 
{
    protected readonly ITelegramBotClient _bot;
    protected readonly IMediator _mediator;
    protected readonly ILogger<StateHandler> _logger;
    protected readonly Dictionary<string, string> _data;

    protected static event Action? ResetState;

    public StateHandler(ITelegramBotClient bot, IMediator mediator, ILogger<StateHandler> logger)
    {
        _bot = bot;
        _mediator = mediator;
        _logger = logger;
        _data = new Dictionary<string, string>();
    }

    public abstract Task HandleUpdateAsync(StateContext context, Update update, CancellationToken cancellationToken);

    protected Task OnUnknownRequestReceived(UpdateType updateType, CancellationToken cancellationToken)
    {
        _logger.LogWarning($"Unknown request received {updateType}");
        return Task.CompletedTask;
    }

    protected virtual async Task OnMessageReceived(Message? message, StateContext context, CancellationToken cancellationToken)
    {
        switch (message?.Text)
        {
            case MENU:
                var user = await _mediator.Send(new GetUserDataRequest {ChatId = message.Chat.Id});
                await _mediator.Send(new GetNewMenuRequest {ChatId = message.Chat.Id});
                context.User = user;
                context.State = State.Menu;
                ResetState?.Invoke();
                break;
        }    
    }
}