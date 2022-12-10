using MediatR;
using Rembot.Core.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using static Rembot.Bus.Buttons;
using static Rembot.Bus.Responses;

namespace Rembot.Bus;

internal class GetBonusesInfoRequestHandler : IRequestHandler<GetBonusesInfoRequest>
{
    private readonly IReferalService _referalService;
    private readonly ICashbackService _cashbackService;
    private readonly IDiscountService _discountService;
    private readonly ITelegramBotClient _bot;

    public GetBonusesInfoRequestHandler(IReferalService referalService, ICashbackService cashbackService, IDiscountService discountService, ITelegramBotClient bot)
    {
        _referalService = referalService;
        _cashbackService = cashbackService;
        _discountService = discountService;
        _bot = bot;
    }

    public async Task<Unit> Handle(GetBonusesInfoRequest request, CancellationToken cancellationToken)
    {
        var referalCount = await _referalService.CountReferals(request.PhoneNumber);
        var cashback = await _cashbackService.CalculateCashback(request.PhoneNumber);
        var discount = await _discountService.CalculateDiscount(request.PhoneNumber);
        var referalLink = $"{request.BotUrl}/?start={request.PhoneNumber}";

        await _bot.EditMessageTextAsync(
            chatId: request.ChatId,
            messageId: request.MessageId,
            text: FormatBonuses(referalCount, cashback, discount, referalLink),
            replyMarkup: new InlineKeyboardMarkup(new[] { InlineKeyboardButton.WithCallbackData(REFERALS), InlineKeyboardButton.WithCallbackData(MENU)} ),
            cancellationToken: cancellationToken
        );
        return Unit.Value;
    }

    private string FormatBonuses(uint referals, decimal cashback, decimal discount, string referalLink)
    {
        return $"{BONUSES} \n\n" +
                $"{CASHBACK} - {cashback} \n" +
                $"{DISCOUNT} - {discount*100}% \n" +
                $"{REFERALS_COUNT} - {referals} \n\n" +
                $"{REFERAL_LINK} - {referalLink}";
    }
}