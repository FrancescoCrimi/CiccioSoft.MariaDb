namespace CiccioSoft.Data.MariaDbEmbedded.Interop.Example;

internal static class ErrorHandlingPolicy
{
    internal static int Run(string operationName, Action action)
    {
        try
        {
            action();
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[ERROR] {operationName} failed.");
            Console.Error.WriteLine(ex.Message);
            return 1;
        }
    }
}
