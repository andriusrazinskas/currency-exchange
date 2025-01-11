using FXExchange.Core;

namespace FXExchange.CLI.Tests;

[TestFixture]
internal class CommandLineArgumentsParserTests
{
    private CommandLineArgumentsParser _parser;

    [SetUp]
    public void SetUp()
    {
        _parser = new CommandLineArgumentsParser();
    }

    [TestCase(new[] { "EUR/USD", "1" }, Currency.EUR)]
    [TestCase(new[] { "usd/gbp", "2" }, Currency.USD)]
    [TestCase(new[] { " gbp / sek ", "3" }, Currency.GBP)]
    public void Parse_ParsesMainCurrency_WhenInputIsValid(string[] args, Currency expectedCurrency)
    {
        var details = _parser.Parse(args);

        Assert.That(details.MainCurrency, Is.EqualTo(expectedCurrency));
    }

    [TestCase(new[] { "EUR/USD", "1" }, Currency.USD)]
    [TestCase(new[] { "usd/gbp", "2" }, Currency.GBP)]
    [TestCase(new[] { " gbp / sek ", "3" }, Currency.SEK)]
    public void Parse_ParsesTargetCurrency_WhenInputIsValid(string[] args, Currency expectedCurrency)
    {
        var details = _parser.Parse(args);

        Assert.That(details.TargetCurrency, Is.EqualTo(expectedCurrency));
    }

    [TestCase(new[] { "EUR/USD", "1.12345" }, 1.12345)]
    [TestCase(new[] { "EUR/USD", "1,12345" }, 1.12345)]
    [TestCase(new[] { "USD/GBP", "2" }, 2)]
    public void Parse_ParsesAmountToExchange_WhenInputIsValid(string[] args, decimal expectedAmount)
    {
        var details = _parser.Parse(args);

        Assert.That(details.AmountToExchange, Is.EqualTo(expectedAmount));
    }

    [TestCase(0)]
    [TestCase(1)]
    [TestCase(3)]
    public void Parse_ThrowsException_WhenIncorrectNumberOfArgumentsAreProvided(int argumentsCount)
    {
        var arguments = Enumerable.Range(0, argumentsCount).Select(i => i.ToString()).ToArray();

        var exception = Assert.Throws<ArgumentException>(() => _parser.Parse(arguments));

        Assert.That(exception.Message, Is.EqualTo("Incorrect arguments count. Usage: FXExchange.CLI.exe <currency pair> <amount to exchange>"));
    }

    [Test]
    public void Parse_ThrowsException_WhenMainCurrencyIsIncorrect()
    {
        var arguments = new [] { "EUG/USD", "1.12345" };

        var exception = Assert.Throws<ArgumentException>(() => _parser.Parse(arguments));

        Assert.That(exception.Message, Is.EqualTo("Currencies cannot be parsed."));
    }

    [Test]
    public void Parse_ThrowsException_WhenTargetCurrencyIsIncorrect()
    {
        var arguments = new[] { "EUR/UGD", "1.12345" };

        var exception = Assert.Throws<ArgumentException>(() => _parser.Parse(arguments));

        Assert.That(exception.Message, Is.EqualTo("Currencies cannot be parsed."));
    }

    [Test]
    public void Parse_ThrowsException_WhenCurrenciesPairIsIncorrect()
    {
        var arguments = new[] { "EUR-USD", "1.12345" };

        var exception = Assert.Throws<ArgumentException>(() => _parser.Parse(arguments));

        Assert.That(exception.Message, Is.EqualTo("Currencies cannot be parsed."));
    }

    [Test]
    public void Parse_ThrowsException_WhenAmountToExchangeIsIncorrect()
    {
        var arguments = new[] { "EUR/USD", "1abc" };

        var exception = Assert.Throws<ArgumentException>(() => _parser.Parse(arguments));

        Assert.That(exception.Message, Is.EqualTo("Amount to exchange cannot be parsed."));
    }
}