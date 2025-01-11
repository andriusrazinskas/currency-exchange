using FXExchange.Core;

namespace FXExchange.Infrastructure.Tests;

[TestFixture]
internal class FakeCurrencyExchangeRateProviderTests
{
    private FakeCurrencyExchangeRateProvider _exchangeRateProvider;

    [SetUp]
    public void SetUp()
    {
        _exchangeRateProvider = new FakeCurrencyExchangeRateProvider();
    }

    [Test]
    public void GetExchangeRateAsync_ThrowsException_WhenUnsupportedCurrencyIsProvided()
    {
        var unsupportedCurrency = Currency.Unknown;

        var exception = Assert.ThrowsAsync<ArgumentException>(() => _exchangeRateProvider.GetExchangeRateAsync(unsupportedCurrency, Currency.EUR));

        Assert.That(exception.Message, Is.EqualTo("Exchange rate not found."));
    }

    [Test]
    public void GetExchangeRateAsync_ThrowsException_WhenSameCurrenciesAreProvided()
    {
        var exception = Assert.ThrowsAsync<ArgumentException>(() => _exchangeRateProvider.GetExchangeRateAsync(Currency.EUR, Currency.EUR));

        Assert.That(exception.Message, Is.EqualTo("Exchange rate not found."));
    }

    [Test]
    public async Task GetExchangeRateAsync_ReturnsExpectedExchangeRate_ForEURToDKKRate()
    {
        var exchangeRate = await _exchangeRateProvider.GetExchangeRateAsync(Currency.EUR, Currency.DKK);

        Assert.That(exchangeRate, Is.EqualTo(7.4394M));
    }

    [TestCaseSource(nameof(GetAllSupportedCurrencyPairs))]
    public void GetExchangeRateAsync_ReturnsExchangeRate_ForAllSupportedCurrenciesCombinations(Currency main, Currency target)
    {
        Assert.DoesNotThrowAsync(() => _exchangeRateProvider.GetExchangeRateAsync(main, target));
    }

    private static object[] GetAllSupportedCurrencyPairs()
    {
        var supportedCurrencies = Enum.GetValues<Currency>().Where(c => c != Currency.Unknown).ToArray();

        return supportedCurrencies
            .Select(currency => (currency, supportedCurrencies.Where(c => c != currency)))
            .SelectMany(currencyGroup => currencyGroup.Item2, (currencyGroup, currency) => (currencyGroup.currency, currency))
            .Select(currencyPair => new object[] { currencyPair.Item1, currencyPair.Item2 })
            .ToArray<object>();
    }
}