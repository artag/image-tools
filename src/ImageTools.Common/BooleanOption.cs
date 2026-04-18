namespace ImageTools.Common;

/// <summary>
/// Option for boolean flags.
/// </summary>
/// <param name="OptionName">Option name.</param>
public record BooleanOption<T>(T OptionName)
    : Option<T>(OptionName, OptionType.Boolean)
    where T : Enum
{
    /// <inheritdoc/>
    public override void Validate(string? input)
    {
        Value = true;
    }
}
