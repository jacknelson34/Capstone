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
        private PictureBox outputImage;
        private bool IsNavigating;

        // Hold instances of the opened pages
        private MainPage _mainPage;

        public ResultPage(Image resultImage, MainPage mainPage)  // Build page with resulting image from ML and previous page's size/location
        {
            IsNavigating = false; 
            InitializeComponent();
            this.Size = ConsistentForm.FormSize;
            this.Location = ConsistentForm.FormLocation;
            this.Text = "GrazeView";
            _mainPage = mainPage;

            if (ConsistentForm.IsFullScreen)
            {
                SetFullScreen();
            }

            MessageBox.Show("ClientSize.Width = " + this.ClientSize.Width.ToString());

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

                // Apply Standard font to all textboxes
                sheepBreedTextBox.Font = new Font("Times New Roman", 10, FontStyle.Regular);
                uploadNameTextBox.Font = new Font("Times New Roman", 10, FontStyle.Regular);
                dateUploadedTextBox.Font = new Font("Times New Roman", 10, FontStyle.Regular);
                dateOfSampleTextBox.Font = new Font("Times New Roman", 10, FontStyle.Regular);
                sampleLocationTextBox.Font = new Font("Times New Roman", 10, FontStyle.Regular);
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

                qufuTextBox.Font = new Font("Times New Roman", 10, FontStyle.Regular);
                erciTextBox.Font = new Font("Times New Roman", 10, FontStyle.Regular);
                stemTextBox.Font = new Font("Times New Roman", 10, FontStyle.Regular);
                bubbleTextBox.Font = new Font("Times New Roman", 10, FontStyle.Regular);
                naleTextBox.Font = new Font("Times New Roman", 10, FontStyle.Regular);
            }
            else
            {
                // Handle issues with GlobalData passing 
                MessageBox.Show("Results Error : ML Data is unavailable");
            }

            CenterPanel();

            // Event Handlers
            this.Resize += ResultsPage_Resize;
            this.FormClosing += ResultsPage_XOut;

        }

        private void SetFullScreen()
        {
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Bounds = Screen.PrimaryScreen.Bounds;
        }

        private void ResultsPage_XOut(object sender, FormClosingEventArgs e)
        {
            if (IsNavigating)
            {
                return;
            }
            if (e.CloseReason == CloseReason.UserClosing)
            {
                _mainPage.Close();
            }
        }

        private void returnButton_Click(object? sender, EventArgs e)  // Method for returning to mainPage
        {
            IsNavigating = true;

            if (this.WindowState == FormWindowState.Maximized)
            {
                ConsistentForm.IsFullScreen = true;
            }
            else
            {
                ConsistentForm.IsFullScreen = false;
            }

            _mainPage.Size = this.Size;
            _mainPage.Location = this.Location;
            _mainPage.Show();    // Show main page
            this.Close();         // Hide current page
        }

        private void dataViewButton_Click(object? sender, EventArgs e)  // Method for returning to mainPage
        {
            IsNavigating = true;

            if (this.WindowState == FormWindowState.Maximized)
            {
                ConsistentForm.IsFullScreen = true;
            }
            else
            {
                ConsistentForm.IsFullScreen = false;
            }

            DataLibrary datalibrary = new DataLibrary(_mainPage);    // Create new dataLibrary
            datalibrary.Size = this.Size;
            datalibrary.Location = this.Location;
            datalibrary.Show();                                 // Show dataLibrary
            this.Hide();                                        // Hide mainPage
        }

        private void returnToUploadButton_Click(object? sender, EventArgs e)  // Method for returning to mainPage
        {
            IsNavigating = true;

            if (this.WindowState == FormWindowState.Maximized)
            {
                ConsistentForm.IsFullScreen = true;
            }
            else
            {
                ConsistentForm.IsFullScreen = false;
            }


            DataUpload dataupload = new DataUpload(_mainPage);       // Create new dataUpload form
            dataupload.Size = this.Size;
            dataupload.Location = this.Location;
            dataupload.Show();                                  // Show dataUpload
            this.Close();                                        // Hide mainPage
        }

        private void ResultsPage_Resize(object? sender, EventArgs e)
        {
            CenterPanel();
        }

        private void CenterPanel()
        {
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
        }
    }
}