using ImageTools.Common;

namespace ImageTools.Unit.Tests;

/// <summary>
/// Tests for <see cref="Arguments{T}"/>.
/// </summary>
public class ArgumentsTests
{
    private readonly Arguments<TestOptions> _sut;

    /// <summary>
    /// Initializes a new instance of the <see cref="ArgumentsTests"/> class.
    /// </summary>
    public ArgumentsTests()
    {
        _sut = new Arguments<TestOptions>();
        SetupStandardOptions();
    }

    [Theory]
    [InlineData("0")]
    [InlineData("5")]
    public void Integer_below_min_Throws_exception(string value)
    {
        string[] args = ["-i", $"{value}", "-f", "test.jpg"];
        var ex = Assert.Throws<ImageToolsException>(() => _sut.Process(args));
        Assert.Equal(ImageToolsExceptionCode.ArgumentOutOfRangeError, ex.Code);
    }

    [Theory]
    [InlineData("25")]
    public void Integer_above_max_Throws_exception(string value)
    {
        string[] args = ["-i", $"{value}", "-f", "test.jpg"];
        var ex = Assert.Throws<ImageToolsException>(() => _sut.Process(args));
        Assert.Equal(ImageToolsExceptionCode.ArgumentOutOfRangeError, ex.Code);
    }

    [Theory]
    [InlineData("15.0")]
    [InlineData("abc")]
    public void Invalid_integer_value_Throws_exception(string value)
    {
        string[] args = ["-i", $"{value}", "-f", "test.jpg"];
        var ex = Assert.Throws<ImageToolsException>(() => _sut.Process(args));
        Assert.Equal(ImageToolsExceptionCode.ArgumentParseError, ex.Code);
    }

    [Theory]
    [InlineData("1")]
    [InlineData("1.0")]
    [InlineData("1,0")]
    public void Float_below_min_Throws_exception(string value)
    {
        string[] args = ["-d", $"{value}", "-f", "test.jpg"];
        var ex = Assert.Throws<ImageToolsException>(() => _sut.Process(args));
        Assert.Equal(ImageToolsExceptionCode.ArgumentOutOfRangeError, ex.Code);
    }

    [Theory]
    [InlineData("6")]
    [InlineData("6.0")]
    [InlineData("6,0")]
    public void Float_above_max_Throws_exception(string value)
    {
        string[] args = ["-d", $"{value}", "-f", "/home/images/test.jpg"];
        var ex = Assert.Throws<ImageToolsException>(() => _sut.Process(args));
        Assert.Equal(ImageToolsExceptionCode.ArgumentOutOfRangeError, ex.Code);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("6.0.0")]
    [InlineData("1,x2")]
    public void Invalid_float_string_Throws_exception(string value)
    {
        string[] args = ["-d", $"{value}", "-f", "d:\\dir\\test.jpg"];
        var ex = Assert.Throws<ImageToolsException>(() => _sut.Process(args));
        Assert.Equal(ImageToolsExceptionCode.ArgumentParseError, ex.Code);
    }

    [Theory]
    [InlineData("file|name.txt")]
    [InlineData("file*some.jpg")]
    [InlineData("file\"Some\".jpg")]
    public void String_with_invalid_chars_Throws_exception(string value)
    {
        string[] args = ["-s", $"{value}", "-f", "ok.jpg"];
        var ex = Assert.Throws<ImageToolsException>(() => _sut.Process(args));
        Assert.Equal(ImageToolsExceptionCode.InvalidArgumentValue, ex.Code);
    }

    [Fact]
    public void Missing_required_option_Throws_exception()
    {
        _sut.AddOptions(new StringOption<TestOptions>(TestOptions.Test)
        {
            Names = ["-req"],
            IsRequired = true,
        });
        string[] args = ["-f", "test.jpg"];

        var ex = Assert.Throws<ImageToolsException>(() => _sut.Process(args));

        Assert.Equal(ImageToolsExceptionCode.MissingRequiredArgument, ex.Code);
    }

