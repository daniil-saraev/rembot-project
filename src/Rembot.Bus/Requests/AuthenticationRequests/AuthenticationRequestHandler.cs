using MediatR;
using Rembot.Core.Exceptions;
using Rembot.Core.Interfaces;
using Rembot.Core.Models;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Rembot.Bus;

internal class AuthenticationRequestHandler : IRequestHandler<GetUserDataRequest, UserDto>,
                                            IRequestHandler<LoginRequest>, 
                                            IRequestHandler<RegisterRequest>, 
                                            IRequestHandler<RegisterWithReferalRequest>
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
            return await _authenticationService.Login(request.ChatId);
        }
        catch (UserNotFoundException)
        {
            throw;
        }
    }

    public async Task<Unit> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _authenticationService.Login(request.ChatId);
            await SendUserInfo(user, request.ChatId, cancellationToken);
        }
        catch (UserNotFoundException)
        {
            await SendPhoneNumberRequest(request.ChatId, cancellationToken);
        }
        return Unit.Value;
    }

    public async Task<Unit> Handle(RegisterRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _authenticationService.Register(request.ChatId, request.PhoneNumber, request.Name);
            await SendUserInfo(user, request.ChatId, cancellationToken);
        }
        catch(UserAlreadyExistsException)
        {
            await _bot.SendTextMessageAsync(request.ChatId, "Такой пользователь уже зарегистрирован.");
        }
        return Unit.Value;
    }

    public async Task<Unit> Handle(RegisterWithReferalRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _authenticationService.RegisterWithReferal(request.ChatId, request.Name, request.UserPhoneNumber, request.LinkOwnerPhoneNumber);
            await SendUserInfo(user, request.ChatId, cancellationToken);
        }
        catch(UserAlreadyExistsException)
        {
            await _bot.SendTextMessageAsync(request.ChatId, "Такой пользователь уже зарегистрирован.");
        }
        return Unit.Value;
    }

    private async Task SendUserInfo(UserDto user, long chatId, CancellationToken cancellationToken)
    {
        await _bot.SendTextMessageAsync(
                chatId: chatId,
                text: $"{user.Name} \n" + $"{user.PhoneNumber}",
                parseMode: ParseMode.MarkdownV2,
                replyMarkup: null,
                cancellationToken: cancellationToken
            );
    }

    private async Task SendPhoneNumberRequest(long chatId, CancellationToken cancellationToken)
    {
        await _bot.SendTextMessageAsync(
                chatId: chatId,
                text: "Для регистрации необходим ваш номер телефона.",
                replyMarkup: new ReplyKeyboardMarkup(KeyboardButton.WithRequestContact("Поделиться номером")){ResizeKeyboard = true},
                cancellationToken: cancellationToken
            );
    }
}