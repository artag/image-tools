using ImageTools.Interfaces;

namespace ImageTools.Common;

/// <summary>
/// Base record representing a command line option.
/// </summary>
/// <param name="OptionType">Option type.</param>
/// <param name="ValueType">Option value type.</param>
/// <param name="DefaultValue">Optional default value.</param>
public abstract record Option<T>(
    T OptionType,
    OptionValueType ValueType,
    object? DefaultValue = default)
    where T : Enum
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
    public object? Value { get; set; }

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
        var required = IsRequired ? "Required" : "Optional";
        var info = $"{namesStr} : {required}. {Description}";
        var defaultValue = DefaultValue is null
            ? string.Empty
            : $"Default value: {DefaultValue}";

        return string.IsNullOrEmpty(defaultValue)
            ? info
            : $"{info}. {defaultValue}";
    }
}
