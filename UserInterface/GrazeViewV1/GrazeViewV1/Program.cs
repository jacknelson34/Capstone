using System.Runtime.InteropServices;
using System.Text.Json;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Windows.Forms.Design;

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

                // Check for driver
                CheckandInstallDriver();

                // Allow UI to refresh while loading
                Application.DoEvents();

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
                    throw new Exception("Unable to connect to Database.  Check your internet connection and try again.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing application: {ex.Message}", "Startup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
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

        private static void ConfigureODBC_DSN()
        {
            try
            {
                string dsnName = "GrazeViewDSN";
                string server = "sqldatabase404.database.windows.net";
                string database = "404ImageDBSql";
                string username = "sql404admin";
                string password = "sheepstool404()";

                // Command to add the DSN
                string arguments = $@"/a {{CONFIGDSN ""ODBC Driver 18 for SQL Server"" ""DSN={dsnName}|Description=My ODBC Connection|Server={server}|Database={database}|Trusted_Connection=Yes""}}";


                Process process = new Process();
                process.StartInfo.FileName = "odbcconf.exe";
                process.StartInfo.Arguments = arguments;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if(process.ExitCode == 0)
                {
                    MessageBox.Show("ODBC DSN Successfully configured", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show($"Failed to configure ODBC DSN. \nError:  {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static bool IsODBCDriverInstalled(string driverName)
        {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\ODBC\ODBCINST.INI\ODBC Drivers"))
            {
                if (key != null)
                {
                    object value = key.GetValue(driverName);
                    return value != null;
                }
            }
            return false;
        }

        private static void CheckandInstallDriver()
        {
            string driverName = "ODBC Driver 18 for SQL Server";

            if (!IsODBCDriverInstalled(driverName))
            {
                DialogResult result = MessageBox.Show(
                    "The required ODBC Driver is not installed.  Would you like to install in now?",
                    "ODBC Driver Missing",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if(result == DialogResult.Yes)
                {
                    InstallODBCDriver();
                }
                else
                {
                    MessageBox.Show("The application may not function correctly without the required driver.",
                        "Warning",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }

            }

        }

        private static void InstallODBCDriver()
        {
            try
            {
                string installerPath = Path.Combine(appDataFolder, "msodbcsql.msi");

                if (!File.Exists(installerPath))
                {
                    MessageBox.Show("Driver installer not found.  Please download the ODBC Driver manually.",
                        "Installation Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    Application.Exit();

                    return;
                }

                Process process = new Process();
                process.StartInfo.FileName = "msiexec.exe";
                process.StartInfo.Arguments = $"/i \"{installerPath}\"";
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.CreateNoWindow = false;
                process.Start();

                MessageBox.Show("The ODBC Driver is being installed.  Please restart the application after installation.",
                    "Installation in progress.",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                process.WaitForExit();

                if(process.ExitCode == 0)
                {
                    MessageBox.Show("Installation Complete.  The DSN will configure now.",
                                        "Please wait",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);

                    ConfigureODBC_DSN();


                    MessageBox.Show("Configuration Complete.  App will close now.",
                                        "Installation/Configuration Complete",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                    Environment.Exit(0);

                }

            }
            catch(Exception ex)
            {
                MessageBox.Show($"Failed to install the ODBC Driver: {ex.Message}",
                    "Installation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                Application.Exit();
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