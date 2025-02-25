using System;
using System.IO;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Metadata;

public static async Task<string> DownScaleImageAsync(string imagePath, int maxWidth = 2600)
{
    try
    {
        // Read image file
        using var image = await Image.LoadAsync(imagePath);
        
        bool needsProcessing = false;
        
        // Check if resizing is needed
        if (image.Width > maxWidth)
        {
            int newWidth = maxWidth;
            int newHeight = (int)Math.Round(newWidth * image.Height / (double)image.Width);
            
            image.Mutate(x => x.Resize(newWidth, newHeight));
            needsProcessing = true;
        }
        
        // Set DPI to 200
        if (image.Metadata.ResolutionUnits != ResolutionUnit.PixelsPerInch ||
            Math.Abs(image.Metadata.HorizontalResolution - 200) > 0.001 ||
            Math.Abs(image.Metadata.VerticalResolution - 200) > 0.001)
        {
            image.Metadata.ResolutionUnits = ResolutionUnit.PixelsPerInch;
            image.Metadata.HorizontalResolution = 200;
            image.Metadata.VerticalResolution = 200;
            needsProcessing = true;
        }
        
        // Save only if changes were made
        if (needsProcessing)
        {
            await image.SaveAsync(imagePath);
        }
        
        return imagePath;
    }
    catch (Exception)
    {
        // Return the original path in case of any error
        return imagePath;
    }
}