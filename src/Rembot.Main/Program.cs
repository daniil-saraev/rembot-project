using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rembot.Main.Configuration;
using Rembot.Main.Services;
using Telegram.Bot;
using Rembot.Persistence.Configuration;
using MediatR;
using Rembot.Bus;
using Telegram.Bot.Polling;

IHost host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        BotConfiguration configuration = new BotConfiguration();
        context.Configuration.Bind("Bot", configuration);
        services.AddSingleton<BotConfiguration>(configuration);
        services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(configuration.Token, new HttpClient()));
        services.AddDataContext(context.Configuration);
        services.AddCoreServices();
        services.AddMediatR(typeof(LoginRequest).Assembly);
        services.AddScoped<IUpdateHandler, UpdateService>();
        services.AddScoped<IUpdateReceiver, ReceiverService>();
        services.AddHostedService<PollingService<IUpdateReceiver>>();
    })
    .Build();

host.Services.EnsureDatabaseCreated();

await host.RunAsync();

