using System.Globalization;
using ImageTools.Interfaces;

namespace ImageTools.Common;

/// <summary>
/// Base record representing a command line option.
/// </summary>
/// <param name="OptionName">Option name.</param>
/// <param name="ValueType">Option value type.</param>
/// <param name="DefaultValue">Optional default value.</param>
public abstract record Option(
    string OptionName,
    OptionValueType ValueType,
    object? DefaultValue = default)
{
    /// <summary>
    /// Gets the list of alternative names (e.g., "-f", "--file").
    /// </summary>
    public string[] Names { get; init; } = Array.Empty<string>();

    /// <summary>
    /// Gets the description for help text.
    /// </summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// Gets a value indicating whether this option is mandatory.
    /// </summary>
    public bool IsRequired { get; init; }

    /// <summary>
    /// Gets or sets the raw value associated with this option.
    /// </summary>
#pragma warning disable CA1721 // Property names should not match get methods
    public object? Value { get; set; }
#pragma warning restore CA1721 // Property names should not match get methods

    /// <summary>
    /// Group name of options where exactly one must be present.
    /// </summary>
    public string? GroupName { get; set; }

    /// <summary>
    /// Gets a value with casting.
    /// </summary>
    /// <typeparam name="T">Value type.</typeparam>
    /// <returns>Value.</returns>
    public T GetValue<T>()
    {
        try
        {
            var value = CastedValue;
            if (value is null)
            {
                throw new ImageToolsException(
                    ImageToolsExceptionCode.InvalidArgumentValue,
                    $"Argument '{OptionName}' has null value.");
            }

            return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
        }
        catch (Exception ex)
        {
            throw new ImageToolsException(
                ImageToolsExceptionCode.InvalidArgumentValue,
                $"Argument '{OptionName}' has invalid value type. {ex.Message}");
        }
    }

    private object? CastedValue => ValueType switch
    {
        OptionValueType.Boolean => Convert.ToBoolean(Value, CultureInfo.InvariantCulture),
        OptionValueType.Integer => Convert.ToInt32(Value, CultureInfo.InvariantCulture),
        OptionValueType.Float => Convert.ToDouble(Value, CultureInfo.InvariantCulture),
        OptionValueType.String => Value?.ToString(),
        _ => Value
    };

    /// <summary>
    /// Validates and parses the input string value.
    /// </summary>
    /// <param name="input">The string value from command line.</param>
    /// <exception cref="ImageToolsException">Thrown when validation fails.</exception>
    public abstract void Validate(string? input);

    /// <summary>
    /// Formats option information for logging and help.
    /// </summary>
    /// <returns>A formatted string with names and constraints.</returns>
    public virtual string ToLog()
    {
        var namesStr = string.Join(", ", Names);
        var required = IsRequired ? "Required." : "Optional.";
        var info = $"{namesStr,-16} {required} {Description}";
        var defaultValue = DefaultValue is null
            ? string.Empty
            : $"Default value: {DefaultValue}";

        return string.IsNullOrEmpty(defaultValue)
            ? info
            : $"{info} {defaultValue}";
    }
}