    [Fact]
    public void No_arguments_Throws_exception()
    {
        string[] args = Array.Empty<string>();
        var ex = Assert.Throws<ImageToolsException>(() => _sut.Process(args));
        Assert.Equal(ImageToolsExceptionCode.NoArgumentsProvided, ex.Code);
    }

    [Fact]
    public void Multiple_exclusive_modes_Throws_exception()
    {
        string[] args = ["-f", "file.jpg", "-dir", "C:\\temp"];
        var ex = Assert.Throws<ImageToolsException>(() => _sut.Process(args));
        Assert.Equal(ImageToolsExceptionCode.InvalidWorkMode, ex.Code);
    }

    [Fact]
    public void Successfully_process_arguments()
    {
        string[] args = ["-i", "15", "-d", "3.5", "-s", "valid.txt", "-bt", "-f", "a.jpg"];

        var actual = _sut.Process(args);

        Assert.Equal(5, actual.Count);
        Assert.Equal(15, actual.First(o => o.Names.Contains("-i")).Value);
        Assert.Equal(3.5, actual.First(o => o.Names.Contains("-d")).Value);
        Assert.Equal("valid.txt", actual.First(o => o.Names.Contains("-s")).Value!);
        Assert.Equal("a.jpg", actual.First(o => o.Names.Contains("-f")).Value!);
        Assert.True((bool)actual.First(o => o.Names.Contains("-bt")).Value!);
    }

    [Fact]
    public void Use_default_values()
    {
        string[] args = ["-f", "/home/images/a.jpg"];

        var actual = _sut.Process(args);

        Assert.Equal(5, actual.Count);
        Assert.Equal(11, actual.First(o => o.Names.Contains("-i")).Value);
        Assert.Equal(1.6, actual.First(o => o.Names.Contains("-d")).Value);
        Assert.Equal("unknown", actual.First(o => o.Names.Contains("-s")).Value!);
        Assert.Equal("/home/images/a.jpg", actual.First(o => o.Names.Contains("-f")).Value!);
        Assert.False((bool)actual.First(o => o.Names.Contains("-bt")).Value!);
    }

    [Fact]
    public void Unknown_parameter_Throws_exception()
    {
        // "-unknown" is not registered in SetupStandardOptions
        string[] args = ["-f", "test.jpg", "-unknown", "value"];
        var ex = Assert.Throws<ImageToolsException>(() => _sut.Process(args));
        Assert.Equal(ImageToolsExceptionCode.UnknownArgument, ex.Code);
    }

    private void SetupStandardOptions()
    {
        var options = new Option<TestOptions>[]
        {
            new IntegerOption<TestOptions>(TestOptions.Test)
            {
                Names = ["-i"],
                Min = 10,
                Max = 20,
                DefaultValue = 11,
            },
            new FloatOption<TestOptions>(TestOptions.Test)
            {
                Names = ["-d"],
                Min = 1.5,
                Max = 5.5,
                DefaultValue = 1.6,
            },
            new StringOption<TestOptions>(TestOptions.Test)
            {
                Names = new[] { "-s" },
                DefaultValue = "unknown",
            },
            new BooleanOption<TestOptions>(TestOptions.Test)
            {
                Names = new[] { "-bt" },
                DefaultValue = false,
            },
            new BooleanOption<TestOptions>(TestOptions.Test)
            {
                Names = new[] { "-bf" },
            },
            new StringOption<TestOptions>(TestOptions.Test)
            {
                Names = new[] { "-dir" },
            },
            new StringOption<TestOptions>(TestOptions.Test)
            {
                Names = new[] { "-f" },
            },
        };

        _sut.AddOptions(options);
        _sut.AddExclusiveGroup("-f", "-dir");
    }
}

public enum TestOptions
{
    /// <summary>
    /// Some test option type.
    /// </summary>
    Test,
}
