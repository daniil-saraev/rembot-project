using System.Text;
using MediatR;
using Rembot.Core.Interfaces;
using Rembot.Core.Models;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using static Rembot.Bus.Buttons;
using static Rembot.Bus.Responses;

namespace Rembot.Bus;

internal class GetOrdersRequestHandler : IRequestHandler<GetOrdersRequest>
{
    private readonly IOrdersService _ordersService;
    private readonly ITelegramBotClient _bot;

    public GetOrdersRequestHandler(IOrdersService service, ITelegramBotClient bot)
    {
        _ordersService = service;
        _bot = bot;
    }

    public async Task<Unit> Handle(GetOrdersRequest request, CancellationToken cancellationToken)
    {
        var orders = await _ordersService.GetOrders(request.PhoneNumber);

        await _bot.EditMessageTextAsync(
            chatId: request.ChatId,
            messageId: request.MessageId,
            text: FormatOrders(orders),
            replyMarkup: new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData(MENU)),
            cancellationToken: cancellationToken
        );
        return Unit.Value;
    }

    private string FormatOrders(IEnumerable<OrderDto> orders)
    {
        if(!orders.Any())
            return NO_ACTIVE_ORDERS;
        else
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(String.Format("{0,15} {1,15} {2,15}\n\n", DEVICE, DESCRIPTION, STATUS));
            foreach (var order in orders)
            {
                builder.Append(String.Format("{0,15} {1,15} {2,15}\n", order.Device, order.Description, order.Status));
            }
            return builder.ToString();
        }
    } 
}