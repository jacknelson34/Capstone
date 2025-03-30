using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrazeViewV1
{
    internal class HeatMap
    {

        public static async Task<Mat> CreateCompositeHeatMap(Mat originalImage, List<Mat> imageTiles, List<float[]> tilePredictions, int tileWidth, int tileHeight, double whitespaceThreshold, float transparency, int resizeWidth, int resizeHeight, LoadingPage loadingpage, int totalTileCount)
        {
            // Clone the original image to keep it intact and create a new image for the heatmap
            Mat compositeHeatmapImage = originalImage;

            List<int> validTileIndices = new List<int>(); // To track the indices of valid (non-whitespace) tiles
            List<float[]> validTilePredictions = new List<float[]>(); // To store predictions for valid tiles

            // Set loadingbar back to 0
            int loadingBarMax = 0;

            // Loop through all the tiles in the image and collect predictions for non-whitespace tiles
            int tileIndex = 0;
            int increment = 0;
            for (int y = 0; y < originalImage.Height; y += tileHeight)
            {
                for (int x = 0; x < originalImage.Width; x += tileWidth)
                {
                    int width = Math.Min(tileWidth, originalImage.Width - x);
                    int height = Math.Min(tileHeight, originalImage.Height - y);
                    Rect roi = new Rect(x, y, width, height);
                    Mat tile = new Mat(originalImage, roi);

                    // Check if the tile contains too much whitespace
                    Mat grayTile = new Mat();
                    Cv2.CvtColor(tile, grayTile, ColorConversionCodes.BGR2GRAY);

                    Mat whiteMask = new Mat(grayTile.Size(), MatType.CV_8UC1, new Scalar(0));
                    Cv2.Compare(grayTile, 240, whiteMask, CmpType.GE);

                    int white_pixels = Cv2.CountNonZero(whiteMask);
                    double total_pixels = grayTile.Total();
                    double whitespace_percentage = (white_pixels / total_pixels) * 100;

                    if (whitespace_percentage < whitespaceThreshold)
                    {
                        // If it's a valid tile, add its index to the valid list
                        //MessageBox.Show(increment);
                        validTileIndices.Add(tileIndex);
                        validTilePredictions.Add(tilePredictions[increment]); // Add the corresponding prediction

                        increment++;
                    }

                    loadingBarMax++;
                    tileIndex++;
                }

            }

            // Now, let's apply the heatmap only on valid tiles
            int validTileIndex = 0;
            tileIndex = 0;

            // Set up loading bar for heatmap generation
            loadingpage.SetProgressBarMax(loadingBarMax);
            int count = 0;
            float percentageComplete = 0;

            // Loop through the image tiles again and apply heatmap color for non-whitespace tiles
            for (int y = 0; y < originalImage.Height; y += tileHeight)
            {
                for (int x = 0; x < originalImage.Width; x += tileWidth)
                {
                    int width = Math.Min(tileWidth, originalImage.Width - x);
                    int height = Math.Min(tileHeight, originalImage.Height - y);
                    Rect roi = new Rect(x, y, width, height);
                    Mat tile = new Mat(originalImage, roi);

                    // Check if this tile is in the list of valid (non-whitespace) tiles
                    if (validTileIndices.Contains(tileIndex))
                    {
                        // Get the prediction for this tile from validTilePredictions
                        float[] probabilities = validTilePredictions[validTileIndex];

                        // Find the class with the highest probability for this tile
                        int predictedClass = Array.IndexOf(probabilities, probabilities.Max());

                        // Assign a specific color to the tile based on the predicted class
                        Scalar colorTint = predictedClass switch
                        {
                            0 => new Scalar(255, 0, 0),    // Blue for Nale
                            1 => new Scalar(0, 255, 0),    // Green for Qufu
                            2 => new Scalar(0, 0, 255),    // Red for Erci
                            3 => new Scalar(0, 255, 255),  // Yellow for Bubble
                            _ => new Scalar(0, 0, 0)       // No tint for unknown classes (shouldn't happen)
                        };

                        // Apply semi-transparency (alpha blending)
                        float alpha = transparency; // increasing % makes colors brighter, add an f to ur number for the method params ex: 0.5f for 50%, 0.7f for 70%

                        // Loop through the pixels of the tile and apply the color to the composite image
                        for (int i = 0; i < height; i++)
                        {
                            for (int j = 0; j < width; j++)
                            {
                                // Get the corresponding pixel from the original image
                                Vec3b originalPixel = originalImage.At<Vec3b>(y + i, x + j);
                                Vec3b overlayPixel = new Vec3b(
                                    (byte)colorTint.Val0,  // Blue channel
                                    (byte)colorTint.Val1,  // Green channel
                                    (byte)colorTint.Val2   // Red channel
                                );

                                // Blend the original pixel with the overlay color using alpha blending
                                Vec3b blendedPixel = new Vec3b(
                                    (byte)(originalPixel.Item0 * (1 - alpha) + overlayPixel.Item0 * alpha),
                                    (byte)(originalPixel.Item1 * (1 - alpha) + overlayPixel.Item1 * alpha),
                                    (byte)(originalPixel.Item2 * (1 - alpha) + overlayPixel.Item2 * alpha)
                                );

                                // Apply the blended pixel back into the composite heatmap image
                                compositeHeatmapImage.At<Vec3b>(y + i, x + j) = blendedPixel;
                            }
                        }

                        validTileIndex++;  // Move to the next valid tile
                    }

                    count++;
                    percentageComplete = ((float)count / loadingBarMax) * 100;
                    loadingpage.UpdateProgress(
                        count,
                        "Generating heat map...",
                        $"{percentageComplete.ToString("0.00")}%"
                    );


                    tileIndex++;  // Move to the next tile index
                }

            }

            // Now, resize the composite heatmap image to 750x750
            Mat resizedHeatmap = new Mat();
            Cv2.Resize(compositeHeatmapImage, resizedHeatmap, new OpenCvSharp.Size(resizeWidth, resizeHeight));

            // Convert Mat to Bitmap
            return resizedHeatmap;
        }

        public static async Task<Bitmap> MatToBitmap(Mat mat)
        {

            // Convert Mat to byte array
            byte[] byteArray = mat.ToBytes();

            // Convert byte array to original bitmap
            using (var memoryStream = new MemoryStream(byteArray))
            using (var originalImage = new Bitmap(memoryStream))
            {
                // Resize proportionally to fit within 1920x1080
                int maxWidth = 1920;
                int maxHeight = 1080;

                float ratioX = (float)maxWidth / originalImage.Width;
                float ratioY = (float)maxHeight / originalImage.Height;
                float ratio = Math.Min(ratioX, ratioY);

                int newWidth = (int)(originalImage.Width * ratio);
                int newHeight = (int)(originalImage.Height * ratio);

                // Create resized bitmap
                Bitmap resizedImage = new Bitmap(newWidth, newHeight);
                using (Graphics graphics = Graphics.FromImage(resizedImage))
                {
                    graphics.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                }

                return resizedImage;
            }

        }

    }
}
