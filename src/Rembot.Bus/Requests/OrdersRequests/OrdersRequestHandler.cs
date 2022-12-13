using System.Text;
using MediatR;
using Rembot.Core.Entities;
using Rembot.Core.Interfaces;
using Rembot.Core.Models;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using static Rembot.Bus.Buttons;
using static Rembot.Bus.Responses;
using static Rembot.Bus.Items;

namespace Rembot.Bus;

internal class OrdersRequestHandler : IRequestHandler<GetOrdersRequest>, IRequestHandler<PlaceOrderRequest>, IRequestHandler<GetDeviceRequest>, IRequestHandler<GetDescriptionRequest>
{
    private readonly IOrdersService _ordersService;
    private readonly ITelegramBotClient _bot;

    public OrdersRequestHandler(IOrdersService service, ITelegramBotClient bot)
    {
        _ordersService = service;
        _bot = bot;
    }

    public async Task<Unit> Handle(PlaceOrderRequest request, CancellationToken cancellationToken)
    {
        await _ordersService.PlaceOrder(request.Device, request.Description, request.PhoneNumber);
        await _bot.SendTextMessageAsync(
            chatId: request.ChatId,
            text: ORDER_CREATED,
            replyMarkup: (new ReplyKeyboardMarkup(new KeyboardButton(MENU)) {ResizeKeyboard = true}),
            cancellationToken: cancellationToken
        );
        return Unit.Value;
    }

    public async Task<Unit> Handle(GetDeviceRequest request, CancellationToken cancellationToken)
    {
        await _bot.SendTextMessageAsync(
            chatId: request.ChatId,
            text: REQUEST_DEVICE,
            cancellationToken: cancellationToken
        );
        return Unit.Value;
    }

    public async Task<Unit> Handle(GetDescriptionRequest request, CancellationToken cancellationToken)
    {
        await _bot.SendTextMessageAsync(
            chatId: request.ChatId,
            text: REQUEST_DESCRIPTION,
            cancellationToken: cancellationToken
        );
        return Unit.Value;
    }

    public async Task<Unit> Handle(GetOrdersRequest request, CancellationToken cancellationToken)
    {
        var orders = await _ordersService.GetOrders(request.PhoneNumber);

        await _bot.EditMessageTextAsync(
            chatId: request.ChatId,
            messageId: request.MessageId,
            text: FormatOrders(orders),
            replyMarkup: new InlineKeyboardMarkup(
                new [] 
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(PLACE_ORDER)
                    },
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData(MENU)
                    } 
                }),
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
            builder.Append(String.Format("{0,-30} {1,-30} {2,-30}\n\n", DEVICE, DESCRIPTION, STATUS));
            foreach (var order in orders)
            {
                string status = order.Status switch
                {
                    Status.Created => "Создан",
                    Status.InProccess => "В работе",
                    Status.Done => "Готов"
                };
                builder.Append(String.Format("{0,-30} {1,-30} {2,-30}\n", order.Device, order.Description, status));
            }
            return builder.ToString();
        }
    } 
}