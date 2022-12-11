using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using static Rembot.Bus.Buttons;

namespace Rembot.Bus;

internal class GetMenuRequestHandler : IRequestHandler<GetNewMenuRequest>, IRequestHandler<GetEditedMenuRequest>
{
    private readonly ITelegramBotClient _bot;

    public GetMenuRequestHandler(ITelegramBotClient bot)
    {
        _bot = bot;
    }

    public async Task<Unit> Handle(GetNewMenuRequest request, CancellationToken cancellationToken)
    {
        await _bot.SendTextMessageAsync(
            chatId: request.ChatId,
            text: MENU,
            replyMarkup: GetReplyMarkup(),
            cancellationToken: cancellationToken
        );

        return Unit.Value;
    }

    public async Task<Unit> Handle(GetEditedMenuRequest request, CancellationToken cancellationToken)
    {
        await _bot.EditMessageTextAsync(
            chatId: request.ChatId,
            messageId: request.MessageId,
            text: MENU,
            replyMarkup: GetReplyMarkup(),
            cancellationToken: cancellationToken
        );

        return Unit.Value;
    }

    private InlineKeyboardMarkup GetReplyMarkup()
    {
        return new InlineKeyboardMarkup(new[]
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData(ORDERS)
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData(BONUSES)
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData(CONTACTS)
            }
        });
    }
}

