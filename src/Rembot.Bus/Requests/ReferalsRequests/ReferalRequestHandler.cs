using System.Text;
using MediatR;
using Rembot.Core.Interfaces;
using Rembot.Core.Models;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Rembot.Bus;

internal class ReferalRequestHandler : IRequestHandler<GetReferalsRequest>
{
    private readonly IReferalService _referalService;
    private readonly ITelegramBotClient _bot;

    public ReferalRequestHandler(IReferalService service, ITelegramBotClient bot)
    {
        _referalService = service;
        _bot = bot;
    }

    public async Task<Unit> Handle(GetReferalsRequest request, CancellationToken cancellationToken)
    {
        var referals = await _referalService.GetListOfReferals(request.PhoneNumber);
        await _bot.SendTextMessageAsync(
            chatId: request.ChatId,
            text: FormatReferals(referals),
            replyMarkup: new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData("Меню")),
            cancellationToken: cancellationToken
        );
        return Unit.Value;

    }

    private string FormatReferals(IEnumerable<UserDto> referals)
    {
        if(!referals.Any())
            return "У вас пока нет рефералов.";
        else
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Рефералы: ");
            foreach (var referal in referals)
            {
                builder.AppendLine($"{referal.PhoneNumber}, {referal.Name}");
            }
            return builder.ToString();
        }
    }
}