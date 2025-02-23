//Nugets needed: Microsoft.ML, Microsoft.ML.OnnxRuntime, OpenCvSharp4, OpenCvSharp4.runtime.win

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using OpenCvSharp;

namespace GrazeViewV1
    {
        internal class MLWork
        {

            public static void MLMain(string imagePath, LoadingPage loadingpage)
            {

            string modelPath = Program.onnxModelFile;

            // For GUI, need MessageBox instead of MessageBox.Show
            //MessageBox.Show("Using Model: " + modelPath);
            //MessageBox.Show("Using Image: " + imagePath);

            //MessageBox.Show($"Using Model: {modelPath}");
            //MessageBox.Show($"Using Image: {imagePath}");

            MLContext mlContext = new MLContext();
                var sessionOptions = new SessionOptions();

            // Replaced with MessageBox
            //MessageBox.Show("Paths called");
            //MessageBox.Show("Paths called");

                using (var session = new InferenceSession(modelPath, sessionOptions))
                {
                    // 1. Get Input and Output Names (Dynamically)
                    var inputName = session.InputMetadata.Keys.FirstOrDefault();
                    var outputName = session.OutputMetadata.Keys.FirstOrDefault();


                    // Replaced with MessageBox
                    //MessageBox.Show("Reading Image");
                    //MessageBox.Show("Reading Image");

                    // 2. Load and Preprocess Image (OpenCVSharp4)
                    Mat originalImage = Cv2.ImRead(imagePath);

                    // 1. Split the image into 100x100 tiles
                    List<Mat> imageTiles = SplitIntoTiles(originalImage, 100, 100, 95);

                    // Replaced with MessageBox
                    //MessageBox.Show($"Number of tiles Split: {imageTiles.Count}");
                    //MessageBox.Show($"Number of tiles Split: {imageTiles.Count}");

                    // Set tick amount for loading page based on image split
                    int loadingPagetick = imageTiles.Count;
                    loadingpage.SetProgressBarMax(loadingPagetick);

                    float[] classProbabilitiesSum = new float[4];

                    // 2. Process each tile and run inference
                    int count = 0;
                    float percentageComplete = 0;
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

                        // Replaced with MessageBox
                        //MessageBox.Show("RGB Preprocessing");
                        //MessageBox.Show("RGB Preprocessing");
                        float[] tileData = new float[targetWidth * targetHeight * 3]; // Assuming 3 channels (RGB or BGR)
                        for (int y = 0; y < targetHeight; y++)
                        {
                            for (int x = 0; x < targetWidth; x++)
                            {
                                Vec3b pixel = tile.At<Vec3b>(y, x);
                                tileData[y * targetWidth * 3 + x * 3 + 0] = pixel.Item1 / 255.0f ; // Blue (BGR)
                                tileData[y * targetWidth * 3 + x * 3 + 1] = pixel.Item2 / 255.0f ; // Green
                                tileData[y * targetWidth * 3 + x * 3 + 2] = pixel.Item0 / 255.0f ; // Red
                            }
                        }

                        // Replaced with MessageBox
                        //MessageBox.Show("Making Tensor");
                        //MessageBox.Show("Making Tensor");
                        // 4. Create Input Tensor and NamedOnnxValue
                        var inputTensor = new DenseTensor<float>(tileData, new[] { 1, targetHeight, targetWidth, 3 }); // Shape(batch_size, Height, Width, Channels)
                        var inputNamedValue = NamedOnnxValue.CreateFromTensor<float>(inputName, inputTensor);

                        // Replaced with MessageBox
                        //MessageBox.Show("Starting Inference");
                        //MessageBox.Show("Starting Inference");
                        // 5. Run Inference
                        using (var results = session.Run(new[] { inputNamedValue }))
                        {
                        // 6. Get and process the output (same as before)
                        foreach (var outputNamedValue in results)
                        {
                            //Output tensor as a float
                            var outputTensor = outputNamedValue.AsTensor<float>();

                            // ... (Process output data for the current tile)
                            var probabilities = outputTensor.ToArray();

                            // Get all output tensors
                            //MessageBox.Show($"Nale: {probabilities[0].ToString(".00")}");
                            //MessageBox.Show($"Erci: {probabilities[1].ToString(".00")}");
                            //MessageBox.Show($"Qufu: {probabilities[2].ToString(".00")}");
                            //MessageBox.Show($"Bubble: {probabilities[3].ToString(".00")}");

                            // Add all four predictions to growing list
                            classProbabilitiesSum[0] += probabilities[0];
                            classProbabilitiesSum[1] += probabilities[1];
                            classProbabilitiesSum[2] += probabilities[2];
                            classProbabilitiesSum[3] += probabilities[3];
                        }
                    }

                        // Correct percentage calculation
                        percentageComplete = ((float)(count + 1) / imageTiles.Count) * 100;

                        // Update UI with formatted percentage
                        loadingpage.UpdateProgress(
                            count + 1,
                            $"Processing Tile #{count + 1} of {imageTiles.Count}",
                            $"{percentageComplete.ToString("0.00")}%"
                        ); 
                        count += 1;
                    }
            

                    // Calculate average probabilities
                    float[] averageProbabilities = classProbabilitiesSum.Select(p => p / imageTiles.Count).ToArray();

                    // Print the average predictions
                    // Replaced with MessageBox
                    //MessageBox.Show("Average Predictions: ");
                    //MessageBox.Show("Average Predictions:");
                    //for (int j = 0; j < 4; j++)
                    //{
                        // Replaced with MessageBox
                        //MessageBox.Show($"Class {j}: {averageProbabilities[j]}");
                    //MessageBox.Show($"Class {i}: {averageProbabilities[i]}");

                    //}

                    // Exclude the white space percentage and get the sum of the 3 grasses + bubble
                    float percentagesWithoutWhiteSpace = averageProbabilities[0] + averageProbabilities[1] + averageProbabilities[2] + averageProbabilities[3];

                    // Normalize the four elements so they sum to 100%
                    MLData mlData = new MLData
                    {
                        nalePercentage = (((averageProbabilities[0] / percentagesWithoutWhiteSpace) * 100).ToString("0.00") + "%"),
                        erciPercentage = (((averageProbabilities[2] / percentagesWithoutWhiteSpace) * 100).ToString("0.00") + "%"),
                        qufuPercentage = (((averageProbabilities[1] / percentagesWithoutWhiteSpace) * 100).ToString("0.00") + "%"),
                        bubblePercentage = (((averageProbabilities[3] / percentagesWithoutWhiteSpace) * 100).ToString("0.00") + "%")
                    };

                    // Push to DB
                    GlobalData.machineLearningData.Add(mlData);

                }

            // Call Results Page once complete
            loadingpage.CompleteProgress("Processing Complete!");



            // Replace with MessageBox
            //MessageBox.Show("Processing complete. Click OK to continue.", "ML Model Output", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //Console.ReadKey();
        }

        static List<Mat> SplitIntoTiles(Mat image, int tileWidth, int tileHeight, double whitespaceThreshold)
        {
            List<Mat> tiles = new List<Mat>();
            int totalTiles = 0;
            for (int y = 0; y < image.Height; y += tileHeight)
            {
                for (int x = 0; x < image.Width; x += tileWidth)
                {
                    totalTiles++;
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
            //MessageBox.Show($"Total Tiles: {totalTiles}");

            return tiles;
        }
    }
    }



