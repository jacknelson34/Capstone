using System.Runtime.InteropServices;
using System.Text.Json;

namespace GrazeViewV1
{
    internal static class Program
    {
        // Temporary Storage for GlobalData for Demo(ECEN 403)
        private static readonly string appDataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ApplicationGlobalData");
        private static readonly string uploadDataFile = Path.Combine(appDataFolder, "Uploads.json");
        private static readonly string mlDataFile = Path.Combine(appDataFolder, "MLData.json");

        // Integrated ML Path
        public static readonly string onnxModelFile = Path.Combine(appDataFolder, "GrazeView.onnx");

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            if (Environment.OSVersion.Version.Major >= 6)
            {
                SetProcessDpiAwareness(PROCESS_DPI_AWARENESS.Process_Per_Monitor_DPI_Aware);
            }

            // Ensure globalData is stored
            Application.ApplicationExit += OnApplicationExit;

            EnsureAppDataFolderExists();

            // Load Data on run
            LoadGlobalData();

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainPage());
        }

        [DllImport("Shcore.dll")]
        private static extern int SetProcessDpiAwareness(PROCESS_DPI_AWARENESS awareness);

        private enum PROCESS_DPI_AWARENESS
        {
            Process_DPI_Unaware = 0,
            Process_System_DPI_Aware = 1,
            Process_Per_Monitor_DPI_Aware = 2
        }

        /*-------------------------------------Temporary Storage Functions for Uploaded Data----------------------------------------------*/

        // Method handler for application close
        private static void OnApplicationExit(object sender, EventArgs e)
        {
            // Save data to files
            SaveGlobalData();
        }

        // Check for valid folder storage
        private static void EnsureAppDataFolderExists()
        {
            try
            {
                if (!Directory.Exists(appDataFolder))
                {
                    Directory.CreateDirectory(appDataFolder);
                }

                // Copy the default ONNX model if missing
                string defaultModelPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DefaultFiles", "GrazeView.onnx");
                if (!File.Exists(onnxModelFile) && File.Exists(defaultModelPath))
                {
                    File.Copy(defaultModelPath, onnxModelFile, true);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting up application files: {ex.Message}", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Save data
        private static void SaveGlobalData()
        {
            try
            {
                // Serialize Uploads with formatting adjustments for Comments
                var normalizedUploads = GlobalData.Uploads.Select(upload =>
                {
                    upload.Comments = NormalizeTextForSave(upload.Comments); // Apply normalization
                    return upload;
                }).ToList();

                var uploadJson = JsonSerializer.Serialize(normalizedUploads, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(uploadDataFile, uploadJson);

                // Serialize Machine Learning Data (MLData does not need text normalization as it contains no special text fields)
                var mlDataJson = JsonSerializer.Serialize(GlobalData.machineLearningData, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(mlDataFile, mlDataJson);
            }
            catch (Exception ex)        // Catch exceptions thrown when attempting to save data
            {
                // Output error message - Currently throwing errors due to JSON temporary storage
                MessageBox.Show($"GrazeView currently has limited external storage support:\n {ex.Message}", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Load data from previous runs
        private static void LoadGlobalData()
        {
            try
            {
                // Load Uploads and restore formatting for Comments
                if (File.Exists(uploadDataFile))
                {
                    // Variable to hold externally stored data
                    var uploadJson = File.ReadAllText(uploadDataFile);
                    // Deserialize external data and import into UploadInfo list
                    var uploads = JsonSerializer.Deserialize<List<UploadInfo>>(uploadJson);
                    if (uploads != null)        // Check for null value
                    {
                        // for loop to fix formatting differences between application and JSON storage
                        foreach (var upload in uploads)
                        {
                            upload.Comments = RestoreTextFormatting(upload.Comments); // Restore formatting
                        }
                        GlobalData.Uploads.AddRange(uploads);
                    }
                }

                // Load Machine Learning Data
                if (File.Exists(mlDataFile))       
                {
                    // Pull all externally stored data into mlDataJson
                    var mlDataJson = File.ReadAllText(mlDataFile);
                    // Deserialize mlDataJson into mlData, and add to the global list MLData
                    var mlData = JsonSerializer.Deserialize<List<MLData>>(mlDataJson);
                    if (mlData != null)
                    {
                        GlobalData.machineLearningData.AddRange(mlData);    // Add each previous upload's data
                    }
                }

                // Check if ONNX model and image exist
                if (!File.Exists(onnxModelFile))
                {
                    MessageBox.Show("ONNX model file is missing!", "Model Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                // Catch exception and output message if an error occured when loading the data in JSON
                MessageBox.Show($"Error loading data: {ex.Message}", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string NormalizeTextForSave(string input)
        {
            return input?.Replace("\n", "\\n").Replace("\r", "\\r"); // Escape newlines and carriage returns
        }

        private static string RestoreTextFormatting(string input)
        {
            return input?.Replace("\\n", "\n").Replace("\\r", "\r"); // Restore newlines and carriage returns
        }

        /*---------------------------------------Temporary Clear Function for Data Stored------------------------------------------------*/
        public static void ClearAllData()
        {
            try
            {
                // Clear the in-memory data
                GlobalData.Uploads.Clear();
                GlobalData.machineLearningData.Clear();

                // Delete the data files if they exist
                if (File.Exists(uploadDataFile))
                {
                    File.Delete(uploadDataFile);
                }

                // Delete all MLData
                if (File.Exists(mlDataFile))
                {
                    File.Delete(mlDataFile);
                }

                // Optionally, show a confirmation message
                MessageBox.Show("All data has been cleared successfully.", "Data Cleared", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)    // Catch exception if clearing data fails
            {
                // Output message with error thrown
                MessageBox.Show($"Error clearing data: {ex.Message}", "Clear Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}