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
            .Returns(7.4394M);

        var convertedAmount = await _currencyConverter.ConvertAsync(Currency.EUR, Currency.DKK, 3.12345M);

        Assert.That(convertedAmount, Is.EqualTo(23.23659393M));
    }

    [Test]
    public async Task ConvertAsync_ReturnsOriginalAmount_WhenConvertingToSameCurrency()
    {
        var amountToConvert = 3.12345M;

        var convertedAmount = await _currencyConverter.ConvertAsync(Currency.EUR, Currency.EUR, amountToConvert);

        await _currencyExchangeRateProvider.DidNotReceive().GetExchangeRateAsync(Arg.Any<Currency>(), Arg.Any<Currency>());

        Assert.That(convertedAmount, Is.EqualTo(amountToConvert));
    }

    [Test]
    public void ConvertAsync_ThrowsException_WhenAmountToExchangeIsNegative()
    {
        var exception = Assert.ThrowsAsync<ArgumentException>(() => _currencyConverter.ConvertAsync(Currency.EUR, Currency.DKK, -1.2M));

        Assert.That(exception.Message, Is.EqualTo("Amount to exchange cannot be negative."));
    }
}