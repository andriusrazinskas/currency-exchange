﻿namespace FXExchange.Core;

public interface ICurrencyConverter
{
    Task<decimal> ConvertAsync(Currency mainCurrency, Currency targetCurrency, decimal amountToConvert);
}

internal class CurrencyConverter(ICurrencyExchangeRateProvider _currencyExchangeRateProvider) : ICurrencyConverter
{
    public async Task<decimal> ConvertAsync(Currency mainCurrency, Currency targetCurrency, decimal amountToConvert)
    {
        if (amountToConvert < 0)
        {
            throw new ArgumentException("Amount to exchange cannot be negative.");
        }

        if (mainCurrency == targetCurrency)
        {
            return amountToConvert;
        }

        var exchangeRate = await _currencyExchangeRateProvider.GetExchangeRateAsync(mainCurrency, targetCurrency).ConfigureAwait(false);

        return amountToConvert * exchangeRate;
    }
}