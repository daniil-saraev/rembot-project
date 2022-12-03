using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace Rembot.Main.Services
{
    internal class ReceiverService : IUpdateReceiver
    {
        private readonly ITelegramBotClient _bot;

        public ReceiverService(ITelegramBotClient bot)
        {
            _bot = bot;
        }

        public async Task ReceiveAsync(IUpdateHandler updateHandler, CancellationToken cancellationToken)
        {
            var receiverOptions = new ReceiverOptions()
            {
                AllowedUpdates = Array.Empty<UpdateType>(),
                ThrowPendingUpdates = true,
            };

            await _bot.ReceiveAsync(updateHandler, receiverOptions, cancellationToken);
        }
    }
}
