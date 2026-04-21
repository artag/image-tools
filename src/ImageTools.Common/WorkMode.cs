namespace ImageTools.Common;

/// <summary>
/// Application work mode.
/// </summary>
public static class WorkMode
{
    /// <summary>
    /// Process single file.
    /// </summary>
    public const string ProcessFile = "ProcessFile";

    /// <summary>
    /// Process files in current (single) directory.
    /// </summary>
    public const string ProcessFilesInDirectory = "ProcessFilesInDirectory";

    /// <summary>
    /// Process files in currenct directory and subdirectories.
    /// </summary>
    public const string ProcessFilesInDirectortyRecursive = "ProcessFilesInDirectortyRecursive";
}
