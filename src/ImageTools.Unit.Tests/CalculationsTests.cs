using ImageTools.Interfaces;
using ImageTools.Square;

namespace ImageTools.Unit.Tests;

public class CalculationsTests
{
    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(101)]
    public void Threshold_has_wrong_value(int threshold)
    {
        var ex = Assert.Throws<ImageToolsException>(
            () => Calculations.SkipProcessing(600, 600, threshold));
        Assert.Equal(ImageToolsExceptionCode.ArgumentOutOfRangeError, ex.Code);
    }

    [Theory]
    [InlineData(600, 600, 1)]
    [InlineData(600, 600, 10)]
    [InlineData(1000, 1000, 20)]
    public void Width_equals_height_Skip_processing(
        int width, int height, int threshold)
    {
        var actual = Calculations.SkipProcessing(width, height, threshold);
        Assert.True(actual);
    }

    [Theory]
    [InlineData(599, 600, 2)]
    [InlineData(591, 600, 10)]
    [InlineData(981, 1000, 20)]
    public void Width_little_less_than_height_Skip_processing(
        int width, int height, int threshold)
    {
        var actual = Calculations.SkipProcessing(width, height, threshold);
        Assert.True(actual);
    }

    [Theory]
    [InlineData(601, 600, 2)]
    [InlineData(609, 600, 10)]
    [InlineData(1019, 1000, 20)]
    public void Width_little_greater_than_height_Skip_processing(
        int width, int height, int threshold)
    {
        var actual = Calculations.SkipProcessing(width, height, threshold);
        Assert.True(actual);
    }

    [Theory]
    [InlineData(590, 600, 1)]
    [InlineData(580, 600, 10)]
    [InlineData(970, 1000, 20)]
    public void Width_much_less_than_height_No_skip_processing(
        int width, int height, int threshold)
    {
        var actual = Calculations.SkipProcessing(width, height, threshold);
        Assert.False(actual);
    }

    [Theory]
    [InlineData(610, 600, 1)]
    [InlineData(620, 600, 10)]
    [InlineData(1030, 1000, 20)]
    public void Width_much_greater_than_height_No_skip_processing(
        int width, int height, int threshold)
    {
        var actual = Calculations.SkipProcessing(width, height, threshold);
        Assert.False(actual);
    }
}
