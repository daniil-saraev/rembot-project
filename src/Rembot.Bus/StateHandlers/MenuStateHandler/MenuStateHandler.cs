using MediatR;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Rembot.Bus.Configuration;
using static Rembot.Bus.Buttons;

namespace Rembot.Bus;

public class MenuStateHandler : StateHandler
{
    private readonly BotConfiguration _configuration;
    private InnerState _state;

    public MenuStateHandler(BotConfiguration configuration, ITelegramBotClient bot, IMediator mediator, ILogger<MenuStateHandler> logger) : base(bot, mediator, logger)
    {
        _configuration = configuration;
        _state = InnerState.MainMenu;
        ResetState += OnResetState;
    }

    public override async Task HandleUpdateAsync(StateContext context, Update update, CancellationToken cancellationToken)
    {
        var endpoint = update.Type switch
        {
            UpdateType.Message => OnMessageReceived(update.Message, context, cancellationToken),
            UpdateType.EditedMessage => OnMessageReceived(update.Message, context, cancellationToken),
            UpdateType.CallbackQuery => OnCallbackQueryReceived(update.CallbackQuery, context, cancellationToken),
            _ => OnUnknownRequestReceived(update.Type, cancellationToken)
        };
        await endpoint;
    }

    private async Task OnCallbackQueryReceived(CallbackQuery? query, StateContext context, CancellationToken cancellationToken)
    {
        if(query != null && query.Data != null)
            switch (_state)
            {
                case InnerState.MainMenu:
                    await HandleMainMenu(query.Data, query.Message.MessageId, query.Message.Chat.Id, context, cancellationToken);
                    break;
                case InnerState.ViewingOrders:
                    await HandleViewingOrders(query.Data, query.Message.MessageId, query.Message.Chat.Id, context, cancellationToken);
                    break;
                case InnerState.ViewingBonuses:
                    await HandleViewingBonuses(query.Data, query.Message.MessageId, query.Message.Chat.Id, context, cancellationToken);
                    break;
                case InnerState.ViewingReferals:
                    await HandleViewingReferals(query.Data, query.Message.MessageId, query.Message.Chat.Id, context, cancellationToken);
                    break;
                case InnerState.ViewingContacts:
                    await HandleViewingContacts(query.Data, query.Message.MessageId, query.Message.Chat.Id, context, cancellationToken);
                    break;
            }
    }

    private async Task HandleMainMenu(string queryData, int messageId, long chatId, StateContext context, CancellationToken cancellationToken)
    {
        var user = context.User;
        switch (queryData)
        {         
            case ORDERS :
                await _mediator.Send(new GetOrdersRequest{ChatId = chatId, MessageId = messageId, PhoneNumber = user.PhoneNumber}, cancellationToken);
                _state = InnerState.ViewingOrders;
                break;
            case BONUSES :
                await _mediator.Send(new GetBonusesInfoRequest {BotUrl = _configuration.Url, ChatId = chatId, MessageId = messageId, PhoneNumber = user.PhoneNumber}, cancellationToken);
                _state = InnerState.ViewingBonuses;
                break;
            case CONTACTS :
                await _mediator.Send(new GetContactsRequest {ChatId = chatId, MessageId = messageId}, cancellationToken);
                _state = InnerState.ViewingContacts;
                break;
        }
    }

    private async Task HandleViewingOrders(string queryData, int messageId, long chatId, StateContext context, CancellationToken cancellationToken)
    {
        switch (queryData)
        {
            case PLACE_ORDER:
                await _mediator.Send(new GetDeviceRequest {ChatId = chatId}, cancellationToken);
                context.State = State.PlacingOrder;
                break;
            case MENU:
                await ReturnToMenu(messageId, chatId, context, cancellationToken);
                break;
        }
    }

    private async Task HandleViewingBonuses(string queryData, int messageId, long chatId, StateContext context, CancellationToken cancellationToken)
    {
        var user = context.User;
        switch (queryData)
        {
            case REFERALS:
                await _mediator.Send(new GetReferalsRequest {ChatId = chatId, MessageId = messageId, PhoneNumber = user.PhoneNumber}, cancellationToken);
                _state = InnerState.ViewingReferals;
                break;
            case MENU:
                await ReturnToMenu(messageId, chatId, context, cancellationToken);
                break;
        }
    }

    private async Task HandleViewingReferals(string queryData, int messageId, long chatId, StateContext context, CancellationToken cancellationToken)
    {
        switch (queryData)
        {
            case MENU:
                await ReturnToMenu(messageId, chatId, context, cancellationToken);
                break;
        }
    }

    private async Task HandleViewingContacts(string queryData, int messageId, long chatId, StateContext context, CancellationToken cancellationToken)
    {
        switch (queryData)
        {
            case MENU:
                await ReturnToMenu(messageId, chatId, context, cancellationToken);
                break;
        }
    }

    private async Task ReturnToMenu(int messageId, long chatId, StateContext context, CancellationToken cancellationToken)
    {
        await _mediator.Send(new GetEditedMenuRequest {ChatId = chatId, MessageId = messageId}, cancellationToken);
        _state = InnerState.MainMenu;
    }

    private void OnResetState()
    {
        _state = InnerState.MainMenu;
    }

    private enum InnerState
    {
        MainMenu,
        ViewingOrders,
        ViewingBonuses,
        ViewingReferals,
        ViewingContacts
    }
}