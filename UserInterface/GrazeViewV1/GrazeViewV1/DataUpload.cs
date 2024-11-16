using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrazeViewV1
{
    public partial class DataUpload : Form
    {
        // hold reference to Main Page form
        private MainPage _mainPage;
        Image thumbnail;

        // variable that tracks if a file has or has not been uploaded
        private bool imageUploaded = false;

        public DataUpload(MainPage mainpage)
        {
            // Form Properties
            InitializeComponent();
            _mainPage = mainpage;
            this.Size = ConsistentForm.FormSize;
            this.Location = ConsistentForm.FormLocation;
            if (ConsistentForm.IsFullScreen)
            {
                SetFullScreen();
            }

        }

        private void SetFullScreen()     // Class to handle screen maximization
        {
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Bounds = Screen.PrimaryScreen.Bounds;
        }

        // when the back button is clicked on
        private void backButton_Click(object? sender, EventArgs e)
        {
            ConsistentForm.FormSize = this.Size;                // Adjust consistent form parameters if form was resized
            ConsistentForm.FormLocation = this.Location;        // Adjust consistent form parameters if form was relocated
            if (this.WindowState == FormWindowState.Maximized)
            {
                ConsistentForm.IsFullScreen = true;
            }
            else
            {
                ConsistentForm.IsFullScreen = false;
            }

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
                        Image originalImage = fileuploadPictureBox.Image; // Store for thumbnail method

                        // Generate the thumbnail image
                        thumbnail = CreateThumbnail(originalImage, 100, 100);  // 100x100 size, adjust as needed

                        // Delete Text in the Picture Box
                        imageUploaded = true;

                        // If the textbox is empty, populate it with the upload time/date
                        if (string.IsNullOrWhiteSpace(filenameTextbox.Text))
                        {
                            // filenameTextbox.Text = Path.GetFileName(openFileDialog.FileName); // Extracts just the file name
                            // Access current time and date
                            DateTime currentTime = DateTime.Now;

                            // Access upload #
                            int uploadNum = GlobalData.Uploads.Count + 1;
                            string nameForcurrentTime = currentTime.ToString("MM-dd-yyyy-") + uploadNum + ".png";
                            filenameTextbox.Text = "Upload-" + nameForcurrentTime;

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
            ConsistentForm.FormSize = this.Size;                // Adjust consistent form parameters if form was resized
            ConsistentForm.FormLocation = this.Location;        // Adjust consistent form parameters if form was relocated
            if (this.WindowState == FormWindowState.Maximized)
            {
                ConsistentForm.IsFullScreen = true;
            }
            else
            {
                ConsistentForm.IsFullScreen = false;
            }

            // Check if the Sample Location, Sheep Breed, or Comments are empty, and set them to "N/A" if they are
            string sampleLocation = string.IsNullOrWhiteSpace(locationTextbox.Text) ? "N/A" : locationTextbox.Text;
            string sheepBreed = string.IsNullOrWhiteSpace(breedTextbox.Text) ? "N/A" : breedTextbox.Text;
            string comments = string.IsNullOrWhiteSpace(commentsTextbox.Text) ? "N/A" : commentsTextbox.Text;

            // Create a new instance of UploadInfo
            UploadInfo uploadInfo = new UploadInfo
            {
                UploadName = filenameTextbox.Text,             // Store the upload name
                SampleLocation = sampleLocation,               // Store the sample location (or N/A)
                SampleDate = DateTime.Parse(datePicker.Text),  // Store the sample date
                SampleTime = DateTime.Parse(timePicker.Text),  // Assuming sampleTime.Text is a valid date/time string
                UploadTime = DateTime.Now,                     // Store the current time of upload
                SheepBreed = sheepBreed,                       // Store the sheep breed (or N/A)
                Comments = comments                            // Store user comments (or N/A)
                // ThumbNail = thumbnail                       // Store thumbnail image
            };

            // Debugging: Confirm data is added to the UploadInfo object
            //MessageBox.Show($"Captured UploadName: {uploadInfo.UploadName}");


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
                LoadingPage loadingPage = new LoadingPage(fileuploadPictureBox.Image, _mainPage);
                loadingPage.Show();
                this.Close();
            }
            else
            {
                // Show error message if no valid image was uploaded
                MessageBox.Show("Please upload a valid image file.", "Invalid Upload", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // helper method to create a thumbnail version of the uploaded image
        private Image CreateThumbnail(Image originalImage, int width, int height)
        {
            return originalImage.GetThumbnailImage(width, height, () => false, IntPtr.Zero);
        }
    }
}