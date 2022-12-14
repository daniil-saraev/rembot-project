using MediatR;
using Rembot.Core.Exceptions;
using Rembot.Core.Interfaces;
using Rembot.Core.Models;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using static Rembot.Bus.Buttons;
using static Rembot.Bus.Responses;

namespace Rembot.Bus;

internal class AuthenticationRequestHandler : IRequestHandler<GetUserDataRequest, UserDto>,
                                            IRequestHandler<RegisterRequest, UserDto>, 
                                            IRequestHandler<RegisterWithReferalRequest, UserDto>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ITelegramBotClient _bot;

    public AuthenticationRequestHandler(IAuthenticationService service, ITelegramBotClient bot)
    {
        _authenticationService = service;
        _bot = bot;
    }

    public async Task<UserDto> Handle(GetUserDataRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _authenticationService.Login(request.ChatId);
            await SendUserInfo(user, request.ChatId, cancellationToken);
            return user;
        }
        catch (UserNotFoundException)
        {
            throw;
        }
    }

    public async Task<UserDto> Handle(RegisterRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _authenticationService.Register(request.ChatId, request.PhoneNumber, request.Name);
            await SendUserInfo(user, request.ChatId, cancellationToken);
            return user;
        }
        catch(UserAlreadyExistsException)
        {
            throw;
        }
    }

    public async Task<UserDto> Handle(RegisterWithReferalRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _authenticationService.RegisterWithReferal(request.ChatId, request.Name, request.UserPhoneNumber, request.LinkOwnerPhoneNumber);
            await SendUserInfo(user, request.ChatId, cancellationToken);
            return user;
        }
        catch(UserAlreadyExistsException)
        {
            throw;
        }
    }

    private async Task SendUserInfo(UserDto user, long chatId, CancellationToken cancellationToken)
    {
        await _bot.SendTextMessageAsync(
                chatId: chatId,
                text: $"{user.Name} \n" + $"{user.PhoneNumber}",
                replyMarkup: (new ReplyKeyboardMarkup(new KeyboardButton(MENU)) {ResizeKeyboard = true}),
                cancellationToken: cancellationToken
            );
    }

    private async Task SendPhoneNumberRequest(long chatId, CancellationToken cancellationToken)
    {
        await _bot.SendTextMessageAsync(
                chatId: chatId,
                text: PHONE_NUMBER_REQUIRED,
                replyMarkup: new ReplyKeyboardMarkup(KeyboardButton.WithRequestContact(SHARE_NUMBER)){ResizeKeyboard = true},
                cancellationToken: cancellationToken
            );
    }
}