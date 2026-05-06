namespace CiccioSoft.Data.MariaDbEmbedded.Interop.Example;

internal static class ConsoleOutput
{
    internal static void Section(string title) => Console.WriteLine($"\n=== {title} ===");

    internal static void Message(string message) => Console.WriteLine($"  {message}");

    internal static void KeyValue(string key, object? value) => Console.WriteLine($"  {key,-8}: {value}");
}
