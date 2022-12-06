using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Rembot.Bus;

internal class GetPhoneNumberRequestHandler : IRequestHandler<GetPhoneNumberRequest>
{
    private readonly ITelegramBotClient _bot;

    public GetPhoneNumberRequestHandler(ITelegramBotClient bot)
    {
        _bot = bot;
    }

    public async Task<Unit> Handle(GetPhoneNumberRequest request, CancellationToken cancellationToken)
    {
        await _bot.SendTextMessageAsync(
                chatId: request.ChatId,
                text: "Для регистрации необходим ваш номер телефона.",
                replyMarkup: new ReplyKeyboardMarkup(KeyboardButton.WithRequestContact("Поделиться номером")){ResizeKeyboard = true},
                cancellationToken: cancellationToken
            );
        return Unit.Value;
    }
}