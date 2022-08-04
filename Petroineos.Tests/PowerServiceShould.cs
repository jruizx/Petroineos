using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Services;

namespace Petroineos.Tests;

public class PowerServiceShould
{
    private IPowerService serivceUnderTest;

    [SetUp]
    public void BeforeEach()
    {
        serivceUnderTest = new PowerService();
    }

    [Test]
    public async Task RetrieveTradeData()
    {
        var result = await serivceUnderTest.GetTradesAsync(DateTime.Today);

        foreach (var trade in result)
        {
            Assert.AreEqual(24, trade.Periods.Length);
        }
    }
}