namespace ImageTools.Common;

/// <summary>
/// Option for boolean flags.
/// </summary>
public record BooleanOption() : Option(OptionType.Boolean)
{
    /// <inheritdoc/>
    public override void Validate(string? input)
    {
        Value = true;
    }
}
