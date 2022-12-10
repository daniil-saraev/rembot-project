using System.Text;
using MediatR;
using Rembot.Core.Interfaces;
using Rembot.Core.Models;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using static Rembot.Bus.Buttons;
using static Rembot.Bus.Responses;

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
        await _bot.EditMessageTextAsync(
            chatId: request.ChatId,
            messageId: request.MessageId,
            text: FormatReferals(referals),
            replyMarkup: new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData(MENU)),
            cancellationToken: cancellationToken
        );
        return Unit.Value;

    }

    private string FormatReferals(IEnumerable<UserDto> referals)
    {
        if(!referals.Any())
            return NO_REFERALS;
        else
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"{REFERALS}: ");
            foreach (var referal in referals)
            {
                builder.AppendLine($"{referal.PhoneNumber}, {referal.Name}");
            }
            return builder.ToString();
        }
    }
}