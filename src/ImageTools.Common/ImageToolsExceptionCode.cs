namespace ImageTools.Common;

/// <summary>
/// Specifies the types of errors that can occur during image processing.
/// </summary>
public enum ImageToolsExceptionCode
{
    /// <summary>
    /// Indicates that the specified file was not found.
    /// </summary>
    FileNotFound,

    /// <summary>
    /// Indicates that the file format is not supported.
    /// </summary>
    UnsupportedFormat,

    /// <summary>
    /// Error on parsing input argument.
    /// </summary>
    ArgumentParseError,

    /// <summary>
    /// Argument was out of range error.
    /// </summary>
    ArgumentOutOfRangeError,

    /// <summary>
    /// Invalid argument value.
    /// </summary>
    InvalidArgumentValue,

    /// <summary>
    /// Empty arguments list provided.
    /// </summary>
    NoArgumentsProvided,

    /// <summary>
    /// Unknown argument.
    /// </summary>
    UnknownArgument,

    /// <summary>
    /// Argument is missing but required.
    /// </summary>
    MissingRequiredArgument,

    /// <summary>
    /// Invalid work mode.
    /// </summary>
    InvalidWorkMode,

    /// <summary>
    /// Unknown option.
    /// </summary>
    OptionNotFound,

    /// <summary>
    /// Indicates an error occurred during the saving process.
    /// </summary>
    SaveFailure,
}
