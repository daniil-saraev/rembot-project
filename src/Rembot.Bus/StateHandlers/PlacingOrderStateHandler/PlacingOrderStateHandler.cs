using MediatR;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Rembot.Bus;

public class PlacingOrderStateHandler : StateHandler
{
    private InnerState _state;
    private PlaceOrderRequest _request;

    public PlacingOrderStateHandler(ITelegramBotClient bot, IMediator mediator, ILogger<PlacingOrderStateHandler> logger) : base(bot, mediator, logger)
    {
        _state = InnerState.GettingDevice;
    }

    public override async Task HandleUpdateAsync(StateContext context, Update update, CancellationToken cancellationToken)
    {
        var endpoint = update.Type switch
        {
            UpdateType.Message => OnMessageReceived(update.Message, context, cancellationToken),
            UpdateType.EditedMessage => OnMessageReceived(update.Message, context, cancellationToken),
            _ => OnUnknownRequestReceived(update.Type, cancellationToken)
        };
        await endpoint;
    }

    protected async override Task OnMessageReceived(Message? message, StateContext context, CancellationToken cancellationToken)
    {
        if(!string.IsNullOrEmpty(message?.Text))
        {
            switch (_state)
            {
                case InnerState.GettingDevice:
                    await HandleGettingDevice(message.Text, message.Chat.Id, context, cancellationToken);
                    break;
                case InnerState.GettingDescription:
                    await HandleGettingDescription(message.Text, message.Chat.Id, context, cancellationToken);
                    break;
            }
            await base.OnMessageReceived(message, context, cancellationToken);
        }
    }

    private async Task HandleGettingDevice(string text, long chatId, StateContext context, CancellationToken cancellationToken)
    {
        _request = new PlaceOrderRequest();
        _request.Device = text;
        await _mediator.Send(new GetDescriptionRequest {ChatId = chatId}, cancellationToken);
        _state = InnerState.GettingDescription;
    }

    private async Task HandleGettingDescription(string text, long chatId, StateContext context, CancellationToken cancellationToken)
    {
        _request.Description = text;
        _request.ChatId = chatId;
        _request.PhoneNumber = context.User.PhoneNumber;
        await _mediator.Send(_request, cancellationToken);
        _state = InnerState.OrderPlaced;
    }

    private enum InnerState
    {
        GettingDevice,
        GettingDescription,
        OrderPlaced
    }
}