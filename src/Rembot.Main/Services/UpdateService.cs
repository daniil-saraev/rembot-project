using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rembot.Bus;
using Rembot.Bus.Configuration;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

#pragma warning disable CS8602

namespace Rembot.Main.Services
{
    internal class UpdateService : IUpdateHandler
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<UpdateService> _logger;
        private readonly BotConfiguration _configuration;
        private static readonly Dictionary<long, StateContext> _stateContextMap = new Dictionary<long, StateContext>();

        public UpdateService(IServiceProvider services, ILogger<UpdateService> logger, BotConfiguration configuration)
        {
            _services = services;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError("Exception thrown while handling update : {Exception}", exception);
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            long userId = GetUserId(update);
            StateContext context = GetUserStateContext(userId);
            var endpoint = context.State switch
            {
                State.Authentication => HandleAuthenticationState(context, update, cancellationToken),
                State.Menu => HandleMenuState(context, update, cancellationToken),
                State.PlacingOrder => HandlePlacingOrderState(context, update, cancellationToken),
                _ => throw new Exception($"Unknown state: {context.State}")
            };
            await endpoint;
        }

        private async Task HandleAuthenticationState(StateContext context, Update update, CancellationToken cancellationToken)
        {
            var stateHandler = _services.GetRequiredService<AuthenticationStateHandler>();
            await stateHandler.HandleUpdateAsync(context, update, cancellationToken);
        }

        private async Task HandleMenuState(StateContext context, Update update, CancellationToken cancellationToken)
        {
            var stateHandler = _services.GetRequiredService<MenuStateHandler>();
            await stateHandler.HandleUpdateAsync(context, update, cancellationToken);
        }

        private async Task HandlePlacingOrderState(StateContext context, Update update, CancellationToken cancellationToken)
        {
            var stateHandler = _services.GetRequiredService<PlacingOrderStateHandler>();
            await stateHandler.HandleUpdateAsync(context, update, cancellationToken);
        }

        private long GetUserId(Update update)
        {
            return update.Type switch
            {
                UpdateType.Message => update.Message.From.Id,
                UpdateType.CallbackQuery => update.CallbackQuery.From.Id,
                UpdateType.EditedMessage => update.EditedMessage.From.Id,
                UpdateType.ChosenInlineResult => update.ChosenInlineResult.From.Id,
                UpdateType.InlineQuery => update.InlineQuery.From.Id,
                _ => throw new Exception($"Unexpected update type: {update.Type}")
            };
        }

        private StateContext GetUserStateContext(long userId)
        {
            if(_stateContextMap.ContainsKey(userId))
                return _stateContextMap[userId]; 
            else
            {
                StateContext context = new StateContext();
                _stateContextMap.Add(userId, context);
                return context;
            }
        }
    }
}

#pragma warning restore CS8602