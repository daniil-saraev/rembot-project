using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Rembot.Main.Services
{
    internal class UpdateService : IUpdateHandler
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<UpdateService> _logger;

        public UpdateService(ITelegramBotClient botClient, ILogger<UpdateService> logger)
        {
            _botClient = botClient;
            _logger = logger;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            
            var endpoint = update.Type switch
            {
                UpdateType.Message => OnMessageReceived(update.Message, cancellationToken),
                UpdateType.EditedMessage => OnMessageReceived(update.Message, cancellationToken),
                UpdateType.CallbackQuery => OnCallbackQueryReceived(update.CallbackQuery, cancellationToken),
                UpdateType.ChosenInlineResult => OnChosenInlineResultReceived(update.ChosenInlineResult, cancellationToken),
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
            if (string.IsNullOrEmpty(message?.Text))
                return;

            //Task<Message> handler = message.Text switch
            //{
            //    "/start" =>
            //}

            //await handler;
            //await _botClient.SendTextMessageAsync
        }

        private async Task OnCallbackQueryReceived(CallbackQuery? callback, CancellationToken cancellationToken)
        {

        }

        private async Task OnInlineQueryReceived(InlineQuery? inlineQuery, CancellationToken cancellationToken)
        {

        }

        private async Task OnChosenInlineResultReceived(ChosenInlineResult? chosenInlineResult, CancellationToken cancellationToken)
        {

        }

        private Task OnUnknownRequestReceived(UpdateType updateType, CancellationToken cancellationToken)
        {
            _logger.LogWarning("Unknown request received", updateType);
            return Task.CompletedTask;
        }
    }
}
