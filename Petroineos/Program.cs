using Petroineos;
using Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddTransient<IPowerService, PowerService>();
        services.AddTransient<TradeAggregator>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
