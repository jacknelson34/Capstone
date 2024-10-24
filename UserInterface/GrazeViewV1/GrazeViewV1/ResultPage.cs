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
    public partial class ResultPage : ConsistentForm
    {
        private Button returnButton;
        private PictureBox outputImage;

        public ResultPage(Image resultImage)  // Build page with resulting image from ML and previous page's size/location
        {
            InitializeComponent();

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

            // Call CenterControls for page resize Events
            CenterControls();
            this.Resize += ResultsPage_Resize;
        }

        private void returnButton_Click(object? sender, EventArgs e)  // Method for returning to mainPage
        {
            MainPage mainPage = new MainPage();  // Initialize new main page
            mainPage.Show();    // Show new main page
            this.Hide();        // Hide current page
        }

        private void CenterControls()  // Method for centering elements of the form
        {
            // Center outputImage to top-center of the page
            outputImage.Location = new Point(
                (this.ClientSize.Width / 2) - (outputImage.Width / 2),
                20);  // Set top margin to 20px

        }

        private void ResultsPage_Resize(object? sender, EventArgs e)  // Event Handler when the Form is resized by user
        {
            CenterControls();  // Call method for adjusting controls
        }
    }
}