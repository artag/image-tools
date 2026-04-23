using ImageTools.Common;
using ImageTools.Implementation;
using ImageTools.Interfaces;
using SkiaSharp;

namespace ImageTools.Square;

/// <summary>
/// Square image.
/// </summary>
/// <param name="versionProvider">Application version provider.</param>
/// <param name="fileSystem">Common file system operations.</param>
/// <param name="log">Logger.</param>
internal sealed class Program(
    IVersionProvider versionProvider,
    IFileSystem fileSystem,
    ILog log)
    : BaseProgram("Square image", versionProvider, fileSystem, log)
{
    private int _threshold;
    private int _blur;
    private int _shade;
    private int _quality;

    /// <summary>
    /// Entry application point.
    /// </summary>
    /// <param name="args">Input arguments from CLI.</param>
    /// <returns>Asynchronous operation.</returns>
    public static Task Main(string[] args)
    {
        var versionProvider = new VersionProvider();
        var fileSystem = new FileSystem();
        var log = new ConsoleLog();
        var app = new Program(versionProvider, fileSystem, log);
        var options = CreateOptions();
        return app.Run(args, options);
    }

    /// <inheritdoc/>
    protected override Task<bool> Process(string path, Options options)
    {
        var processed = false;

        try
        {
            LoadOptions(options);
            processed = Square(path);
        }
        catch (ImageToolsException)
        {
            throw;
        }
        catch (Exception ex)
        {
            var code = ImageToolsExceptionCode.ErrorOnImageProcessing;
            throw new ImageToolsException(code, ex.Message);
        }

        return Task.FromResult(processed);
    }

#pragma warning disable SP2101 // Method is too long
    private static Option[] CreateOptions() =>
#pragma warning restore SP2101 // Method is too long
        [
            new IntegerOption(SquareOptions.Threshold.ToString())
            {
                Names = ["-t", "--threshold"],
                Description = "Threshold to process. " +
                              "The minimal difference between image width and height.",
                Min = 1,
                Max = 99,
                DefaultValue = 20,
            },
            new IntegerOption(SquareOptions.Blurring.ToString())
            {
                Names = ["-b", "--blur"],
                Description = "Blurring degree.",
                Min = 1,
                Max = 99,
                DefaultValue = 25,
            },
            new IntegerOption(SquareOptions.Shading.ToString())
            {
                Names = ["-s", "--shade"],
                Description = "Shading degree.",
                Min = 0,
                Max = 255,
                DefaultValue = 128,
            },
            new IntegerOption(SquareOptions.Quality.ToString())
            {
                Names = ["-q", "--quality"],
                Description = "Processed image quality.",
                Min = 1,
                Max = 100,
                DefaultValue = 90,
            },
            new BooleanOption(SquareOptions.MakeBackup.ToString())
            {
                Names = ["-b", "--backup"],
                Description = "Make source image backup.",
                DefaultValue = false,
            },
        ];

    private void LoadOptions(Options options)
    {
        _threshold = options.GetOption(nameof(SquareOptions.Threshold)).GetValue<int>();
        _blur = options.GetOption(nameof(SquareOptions.Blurring)).GetValue<int>();
        _shade = options.GetOption(nameof(SquareOptions.Shading)).GetValue<int>();
        _quality = options.GetOption(nameof(SquareOptions.Quality)).GetValue<int>();
    }

    private bool Square(string inputPath)
    {
        using var inputStream = File.OpenRead(inputPath);
        using var originalBitmap = SKBitmap.Decode(inputStream);
        var (width, height) = (originalBitmap.Width, originalBitmap.Height);

        var skip = Calculations.SkipProcessing(width, height, _threshold);
        if (skip)
        {
            Log.LogInfo($"Skip: '{inputPath}'");
            return false;
        }

        var size = Math.Max(width, height);
        using var surface = SKSurface.Create(new SKImageInfo(size, size));

        var canvas = surface.Canvas;
        var destRect = new SKRect(0, 0, size, size);
        DrawBlurredBackground(originalBitmap, canvas, destRect);
        ShadeBackground(canvas, destRect);
        PlaceOriginalImageToCentre(originalBitmap, canvas, size);
        SaveChangedImage(inputPath, surface);
        return true;
    }

    private void DrawBlurredBackground(SKBitmap originalBitmap, SKCanvas canvas, SKRect rectangle)
    {
        using var paint = new SKPaint();
        paint.ImageFilter = SKImageFilter.CreateBlur(_blur, _blur);
        canvas.DrawBitmap(originalBitmap, rectangle, paint);
    }

    private void ShadeBackground(SKCanvas canvas, SKRect destRect)
    {
        using var darkenPaint = new SKPaint();
        darkenPaint.Color = SKColors.Black.WithAlpha((byte)_shade);
        canvas.DrawRect(destRect, darkenPaint);
    }

#pragma warning disable SA1204 // Static elements should appear before instance elements
    private static void PlaceOriginalImageToCentre(
        SKBitmap originalBitmap, SKCanvas canvas, int size)
#pragma warning restore SA1204 // Static elements should appear before instance elements
    {
        var left = (size - originalBitmap.Width) / 2f;
        var top = (size - originalBitmap.Height) / 2f;
        canvas.DrawBitmap(originalBitmap, left, top);
    }

    private void SaveChangedImage(string inputPath, SKSurface surface)
    {
        using var image = surface.Snapshot();
        using var data = image.Encode(SKEncodedImageFormat.Jpeg, _quality);
        using var outputStream = File.OpenWrite(inputPath);
        data.SaveTo(outputStream);
    }
}
