using MediatR;
using Microsoft.Extensions.Logging;
using Rembot.Core.Exceptions;
using Rembot.Core.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static Rembot.Bus.Buttons;

namespace Rembot.Bus;

public class AuthenticationStateHandler : StateHandler
{
    private InnerState _state;

    public AuthenticationStateHandler(ITelegramBotClient bot, IMediator mediator, ILogger<AuthenticationStateHandler> logger) : base(bot, mediator, logger)
    {
        _state = InnerState.SigningIn;
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
        if(message != null)
            switch (_state)
            {
                case InnerState.SigningIn:
                    await HandleSignIn(message.Text, context, message.Chat.Id, cancellationToken);
                    break;
                case InnerState.Registering:
                    await HandleRegister(message.Contact, context, message.Chat.Id, cancellationToken);
                    break;
                case InnerState.RegisterigWithReferal:
                    await HandleRegisterWithReferal(message.Contact, context, message.Chat.Id, cancellationToken);
                    break;
            }
    }

    private async Task HandleSignIn(string? message, StateContext context, long chatId, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _mediator.Send(new GetUserDataRequest{ChatId = chatId}, cancellationToken);
            await SignIn(context, user, chatId, cancellationToken);
        }
        catch (UserNotFoundException)
        {
            if(message != null && message.Contains(START))
            {
                string[] args = message.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if(args.Count() == 2)
                {
                    _state = InnerState.RegisterigWithReferal;
                    _data.Add("phone", args[1]);
                } 
                else
                    _state = InnerState.Registering;
                await _mediator.Send(new GetPhoneNumberRequest {ChatId = chatId}, cancellationToken);
            }  
        }
    }

    private async Task HandleRegister(Contact? contact, StateContext context, long chatId, CancellationToken cancellationToken)
    {
        if(contact != null)
        {
            var user = await _mediator.Send(new RegisterRequest {
                ChatId = chatId, 
                Name = contact.FirstName, 
                PhoneNumber = contact.PhoneNumber}, cancellationToken);
            await SignIn(context, user, chatId, cancellationToken);
        } 
    }

    private async Task HandleRegisterWithReferal(Contact? contact, StateContext context, long chatId, CancellationToken cancellationToken)
    {
        if(contact != null)
        {
            var user = await _mediator.Send(new RegisterWithReferalRequest {
                ChatId = chatId, 
                Name = contact.FirstName, 
                UserPhoneNumber = contact.PhoneNumber, 
                LinkOwnerPhoneNumber = _data["phone"]}, cancellationToken);
            await SignIn(context, user, chatId, cancellationToken);
        }
    }

    private async Task SignIn(StateContext context, UserDto user, long chatId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new GetNewMenuRequest {ChatId = chatId}, cancellationToken);     
        context.User = user;
        context.State = State.Menu;     
    }

    private enum InnerState
    {
        SigningIn,
        Registering,
        RegisterigWithReferal
    }
}