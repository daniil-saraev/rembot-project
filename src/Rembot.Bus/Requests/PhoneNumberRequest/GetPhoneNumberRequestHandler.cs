using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using static Rembot.Bus.Buttons;
using static Rembot.Bus.Responses;

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
                text: PHONE_NUMBER_REQUIRED,
                replyMarkup: new ReplyKeyboardMarkup(KeyboardButton.WithRequestContact(SHARE_NUMBER)){ResizeKeyboard = true},
                cancellationToken: cancellationToken
            );
        return Unit.Value;
    }
}