//Nugets needed: Microsoft.ML, Microsoft.ML.OnnxRuntime, OpenCvSharp4, OpenCvSharp4.runtime.win

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.ML;
using OpenCvSharp;

namespace GrazeViewML
{
    internal class MLWork
    {
<<<<<<< Updated upstream


=======
        static void MLMain(string[] args)
        {
            string modelPath = "C:/Users/iruoe/Downloads/GrazeView.onnx"; // Replace with your ONNX model path
            string imagePath = "C:/Users/iruoe/Downloads/184HD_001_RESIZE.png"; // Replace with your image path

            MLContext mlContext = new MLContext();
            var sessionOptions = new SessionOptions();

            Console.WriteLine("Paths called");

            using (var session = new InferenceSession(modelPath, sessionOptions))
            {
                // 1. Get Input and Output Names (Dynamically)
                var inputName = session.InputMetadata.Keys.FirstOrDefault();
                var outputName = session.OutputMetadata.Keys.FirstOrDefault();

                Console.WriteLine("Reading Image");

                // 2. Load and Preprocess Image (OpenCVSharp4)
                Mat originalImage = Cv2.ImRead(imagePath);

                // 1. Split the image into 100x100 tiles
                List<Mat> imageTiles = SplitIntoTiles(originalImage, 100, 100, 95);
                Console.WriteLine($"Number of tiles Split: {imageTiles.Count}");

                float[] classProbabilitiesSum = new float[4];

                // 2. Process each tile and run inference
                foreach (Mat tile in imageTiles)
                {
                    // 3. Preprocess the tile and normalize
                    var inputShape = session.InputMetadata[inputName].Dimensions.ToArray();
                    int targetWidth = inputShape[2];  // Get from model
                    int targetHeight = inputShape[1]; // Get from model

                    if (tile.Width != targetWidth || tile.Height != targetHeight) //Resize only if needed
                    {
                        Cv2.Resize(tile, tile, new OpenCvSharp.Size(targetWidth, targetHeight));
                    }

                    Console.WriteLine("RGB Preprocessing");
                    float[] tileData = new float[targetWidth * targetHeight * 3]; // Assuming 3 channels (RGB or BGR)
                    for (int y = 0; y < targetHeight; y++)
                    {
                        for (int x = 0; x < targetWidth; x++)
                        {
                            Vec3b pixel = tile.At<Vec3b>(y, x);
                            tileData[y * targetWidth * 3 + x * 3 + 0] = pixel.Item1 / 255.0f; // Blue (BGR)
                            tileData[y * targetWidth * 3 + x * 3 + 1] = pixel.Item2 / 255.0f; // Green
                            tileData[y * targetWidth * 3 + x * 3 + 2] = pixel.Item0 / 255.0f; // Red
                        }
                    }

                    Console.WriteLine("Making Tensor");
                    // 4. Create Input Tensor and NamedOnnxValue
                    var inputTensor = new DenseTensor<float>(tileData, new[] { 1, targetHeight, targetWidth, 3 }); // Shape(batch_size, Height, Width, Channels)
                    var inputNamedValue = NamedOnnxValue.CreateFromTensor<float>(inputName, inputTensor);

                    Console.WriteLine("Starting Inference");
                    // 5. Run Inference
                    using (var results = session.Run(new[] { inputNamedValue }))
                    {
                        // 6. Get and process the output (same as before)
                        var outputNamedValue = results.FirstOrDefault(r => r.Name == outputName);
                        //Output tensor as a float
                        var outputTensor = outputNamedValue.AsTensor<float>();

                        // ... (Process output data for the current tile)
                        var outputData = outputTensor.ToArray();
                        Console.WriteLine("Inference complete for one tile!");
                        Cv2.ImShow("Tile", tile);
                        Cv2.WaitKey(0);

                        Console.WriteLine($"Nale: {outputData[0]}");
                        Console.WriteLine($"Erci: {outputData[1]}");
                        Console.WriteLine($"Qufu: {outputData[2]}");
                        Console.WriteLine($"Bubble: {outputData[3]}");

                        // Add all four predictions to growing list
                        for (int i = 0; i < 4; i++)
                        {
                            classProbabilitiesSum[i] += outputData[i];
                        }
                    }
                }

                // Calculate average probabilities
                float[] averageProbabilities = classProbabilitiesSum.Select(p => p / imageTiles.Count).ToArray();

                // Print the average predictions
                Console.WriteLine("Average Predictions:");
                for (int i = 0; i < 4; i++)
                {
                    Console.WriteLine($"Class {i}: {averageProbabilities[i]}");
                }

            }

            Console.ReadKey();
        }

        static List<Mat> SplitIntoTiles(Mat image, int tileWidth, int tileHeight, double whitespaceThreshold)
        {
            List<Mat> tiles = new List<Mat>();
            for (int y = 0; y < image.Height; y += tileHeight)
            {
                for (int x = 0; x < image.Width; x += tileWidth)
                {
                    int width = Math.Min(tileWidth, image.Width - x);
                    int height = Math.Min(tileHeight, image.Height - y);
                    Rect roi = new Rect(x, y, width, height);
                    Mat tile = new Mat(image, roi);

                    // Check for excessive whitespace

                    //Create a gray version of tile
                    Mat grayTile = new Mat();
                    Cv2.CvtColor(tile, grayTile, ColorConversionCodes.BGR2GRAY);

                    //Creates an approximate mask for white pixels
                    Mat whiteMask = new Mat(grayTile.Size(), MatType.CV_8UC1, new Scalar(0));
                    Cv2.Compare(grayTile, 240, whiteMask, CmpType.GE);

                    //Calculates percent of white pixels
                    int white_pixels = Cv2.CountNonZero(whiteMask);
                    double total_pixels = grayTile.Total();
                    double whitespace_percentage = (white_pixels / total_pixels) * 100;

                    //whole numbers not percents
                    if (whitespace_percentage < whitespaceThreshold)
                    {
                        tiles.Add(tile);
                    }
                }
            }
            return tiles;
        }
>>>>>>> Stashed changes
    }
}
