using Microsoft.Extensions.DependencyInjection;

namespace FXExchange.CLI;

internal class Program
{
    public static async Task<int> Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddCliDependencies()
            .BuildServiceProvider();

        try
        {
            var commandLineArgumentsParser = serviceProvider.GetRequiredService<ICommandLineArgumentsParser>();

            var request = commandLineArgumentsParser.Parse(args);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            return -1;
        }

        return 0;
    }
}