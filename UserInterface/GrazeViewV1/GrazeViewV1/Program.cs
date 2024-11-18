using System.Text.Json;

namespace GrazeViewV1
{
    internal static class Program
    {
        // Temporary Storage for GlobalData for Demo(ECEN 403)
        private static readonly string appDataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ApplicationGlobalData");
        private static readonly string uploadDataFile = Path.Combine(appDataFolder, "Uploads.json");
        private static readonly string mlDataFile = Path.Combine(appDataFolder, "MLData.json");

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Ensure globalData is stored
            Application.ApplicationExit += OnApplicationExit;

            EnsureAppDataFolderExists();

            // Load Data on run
            LoadGlobalData();

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            Application.EnableVisualStyles();
            ApplicationConfiguration.Initialize();
            Application.Run(new MainPage());
        }


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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating ApplicationGlobalData folder: {ex.Message}", "Folder Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Save data
        private static void SaveGlobalData()
        {
            try
            {
                // Serialize Uploads
                var uploadJson = JsonSerializer.Serialize(GlobalData.Uploads, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(uploadDataFile, uploadJson);

                // Serialize Machine Learning Data
                var mlDataJson = JsonSerializer.Serialize(GlobalData.machineLearningData, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(mlDataFile, mlDataJson);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data: {ex.Message}", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Load data from previous runs
        private static void LoadGlobalData()
        {
            try
            {
                // Load Uploads
                if (File.Exists(uploadDataFile))
                {
                    var uploadJson = File.ReadAllText(uploadDataFile);
                    var uploads = JsonSerializer.Deserialize<List<UploadInfo>>(uploadJson);
                    if (uploads != null)
                        GlobalData.Uploads.AddRange(uploads);
                }

                // Load Machine Learning Data
                if (File.Exists(mlDataFile))
                {
                    var mlDataJson = File.ReadAllText(mlDataFile);
                    var mlData = JsonSerializer.Deserialize<List<MLData>>(mlDataJson);
                    if (mlData != null)
                        GlobalData.machineLearningData.AddRange(mlData);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

                if (File.Exists(mlDataFile))
                {
                    File.Delete(mlDataFile);
                }

                // Optionally, show a confirmation message
                MessageBox.Show("All data has been cleared successfully.", "Data Cleared", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error clearing data: {ex.Message}", "Clear Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}