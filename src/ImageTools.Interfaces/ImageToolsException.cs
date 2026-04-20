#pragma warning disable CA1032 // Implement standard exception constructors
#pragma warning disable RCS1194 // Implement exception constructors

namespace ImageTools.Interfaces;

/// <summary>
/// Represents errors that occur during image tool operations.
/// </summary>
public class ImageToolsException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ImageToolsException"/> class.
    /// </summary>
    /// <param name="code">The error code from ImageToolsExceptionCode.</param>
    /// <param name="message">The error description.</param>
    public ImageToolsException(ImageToolsExceptionCode code, string message)
        : base(message)
    {
        Code = code;
    }

    /// <summary>
    /// Gets the specific error code associated with this exception.
    /// </summary>
    public ImageToolsExceptionCode Code { get; }
}

#pragma warning restore CA1032 // Implement standard exception constructors
#pragma warning restore RCS1194 // Implement exception constructors
