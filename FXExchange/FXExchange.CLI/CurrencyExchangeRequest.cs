using FXExchange.Core;

namespace FXExchange.CLI;

internal record CurrencyExchangeRequest(Currency MainCurrency, Currency TargetCurrency, decimal AmountToExchange);