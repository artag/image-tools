using ImageTools.Interfaces;

namespace ImageTools.Common;

/// <summary>
/// A collection of initialized <see cref="Option"/>.
/// </summary>
public class Options
{
    private readonly Dictionary<string, Option> _allOptions =
        new Dictionary<string, Option>(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Initializes a new instance of the <see cref="Options"/> class.
    /// </summary>
    /// <param name="allOptions">Initialized options.</param>
    public Options(ICollection<Option> allOptions)
    {
        _allOptions = allOptions.ToDictionary(o => o.OptionName, o => o);
    }

    /// <summary>
    /// Initialized options <see cref="Option"/>.
    /// </summary>
    public IEnumerable<Option> AllOptions => _allOptions.Values;

    /// <summary>
    /// Gets option by type.<br/>
    /// Throw exception if option is not found.
    /// </summary>
    /// <param name="optionNames">Option name(s).</param>
    /// <returns>Option.</returns>
    /// <exception cref="ImageToolsException">Option is not found.</exception>
    public Option GetOption(params string[] optionNames)
    {
        ArgumentNullException.ThrowIfNull(optionNames);

        if (optionNames.Length > 1)
        {
            var set = optionNames.ToHashSet(StringComparer.OrdinalIgnoreCase);
            var opts = _allOptions
                .Where(option => set.Contains(option.Key, StringComparer.OrdinalIgnoreCase));
            foreach (var opt in opts)
            {
                return opt.Value;
            }
        }
        else if (_allOptions.TryGetValue(optionNames[0], out var value))
        {
            return value;
        }

        var names = string.Join(",", optionNames);
        throw new ImageToolsException(
            ImageToolsExceptionCode.OptionNotFound,
            $"Option is not found by name '{names}'.");
    }
}
