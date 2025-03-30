using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrazeViewV1
{
    public partial class MainPage : Form
    {
        private DataLibrary _datalibrary; // Reference to the DataLibrary form instance
        private readonly DBConnections _dbConnections;
        private System.Windows.Forms.Timer splashTimer;

        // Constructor for initializing MainPage
        public MainPage(DBQueries dbQueries)
        {
            InitializeComponent(); // Initialize form components
            // Enable double buffering
            this.BringToFront();
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
            StartPosition = FormStartPosition.CenterScreen;
            ResizeControls(); // Adjust and position controls dynamically

            bool isFirstLoad = true;          // Boolean to determine if the app is opened or returing to main
            if (isFirstLoad)
            {
                this.WindowState = FormWindowState.Maximized;
                isFirstLoad = false;
            }

            this.Text = "GrazeView"; // Set the title of the MainPage form

            _dbConnections = new DBConnections(new DBSettings(
                "ODBC Driver 18 for SQL Server", // Driver
                "sqldatabase404.database.windows.net", // Server
                "404ImageDBsql", // Database
                "sql404admin", // Username
                "sheepstool404()", // Password
                "TrustServerCertificate=Yes;" // Connection Options
            ));

            //MessageBox.Show("Connection: " + _dbConnections.ConnectionString);

            dbQueries = new DBQueries(_dbConnections.ConnectionString); // Ensure connectionString is correct
            var dataLibrary = new DataLibrary(this, dbQueries);

            this.Resize += MainPage_Resize; // Attach the resize event handler
            this.Load += MainPage_Load; // Attach the load event handler for initial adjustments

            splashTimer = new System.Windows.Forms.Timer();
            splashTimer.Interval = 3000;
            splashTimer.Tick += (s, e) =>
            {
                splashText.SetRandomSplashText();
                float splashFont = Math.Min(25, this.ClientSize.Width / 80f);
                splashText.Font = new Font("Arial", splashFont, FontStyle.Italic | FontStyle.Bold);

                // Update the splash label's text to get accurate size
                splashText.SetRandomSplashText();

                // Optionally: Manually resize based on text (in case AutoSize is not reliable)
                using (Graphics g = splashText.CreateGraphics())
                {
                    SizeF textSize = g.MeasureString(splashText.Text, splashText.Font);
                    splashText.Size = new Size(
                                (int)(Math.Ceiling(textSize.Width * 2)),
                                (int)(Math.Ceiling(textSize.Height * 2) * 5) + 20 // 🔼 increased from 4 to 5 and +10 to +20
                            );
                }

                // Position splashText centered horizontally under mainLabel with some spacing
                splashText.Location = new Point(
                    (this.ClientSize.Width - splashText.Width) / 2,
                    mainLabel.Bottom + 10
                );
            };
            splashTimer.Start();
        }

        // Event handler for the Data Upload button click
        private void dataUploadButton_Click(object? sender, EventArgs e)
        {
            DataUpload dataupload = new DataUpload(this); // Create a new instance of DataUpload form
            dataupload.Size = this.Size; // Set the size of the new form to match MainPage
            dataupload.Location = this.Location; // Set the location of the new form to match MainPage
            dataupload.WindowState = this.WindowState;
            dataupload.Show(); // Show the DataUpload form
            this.Visible = false; // Hide the MainPage form
        }

        // Event handler for the Data Viewer button click
        private void dataViewerButton_Click(object? sender, EventArgs e)
        {
            var dbQueries = new DBQueries(_dbConnections.ConnectionString); // Ensure connectionString is correct
            DataLibrary datalibrary = new DataLibrary(this, dbQueries);
            datalibrary.Size = this.Size; // Set the size of the new form to match MainPage
            datalibrary.Location = this.Location; // Set the location of the new form to match MainPage
            datalibrary.WindowState = this.WindowState;
            datalibrary.Show(); // Show the DataLibrary form
            this.Hide(); // Hide the MainPage form
        }

        // Event handler for the Help button click
        private void helpButton_Click(object sender, EventArgs e)
        {
            UserGuide.ShowHelpGuide(); // Open the user guide, ensuring only one instance is active
        }

        // Event handler for the form's load event
        private void MainPage_Load(object sender, EventArgs e)
        {
            ResizeControls(); // Resize and adjust the controls
            Refresh(); // Refresh the form to apply all changes
        }

        // Event handler for the form's resize event
        private async void MainPage_Resize(object sender, EventArgs e)
        {

            if (this.WindowState == FormWindowState.Minimized)
            {
                return;
            }

            float fontSize = Math.Min(65, this.ClientSize.Width / 25f); // Calculate font size based on form width
            //MessageBox.Show("Main Page Width : " + this.ClientSize.Width.ToString() + "\nFont Size : " + fontSize.ToString());
            mainLabel.Font = new Font("Times New Roman", fontSize, FontStyle.Bold, GraphicsUnit.Point, 0); // Set label font
            mainLabel.Size = new Size((this.ClientSize.Width / 2), (this.ClientSize.Height / 7)); // Adjust label size
            ResizeControls(); // Adjust the controls

        }

        // Method to dynamically resize and position controls
        private void ResizeControls()
        {

            mainLabel.Location = new Point((this.ClientSize.Width / 2) - (mainLabel.Width / 2), // Center the label horizontally
                                           (this.ClientSize.Height / 6) - 40); // Position the label vertically

            // Set splash label font size and style
            float splashFont = Math.Min(25, this.ClientSize.Width / 80f);
            splashText.Font = new Font("Arial", splashFont, FontStyle.Italic | FontStyle.Bold);

            // Update the splash label's text to get accurate size
            splashText.SetRandomSplashText();

            // Optionally: Manually resize based on text (in case AutoSize is not reliable)
            using (Graphics g = splashText.CreateGraphics())
            {
                SizeF textSize = g.MeasureString(splashText.Text, splashText.Font);
                splashText.Size = new Size(
                                                (int)(Math.Ceiling(textSize.Width * 2)),
                                                (int)(Math.Ceiling(textSize.Height * 2) * 5) + 20 // 🔼 increased from 4 to 5 and +10 to +20
                                            );

            }

            // Position splashText centered horizontally under mainLabel with some spacing
            splashText.Location = new Point(
                (this.ClientSize.Width - splashText.Width) / 2,
                mainLabel.Bottom + 10
            );


            dataUploadButton.Size = new Size((this.ClientSize.Width / 3), 100); // Set size of the DataUpload button
            dataUploadButton.Location = new Point((this.ClientSize.Width / 2) - (dataUploadButton.Width / 2), // Center the button horizontally
                                                  ((this.ClientSize.Height - mainLabel.Height) / 2) - 50); // Position the button vertically

            dataViewerButton.Size = new Size((this.ClientSize.Width / 3), 100); // Set size of the DataViewer button
            dataViewerButton.Location = new Point((this.ClientSize.Width / 2) - (dataViewerButton.Width / 2), // Center the button horizontally
                                                  ((this.ClientSize.Height - mainLabel.Height) / 2) + 90); // Position the button vertically

            float buttonFontSize = Math.Max(8, dataViewerButton.Width / 20f); // Calculate button font size based on button width
            dataUploadButton.Font = new Font("Times New Roman", buttonFontSize, FontStyle.Regular, GraphicsUnit.Point, 0); // Set font for DataUpload button
            dataViewerButton.Font = new Font("Times New Roman", buttonFontSize, FontStyle.Regular, GraphicsUnit.Point, 0); // Set font for DataViewer button

            // MessageBox.Show("Form Size : " + this.ClientSize.ToString() + "\nButton Size : " + dataUploadButton.Size.ToString() + "\nButton Font Size : " + buttonFontSize.ToString());

            this.Refresh(); // Refresh the form to apply changes
        }


        // Helper method to resize main from other forms
        public void ExternalResize(Size s, Point p)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Normal;
                this.Size = s;
                this.Location = p;
            }

        }
    }
}