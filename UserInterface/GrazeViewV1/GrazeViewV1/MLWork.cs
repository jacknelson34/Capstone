//Nugets needed: Microsoft.ML, Microsoft.ML.OnnxRuntime, OpenCvSharp4, OpenCvSharp4.runtime.win

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

        public static async void MLMain(string imagePath, LoadingPage loadingpage)
        {
            //MessageBox.Show("OpenCVSharp");

            string modelPath = Program.onnxModelFile;

            MLContext mlContext = new MLContext();
            var sessionOptions = new SessionOptions();

            //MessageBox.Show("Paths called");

            List<float[]> tilePredictions = new List<float[]>();

            // Create return image
            Bitmap originalBitmap;
            Bitmap Heatmap;

            using (var session = new InferenceSession(modelPath, sessionOptions))
            {
                // 1. Get Input and Output Names (Dynamically)
                var inputName = session.InputMetadata.Keys.FirstOrDefault();
                var outputName = session.OutputMetadata.Keys.FirstOrDefault();

                //MessageBox.Show("Reading Image");

                // 2. Load and Preprocess Image (OpenCVSharp4)
                Mat originalImage = Cv2.ImRead(imagePath);

                // Begin task to convert the original image to bitmap
                originalBitmap = await HeatMap.MatToBitmap(originalImage);

                // 3. Split the image into 100x100 tiles
                List<Mat> imageTiles = SplitIntoTiles(originalImage, 100, 100, 95);
                //MessageBox.Show($"Number of tiles Split: {imageTiles.Count}");

                // Set tick amount for loading page based on image split
                int loadingPagetick = imageTiles.Count + 1;
                loadingpage.SetProgressBarMax(loadingPagetick);

                float[] classProbabilitiesSum = new float[4];

                // 4. Process each tile and run inference
                int count = 0;
                float percentageComplete = 0;
                foreach (Mat tile in imageTiles)
                {
                    // 5. Preprocess the tile and normalize
                    var inputShape = session.InputMetadata[inputName].Dimensions.ToArray();
                    int targetWidth = inputShape[2];  // Get from model
                    int targetHeight = inputShape[1]; // Get from model

                    if (tile.Width != targetWidth || tile.Height != targetHeight) //Resize only if needed
                    {
                        Cv2.Resize(tile, tile, new OpenCvSharp.Size(targetWidth, targetHeight));
                    }

                    //MessageBox.Show("RGB Preprocessing");
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

                    //MessageBox.Show("Making Tensor");
                    // 6. Create Input Tensor and NamedOnnxValue
                    var inputTensor = new DenseTensor<float>(tileData, new[] { 1, targetHeight, targetWidth, 3 }); // Shape(batch_size, Height, Width, Channels)
                    var inputNamedValue = NamedOnnxValue.CreateFromTensor<float>(inputName, inputTensor);

                    //MessageBox.Show("Starting Inference");
                    // 7. Run Inference
                    using (var results = session.Run(new[] { inputNamedValue }))
                    {
                        // 8. Get and process the output (same as before)
                        foreach (var outputNamedValue in results)
                        {
                            //Output tensor as a float
                            var outputTensor = outputNamedValue.AsTensor<float>();

                            // ... (Process output data for the current tile)
                            var probabilities = outputTensor.ToArray();

                            tilePredictions.Add(probabilities);
                            // Get all output tensors
                            //MessageBox.Show($"Nale: {probabilities[0].ToString(".00")}");
                            //MessageBox.Show($"Qufu: {probabilities[1].ToString(".00")}");
                            //MessageBox.Show($"Erci: {probabilities[2].ToString(".00")}");
                            //MessageBox.Show($"Bubble: {probabilities[3].ToString(".00")}");

                            // Add all four predictions to growing list
                            classProbabilitiesSum[0] += probabilities[0];
                            classProbabilitiesSum[1] += probabilities[1];
                            classProbabilitiesSum[2] += probabilities[2];
                            classProbabilitiesSum[3] += probabilities[3];
                        }

                        //MessageBox.Show("Inference complete for one tile!");
                    }

                    // Correct percentage calculation for loading page
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

                //// Print the average predictions
                ////MessageBox.Show("\nAverage Predictions:");
                //for (int i = 0; i < 4; i++)
                //{
                //   //MessageBox.Show($"Class {i}: {averageProbabilities[i]}");
                //}

                float percent_no_whitespace = averageProbabilities[0] + averageProbabilities[1] + averageProbabilities[2] + averageProbabilities[3];

                // Normalize the four elements so they sum to 100%
                MLData mlData = new MLData
                {
                    nalePercentage = ((averageProbabilities[0] / percent_no_whitespace) * 100).ToString("00.00") + "%",
                    erciPercentage = ((averageProbabilities[1] / percent_no_whitespace) * 100).ToString("00.00") + "%",
                    qufuPercentage = ((averageProbabilities[2] / percent_no_whitespace) * 100).ToString("00.00") + "%",
                    bubblePercentage = ((averageProbabilities[3] / percent_no_whitespace) * 100).ToString("00.00") + "%"
                };

                // Push to DB
                GlobalData.machineLearningData.Add(mlData);

                //MessageBox.Show("\nNormalized Predictions:");
                //MessageBox.Show($"Nale: {((averageProbabilities[0] / percent_no_whitespace) * 100).ToString(".00") + "%"}");
                //MessageBox.Show($"Qufu: {((averageProbabilities[1] / percent_no_whitespace) * 100).ToString(".00") + "%"}");
                //MessageBox.Show($"Erci: {((averageProbabilities[2] / percent_no_whitespace) * 100).ToString(".00") + "%"}");
                //MessageBox.Show($"Bubble: {((averageProbabilities[3] / percent_no_whitespace) * 100).ToString(".00") + "%"}");

                loadingpage.CompleteMLProgress("Creating HeatMap...");

                //Creates the heatmap Mat, can edit the transparency and image size through the params - Need to add loading page progress updating
                Mat compositeHeatmapImage = await Task.Run(() => HeatMap.CreateCompositeHeatMap(originalImage, imageTiles, tilePredictions, 100, 100, 95, 0.5f, 750, 750, loadingpage, loadingPagetick));

                //Creates the bitmap from the Mat image and saves it
                Heatmap = await HeatMap.MatToBitmap(compositeHeatmapImage);

                // Save only image name
                string imageName = Path.GetFileName(imagePath);

                // Connect to DB and save both images
                DBQueries dbQueries = new DBQueries("Driver={ODBC Driver 18 for SQL Server};Server=sqldatabase404.database.windows.net;Database=404ImageDBsql;Uid=sql404admin;Pwd=sheepstool404();TrustServerCertificate=no;MultipleActiveResultSets=True;");
                dbQueries.UploadImageToDB(imageName, originalBitmap, Heatmap);

                //MessageBox.Show("Bitmap image saved.");

            }
            // Call Results Page once complete
            loadingpage.CompleteFullProgress("Processing Complete!", originalBitmap, Heatmap);
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
                    int width = Math.Min(tileWidth, image.Width - x); //means the created tiles dont have to be 100 x 100 
                    int height = Math.Min(tileHeight, image.Height - y);//if for instance we are near edge of image
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



