namespace ImageTools.Common;

/// <summary>
/// Option for boolean flags.
/// </summary>
/// <param name="OptionType">Option type.</param>
public record BooleanOption<T>(T OptionType)
    : Option<T>(OptionType, OptionValueType.Boolean)
    where T : Enum
{
    /// <inheritdoc/>
    public override void Validate(string? input)
    {
        Value = true;
    }
}
