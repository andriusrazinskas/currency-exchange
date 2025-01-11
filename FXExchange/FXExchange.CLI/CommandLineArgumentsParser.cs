using FXExchange.Core;

namespace FXExchange.CLI;

internal interface ICommandLineArgumentsParser
{
    CurrencyExchangeRequest Parse(string[] arguments);
}

internal class CommandLineArgumentsParser : ICommandLineArgumentsParser
{
    public CurrencyExchangeRequest Parse(string[] args)
    {
        if (args.Length != 2)
        {
            throw new ArgumentException("Incorrect arguments count. Usage: FXExchange.CLI.exe <currency pair> <amount to exchange>");
        }

        return new CurrencyExchangeRequest(
            TryParseMainCurrency(args, out var mainCurrency) ? mainCurrency : throw new ArgumentException("Currencies cannot be parsed."),
            TryParseTargetCurrency(args, out var targetCurrency) ? targetCurrency : throw new ArgumentException("Currencies cannot be parsed."),
            TryParseAmountToExchange(args, out var amountToExchange) ? amountToExchange : throw new ArgumentException("Amount to exchange cannot be parsed."));
    }

    private static bool TryParseMainCurrency(string[] args, out Currency currency)
    {
        currency = Currency.Unknown;

        var firstCurrency = args.FirstOrDefault()?.Split('/').FirstOrDefault()?.Trim();

        if (Enum.TryParse<Currency>(firstCurrency, true, out var result))
        {
            currency = result;
            return true;
        }

        return false;
    }

    private static bool TryParseTargetCurrency(string[] args, out Currency currency)
    {
        currency = Currency.Unknown;

        var secondCurrency = args.FirstOrDefault()?.Split('/').ElementAtOrDefault(1)?.Trim();

        if (Enum.TryParse<Currency>(secondCurrency, true, out var result))
        {
            currency = result;
            return true;
        }

        return false;
    }

    private static bool TryParseAmountToExchange(string[] args, out decimal amountToExchange)
    {
        amountToExchange = 0;

        if (decimal.TryParse(args.ElementAtOrDefault(1)?.Replace(',', '.'), out var result))
        {
            amountToExchange = result;

            // TODO extract this, parser should not be responsible for validating this
            if (amountToExchange < 0)
            {
                throw new ArgumentException("Amount to exchange cannot be negative.");
            }

            return true;
        }

        return false;
    }
}