using Microsoft.VisualBasic;
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
        private MainPage _mainPage;  // hold reference to Main Page form
        Image uploadImage;           // variable to hold user-uploaded image
        private bool IsNavigating;   // boolean variable that checks if the user is still using the app

        // variable that tracks if a file has or has not been uploaded
        private bool imageUploaded = false;
        private System.Windows.Forms.Timer resizeTimer;

        public DataUpload(MainPage mainpage)
        {
            IsNavigating = false;   // user is no longer using (default setting)
            // Enable double buffering
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();

            // Form Properties
            InitializeComponent();
            _mainPage = mainpage;
            this.Text = "GrazeView";
            this.Refresh();


            // Add timer to reduce resizing lag
            resizeTimer = new System.Windows.Forms.Timer { Interval = 250 };
            resizeTimer.Tick += (s, e) =>
            {
                resizeTimer.Stop();
                controlsResize();
            };

            // Event Handler for form close
            this.FormClosing += DataUpload_XOut;
            this.Resize += DataUpload_Resize;

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

        // Helper method to close down the app if the top right exit button is pressed
        private void DataUpload_XOut(object sender, FormClosingEventArgs e)
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


        // Event handler for when the back button is clicked on
        private void backButton_Click(object? sender, EventArgs e)
        {
            IsNavigating = true;   // User is still using the app

            // Checks to make sure MainPage form is not null
            if (_mainPage != null)
            {
                this.Refresh();
                _mainPage.SuspendLayout();
                _mainPage.WindowState = this.WindowState; // Ensure state is applied first
                if (this.WindowState == FormWindowState.Normal)
                {
                    // Safeguard to prevent unnecessary size/location changes
                    _mainPage.ExternalResize(this.Size, this.Location);
                }
                _mainPage.ResumeLayout();
            }
            _mainPage.Show();                                       // Open Main Page
            this.Close();                                            // Close Data Upload Page
        }

        // Event handler for when the help icon is clicked on
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
                        // Generate Thumbnail for display
                        Image thumbnail = CreateThumbnail(openFileDialog.FileName, fileuploadPictureBox.Width, fileuploadPictureBox.Height);

                        // Load the selected image into the PictureBox
                        fileuploadPictureBox.Image = thumbnail;
                        fileuploadPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

                        // Delete Text in the Picture Box
                        imageUploaded = true;
                        uploadImage = Image.FromFile(openFileDialog.FileName);

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

                    uploadImage = fileuploadPictureBox.Image;
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
                        uploadImage = fileuploadPictureBox.Image;

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
            IsNavigating = true;  // User is still using the app

            // Maintain consistent form sizing
            if (this.WindowState == FormWindowState.Maximized)
            {
                ConsistentForm.IsFullScreen = true;
            }
            else
            {
                ConsistentForm.IsFullScreen = false;
            }

            // Date/Time Input Validation
            // Make sure date is after Jan 1st, 2000 and before current date
            if (DateTime.Parse(datePicker.Text) < new DateTime(2000, 1, 1) || DateTime.Parse(datePicker.Text) > DateTime.Today)
            {
                MessageBox.Show("Please Enter a Date between January 1st, 2000 and today.",
                    "Invalid Date Entered");
                return;
            }
            // Handle format errors from user/validate input
            try
            {
                // Make sure that the time is valid format
                DateTime minTime = DateTime.Today.AddHours(0);
                DateTime maxTime = DateTime.Today.AddHours(24);
                if (DateTime.Parse(timePicker.Text) < minTime || DateTime.Parse(timePicker.Text) > maxTime)
                {
                    MessageBox.Show("Please enter a valid time.",
                        "Invalid Time");
                    timePicker.Text = "0800AM";
                    return;
                }
            }
            catch (FormatException)
            {
                MessageBox.Show($"Invalid Time Entered: {timePicker.Text}.  Please provide a valid time.",
                                "Invalid Time",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            // Check if the Sample Location, Sheep Breed, or Comments are empty, and set them to "N/A" if they are
            string sampleLocation = string.IsNullOrWhiteSpace(locationTextbox.Text) ? "N/A" : locationTextbox.Text;
            string sheepBreed = string.IsNullOrWhiteSpace(breedTextbox.Text) ? "N/A" : breedTextbox.Text;
            string comments = string.IsNullOrWhiteSpace(commentsTextbox.Text) ? "N/A" : commentsTextbox.Text;

            /// ---------------------------------------------- INTEGRATION POINT --------------------------------------------------///

            // Create a new instance of UploadInfo
            UploadInfo uploadInfo = new UploadInfo
            {
                UploadName = filenameTextbox.Text,             // Store the upload name
                SampleLocation = sampleLocation,               // Store the sample location (or N/A)
                SampleDate = DateTime.Parse(datePicker.Text),  // Store the sample date
                SampleTime = DateTime.Parse(timePicker.Text),  // Assuming sampleTime.Text is a valid date/time string
                UploadTime = DateTime.Now,                     // Store the current time of upload
                SheepBreed = sheepBreed,                       // Store the sheep breed (or N/A)
                Comments = comments,                           // Store user comments (or N/A)
                ImageFile = uploadImage                        // Store image
            };

            // Add the new upload info to the GlobalData uploads list
            GlobalData.Uploads.Add(uploadInfo);

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
                ConsistentForm.FormLocation = this.Location;
                ConsistentForm.FormSize = this.Size;
                loadingPage.Show();
                this.Close();
            }
            else
            {
                // Show error message if no valid image was uploaded
                MessageBox.Show("Please upload a valid image file.", "Invalid Upload", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            /// ---------------------------------------------------- INTEGRATION POINT -----------------------------------------------------///
        }


        private void DataUpload_Resize(object sender, EventArgs e)
        {
            resizeTimer.Stop();
            resizeTimer.Start();
        }

        private void controlsResize()
        {
            this.SuspendLayout();
            inputPanel.Visible = false;
            fileuploadPictureBox.Visible = false;

            // Update panel size according to form size
            uploadPanel.Size = new Size(this.ClientSize.Width, (int)(this.ClientSize.Height));
            uploadPanel.Location = new Point(0, 0);

            // Update PictureBox Location
            fileuploadPictureBox.Location = new Point((int)((uploadPanel.Width / 2) - 5), (int)(uploadPanel.Height * 0.12));

            // Update PictureBox Size
            fileuploadPictureBox.Size = new Size((int)(uploadPanel.Width * 0.45), (int)(uploadPanel.Height * 0.75));

            // Update inputPanel size/location
            inputPanel.Location = new Point(40, (int)(uploadPanel.Height * 0.12));
            inputPanel.Size = new Size((int)(uploadPanel.Width * 0.45), (int)(uploadPanel.Height * 0.75));

            // Update Upload Button Location
            uploadButton.Location = new Point((int)(uploadPanel.Width - 140), (int)(uploadPanel.Height - 62));

            // Update Filename Textbox size
            Size textboxSize = new Size((int)(inputPanel.Width * 0.8), 39);
            filenameTextbox.Size = textboxSize;
            locationTextbox.Size = textboxSize;
            breedTextbox.Size = textboxSize;
            commentsTextbox.Size = new Size((int)(inputPanel.Width * 0.8), (int)(inputPanel.Height * 0.25));

            // Update label font sizes
            int fontSize = Math.Max(12, (int)(uploadPanel.Height * 0.014));
            Font updatedFont = new Font("Times New Roman", fontSize, FontStyle.Regular, GraphicsUnit.Point, 0);
            filenameLabel.Font = updatedFont;
            locationLabel.Font = updatedFont;
            datetimeLabel.Font = updatedFont;
            breedLabel.Font = updatedFont;
            commentsLabel.Font = updatedFont;

            // Update File Name Label/TextBox location
            filenameTextbox.Location = new Point((int)((inputPanel.Width / 2) - (filenameTextbox.Width / 2)), (int)(inputPanel.Height * 0.16));
            filenameLabel.Location = new Point(filenameTextbox.Left - 4, filenameTextbox.Top - 25);

            // Update Location Textbox/Label Location
            locationTextbox.Location = new Point((int)((inputPanel.Width / 2) - (locationTextbox.Width / 2)), (int)(inputPanel.Height * 0.28));
            locationLabel.Location = new Point(locationTextbox.Left - 4, locationTextbox.Top - 25);

            // Update Date Time Label/Boxes Location
            datetimeLabel.Location = new Point((int)((inputPanel.Width / 2) - (datetimeLabel.Width / 2)), (int)(inputPanel.Height * 0.36));
            datePicker.Location = new Point((int)((inputPanel.Width / 2) - (datePicker.Width / 2) - 75), (int)(inputPanel.Height * 0.41));
            timePicker.Location = new Point((int)((inputPanel.Width / 2) - (timePicker.Width / 2) + 75), (int)(inputPanel.Height * 0.41));

            // Update Breed Label/Textbox Locations
            breedTextbox.Location = new Point((int)((inputPanel.Width / 2) - (breedTextbox.Width / 2)), (int)(inputPanel.Height * 0.54));
            breedLabel.Location = new Point(breedTextbox.Left - 4, breedTextbox.Top - 25);

            // Update Comments Location Location
            commentsTextbox.Location = new Point((int)((inputPanel.Width / 2) - (commentsTextbox.Width / 2)), (int)(inputPanel.Height * 0.67));
            commentsLabel.Location = new Point(commentsTextbox.Left - 4, commentsTextbox.Top - 25);

            this.Refresh();
            inputPanel.Visible = true;
            fileuploadPictureBox.Visible = true;
            this.ResumeLayout();
        }

        private Image CreateThumbnail(string imagePath, int thumbWidth, int thumbHeight)
        {
            // Load the original image
            using (Image originalImage = Image.FromFile(imagePath))
            {
                // Calculate aspect ratio
                float aspectRatio = (float)originalImage.Width / originalImage.Height;
                if (thumbWidth / (float)thumbHeight > aspectRatio)
                {
                    thumbWidth = (int)(thumbHeight * aspectRatio);
                }
                else
                {
                    thumbHeight = (int)(thumbWidth / aspectRatio);
                }

                // Create the thumbnail
                Bitmap thumbnail = new Bitmap(thumbWidth, thumbHeight);
                using (Graphics graphics = Graphics.FromImage(thumbnail))
                {
                    graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                    // Draw the resized image onto the thumbnail
                    graphics.DrawImage(originalImage, 0, 0, thumbWidth, thumbHeight);
                }

                return thumbnail;
            }
        }
    }
}