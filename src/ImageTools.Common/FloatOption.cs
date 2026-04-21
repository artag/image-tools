using System.Globalization;
using ImageTools.Interfaces;

namespace ImageTools.Common;

/// <summary>
/// Option for floating point values with range validation.
/// </summary>
/// <param name="OptionName">Option name.</param>
public record FloatOption(string OptionName)
    : Option(OptionName, OptionValueType.Float)
{
    /// <summary>
    /// Gets the minimum allowed value.
    /// </summary>
    public double Min { get; init; } = double.MinValue;

    /// <summary>
    /// Gets the maximum allowed value.
    /// </summary>
    public double Max { get; init; } = double.MaxValue;

    /// <summary>
    /// Validates that input is a valid double within the specified range.
    /// </summary>
    /// <param name="input">Input value.</param>
    public override void Validate(string? input)
    {
        var success = double.TryParse(input, CultureInfo.InvariantCulture, out var result);
        if (!success)
        {
            throw new ImageToolsException(
                ImageToolsExceptionCode.ArgumentParseError,
                $"Option {Names[0]} must be a float.");
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
