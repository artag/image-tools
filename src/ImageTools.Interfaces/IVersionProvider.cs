namespace ImageTools.Interfaces;

/// <summary>
/// Provides application version as string.
/// </summary>
public interface IVersionProvider
{
    /// <summary>
    /// Gets application version.<br/>
    /// Return empty string if version can not be get.
    /// </summary>
    /// <returns>Version as string.</returns>
    string GetVersion();
}
