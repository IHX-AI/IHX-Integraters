using System;
using System.IO;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Metadata;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;

public static class ImageProcessor
{
    /// <summary>
    /// Downscales an image if its width exceeds a maximum value and sets its DPI to 200.
    /// </summary>
    /// <param name="imagePath">The path to the image file</param>
    /// <param name="maxWidth">Optional maximum width (defaults to 2600)</param>
    /// <returns>The path to the processed image</returns>
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
                Console.WriteLine($"Image width ({image.Width}px) exceeds maximum ({maxWidth}px). Resizing...");
                int newWidth = maxWidth;
                int newHeight = (int)Math.Round(newWidth * image.Height / (double)image.Width);
                
                image.Mutate(x => x.Resize(newWidth, newHeight));
                needsProcessing = true;
                Console.WriteLine($"Resized to {newWidth}x{newHeight}px");
            }
            else
            {
                Console.WriteLine($"Image width ({image.Width}px) is within limits. No resizing needed.");
            }
            
            // Set DPI to 200
            if (image.Metadata.HorizontalResolution != 200 ||
                image.Metadata.VerticalResolution != 200)
            {
                Console.WriteLine($"Setting DPI to 200");
                image.Metadata.HorizontalResolution = 200;
                image.Metadata.VerticalResolution = 200;
                needsProcessing = true;
            }
            
            // Save only if changes were made
            if (needsProcessing)
            {
                Console.WriteLine("Saving processed image...");
                await image.SaveAsync(imagePath);
                Console.WriteLine("Image saved successfully");
            }
            else
            {
                Console.WriteLine("No processing needed");
            }
            
            return imagePath;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing image: {ex.Message}");
            // Return the original path in case of any error
            return imagePath;
        }
    }
}
