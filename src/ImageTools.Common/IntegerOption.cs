using System.Globalization;
using ImageTools.Interfaces;

namespace ImageTools.Common;

/// <summary>
/// Option for integer values with range validation.
/// </summary>
/// <param name="OptionType">Option type.</param>
public record IntegerOption<T>(T OptionType)
    : Option<T>(OptionType, OptionValueType.Integer)
    where T : Enum
{
    /// <summary>
    /// Gets the minimum allowed value.
    /// </summary>
    public int Min { get; init; } = int.MinValue;

    /// <summary>
    /// Gets the maximum allowed value.
    /// </summary>
    public int Max { get; init; } = int.MaxValue;

    /// <summary>
    /// Validates that input is a valid integer within the specified range.
    /// </summary>
    /// <param name="input">Input value.</param>
    public override void Validate(string? input)
    {
        var success = int.TryParse(input, CultureInfo.InvariantCulture, out var result);

        if (!success)
        {
            throw new ImageToolsException(
                ImageToolsExceptionCode.ArgumentParseError,
                $"Option {Names[0]} must be an integer.");
        }

        if (result < Min || result > Max)
        {
            throw new ImageToolsException(
                ImageToolsExceptionCode.ArgumentOutOfRangeError,
                $"Option {Names[0]} must be between {Min} and {Max}.");
        }

        Value = result;
    }
}
