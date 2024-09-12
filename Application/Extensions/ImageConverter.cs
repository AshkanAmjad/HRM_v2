using SkiaSharp;

namespace Application.Extensions
{
    public class ImageConverter
    {
        public static SKBitmap ResizeImage(string inputImagePath, int newWidth, int quality)
        {
            using var originalBitmap = LoadBitmap(inputImagePath);
            var newHeight = CalculateNewHeight(originalBitmap, newWidth);
            return originalBitmap.Resize(new SKImageInfo(newWidth, newHeight), (SKFilterQuality)quality);
        }

        public static SKBitmap WatermarkImage(SKBitmap resizedBitmap, string watermarkImagePath)
        {
            using var watermarkBitmap = LoadBitmap(watermarkImagePath);
            var (x, y) = CalculateWatermarkPosition(resizedBitmap, watermarkBitmap);

            var outputBitmap = new SKBitmap(resizedBitmap.Width, resizedBitmap.Height);
            using var canvas = new SKCanvas(outputBitmap);

            canvas.DrawBitmap(resizedBitmap, 0, 0);
            canvas.DrawBitmap(watermarkBitmap, x, y);

            return outputBitmap;
        }

        public static void SaveImage(SKBitmap bitmap, string outputImagePath, int quality)
        {
            using var output = File.OpenWrite(outputImagePath);
            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, quality);
            data.SaveTo(output);
        }

        private static SKBitmap LoadBitmap(string imagePath)
        {
            using var stream = File.OpenRead(imagePath);
            return SKBitmap.Decode(stream);
        }

        private static int CalculateNewHeight(SKBitmap bitmap, int newWidth)
        {
            double aspectRatio = (double)bitmap.Height / bitmap.Width;
            return (int)Math.Round(newWidth * aspectRatio);
        }
        private static (int x, int y) CalculateWatermarkPosition(SKBitmap baseBitmap, SKBitmap watermarkBitmap)
        {
            int x = (baseBitmap.Width - watermarkBitmap.Width)-5;
            int y = (baseBitmap.Height - watermarkBitmap.Height)-5;
            return (x, y);
        }
    }
}