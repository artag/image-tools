using ImageTools.Interfaces;

namespace ImageTools.Square;

/// <summary>
/// Calculations methods.
/// </summary>
internal static class Calculations
{
    /// <summary>
    /// Determines whether the processing should be skipped based on
    /// the difference between width and height.
    /// </summary>
    /// <param name="width">The width of the object.</param>
    /// <param name="height">The height of the object.</param>
    /// <param name="threshold">The difference threshold (1 to 100).</param>
    /// <returns>
    /// True if the absolute difference is less than the threshold; otherwise, false.
    /// </returns>
    /// <exception cref="ImageToolsException">Threshold is not between 1 and 100.</exception>
    public static bool SkipProcessing(int width, int height, int threshold)
    {
        if (threshold < 1 || threshold > 100)
        {
            throw new ImageToolsException(
                ImageToolsExceptionCode.ArgumentOutOfRangeError,
                "Threshold value must be between 1 and 100.");
        }

        var difference = Math.Abs(width - height);
        return difference < threshold;
    }
}
