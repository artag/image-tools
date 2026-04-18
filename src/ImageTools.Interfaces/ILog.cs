namespace ImageTools.Interfaces;

/// <summary>
/// Provides logging functionality for the application.
/// </summary>
public interface ILog
{
    /// <summary>
    /// Logs an informational message to the console.
    /// </summary>
    /// <param name="message">The message to be logged.</param>
    void LogInfo(string message);

    /// <summary>
    /// Logs an error message to the console with a specific prefix.
    /// </summary>
    /// <param name="message">The error message to be logged.</param>
    void LogError(string message);
}
