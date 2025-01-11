using NSubstitute;

namespace FXExchange.Core.Tests;

[TestFixture]
internal class CurrencyConverterTests
{
    private ICurrencyExchangeRateProvider _currencyExchangeRateProvider;
    private CurrencyConverter _currencyConverter;

    [SetUp]
    public void SetUp()
    {
        _currencyExchangeRateProvider = Substitute.For<ICurrencyExchangeRateProvider>();
        _currencyConverter = new CurrencyConverter(_currencyExchangeRateProvider);
    }

    [Test]
    public async Task ConvertAsync_ReturnsCorrectConvertedAmount()
    {
        _currencyExchangeRateProvider
            .GetExchangeRateAsync(Currency.EUR, Currency.DKK)
            .Returns(7.4394m);

        var convertedAmount = await _currencyConverter.ConvertAsync(Currency.EUR, Currency.DKK, 3.12345m);

        Assert.That(convertedAmount, Is.EqualTo(23.23659393m));
    }
}