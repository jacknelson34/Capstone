using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrazeViewV1
{
    public partial class DataUpload : Form
    {
        // hold reference to Main Page form
        private MainPage _mainPage;

        // variable that tracks if a file has or has not been uploaded
        private bool imageUploaded = false;

        public DataUpload(MainPage mainpage)
        {
            // Form Properties
            InitializeComponent();
            _mainPage = mainpage;
            this.Text = "Data Upload";

            // Back Button Functionality
            backButton.Click += backButton_Click; // handles click event
            this.Controls.Add(backButton);

            // Help Button Functionality
            helpButton.Click += helpButton_Click; // handles click event
            this.Controls.Add(helpButton);

            // File Upload Picture Box Functionality
            fileuploadPictureBox.Click += fileuploadPictureBox_Click;           // handles click-to-upload event
            fileuploadPictureBox.DragEnter += fileuploadPictureBox_DragEnter;   // handles drag enter event
            fileuploadPictureBox.DragDrop += fileuploadPictureBox_DragDrop;     // handles drag-and-drop event
            fileuploadPictureBox.Paint += fileuploadPictureBox_Prompt;          // adds prompt to picture box
            this.Controls.Add(fileuploadPictureBox);

            // File Name Text Box Functionality
            this.Controls.Add(filenameTextbox);

            // Upload Button Functionality
            uploadButton.Click += uploadButton_Click;
            this.Controls.Add(uploadButton);

            // File Name Text Box Functionality
            this.Controls.Add(filenameTextbox);

            // Sample Location Text Box Functionality
            this.Controls.Add(locationTextbox);

            // Sample Date Picker Functionality
            this.Controls.Add(datePicker);

            // Sample Time Picker Functionality
            this.Controls.Add(timePicker);

            // Sheep Breed Text Box Functionality
            this.Controls.Add(breedTextbox);

            // Comments Text Box Functionality
            this.Controls.Add(commentsTextbox);

            // Add other needed information text boxes here ->

        }

        // when the back button is clicked on
        private void backButton_Click(object? sender, EventArgs e)
        {
            _mainPage.Show();                                       // Open Main Page
            this.Hide();                                            // Close Data Upload Page
        }

        // when the help icon is clicked on
        private void helpButton_Click(object sender, EventArgs e)
        {
            UserGuide.ShowHelpGuide();  // Call Method to only allow one instance open at a time
        }

        // add text inside the picture box to prompt the user to either click on or drag-and-drop files
        private void fileuploadPictureBox_Prompt(object sender, PaintEventArgs e)
        {
            // Only show text if no image is loaded
            if (!imageUploaded)
            {
                string promptText = "Click or Drop Your Files Here";
                Font font = new Font("Arial", 12, FontStyle.Regular);
                SizeF textSize = e.Graphics.MeasureString(promptText, font);

                // Draw dashed border
                using (Pen dashedPen = new Pen(Color.Gainsboro, 2))
                {
                    dashedPen.DashPattern = new float[] { 5, 2 };
                    e.Graphics.DrawRectangle(dashedPen, 0, 0, fileuploadPictureBox.Width - 1, fileuploadPictureBox.Height - 1);
                }

                // Draw the text in the center of the PictureBox
                e.Graphics.DrawString(promptText, font, Brushes.Gray,
                    (fileuploadPictureBox.Width - textSize.Width) / 2,
                    (fileuploadPictureBox.Height - textSize.Height) / 2);
            }
        }

        // when the file upload picture box is clicked on
        private void fileuploadPictureBox_Click(object sender, EventArgs e)
        {
            // Open a file dialog to allow the user to select an image file
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // Set the filter to allow only image files
                openFileDialog.Filter = "PNG Files|*.png;";
                openFileDialog.Title = "Select a PNG Image";

                // If the user selects a file and clicks OK
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Load the selected image into the PictureBox
                        fileuploadPictureBox.Image = Image.FromFile(openFileDialog.FileName);
                        fileuploadPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

                        // Delete Text in the Picture Box
                        imageUploaded = true;

                        // If the textbox is empty, populate it with the file name
                        if (string.IsNullOrWhiteSpace(filenameTextbox.Text))
                        {
                            filenameTextbox.Text = Path.GetFileName(openFileDialog.FileName); // Extracts just the file name
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle any errors that occur during image loading
                        MessageBox.Show("An error occurred while trying to load the image: " + ex.Message);
                    }
                }
            }
        }

        // when an image file is dragged to the picture box
        private void fileuploadPictureBox_DragEnter(object sender, DragEventArgs e)
        {
            // Check if the dragged item is a file and is a .png image
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0 && IsPngFile(files[0]))
                {
                    e.Effect = DragDropEffects.Copy;  // Show that we can copy the file
                }
                else
                {
                    e.Effect = DragDropEffects.None;  // Invalid file type
                }
            }
        }

        // when an image file is dragged and dropped onto the picture box
        private void fileuploadPictureBox_DragDrop(object sender, DragEventArgs e)
        {
            // Get the file(s) that are dropped
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                // Load the first .png image file into the PictureBox
                if (files.Length > 0 && IsPngFile(files[0]))
                {
                    try
                    {
                        // Load the selected image into the PictureBox
                        fileuploadPictureBox.Image = Image.FromFile(files[0]);
                        fileuploadPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

                        // Delete Text in the Picture Box
                        imageUploaded = true;

                        // If the textbox is empty, populate it with the file name
                        if (string.IsNullOrWhiteSpace(filenameTextbox.Text))
                        {
                            filenameTextbox.Text = Path.GetFileName(files[0]); // Extracts just the file name
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle any errors that occur during image loading
                        MessageBox.Show("An error occurred while trying to load the image: " + ex.Message);
                    }
                }
            }
        }

        // helper method to check if the file is a .png image
        private bool IsPngFile(string fileName)
        {
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            return ext == ".png";  // Only allow .png files
        }

        // when the upload button is clicked on
        private void uploadButton_Click(object? sender, EventArgs e)
        {
            // Create a new instance of UploadInfo
            UploadInfo uploadInfo = new UploadInfo
            {
                UploadName = filenameTextbox.Text,             // Store the upload name
                SampleLocation = locationTextbox.Text,         // Store the sample location
                SampleDate = DateTime.Parse(datePicker.Text),  // Store the sample date
                SampleTime = DateTime.Parse(timePicker.Text),  // Assuming sampleTime.Text is a valid date/time string
                UploadTime = DateTime.Now,                     // Store the current time of upload
                SheepBreed = breedTextbox.Text,                // Store the sheep breed
                Comments = commentsTextbox.Text                // Store user comments
            };

            // Debugging: Confirm data is added to the UploadInfo object
            // MessageBox.Show($"Captured UploadName: {uploadInfo.UploadName}");


            // Add the new upload info to the GlobalData uploads list
            GlobalData.Uploads.Add(uploadInfo);

            // Debugging: Check the contents of GlobalData.Uploads
            // MessageBox.Show($"Total uploads in GlobalData: {GlobalData.Uploads.Count}");

            // Add user inputs and image to data library
            var dataLibrary = _mainPage.GetDataLibrary();
            if (dataLibrary != null)
            {
                // Ensure the DataLibrary updates its grid with all uploads
                dataLibrary.LoadUploadsFromGlobalData();  // Call the new method to reload all data from GlobalData
            }
            else
            {
                MessageBox.Show("DataLibrary not connected.");
            }

            // checks to see if a file was uploaded to the picturebox
            if (fileuploadPictureBox.Image != null)
            {
                // Proceed to the loading page if a valid image is uploaded
                LoadingPage loadingPage = new LoadingPage(fileuploadPictureBox.Image);
                loadingPage.Show();
                this.Hide();
            }
            else
            {
                // Show error message if no valid image was uploaded
                MessageBox.Show("Please upload a valid image file.", "Invalid Upload", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

    }
}