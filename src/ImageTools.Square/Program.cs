namespace ImageTools.Square;

/// <summary>
/// Square image.
/// </summary>
internal static class Program
{
    /// <summary>
    /// Entty application point.
    /// </summary>
    /// <param name="args">Input arguments from CLI.</param>
    public static void Main(string[] args)
    {
        if (args.Length is 0)
        {
            Console.WriteLine("Hello, World!");
        }
        else
        {
            var str = string.Join(',', args);
            Console.WriteLine(str);
        }
    }
}
