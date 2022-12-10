using MediatR;
using Microsoft.Extensions.Logging;
using Rembot.Bus;
using Rembot.Core.Models;
using Rembot.Main.Configuration;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static Rembot.Bus.Buttons;

namespace Rembot.Main.Services
{
    internal class UpdateService : IUpdateHandler
    {
        private readonly IMediator _mediator; 
        private readonly ILogger<UpdateService> _logger;
        private readonly BotConfiguration _configuration;

        public UpdateService(IMediator mediator, ILogger<UpdateService> logger, BotConfiguration configuration)
        {
            _mediator = mediator;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var endpoint = update.Type switch
            {
                UpdateType.Message => OnMessageReceived(update.Message, cancellationToken),
                UpdateType.EditedMessage => OnMessageReceived(update.Message, cancellationToken),
                UpdateType.CallbackQuery => OnCallbackQueryReceived(update.CallbackQuery, botClient, cancellationToken),
                _ => OnUnknownRequestReceived(update.Type, cancellationToken)
            };
            await endpoint;
        }

        public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError("Exception thrown while handling update : {Exception}", exception);
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
        }

        private async Task OnMessageReceived(Message? message, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(message?.Text))
            {
                await OnTextMessageReceived(message.Text, message.Chat.Id);
            }          
            else if(message?.Contact != null)
            {
                await OnContactMessageReceived(message.Contact, message.Chat.Id);
            }
        }

        private async Task OnTextMessageReceived(string text, long chatId)
        {
            if(text.Contains(START))
            {
                string[] args = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if(args.Count() == 2)

                await _mediator.Send(new GetPhoneNumberRequest {ChatId = chatId});
            }  
            if(text.Contains(MENU))
            {
                await _mediator.Send(new LoginRequest {ChatId = chatId});
                await _mediator.Send(new GetNewMenuRequest {ChatId = chatId});
            }
        }

        private async Task OnContactMessageReceived(Contact contact, long chatId)
        {
            await _mediator.Send(new RegisterRequest {ChatId = chatId, Name = contact.FirstName, PhoneNumber = contact.PhoneNumber});
            await _mediator.Send(new GetNewMenuRequest {ChatId = chatId});
        }

        private async Task OnCallbackQueryReceived(CallbackQuery? callbackQuery, ITelegramBotClient bot, CancellationToken cancellationToken)
        {
            if(callbackQuery?.Message == null)
                return;
            await bot.AnswerCallbackQueryAsync(callbackQuery.Id);
            UserDto user = await _mediator.Send(new GetUserDataRequest{ChatId = callbackQuery.Message.Chat.Id});
            long chatId = callbackQuery.Message.Chat.Id;
            int messageId = callbackQuery.Message.MessageId;
            switch (callbackQuery.Data)
            {         
                case MENU :      
                    await _mediator.Send(new GetEditedMenuRequest {ChatId = chatId, MessageId = messageId});               
                    break;
                case ORDERS :
                    await _mediator.Send(new GetOrdersRequest{ChatId = chatId, MessageId = messageId, PhoneNumber = user.PhoneNumber});
                    break;
                case BONUSES :
                    await _mediator.Send(new GetBonusesInfoRequest {BotUrl = _configuration.Url, ChatId = chatId, MessageId = messageId, PhoneNumber = user.PhoneNumber});
                    break;
                case CONTACTS :
                    await _mediator.Send(new GetContactsRequest {ChatId = chatId, MessageId = messageId});
                    break;
                case REFERALS:
                    await _mediator.Send(new GetReferalsRequest {ChatId = chatId, MessageId = messageId, PhoneNumber = user.PhoneNumber});
                    break;
                default:
                    break;
            }
        }

        private Task OnUnknownRequestReceived(UpdateType updateType, CancellationToken cancellationToken)
        {
            _logger.LogWarning("Unknown request received", updateType);
            return Task.CompletedTask;
        }
    }
}
