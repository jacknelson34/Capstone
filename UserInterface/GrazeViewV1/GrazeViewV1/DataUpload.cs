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
    public partial class DataUpload : Form
    {
        private Image uploadedImage;
        private Button uploadButton;
        private TextBox uploadName;
        private Label uploadNameLabel;
        private TextBox sampleLocation;
        private Label sampleLocationLabel;
        private TextBox sampleTime;
        private Label sampleTimeLabel;
        private TextBox sheepBreed;
        private Label sheepBreedLabel;

        public DataUpload()
        {
            // Form Properties
            InitializeComponent();
            this.Text = "Data Upload";

            // Upload Button Properties
            uploadButton = new Button();
            uploadButton.Text = "Upload Image";
            uploadButton.Size = new Size(200, 75);
            uploadButton.Click += uploadButton_Click;
            this.Controls.Add(uploadButton);

            // Upload Name - Text Box and Label Properties
            uploadNameLabel = new Label();
            uploadNameLabel.Text = "Upload Name:";
            uploadNameLabel.AutoSize = true;
            this.Controls.Add(uploadNameLabel);

            uploadName = new TextBox();
            uploadName.Size = new Size(100, 50);
            this.Controls.Add(uploadName);

            // Sample Location - Text Box and Label Properties
            sampleLocationLabel = new Label();
            sampleLocationLabel.Text = "Sample Location (Enter as City, State) :";
            sampleLocationLabel.AutoSize = true;
            this.Controls.Add(sampleLocationLabel);

            sampleLocation = new TextBox();
            this.Controls.Add(sampleLocation);

            // Sample Time - Text Box and Label Properties
            sampleTimeLabel = new Label();
            sampleTimeLabel.Text = "Sample Time (Enter as MM/DD/YYYY hh:mm) :";
            sampleTimeLabel.AutoSize = true;
            this.Controls.Add(sampleTimeLabel);

            sampleTime = new TextBox();
            this.Controls.Add(sampleTime);

            // Sheep Breed - Text Box and Label Properties
            sheepBreedLabel = new Label();
            sheepBreedLabel.Text = "Sheep Breed:";
            sheepBreedLabel.AutoSize = true;
            this.Controls.Add(sheepBreedLabel);

            sheepBreed = new TextBox();
            this.Controls.Add(sheepBreed);

            // Add other needed information text boxes here ->

            CenterControls(); // Method for positioning all buttons/textboxes
            this.Resize += DataPageResize;
        }

        private void uploadButton_Click(object? sender, EventArgs e)
        {

            // Ensure that the sample location is not empty
            if (string.IsNullOrWhiteSpace(sampleLocation.Text))
            {
                MessageBox.Show("Sample location is required.");
                return;  // Exit if sample location is empty
            }

            // Ensure that a valid DateTime is entered in sampleTime TextBox
            bool validSampleTime = DateTime.TryParse(sampleTime.Text, out DateTime parsedSampleTime);
            if (!validSampleTime)
            {
                MessageBox.Show("Please enter a valid date/time for the sample time.");
                return;  // Exit the method if the input is invalid
            }

            // Generate a generic name if uploadName is left blank
            string finalUploadName;
            if (string.IsNullOrWhiteSpace(uploadName.Text))
            {
                // Create a name based on the current date and time
                finalUploadName = $"Upload_{DateTime.Now.ToString("yyyyMMdd_HHmm")}";
            }
            else
            {
                finalUploadName = uploadName.Text;  // Use the provided name
            }

            // Create a new instance of UploadInfo
            UploadInfo uploadInfo = new UploadInfo
            {
                UploadDataText = finalUploadName,        // Store the upload name
                SampleLocationText = sampleLocation.Text, // Store the sample location
                SampleTime = DateTime.Parse(sampleTime.Text), // Assuming sampleTime.Text is a valid date/time string
                SheepBreedText = sheepBreed.Text,         // Store the sheep breed
                UploadTime = DateTime.Now                // Store the current time of upload
            };

            // Add the new upload info to the GlobalData uploads list
            GlobalData.Uploads.Add(uploadInfo);

            // Image Upload Code here
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*jpeg;*.png;*.gif";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string ImageFilePath = openFileDialog.FileName;
                    uploadedImage = Image.FromFile(ImageFilePath);
                }
            }

            if (uploadedImage != null)
            {
                LoadingPage loadingPage = new LoadingPage(uploadedImage);
                loadingPage.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid Upload Type.");
            }
        }

        private void CenterControls()  // Method for centering all textboxes/buttons with labels on the left
        {
            // Set a fixed width for the text boxes
            int textBoxWidth = 250;    // Fixed width for all text boxes
            int labelSpacing = 20;     // Spacing between labels and text boxes
            int verticalSpacing = 30;  // Vertical spacing between rows of controls

            // Calculate the longest label width to align text boxes based on it
            int longestLabelWidth = Math.Max(uploadNameLabel.Width,
                                    Math.Max(sampleLocationLabel.Width,
                                    Math.Max(sampleTimeLabel.Width, sheepBreedLabel.Width)));

            // Calculate the total width of the label and text box combination
            int totalWidth = longestLabelWidth + labelSpacing + textBoxWidth;

            // Calculate the center X position for the label-text box combination
            int centerX = (this.ClientSize.Width / 2) - (totalWidth / 2);

            // Center the upload button
            uploadButton.Location = new Point(
                (this.ClientSize.Width / 2) - (uploadButton.Width / 2),
                (this.ClientSize.Height / 8));

            // Calculate vertical position based on form height
            int topMargin = uploadButton.Bottom + 30;

            // Set the width for all text boxes and position them with their labels

            // Position uploadName and its label
            uploadNameLabel.Location = new Point(centerX, topMargin);
            uploadName.Size = new Size(textBoxWidth, uploadName.Height);  // Fixed width
            uploadName.Location = new Point(centerX + longestLabelWidth + labelSpacing, topMargin);

            // Position sampleLocation and its label
            topMargin += uploadName.Height + verticalSpacing;
            sampleLocationLabel.Location = new Point(centerX, topMargin);
            sampleLocation.Size = new Size(textBoxWidth, sampleLocation.Height);  // Fixed width
            sampleLocation.Location = new Point(centerX + longestLabelWidth + labelSpacing, topMargin);

            // Position sampleTime and its label
            topMargin += sampleLocation.Height + verticalSpacing;
            sampleTimeLabel.Location = new Point(centerX, topMargin);
            sampleTime.Size = new Size(textBoxWidth, sampleTime.Height);  // Fixed width
            sampleTime.Location = new Point(centerX + longestLabelWidth + labelSpacing, topMargin);

            // Position sheepBreed and its label
            topMargin += sampleTime.Height + verticalSpacing;
            sheepBreedLabel.Location = new Point(centerX, topMargin);
            sheepBreed.Size = new Size(textBoxWidth, sheepBreed.Height);  // Fixed width
            sheepBreed.Location = new Point(centerX + longestLabelWidth + labelSpacing, topMargin);
        }

        private void ResizeControls()
        {
            int controlWidth = this.ClientSize.Width / 3;
            int controlHeight = this.ClientSize.Height / 15;

            // Resize all textboxes
            uploadName.Size = new Size(controlWidth, controlHeight);
            sampleLocation.Size = new Size(controlWidth, controlHeight);
            sampleTime.Size = new Size(controlWidth, controlHeight);
            sheepBreed.Size = new Size(controlWidth, controlHeight);

            // Resize upload button
            uploadButton.Size = new Size(controlWidth, controlHeight);
        }

        private void DataPageResize(object? sender, EventArgs e)
        {
            ResizeControls(); // Adjust control sizes
            CenterControls(); // Adjust control positions
        }
    }
}