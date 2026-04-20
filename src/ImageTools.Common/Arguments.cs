using ImageTools.Interfaces;

namespace ImageTools.Common;

/// <summary>
/// Handles CLI arguments parsing and validation.
/// </summary>
/// <typeparam name="T">Options name.</typeparam>
public class Arguments<T>
    where T : Enum
{
    private readonly List<Option<T>> _options = new List<Option<T>>();
    private readonly List<string[]> _exclusiveGroups = new List<string[]>();

    /// <summary>
    /// Adds a possible options to the processor.
    /// </summary>
    /// <param name="options">The option definitions.</param>
    public void AddOptions(params Option<T>[] options)
    {
        _options.AddRange(options);
    }

    /// <summary>
    /// Defines a group of options where exactly one must be present.
    /// </summary>
    /// <param name="optionNames">Array of primary names of options.</param>
    public void AddExclusiveGroup(params string[] optionNames)
    {
        _exclusiveGroups.Add(optionNames);
    }

    /// <summary>
    /// Processes command line arguments.
    /// </summary>
    /// <param name="args">Raw arguments from CLI.</param>
    /// <returns>Array of populated options.</returns>
    public ICollection<Option<T>> Process(ICollection<string> args)
    {
        if (args is null || args.Count is 0)
        {
            throw new ImageToolsException(
                ImageToolsExceptionCode.NoArgumentsProvided,
                "Empty arguments.");
        }

        ParseArgs(args.ToArray());
        ValidateRequirements();
        return SetOptionsDefaultValues().ToArray();
    }

    private IEnumerable<Option<T>> SetOptionsDefaultValues()
    {
        foreach (var o in _options)
        {
            if (o.Value is not null)
            {
                yield return o;
                continue;
            }

            if (o.DefaultValue is not null)
            {
               o.Value = o.DefaultValue;
               yield return o;
            }
        }
    }

    private void ParseArgs(string[] args)
    {
        for (var i = 0; i < args.Length; i++)
        {
            var opt = FindOption(args[i]);
            if (opt is null)
            {
                throw new ImageToolsException(
                    ImageToolsExceptionCode.UnknownArgument,
                    $"Unknown argument {args[i]}.");
            }

            var val = (opt.ValueType is not OptionValueType.Boolean && i + 1 < args.Length)
                ? args[++i]
                : null;

            opt.Validate(val);
        }
    }

    private Option<T>? FindOption(string name)
    {
        return _options.FirstOrDefault(o => o.Names.Contains(name));
    }

    private void ValidateRequirements()
    {
        var opts = _options.Where(o => o.IsRequired && o.Value == null);
        foreach (var opt in opts)
        {
            throw new ImageToolsException(
                ImageToolsExceptionCode.MissingRequiredArgument,
                $"Required option {opt.Names[0]} is missing.");
        }

        foreach (var group in _exclusiveGroups)
        {
            var count = _options
                .Count(o => group.Contains(o.Names[0]) && o.Value != null);

            if (count is not 1)
            {
                throw new ImageToolsException(
                    ImageToolsExceptionCode.InvalidWorkMode,
                    $"Exactly one of these must be set: {string.Join(", ", group)}");
            }
        }
    }
}
