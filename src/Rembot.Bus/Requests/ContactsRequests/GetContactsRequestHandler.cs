using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using static Rembot.Bus.Buttons;
using static Rembot.Bus.Responses;

namespace Rembot.Bus;

internal class GetContactsRequestHandler : IRequestHandler<GetContactsRequest>
{
    private readonly ITelegramBotClient _bot;

    public GetContactsRequestHandler(ITelegramBotClient bot)
    {
        _bot = bot;
    }

    public async Task<Unit> Handle(GetContactsRequest request, CancellationToken cancellationToken)
    {
        await _bot.EditMessageTextAsync(
            chatId: request.ChatId,
            messageId: request.MessageId,
            text: CONTACTS_INFO,
            replyMarkup: new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData(MENU)),
            cancellationToken: cancellationToken
        );

        return Unit.Value;
    }
}