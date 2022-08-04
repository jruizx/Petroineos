using Services;

namespace Petroineos;

public class TradeAggregator
{
    private readonly ILogger<TradeAggregator> logger;
    private readonly IPowerService powerService;
    private Dictionary<int, TradesAggregated> mapping;

    public TradeAggregator(IPowerService powerService, ILogger<TradeAggregator> logger)
    {
        this.powerService = powerService;
        this.logger = logger;
        mapping = new Dictionary<int, TradesAggregated>
        {
            {1, new TradesAggregated("23:00")},
            {2,  new TradesAggregated("00:00")},
            {3,  new TradesAggregated("01:00")},
            {4,  new TradesAggregated("02:00")},
            {5,  new TradesAggregated("03:00")},
            {6,  new TradesAggregated("04:00")},
            {7,  new TradesAggregated("05:00")},
            {8,  new TradesAggregated("06:00")},
            {9,  new TradesAggregated("07:00")},
            {10, new TradesAggregated( "08:00")},
            {11, new TradesAggregated( "09:00")},
            {12, new TradesAggregated( "10:00")},
            {13, new TradesAggregated( "11:00")},
            {14, new TradesAggregated( "12:00")},
            {15, new TradesAggregated( "13:00")},
            {16, new TradesAggregated( "14:00")},
            {17, new TradesAggregated( "15:00")},
            {18, new TradesAggregated( "16:00")},
            {19, new TradesAggregated( "17:00")},
            {20, new TradesAggregated( "18:00")},
            {21, new TradesAggregated( "19:00")},
            {22, new TradesAggregated( "20:00")},
            {23, new TradesAggregated( "21:00")},
            {24, new TradesAggregated( "22:00")},
        };
    }

    public async Task<IEnumerable<TradesAggregated>> Aggregate(DateTime date)
    {
        try
        {
            logger.LogInformation("Retrieving trades...");

            var result = await powerService.GetTradesAsync(date);

            logger.LogInformation($"Number of trades received {result.Count()}");

            foreach (var trade in result)
            {
                foreach (var period in trade.Periods)
                {
                    mapping[period.Period].AddVolume(period.Volume);
                }
            }

            return mapping.Values;
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred during the aggregation.");
            return Enumerable.Empty<TradesAggregated>();
        }
    }
}