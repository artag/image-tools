namespace ImageTools.Common;

/// <summary>
/// Option for boolean flags.
/// </summary>
/// <param name="OptionName">Option name.</param>
public record BooleanOption(string OptionName)
    : Option(OptionName, OptionValueType.Boolean)
{
    /// <inheritdoc/>
    public override void Validate(string? input)
    {
        Value = true;
    }
}
