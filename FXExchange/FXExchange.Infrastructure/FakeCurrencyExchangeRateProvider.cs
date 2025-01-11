using FXExchange.Core;
using System.Collections.Frozen;

namespace FXExchange.Infrastructure;

internal class FakeCurrencyExchangeRateProvider : ICurrencyExchangeRateProvider
{
    private readonly FrozenDictionary<(Currency Main, Currency Target), decimal> _exchangeRateStore = new ExchangeRateStoreBuilder()
        .WithExchangeRate(Currency.EUR, Currency.USD, 743.94M / 663.11M)
        .WithExchangeRate(Currency.EUR, Currency.GBP, 743.94M / 852.85M)
        .WithExchangeRate(Currency.EUR, Currency.SEK, 743.94M / 76.10M)
        .WithExchangeRate(Currency.EUR, Currency.NOK, 743.94M / 78.40M)
        .WithExchangeRate(Currency.EUR, Currency.CHF, 743.94M / 683.58M)
        .WithExchangeRate(Currency.EUR, Currency.JPY, 743.94M / 5.9740M)
        .WithExchangeRate(Currency.EUR, Currency.DKK, 743.94M / 100M)

        .WithExchangeRate(Currency.USD, Currency.GBP, 663.11M / 852.85M)
        .WithExchangeRate(Currency.USD, Currency.SEK, 663.11M / 76.10M)
        .WithExchangeRate(Currency.USD, Currency.NOK, 663.11M / 78.40M)
        .WithExchangeRate(Currency.USD, Currency.CHF, 663.11M / 683.58M)
        .WithExchangeRate(Currency.USD, Currency.JPY, 663.11M / 5.9740M)
        .WithExchangeRate(Currency.USD, Currency.DKK, 663.11M / 100M)

        .WithExchangeRate(Currency.GBP, Currency.SEK, 852.85M / 76.10M)
        .WithExchangeRate(Currency.GBP, Currency.NOK, 852.85M / 78.40M)
        .WithExchangeRate(Currency.GBP, Currency.CHF, 852.85M / 683.58M)
        .WithExchangeRate(Currency.GBP, Currency.JPY, 852.85M / 5.9740M)
        .WithExchangeRate(Currency.GBP, Currency.DKK, 852.85M / 100M)

        .WithExchangeRate(Currency.SEK, Currency.NOK, 76.10M / 78.40M)
        .WithExchangeRate(Currency.SEK, Currency.CHF, 76.10M / 683.58M)
        .WithExchangeRate(Currency.SEK, Currency.JPY, 76.10M / 5.9740M)
        .WithExchangeRate(Currency.SEK, Currency.DKK, 76.10M / 100M)

        .WithExchangeRate(Currency.NOK, Currency.CHF, 78.40M / 683.58M)
        .WithExchangeRate(Currency.NOK, Currency.JPY, 78.40M / 5.9740M)
        .WithExchangeRate(Currency.NOK, Currency.DKK, 78.40M / 100M)

        .WithExchangeRate(Currency.CHF, Currency.JPY, 683.58M / 5.9740M)
        .WithExchangeRate(Currency.CHF, Currency.DKK, 683.58M / 100M)

        .WithExchangeRate(Currency.JPY, Currency.DKK, 5.9740M / 100M)
        .Build();

    public Task<decimal> GetExchangeRateAsync(Currency mainCurrency, Currency targetCurrency)
    {
        return _exchangeRateStore.TryGetValue((mainCurrency, targetCurrency), out var exchangeRate) ?
            Task.FromResult(exchangeRate) :
            throw new ArgumentException("Exchange rate not found.");
    }

    private class ExchangeRateStoreBuilder
    {
        private readonly Dictionary<(Currency Main, Currency Target), decimal> _exchangeRates = new();

        /// <summary>
        /// Adds exchange rate in both ways, to reduce number of exchange rates to be added manually.
        /// </summary>
        public ExchangeRateStoreBuilder WithExchangeRate(Currency mainCurrency, Currency targetCurrency, decimal exchangeRate)
        {
            _exchangeRates[(mainCurrency, targetCurrency)] = exchangeRate;
            _exchangeRates[(targetCurrency, mainCurrency)] = 1M / exchangeRate;
            return this;
        }

        public FrozenDictionary<(Currency Main, Currency Target), decimal> Build()
        {
            return _exchangeRates.ToFrozenDictionary();
        }
    }
}