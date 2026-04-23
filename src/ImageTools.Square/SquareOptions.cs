namespace ImageTools.Square;

/// <summary>
/// Square option types.
/// </summary>
internal enum SquareOptions
{
    /// <summary>
    /// Make backup.
    /// </summary>
    MakeBackup,

    /// <summary>
    /// Blurring degree.
    /// </summary>
    Blurring,

    /// <summary>
    /// Shading degree.
    /// </summary>
    Shading,

    /// <summary>
    /// Image quality.
    /// </summary>
    Quality,

    /// <summary>
    /// Threshold to process. The minimal difference between image width and height.
    /// </summary>
    Threshold,
}
