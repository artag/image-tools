using System.Text.RegularExpressions;

namespace ImageTools.Common;

/// <summary>
/// Option for string values with file system character validation.
/// </summary>
public record StringOption() : Option(OptionType.String)
{
    private static char[] GetInvalidChars()
    {
        var ipc = Path.GetInvalidPathChars();
        var ifc = Path.GetInvalidFileNameChars().Except([':', '\\', '/']);
        char[] ic = [.. ipc, .. ifc];
        return ic;
    }

    /// <summary>
    /// Validates that input is not empty and contains valid file system characters.
    /// </summary>
    /// <param name="input">Input value.</param>
    public override void Validate(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            throw new ImageToolsException(
                ImageToolsExceptionCode.InvalidArgumentValue,
                $"Option {Names[0]} cannot be empty.");
        }

        var pattern = "[" + Regex.Escape(new string(GetInvalidChars())) + "]";
        if (Regex.IsMatch(input, pattern))
        {
            throw new ImageToolsException(
                ImageToolsExceptionCode.InvalidArgumentValue,
                $"Option {Names[0]} contains invalid characters '{input}'.");
        }

        Value = input;
    }
}
