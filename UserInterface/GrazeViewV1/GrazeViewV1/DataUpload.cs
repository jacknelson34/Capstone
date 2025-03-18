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
using System.Data.SqlClient;

namespace GrazeViewV1
{
    public partial class DataUpload : Form
    {
        private MainPage _mainPage;  // hold reference to Main Page form
        Image uploadImage;           // variable to hold user-uploaded image
        private bool IsNavigating;   // boolean variable that checks if the user is still using the app
        public string imageFilePath;    // String to pass image file path to MLWork
        private string promptText;
        private bool imageLoading = false;


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

            // Event Handler for form close
            this.FormClosing += DataUpload_XOut;
            this.Load += DataUpload_Resize;
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
                this.Invalidate();

                //MessageBox.Show("Main Page Size 5 : " + _mainPage.ClientSize.ToString());
                _mainPage.Visible = true;                 // open main page
                _mainPage.WindowState = this.WindowState; // Force this window state onto main page
                _mainPage.ExternalResize(this.Size, this.Location);

            }
                
            //MessageBox.Show("Main Page State Final: " + _mainPage.WindowState.ToString() + "\nMain Page Size : " + _mainPage.ClientSize.ToString());
            this.Close();// Close Data Upload Page
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
            if (!imageUploaded && !pictureBoxLoader.Visible)
            {
                promptText = "Click or Drop Your Files Here";
                Font font = new Font("Times New Roman", 14, FontStyle.Regular);
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


                pictureBoxLoader.SendToBack();

                this.Update();
            }
        }

        // when the file upload picture box is clicked on
        private async void fileuploadPictureBox_Click(object sender, EventArgs e)
        {

            // Open a file dialog to allow the user to select an image file
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // Ensure loader appears on top
                pictureBoxLoader.BringToFront();
                pictureBoxLoader.Visible = true;

                // Center the loader inside fileuploadPictureBox
                pictureBoxLoader.Location = new Point(
                    fileuploadPictureBox.Left + (fileuploadPictureBox.Width - pictureBoxLoader.Width) / 2,
                    fileuploadPictureBox.Top + (fileuploadPictureBox.Height - pictureBoxLoader.Height) / 2
                );

                // Remove any existing text from fileuploadPictureBox
                fileuploadPictureBox.Invalidate();

                // Force UI refresh to show the loading icon immediately
                fileuploadPictureBox.Update();
                pictureBoxLoader.Update();

                // Set the filter to allow only image files
                openFileDialog.Filter = "PNG Files|*.png;";
                openFileDialog.Title = "Select a PNG Image";

                // If the user selects a file and clicks OK
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {

                        // Generate Thumbnail for display
                        imageLoading = true;
                        Image thumbnail = CreateThumbnail(openFileDialog.FileName, fileuploadPictureBox.Width, fileuploadPictureBox.Height);

                        // Delete Text in the Picture Box
                        imageUploaded = true;
                        imageFilePath = openFileDialog.FileName;

                        // Check if image is a duplicate in the background
                        DBQueries dbQueries = new DBQueries("Driver={ODBC Driver 18 for SQL Server};Server=sqldatabase404.database.windows.net;Database=404ImageDBsql;Uid=sql404admin;Pwd=sheepstool404();TrustServerCertificate=no;MultipleActiveResultSets=True;");
                        int dupCheck = await Task.Run(() => dbQueries.DuplicateImageCheck(imageFilePath));

                        if(dupCheck != 2)
                        {
                            ClearImage();
                            dbQueries.Dispose();
                            IsNavigating = false;

                            uploadButton.Text = "Upload";
                            uploadLoader.Visible = false;
                            return;
                        }

                        await LoadImageAsync(imageFilePath); // Load image asynchronously
                        uploadImage = Image.FromFile(openFileDialog.FileName);


                        // If the textbox is empty, populate it with the upload time/date
                        if (string.IsNullOrWhiteSpace(filenameTextbox.Text))
                        {
                            // filenameTextbox.Text = Path.GetFileName(openFileDialog.FileName); // Extracts just the file name
                            // Access current time and date
                            DateTime currentTime = DateTime.Now;

                            // Access upload #
                            int uploadNum = GetUploadCountFromDB() + 1;
                            filenameTextbox.Text = "Upload-" + uploadNum + ".png";

                        }
                        dbQueries.Dispose();
                    }
                    catch (Exception ex)
                    {
                        // Handle any errors that occur during image loading
                        MessageBox.Show("An error occurred while trying to load the image: " + ex.Message);
                        imageLoading = false;
                    }
                }
                else
                {
                    pictureBoxLoader.Visible = false;
                    pictureBoxLoader.SendToBack();
                    fileuploadPictureBox.Update();
                    imageLoading = false;
                    return;
                }
                imageLoading = false;
            }
        }

        // Helper method to count uploads
        private int GetUploadCountFromDB()
        {
            int count = 0;

            string _connectionString = "Server=sqldatabase404.database.windows.net;Database=404ImageDBsql;Uid=sql404admin;Pwd=sheepstool404();TrustServerCertificate=no;MultipleActiveResultSets=True;";

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM CSVDB"; // Use actual table name

                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        count = (int)command.ExecuteScalar(); // Get row count
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return count; // Return total uploads
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
        private async void fileuploadPictureBox_DragDrop(object sender, DragEventArgs e)
        {
            // Get the file(s) that are dropped
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Ensure loader appears on top
                pictureBoxLoader.BringToFront();
                pictureBoxLoader.Visible = true;

                // Center the loader inside fileuploadPictureBox
                pictureBoxLoader.Location = new Point(
                    fileuploadPictureBox.Left + (fileuploadPictureBox.Width - pictureBoxLoader.Width) / 2,
                    fileuploadPictureBox.Top + (fileuploadPictureBox.Height - pictureBoxLoader.Height) / 2
                );

                // Remove any existing text from fileuploadPictureBox
                fileuploadPictureBox.Invalidate();

                // Force UI refresh to show the loading icon immediately
                fileuploadPictureBox.Update();
                pictureBoxLoader.Update();

                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                // Load the first .png image file into the PictureBox
                if (files.Length > 0 && IsPngFile(files[0]))
                {
                    try
                    {

                        // Delete Text in the Picture Box
                        imageUploaded = true;
                        imageFilePath = files[0];
                        imageLoading= true;

                        // Generate a thumbnail like Click Upload
                        Image thumbnail = CreateThumbnail(imageFilePath, fileuploadPictureBox.Width, fileuploadPictureBox.Height);

                        // Check if image is a duplicate in the background
                        DBQueries dbQueries = new DBQueries("Driver={ODBC Driver 18 for SQL Server};Server=sqldatabase404.database.windows.net;Database=404ImageDBsql;Uid=sql404admin;Pwd=sheepstool404();TrustServerCertificate=no;MultipleActiveResultSets=True;");
                        int dupCheck = await Task.Run(() => dbQueries.DuplicateImageCheck(imageFilePath));

                        if (dupCheck != 2)
                        {
                            ClearImage();
                            dbQueries.Dispose();
                            IsNavigating = false;

                            uploadButton.Text = "Upload";
                            uploadLoader.Visible = false;
                            imageLoading = false;
                            return;
                        }

                        await LoadImageAsync(imageFilePath); // Load image asynchronously

                        uploadImage = fileuploadPictureBox.Image;

                        // If the textbox is empty, populate it with the file name
                        if (string.IsNullOrWhiteSpace(filenameTextbox.Text))
                        {
                            filenameTextbox.Text = Path.GetFileName(files[0]); // Extracts just the file name
                        }
                        dbQueries.Dispose();
                    }
                    catch (Exception ex)
                    {
                        // Handle any errors that occur during image loading
                        MessageBox.Show("An error occurred while trying to load the image: " + ex.Message);
                        imageLoading = false;
                    }
                    imageLoading = false;
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
        private async void uploadButton_Click(object? sender, EventArgs e)
        {
            IsNavigating = true;  // User is still using the app

            try
            {

                string selectedDate = DateTime.Parse(datePicker.Text).ToString();    // Variables to check valid dates/times
                string selectedTime = DateTime.Parse(timePicker.Text).ToString();

            }
            catch(FormatException)
            {
                MessageBox.Show($"Invalid Time Entered: {timePicker.Text}.  Please provide a valid time.",
                "Invalid Time",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return;
            }

            // Maintain consistent form sizing
            if (this.WindowState == FormWindowState.Maximized)
            {
                ConsistentForm.IsFullScreen = true;
            }
            else
            {
                ConsistentForm.IsFullScreen = false;
            }

            // Center the loader inside fileuploadPictureBox
            uploadLoader.Location = new Point(
                uploadButton.Left + (uploadButton.Width - uploadLoader.Width) / 2,
                uploadButton.Top + (uploadButton.Height - uploadLoader.Height) / 2
            );

            // Ensure loader appears on top
            uploadLoader.BringToFront();
            uploadLoader.Visible = true;
            uploadButton.Text = "";

            // Date/Time Input Validation
            // Make sure date is after Jan 1st, 2000 and before current date
            if (DateTime.Parse(datePicker.Text) < new DateTime(2000, 1, 1) || DateTime.Parse(datePicker.Text) > DateTime.Today)
            {
                MessageBox.Show("Please Enter a Date between January 1st, 2000 and today.",
                    "Invalid Date Entered");

                uploadButton.Text = "Upload";
                uploadLoader.Visible = false;
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

                    uploadButton.Text = "Upload";
                    uploadLoader.Visible = false;
                    return;
                }
            }
            catch (FormatException)
            {
                MessageBox.Show($"Invalid Time Entered: {timePicker.Text}.  Please provide a valid time.",
                                "Invalid Time",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                uploadButton.Text = "Upload";
                uploadLoader.Visible = false;
                return;
            }

            // Check if date was entered by user

            // Get selected date and time from controls
            DateTime selectedDate2 = datePicker.Value;
            DateTime selectedTime2 = DateTime.Parse(timePicker.Text);

            // Check if user changed the values
            bool isDateDefault = (selectedDate2.Date == DateTime.Today); // Assuming default is today
            bool isTimeDefault = (selectedTime2.TimeOfDay == new TimeSpan(8, 0, 0)); // Assuming default is 08:00 AM

            // Only set to "N/A" if the user did not change the values
            string finalDate = isDateDefault ? "N/A" : selectedDate2.ToString("MM/dd/yyyy");
            string finalTime = isTimeDefault ? "N/A" : selectedTime2.ToString("HH:mm:ss");

            // Check if the Sample Location, Sheep Breed, or Comments are empty, and set them to "N/A" if they are
            string sampleLocation = string.IsNullOrWhiteSpace(locationTextbox.Text) ? "N/A" : locationTextbox.Text;
            string sheepBreed = string.IsNullOrWhiteSpace(breedTextbox.Text) ? "N/A" : breedTextbox.Text;
            string comments = string.IsNullOrWhiteSpace(commentsTextbox.Text) ? "N/A" : commentsTextbox.Text;

            /// ---------------------------------------------- INTEGRATION POINT --------------------------------------------------///

            // Debugging
            //MessageBox.Show("Sample Date: " + finalDate +"\nSample Time: " + finalTime);
            

            // Create a new instance of UploadInfo
            UploadInfo uploadInfo = new UploadInfo
            {
                UploadName = filenameTextbox.Text,             // Store the upload name
                SampleLocation = sampleLocation,               // Store the sample location (or N/A)
                SampleDate = finalDate,  // Store the sample date
                SampleTime = finalTime,  // Assuming sampleTime.Text is a valid date/time string
                UploadTime = DateTime.Now,                     // Store the current time of upload
                SheepBreed = sheepBreed,                       // Store the sheep breed (or N/A)
                Comments = comments,                           // Store user comments (or N/A)
                ImageFile = uploadImage                        // Store image
            };

            // Add the new upload info to the GlobalData uploads list
            GlobalData.Uploads.Add(uploadInfo);


            // Upload Imagefile to DB
            string imagePath = imageFilePath;
            DBQueries dbQueries = new DBQueries("Driver={ODBC Driver 18 for SQL Server};Server=sqldatabase404.database.windows.net;Database=404ImageDBsql;Uid=sql404admin;Pwd=sheepstool404();TrustServerCertificate=no;MultipleActiveResultSets=True;");
            Task uploadTask = Task.Run(()=> dbQueries.UploadImageToDB(imageFilePath));


            // checks to see if a file was uploaded to the picturebox
            if (fileuploadPictureBox.Image != null)
            {
                // Test calling MLWork
                try
                {

                    ConsistentForm.FormLocation = this.Location;
                    ConsistentForm.FormSize = this.Size;
                    LoadingPage loadingPage = new LoadingPage(fileuploadPictureBox.Image, _mainPage);
                    loadingPage.Show();

                    // Run ML on a new task to keep ML responsive
                    Task.Run (() => MLWork.MLMain(imagePath, loadingPage));

                    this.Hide();

                    // Wait on upload to DB to close page and end task
                    await Task.WhenAll(uploadTask);

                    //MessageBox.Show("Image sent to MLWork for processing.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    uploadButton.Visible = true;
                    uploadLoader.Visible = false;

                    MessageBox.Show($"Error running ML model: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                
            }
            else
            {
                if (imageLoading)
                {
                    // Show error message if image is still loading
                    MessageBox.Show("Please wait for image to load before uploading.", "Image Processing...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    uploadButton.Text = "Upload";
                    uploadLoader.Visible = false;
                    return;
                }

                // Show error message if no valid image was uploaded
                MessageBox.Show("Please upload a valid image file.", "Invalid Upload", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                uploadButton.Text = "Upload";
                uploadLoader.Visible = false;
            }
            dbQueries.Dispose();

        }

        // Method to clear picturebox
        private void ClearImage()
        {
            fileuploadPictureBox.Image = null;
            imageUploaded = false;
        }

        private async void DataUpload_Resize(object sender, EventArgs e)
        {
            this.SuspendLayout();

            // Update Filename Textbox size
            Size textboxSize = new Size((int)(this.ClientSize.Width * 0.4), 39);
            filenameTextbox.Size = textboxSize;
            locationTextbox.Size = textboxSize;
            breedTextbox.Size = textboxSize;
            commentsTextbox.Size = new Size((int)(this.ClientSize.Width * 0.4), (int)(this.ClientSize.Height * 0.2));

            // Update label font sizes
            int fontSize = Math.Max(12, (int)(this.ClientSize.Height * 0.014));
            Font updatedFont = new Font("Times New Roman", fontSize, FontStyle.Regular, GraphicsUnit.Point, 0);
            filenameLabel.Font = updatedFont;
            locationLabel.Font = updatedFont;
            datetimeLabel.Font = updatedFont;
            breedLabel.Font = updatedFont;
            commentsLabel.Font = updatedFont;
            uploadButton.Font = updatedFont;
            backButton.Font = updatedFont;
            fileuploadPictureBox.Font = updatedFont;


            // Update File Name Label/TextBox location
            filenameTextbox.Location = new Point((int)((this.ClientSize.Width / 4) - (filenameTextbox.Width / 2) + 25), (int)(this.ClientSize.Height * 0.16));
            filenameLabel.Location = new Point(filenameTextbox.Left - 4, filenameTextbox.Top - 25);

            // Update Location Textbox/Label Location
            locationTextbox.Location = new Point((int)((this.ClientSize.Width / 4) - (locationTextbox.Width / 2) + 25), (int)(this.ClientSize.Height * 0.28));
            locationLabel.Location = new Point(locationTextbox.Left - 4, locationTextbox.Top - 25);

            // Update Date Time Label/Boxes Location
            datetimeLabel.Location = new Point((int)((this.ClientSize.Width / 4) - (datetimeLabel.Width / 2) + 25), (int)(this.ClientSize.Height * 0.36));
            datePicker.Location = new Point((int)((this.ClientSize.Width / 4) - (datePicker.Width / 2) - 50), (int)(this.ClientSize.Height * 0.41));
            timePicker.Location = new Point((int)((this.ClientSize.Width / 4) - (timePicker.Width / 2) + 100), (int)(this.ClientSize.Height * 0.41));

            // Update Breed Label/Textbox Locations
            breedTextbox.Location = new Point((int)((this.ClientSize.Width / 4) - (breedTextbox.Width / 2) + 25), (int)(this.ClientSize.Height * 0.54));
            breedLabel.Location = new Point(breedTextbox.Left - 4, breedTextbox.Top - 25);

            // Update Comments Location Location
            commentsTextbox.Location = new Point((int)((this.ClientSize.Width / 4) - (commentsTextbox.Width / 2) + 25), (int)(this.ClientSize.Height * 0.67));
            commentsLabel.Location = new Point(commentsTextbox.Left - 4, commentsTextbox.Top - 25);

            // Picturebox location update
            fileuploadPictureBox.Location = new Point((int)((this.ClientSize.Width / 2) + 30), (int)(this.ClientSize.Height * 0.12));
            fileuploadPictureBox.Size = new Size((int)(this.ClientSize.Width * 0.4), (int)(this.ClientSize.Height * 0.75));

            // Buttons Resize
            int maxButtonWidth = Math.Min(200, (int)(this.ClientSize.Width * 0.164));
            int maxButtonWidth2 = Math.Min(200, (int)(this.ClientSize.Width * 0.2));
            int maxButtonHeight = Math.Min(75, (int)(this.ClientSize.Height * 0.062));

            uploadButton.Size = new Size(maxButtonWidth, maxButtonHeight);
            backButton.Size = new Size(maxButtonWidth2, maxButtonHeight);

            // Upload Button Location Update
            uploadButton.Location = new Point((int)(this.ClientSize.Width - uploadButton.Width - 20), (int)(this.ClientSize.Height - uploadButton.Height - 20));

            //MessageBox.Show("Page size : " + this.ClientSize.ToString() + "\nUpload Button Size: " + uploadButton.Size.ToString());

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

        // Method to shown loader while image is pulled
        private async Task LoadImageAsync(string imagePath)
        {
            try
            {

                // Load image asynchronously
                Image loadedImage = await Task.Run(() =>
                {
                    return CreateThumbnail(imagePath, fileuploadPictureBox.Width, fileuploadPictureBox.Height);
                });

                // Assign the image to the PictureBox on the UI thread
                fileuploadPictureBox.Invoke((Action)(() =>
                {
                    fileuploadPictureBox.Image = loadedImage;
                    fileuploadPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBoxLoader.Visible = false; // Hide the loader
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}", "Image Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                pictureBoxLoader.Visible = false; // Hide loader on error
            }
        }

    }
}