using ImageTools.Interfaces;

namespace ImageTools.Implementation;

/// <summary>
/// File system operations.
/// </summary>
public class FileSystem : IFileSystem
{
    /// <inheritdoc/>
    public bool FileExists(string path) =>
        File.Exists(path);

    /// <inheritdoc/>
    public string GetCurrentDirectory()
    {
        try
        {
            return AppContext.BaseDirectory;
        }
        catch (ImageToolsException)
        {
            throw;
        }
        catch (Exception ex)
        {
            var code = ImageToolsExceptionCode.GetCurrentDirectoryError;
            throw new ImageToolsException(code, ex.Message);
        }
    }

    /// <inheritdoc/>
    public ICollection<string> GetFiles(string searchPattern)
    {
        try
        {
            var path = GetCurrentDirectory();
            var option = SearchOption.TopDirectoryOnly;
            return Directory.GetFiles(path, searchPattern, option);
        }
        catch (ImageToolsException)
        {
            throw;
        }
        catch (Exception ex)
        {
            var code = ImageToolsExceptionCode.GetFilesFromDirectoryError;
            throw new ImageToolsException(code, ex.Message);
        }
    }

    /// <inheritdoc/>
    public ICollection<string> GetFilesRecursive(string searchPattern)
    {
        try
        {
            var path = GetCurrentDirectory();
            var option = SearchOption.AllDirectories;
            return Directory.GetFiles(path, searchPattern, option);
        }
        catch (ImageToolsException)
        {
            throw;
        }
        catch (Exception ex)
        {
            var code = ImageToolsExceptionCode.GetFilesFromDirectoryRecursive;
            throw new ImageToolsException(code, ex.Message);
        }
    }

    /// <inheritdoc/>
    public void CopyFile(string sourcePath, string destinationPath)
    {
        try
        {
            File.Copy(sourcePath, destinationPath, overwrite: true);
        }
        catch (ImageToolsException)
        {
            throw;
        }
        catch (Exception ex)
        {
            var code = ImageToolsExceptionCode.CopyFileError;
            throw new ImageToolsException(code, ex.Message);
        }
    }
}
