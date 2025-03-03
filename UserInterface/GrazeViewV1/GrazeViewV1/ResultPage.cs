using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrazeViewV1
{
    public partial class ResultPage : Form
    {
        private PictureBox outputImage;
        private bool IsNavigating;
        private readonly DBConnections _dbConnections;

        // Hold instances of the opened pages
        private MainPage _mainPage;

        public ResultPage(Image resultImage, MainPage mainPage)  // Build page with resulting image from ML and previous page's size/location
        {
            IsNavigating = false;
            _dbConnections = new DBConnections(new DBSettings(
                    server: "sqldatabase404.database.windows.net",
                    database: "404ImageDBsql",
                    username: "sql404admin",
                    password: "sheepstool404()"
                    ));

            InitializeComponent();
            this.Size = ConsistentForm.FormSize;                // Set form size to the same as the previous page
            this.Location = ConsistentForm.FormLocation;        // Set form location to the same as previous page
            this.Text = "GrazeView";                            // Add GrazeView header
            _mainPage = mainPage;                               // Hold reference for mainpage
            // Enable double buffering
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();

            if (ConsistentForm.IsFullScreen)                    // Check if the previous page was set to fullscreen
            {   
                SetFullScreen();    // Set this form to fullscreen if true
            }

            // Initialize Results Image
            outputImage = new PictureBox();                                           // Initialize new pictureBox to hold results
            outputImage.Image = resultImage;                                          // Insert image into pictureBox
            outputImage.SizeMode = PictureBoxSizeMode.Zoom;                          // Zoom image to fit size
            outputImage.Size = new Size(800, 400);                                    // Size image to 300 x 200
            outputImage.Location = new Point(                                         // Position Image to center top of the page
                (resultsPagePanel.Width / 2)-(outputImage.Width / 2),
                (resultsPagePanel.Height / 2)-(outputImage.Height / 2));
            resultsPagePanel.Controls.Add(outputImage);                                           // Create new picturebox on page

            // Populate user provided data text boxes with data from the last upload
            if (GlobalData.Uploads.Any())
            {
                UploadInfo lastUpload = GlobalData.Uploads.Last();            // Get the most recent upload
                uploadNameTextBox.Text = lastUpload.UploadName;               // Upload Name
                dateUploadedTextBox.Text = lastUpload.UploadTime.ToString();  // Date Uploaded
                dateOfSampleTextBox.Text = lastUpload.SampleTime;  // Date of Sample
                sampleLocationTextBox.Text = lastUpload.SampleLocation;       // Location of Sample
                sheepBreedTextBox.Text = lastUpload.SheepBreed;               // Sheep Breed

                // Apply Standard font to all textboxes
                sheepBreedTextBox.Font = new Font("Times New Roman", 10, FontStyle.Regular);        // Set font for sheepbreed text box
                uploadNameTextBox.Font = new Font("Times New Roman", 10, FontStyle.Regular);        // Set font for upload name text box
                dateUploadedTextBox.Font = new Font("Times New Roman", 10, FontStyle.Regular);      // Set font for upload date text box
                dateOfSampleTextBox.Font = new Font("Times New Roman", 10, FontStyle.Regular);      // Set font for sample date text box
                sampleLocationTextBox.Font = new Font("Times New Roman", 10, FontStyle.Regular);    // Set font for sample location text box
            }
            else
            {
                // Handle case where there is no data
                MessageBox.Show("Results Error : User Provided Data unavailable");
            }

            // Populate machine learning model textboxes with data from current upload
            if (GlobalData.machineLearningData.Any())
            {
                MLData lastMLProcess = GlobalData.machineLearningData.Last();       // Access most recent ML Data Provided
                qufuTextBox.Text = lastMLProcess.qufuPercentage;                    // Access qufu percentage
                erciTextBox.Text = lastMLProcess.erciPercentage;                    // Access erci percentage
                //stemTextBox.Text = lastMLProcess.qufustemPercentage;                // Access qufu stem percentage
                bubbleTextBox.Text = lastMLProcess.bubblePercentage;                // Access air bubble percentage
                naleTextBox.Text = lastMLProcess.nalePercentage;                    // Access nale percentage

                qufuTextBox.Font = new Font("Times New Roman", 10, FontStyle.Regular);      // Set Font for Qufu TextBox
                erciTextBox.Font = new Font("Times New Roman", 10, FontStyle.Regular);      // Set Font for Erci TextBox
                //stemTextBox.Font = new Font("Times New Roman", 10, FontStyle.Regular);      // Set Font for Qufu Stem TextBox
                bubbleTextBox.Font = new Font("Times New Roman", 10, FontStyle.Regular);    // Set Font for Bubble TextBox
                naleTextBox.Font = new Font("Times New Roman", 10, FontStyle.Regular);      // Set Font for Nale TextBox
            }
            else
            {
                // Handle issues with GlobalData passing 
                MessageBox.Show("Results Error : ML Data is unavailable");
            }

            CenterPanel();

            // Event Handlers
            this.Resize += ResultsPage_Resize;      // Handle form resize
            this.FormClosing += ResultsPage_XOut;   // handle form being closed

            // Save data to DB
            SaveResultsToCSVAndUpload();

        }

        // Override the Windows procedure to intercept window messages
        protected override void WndProc(ref Message m)
        {
            // Store the original window state before processing the message
            FormWindowState org = this.WindowState;

            // Call the base class implementation to process the window message
            base.WndProc(ref m);

            // Check if the window state has changed after processing the message
            if (this.WindowState != org)
                // Trigger the custom event handler for window state changes
                this.OnFormWindowStateChanged(EventArgs.Empty);
        }

        // Define a virtual method to handle the form's window state changes
        protected virtual void OnFormWindowStateChanged(EventArgs e)
        {
            // Check if the window is in the normal state (restored from minimized or maximized)
            if (this.WindowState == FormWindowState.Normal)
            {
                // Set the form's size to its minimum size in normal state
                this.Size = MinimumSize;
            }
            // Check if the window is in the maximized state
            else if (this.WindowState == FormWindowState.Maximized)
            {
                // Set the form's size to its maximum size in maximized state
                this.Size = MaximumSize;
            }

            // Force the form to refresh its appearance after the size change
            Refresh();
        }

        // Method to set full screen
        private void SetFullScreen()    
        {
            this.WindowState = FormWindowState.Maximized;       // Maximize the form
            this.FormBorderStyle = FormBorderStyle.Sizable;     // Set the border style for fullscreen
            Bounds = Screen.PrimaryScreen.Bounds;          // Set the bounds of the form to be fullscreen
        }

        // Handler for user x-ing out of the page   
        private void ResultsPage_XOut(object sender, FormClosingEventArgs e)
        {
            // Check that the user intentionally is closing the page
            if (IsNavigating)
            {
                return;
            }
            // If user intentionally closes, close all app processes
            if (e.CloseReason == CloseReason.UserClosing)
            {
                ClearGlobalData();
                _mainPage.Close();      // Close reference to mainpage
            }
        }

        private void returnButton_Click(object? sender, EventArgs e)  // Method for returning to mainPage
        {
            IsNavigating = true;   // User is still using the app

            // Checks to make sure MainPage form is not null
            if (_mainPage != null)
            {
                this.Invalidate();

                //MessageBox.Show("Main Page Size 5 : " + _mainPage.ClientSize.ToString());
                _mainPage.Visible = true;                 // open main page
                _mainPage.WindowState = this.WindowState; // Force this window state onto main page
                _mainPage.ExternalResize(this.Size, this.Location);

            }

            //MessageBox.Show("Main Page State Final: " + _mainPage.WindowState.ToString() + "\nMain Page Size : " + _mainPage.ClientSize.ToString());
            this.Close();// Close Data Upload Page
        }

        private void dataViewButton_Click(object? sender, EventArgs e)  // Method for returning to mainPage
        {
            IsNavigating = true;    // Check to make sure the user is not closing the application

            // Check if current form is fullscreen
            if (this.WindowState == FormWindowState.Maximized)
            {
                ConsistentForm.IsFullScreen = true;     // If true, set next page to fullscreen
            }
            else
            {
                ConsistentForm.IsFullScreen = false;    // If false, do not set next screen to fullscreen
            }

            
            var dbQueries = new DBQueries(_dbConnections.ConnectionString); // Ensure connectionString is correct
            DataLibrary datalibrary = new DataLibrary(_mainPage, dbQueries);
            datalibrary.Size = this.Size;                       // Set next page to the same size as this page
            datalibrary.Location = this.Location;               // Set next page to the same location as this page
            datalibrary.Show();                                 // Show dataLibrary
            this.Hide();                                        // Hide mainPage
        }

        private void returnToUploadButton_Click(object? sender, EventArgs e)  // Method for returning to mainPage
        {
            IsNavigating = true;    // Check to make sure the user is not closing the application

            // Check if current form is fullscreen
            if (this.WindowState == FormWindowState.Maximized)
            {
                ConsistentForm.IsFullScreen = true;     // If true, set next page to fullscreen
            }
            else
            {
                ConsistentForm.IsFullScreen = false;    // If false, do not set next screen to fullscreen
            }


            DataUpload dataupload = new DataUpload(_mainPage);       // Create new dataUpload form
            dataupload.Size = this.Size;                        // Set next page to the same size as this page
            dataupload.Location = this.Location;                // Set next page to the same location as this page
            dataupload.Show();                                  // Show dataUpload
            this.Close();                                        // Hide mainPage
        }

        // Event handler used to call resizing methods
        private void ResultsPage_Resize(object? sender, EventArgs e)
        {
            CenterPanel();  // Call CenterPanel
        }

        // Method to handle resizing - keeps the results panel in the center and handles element repositioning
        private void CenterPanel()
        {
            resultsPagePanel.Visible = false;

            // Update the size and location of resultsPagePanel
            resultsPagePanel.Size = new Size((int)(this.ClientSize.Width * 0.66), (int)(this.ClientSize.Height * 0.708));
            resultsPagePanel.Location = new Point(
                (this.ClientSize.Width / 2) - (resultsPagePanel.Width / 2),
                (this.ClientSize.Height / 2) - (resultsPagePanel.Height / 2) - 50
            );

            // Adjust UserOutputPanel
            UserOutputPanel.Size = new Size((int)(resultsPagePanel.Width * 0.63), 125); // 63% width
            UserOutputPanel.Location = new Point(0, resultsPagePanel.Height - UserOutputPanel.Height); // Align bottom left

            // Adjust MLOutputPanel
            MLOutputPanel.Size = new Size((int)(resultsPagePanel.Width * 0.37), 125); // 37% width
            MLOutputPanel.Location = new Point(UserOutputPanel.Width, resultsPagePanel.Height - MLOutputPanel.Height); // Align to the right of UserOutputPanel

            // Adjust PictureBox to fill the remaining space
            outputImage.Size = new Size(resultsPagePanel.Width, resultsPagePanel.Height - UserOutputPanel.Height);
            outputImage.Location = new Point(0, 0); // Align top left of resultsPagePanel

            this.Refresh(); // Refresh to apply changes
            resultsPagePanel.Visible = true;
        }

        // Event handler for the Help button click
        private void helpButton_Click(object sender, EventArgs e)
        {
            UserGuide.ShowHelpGuide(); // Open the user guide, ensuring only one instance is active
        }

        // Method for saving ML results and strings to DB
        public void SaveResultsToCSVAndUpload()
        {
            if (!GlobalData.Uploads.Any() || !GlobalData.machineLearningData.Any())
            {
                MessageBox.Show("Error: Missing required data to save.");
                return;
            }

            UploadInfo lastUpload = GlobalData.Uploads.Last();
            MLData lastMLProcess = GlobalData.machineLearningData.Last();

            // Debugging
            MessageBox.Show($"Uploading Data:\n" +
                $"Qufu: {lastMLProcess.qufuPercentage}\n" +
                $"Nale: {lastMLProcess.nalePercentage}\n" +
                $"Erci: {lastMLProcess.erciPercentage}\n" +
                $"Bubbles: {lastMLProcess.bubblePercentage}");


            List<string> csvData = new List<string>
            {
                lastUpload.UploadName,  // SourceFile
                lastMLProcess.qufuPercentage.Replace("%", ""),  // QufuPercent
                lastMLProcess.nalePercentage.Replace("%", ""),  // NalePercent
                lastMLProcess.erciPercentage.Replace("%", ""),  // ErciPercent
                lastMLProcess.bubblePercentage.Replace("%", ""),  // AirBubblePercent
                ConvertToValidDateTime(lastUpload.SampleTime),  // DateSampleTaken
                DateTime.Now.ToString("HH:mm:ss"),  // TimeSampleTaken (assuming current time)
                lastUpload.UploadTime.ToString("yyyy-MM-dd"),  // UploadDate
                lastUpload.UploadTime.ToString("HH:mm:ss"),  // UploadTime
                lastUpload.SampleLocation,  // SampleLocation
                lastUpload.SheepBreed,  // SheepBreed
                "No Comments"  // Default comment field
            };

            DBQueries dbQueries = new DBQueries("Server=sqldatabase404.database.windows.net;Database=404ImageDBsql;User Id=sql404admin;Password=sheepstool404();TrustServerCertificate=False;MultipleActiveResultSets=True;");
            bool success = dbQueries.UploadCSVToDB(csvData);

            if (success)
            {
                MessageBox.Show("Results successfully saved to the database.");
            }
            else
            {
                MessageBox.Show("Error saving results to the database.");
            }
        }

        // Method for clearing GlobalData after each upload
        private void ClearGlobalData()
        {
            GlobalData.Uploads.Clear();               // Clears all uploaded data
            GlobalData.machineLearningData.Clear();   // Clears all ML processed data
        }


        private string ConvertToValidDateTime(string input)
        {
            if (DateTime.TryParse(input, out DateTime result))
            {
                return result.ToString("yyyy-MM-dd"); // Ensure correct format
            }

            return DateTime.Now.ToString("yyyy-MM-dd"); // Default to today’s date if invalid
        }
    }
}