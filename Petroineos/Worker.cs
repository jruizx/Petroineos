using System.Globalization;
using CsvHelper;

namespace Petroineos;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> logger;
    private readonly TradeAggregator aggregator;
    private int intervalMinutes;
    private string CSVFileLocation;

    public Worker(ILogger<Worker> logger, IConfiguration configuration, TradeAggregator aggregator)
    {
        this.logger = logger;
        this.aggregator = aggregator;
        intervalMinutes = configuration.GetValue<int>("Interval");
        CSVFileLocation = configuration.GetValue<string>("CSVFileLocation");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var date = DateTime.Now;
            var result = await aggregator.Aggregate(date);

            if (result.Any())
            {
                var fileName = Path.Combine(CSVFileLocation, $"PowerPosition_{date:YYYYMMDD}_{date:HHmm}.csv");
                using (var writer = new StreamWriter(fileName))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    logger.LogInformation($"Document {fileName} generated");
                    await csv.WriteRecordsAsync(result, stoppingToken);
                }
            }

            logger.LogInformation($"Waiting for {intervalMinutes} minutes");

            await Task.Delay(intervalMinutes * 60 * 1000, stoppingToken);
        }
    }
}
