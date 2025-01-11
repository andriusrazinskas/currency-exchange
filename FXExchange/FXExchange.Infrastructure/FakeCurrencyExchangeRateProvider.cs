using FXExchange.Core;

namespace FXExchange.Infrastructure;

internal class FakeCurrencyExchangeRateProvider : ICurrencyExchangeRateProvider
{
    public Task<decimal> GetExchangeRateAsync(Currency mainCurrency, Currency targetCurrency)
    {
        return Task.FromResult(1m);
    }
}