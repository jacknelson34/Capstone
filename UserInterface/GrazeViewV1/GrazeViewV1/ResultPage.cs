using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrazeViewV1
{
    public partial class ResultPage : Form
    {
        private Button returnButton;
        private PictureBox outputImage;

        // Hold instances of the opened pages
        private MainPage _mainPage;

        public ResultPage(Image resultImage, MainPage mainPage)  // Build page with resulting image from ML and previous page's size/location
        {
            InitializeComponent();
            _mainPage = mainPage;
            this.Size = ConsistentForm.FormSize;
            this.Location = ConsistentForm.FormLocation;
            if (ConsistentForm.IsFullScreen)
            {
                SetFullScreen();
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
                dateOfSampleTextBox.Text = lastUpload.SampleTime.ToString();  // Date of Sample
                sampleLocationTextBox.Text = lastUpload.SampleLocation;       // Location of Sample
                sheepBreedTextBox.Text = lastUpload.SheepBreed;               // Sheep Breed
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
                stemTextBox.Text = lastMLProcess.qufustemPercentage;                // Access qufu stem percentage
                bubbleTextBox.Text = lastMLProcess.bubblePercentage;                // Access air bubble percentage
                naleTextBox.Text = lastMLProcess.nalePercentage;                    // Access nale percentage
            }
            else
            {
                // Handle issues with GlobalData passing 
                MessageBox.Show("Results Error : ML Data is unavailable");
            }

            CenterPanel();
            this.Resize += ResultsPage_Resize;

        }

        private void returnButton_Click(object? sender, EventArgs e)  // Method for returning to mainPage
        {
            ConsistentForm.FormSize = this.Size;
            ConsistentForm.FormLocation = this.Location;
            if (this.WindowState == FormWindowState.Maximized)
            {
                ConsistentForm.IsFullScreen = true;
            }
            else
            {
                ConsistentForm.IsFullScreen = false;
            }

            _mainPage.Show();    // Show main page
            this.Close();         // Hide current page
        }

        private void dataViewButton_Click(object? sender, EventArgs e)  // Method for returning to mainPage
        {
            ConsistentForm.FormSize = this.Size;
            ConsistentForm.FormLocation = this.Location;
            if (this.WindowState == FormWindowState.Maximized)
            {
                ConsistentForm.IsFullScreen = true;
            }
            else
            {
                ConsistentForm.IsFullScreen = false;
            }

            DataLibrary datalibrary = new DataLibrary(_mainPage);    // Create new dataLibrary
            datalibrary.Show();                                 // Show dataLibrary
            this.Hide();                                        // Hide mainPage
        }

        private void returnToUploadButton_Click(object? sender, EventArgs e)  // Method for returning to mainPage
        {
            ConsistentForm.FormSize = this.Size;
            ConsistentForm.FormLocation = this.Location;
            if (this.WindowState == FormWindowState.Maximized)
            {
                ConsistentForm.IsFullScreen = true;
            }
            else
            {
                ConsistentForm.IsFullScreen = false;
            }

            DataUpload dataupload = new DataUpload(_mainPage);       // Create new dataUpload form
            dataupload.Show();                                  // Show dataUpload
            this.Close();                                        // Hide mainPage
        }

        private void SetFullScreen()     // Class to handle screen maximization
        {
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Bounds = Screen.PrimaryScreen.Bounds;
        }

        private void ResultsPage_Resize(object? sender, EventArgs e)
        {
            CenterPanel();
        }

        private void CenterPanel()
        {
            // Center the panel and adjust size with resizing
            resultsPagePanel.Size = new Size((int)(this.ClientSize.Width * 0.637), (int)(this.ClientSize.Height * .708));
            resultsPagePanel.Location = new Point((this.ClientSize.Width / 2) - (resultsPagePanel.Width / 2), (this.ClientSize.Height / 2) - (resultsPagePanel.Height / 2));
            this.Refresh();

            // Center and Adjust sizing of the user input data
            UserOutputPanel.Size = new Size((int)(resultsPagePanel.Width * 0.625), 200);

            this.Refresh();
            // Center and adjust sizing of the ML Output
            MLOutputPanel.Location = new Point(UserOutputPanel.Width, UserOutputPanel.Location.Y);
            MLOutputPanel.Size = new Size(resultsPagePanel.Width - UserOutputPanel.Width, UserOutputPanel.Height);


        }
    }
}