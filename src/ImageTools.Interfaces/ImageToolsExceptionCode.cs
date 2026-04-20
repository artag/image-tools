namespace ImageTools.Interfaces;

/// <summary>
/// Specifies the types of errors that can occur during image processing.
/// </summary>
public enum ImageToolsExceptionCode
{
    /// <summary>
    /// Error occurred while determining the application directory.
    /// </summary>
    GetCurrentDirectoryError,

    /// <summary>
    /// Error occurred while retrieving files pathes from a single directory.
    /// </summary>
    GetFilesFromDirectoryError,

    /// <summary>
    /// Error occurred during recursive retrieving files pathes.
    /// </summary>
    GetFilesFromDirectoryRecursive,

    /// <summary>
    /// Error occurred while copying a file.
    /// </summary>
    CopyFileError,

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
    /// Indicates an error occurred during the image processing.
    /// </summary>
    ErrorOnImageProcessing,
}
