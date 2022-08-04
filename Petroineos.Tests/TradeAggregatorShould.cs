using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Services;

namespace Petroineos.Tests;

public class TradeAggregatorShould
{
    private TradeAggregator aggregator;
    private Mock<IPowerService> powerService;
    private Mock<ILogger<TradeAggregator>> logger;

    [SetUp]
    public void BeforeEach()
    {
        powerService = new Mock<IPowerService>();
        logger = new Mock<ILogger<TradeAggregator>>();
        aggregator = new TradeAggregator(powerService.Object, logger.Object);
    }

    [Test]
    public async Task AggregateTrades()
    {
        var trade1 = PowerTrade.Create(DateTime.Today, 24);
        var trade2 = PowerTrade.Create(DateTime.Today, 24);
        powerService.Setup(x => x.GetTradesAsync(DateTime.Today)).ReturnsAsync(new List<PowerTrade> {trade1, trade2});

        var result = await aggregator.Aggregate(DateTime.Today);

        Assert.AreEqual(24, result.Count());
    }
}