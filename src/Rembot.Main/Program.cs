using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rembot.Main.Configuration;
using Rembot.Main.Services;
using Telegram.Bot;
using Rembot.Persistence.Configuration;

IHost host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        BotConfiguration configuration = new BotConfiguration();
        context.Configuration.Bind("Bot", configuration);


        services.AddHttpClient<ITelegramBotClient>(httpClient =>
        {
            TelegramBotClientOptions options = new(configuration.Token);
            new TelegramBotClient(options, httpClient);
        });
        services.AddDataContext(context.Configuration);
        services.AddScoped<UpdateService>();
        services.AddScoped<ReceiverService>();
        services.AddHostedService<PollingService<ReceiverService>>();
    })
    .Build();

host.Services.EnsureDatabaseCreated();

await host.RunAsync();

