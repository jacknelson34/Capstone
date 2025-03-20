using System.Runtime.InteropServices;
using System.Text.Json;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Win32;

namespace GrazeViewV1
{
    internal static class Program
    {
        // Temporary Storage for GlobalData for Demo(ECEN 403)
        private static readonly string appDataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ApplicationGlobalData");

        // Integrated ML Path
        public static readonly string onnxModelFile = Path.Combine(appDataFolder, "GrazeView_accur93_FinalModel.onnx");

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

                bool driverCheck = IsODBCDriverInstalled("ODBC Driver 18 for SQL Server");
                if (!driverCheck)
                {
                    MessageBox.Show("Driver being downloaded.");
                    InstallODBCDriver();
                }

                // Connect to database
                dbConnections = new DBConnections(new DBSettings(
                    driver: "ODBC Driver 18 for SQL Server",
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
                    throw new Exception("Connection to Database failed.");
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
            Application.ApplicationExit += (s, e) => Process.GetCurrentProcess().Kill();
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

        private static bool IsODBCDriverInstalled(string driverName)
        {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\ODBC\ODBCINST.INI\ODBC Drivers"))
            {
                if (key != null && key.GetValue(driverName) != null)
                {
                    return true; // Driver exists
                }
            }
            return false; // Driver is missing
        }

        private static void InstallODBCDriver()
        {
            if (!IsODBCDriverInstalled("ODBC Driver 18 for SQL Server"))
            {
                Process process = new Process();
                process.StartInfo.FileName = "msodbcsql18.msi";
                process.StartInfo.Arguments = "/quiet /norestart";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                process.WaitForExit();
            }
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