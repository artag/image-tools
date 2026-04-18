namespace ImageTools.Common;

/// <summary>
/// A collection of <see cref="Option{T}"/>.
/// </summary>
/// <typeparam name="T">Option type.</typeparam>
/// <param name="AllOptions">Options.</param>
public class Options<T>(ICollection<Option<T>> AllOptions)
    where T : Enum
{
    /// <summary>
    /// Gets option by type.<br/>
    /// Throw exception if option is not found.
    /// </summary>
    /// <param name="optionType">Option type.</param>
    /// <returns>Option.</returns>
    /// <exception cref="ImageToolsException">Option is not found.</exception>
    public Option<T> GetOption(T optionType)
    {
        var opt = AllOptions.FirstOrDefault(o => o.OptionType.Equals(optionType));
        if (opt is null)
        {
            throw new ImageToolsException(
                ImageToolsExceptionCode.OptionNotFound,
                $"Option '{optionType}' is not found.");
        }

        return opt;
    }
}
