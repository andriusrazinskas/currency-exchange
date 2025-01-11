using FXExchange.Core;

namespace FXExchange.CLI;

internal record CurrencyExchangeDetails(Currency MainCurrency, Currency TargetCurrency, decimal AmountToExchange);