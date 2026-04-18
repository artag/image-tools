namespace ImageTools.Common;

/// <summary>
/// Defines the data types supported by the command line options.
/// </summary>
public enum OptionType
{
    /// <summary>
    /// Represents a true/false switch.
    /// </summary>
    Boolean,

    /// <summary>
    /// Represents a 32-bit signed integer.
    /// </summary>
    Integer,

    /// <summary>
    /// Represents a variable-length string.
    /// </summary>
    String,

    /// <summary>
    /// Represents a double-precision floating point number.
    /// </summary>
    Float,
}
