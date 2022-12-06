using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Rembot.Bus;

internal class GetMenuRequestHandler : IRequestHandler<GetMenuRequest>, IRequestHandler<GetContactsRequest>
{
    private readonly ITelegramBotClient _bot;

    public GetMenuRequestHandler(ITelegramBotClient bot)
    {
        _bot = bot;
    }

    public async Task<Unit> Handle(GetMenuRequest request, CancellationToken cancellationToken)
    {
        InlineKeyboardMarkup inlineKeyboard = new(new[]
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData("Заказы")
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("Бонусы и рефералы")
            },
            new []
            {
                InlineKeyboardButton.WithCallbackData("Контакты")
            }
        });
        await _bot.SendTextMessageAsync(
            chatId: request.ChatId,
            text: "Меню",
            replyMarkup: inlineKeyboard,
            cancellationToken: cancellationToken
        );

        return Unit.Value;
    }

    public async Task<Unit> Handle(GetContactsRequest request, CancellationToken cancellationToken)
    {
        string contactInfo = "Контакты: \n" +
                "Тел: +79998887766 \n" +
                "Почта: rembot@mail.ru" + 
                "Адрес: Невский проспект, 100";

        await _bot.SendTextMessageAsync(
            chatId: request.ChatId,
            text: contactInfo,
            replyMarkup: new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData("Меню")),
            cancellationToken: cancellationToken
        );

        return Unit.Value;
    }
}

