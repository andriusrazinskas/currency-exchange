namespace FXExchange.Core;

public interface ICurrencyExchangeRateProvider
{
    Task<decimal> GetExchangeRateAsync(Currency mainCurrency, Currency targetCurrency);
}