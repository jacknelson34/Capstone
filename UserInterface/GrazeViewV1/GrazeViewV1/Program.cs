using System.Runtime.InteropServices;
using System.Text.Json;

namespace GrazeViewV1
{
    internal static class Program
    {
        // Temporary Storage for GlobalData for Demo(ECEN 403)
        private static readonly string appDataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ApplicationGlobalData");

        // Integrated ML Path
        public static readonly string onnxModelFile = Path.Combine(appDataFolder, "GrazeView_accur91_fixedNale.onnx");

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

                        Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Initialize DB variables
            DBQueries dbQueries;
            DBConnections dbConnections;

            // Show SplashScreen in a separate thread
            SplashScreen splashScreen = new SplashScreen();
            Thread splashThread = new Thread(() => Application.Run(splashScreen));
            splashThread.SetApartmentState(ApartmentState.STA);
            splashThread.Start();

            try
            {
                // Allow UI to refresh while loading
                Application.DoEvents();

                // Connect to database
                dbConnections = new DBConnections(new DBSettings(
                    server: "sqldatabase404.database.windows.net",
                    database: "404ImageDBSql",
                    username: "sql404admin",
                    password: "sheepstool404()"
                ));

                dbQueries = new DBQueries(dbConnections.ConnectionString);

                bool isConnected = dbConnections.TestConnectionAsync().GetAwaiter().GetResult(); // Blocking call

                if (!isConnected)
                {
                    MessageBox.Show("Failed to connect to the database. Exiting application.",
                                    "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing application: {ex.Message}", "Startup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Close the splash screen once everything is ready
            splashScreen.Invoke(new Action(() => splashScreen.Close()));
            splashThread.Join();  // Ensure the splash screen thread fully exits before continuing

            // Ensure proper storage handling
            Application.ApplicationExit += async (sender, e) => await SaveGlobalDataAsync(dbQueries);
            EnsureAppDataFolderExists();

            // Start application
            Application.Run(new MainPage(dbQueries));

        }

        [DllImport("Shcore.dll")]
        private static extern int SetProcessDpiAwareness(PROCESS_DPI_AWARENESS awareness);

        private enum PROCESS_DPI_AWARENESS
        {
            Process_DPI_Unaware = 0,
            Process_System_DPI_Aware = 1,
            Process_Per_Monitor_DPI_Aware = 2
        }

        // Method handler for application close
        private static void OnApplicationExit(object sender, EventArgs e)
        {
            var dbConnections = new DBConnections(new DBSettings(
                server: "sqldatabase404.database.windows.net",
                database: "404ImageDBsql",
                username: "sql404admin",
                password: "sheepstool404()"
            ));
            var dbQueries = new DBQueries(dbConnections.ConnectionString); // Define dbQueries before use

            // Save data to files
            SaveGlobalDataAsync(dbQueries);
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
        private static async Task SaveGlobalDataAsync(DBQueries dbQueries)
        {
            try
            {
                foreach (var upload in GlobalData.Uploads)
                {
                    var mlData = GlobalData.machineLearningData
                        .FirstOrDefault(m => m != null && m.nalePercentage == upload.UploadName);

                    //await dbQueries.InsertUploadAsync(upload, mlData ?? new MLData());
                }

                //MessageBox.Show("All data saved to SQL successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data to database: {ex.Message}", "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        // TODO
        public static async Task ClearAllData(DBQueries dbQueries)
        {
            try
            {
                await dbQueries.ClearAllUploadsAsync();
                GlobalData.Uploads.Clear();
                GlobalData.machineLearningData.Clear();

                //MessageBox.Show("All data has been cleared successfully.", "Data Cleared", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error clearing database: {ex.Message}", "Clear Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}