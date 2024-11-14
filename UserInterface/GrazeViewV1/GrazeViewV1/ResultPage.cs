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

        public ResultPage(Image resultImage)  // Build page with resulting image from ML and previous page's size/location
        {
            InitializeComponent();
            this.Size = ConsistentForm.FormSize;
            this.Location = ConsistentForm.FormLocation;
            if (ConsistentForm.IsFullScreen)
            {
                SetFullScreen();
            }

            // Initialize Results Image
            outputImage = new PictureBox();                                           // Initialize new pictureBox to hold results
            outputImage.Image = resultImage;                                          // Insert image into pictureBox
            outputImage.SizeMode = PictureBoxSizeMode.Zoom;                           // Zoom image to fit size
            outputImage.Size = new Size(300, 200);                                    // Size image to 300 x 200
            outputImage.Location = new Point(                                         // Position Image to center top of the page
                (this.ClientSize.Width / 2) - (outputImage.Width / 2),
                20);
            this.Controls.Add(outputImage);                                           // Create new picturebox on page

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

            MainPage mainPage = new MainPage();  // Initialize new main page
            mainPage.Show();    // Show new main page
            this.Hide();        // Hide current page
        }

        private void SetFullScreen()     // Class to handle screen maximization
        {
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Bounds = Screen.PrimaryScreen.Bounds;
        }

    }
}