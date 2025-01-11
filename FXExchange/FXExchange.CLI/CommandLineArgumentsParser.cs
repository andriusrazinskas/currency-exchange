namespace FXExchange.CLI;

internal interface ICommandLineArgumentsParser
{
    CurrencyExchangeRequest Parse(string[] arguments);
}

internal class CommandLineArgumentsParser : ICommandLineArgumentsParser
{
    public CurrencyExchangeRequest Parse(string[] args)
    {
        throw new NotImplementedException();
    }
}