namespace ImageTools.Interfaces;

/// <summary>
/// Common file system operations.
/// </summary>
public interface IFileSystem
{
    /// <summary>
    /// Checks if the file exists at the specified path.
    /// </summary>
    /// <param name="path">the path to the file.</param>
    /// <returns>True if the file exists; otherwise, false.</returns>
    bool FileExists(string path);

    /// <summary>
    /// Gets the absolute path to the current application directory.
    /// </summary>
    /// <returns>The current directory path.</returns>
    /// <exception cref="ImageToolsException">Thrown when directory access fails.</exception>
    string GetCurrentDirectory();

    /// <summary>
    /// Retrieves files from the current directory matching the pattern.
    /// </summary>
    /// <param name="searchPattern">The search string (e.g., "*.jpg").</param>
    /// <returns>A collection of file paths.</returns>
    /// <exception cref="ImageToolsException">Thrown if search fails.</exception>
    ICollection<string> GetFiles(string searchPattern);

    /// <summary>
    /// Retrieves files from the current directory and subdirectories.
    /// </summary>
    /// <param name="searchPattern">The search string (e.g., "*.png").</param>
    /// <returns>A collection of file paths.</returns>
    /// <exception cref="ImageToolsException">Thrown if recursive search fails.</exception>
    ICollection<string> GetFilesRecursive(string searchPattern);

    /// <summary>
    /// Copies an existing file to a new location.
    /// </summary>
    /// <param name="sourcePath">The path of the file to copy.</param>
    /// <param name="destinationPath">The path to the new file.</param>
    /// <exception cref="ImageToolsException">Thrown if copy process fails.</exception>
    void CopyFile(string sourcePath, string destinationPath);
}
