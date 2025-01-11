namespace FXExchange.Core;

internal interface ICurrencyExchangeRateProvider
{
    Task<decimal> GetExchangeRateAsync(Currency mainCurrency, Currency targetCurrency);
}