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
        private PictureBox resultBox;

        // Individual labels and text boxes for each result
        private Label uploadNameLabel;
        private TextBox uploadNameTextBox;

        private Label dateUploadedLabel;
        private TextBox dateUploadedTextBox;

        private Label dateOfSampleLabel;
        private TextBox dateOfSampleTextBox;

        private Label sampleLocationLabel;
        private TextBox sampleLocationTextBox;

        private Label sheepBreedLabel;
        private TextBox sheepBreedTextBox;

        public ResultPage(Image resultImage)  // Build page with resulting image from ML and previous page's size/location
        {
            InitializeComponent();

            // Initialize Form Properties
            this.Text = "ResultsPage";

            // Initialize Button to return to MainPage  
            returnButton = new Button();
            returnButton.Text = "Exit";                                             // Text on button to say "Exit"
            returnButton.Font = new Font("Times New Roman", 12, FontStyle.Italic);  // Font size/style
            returnButton.AutoSize = true;                                           // Autosize button
            returnButton.Click += returnButton_Click;                               // Add event of click
            this.Controls.Add(returnButton);                                        // create new control returnButton

            // Initialize Results Image
            resultBox = new PictureBox();                                           // Initialize new pictureBox to hold results
            resultBox.Image = resultImage;                                          // Insert image into pictureBox
            resultBox.SizeMode = PictureBoxSizeMode.Zoom;                           // Zoom image to fit size
            resultBox.Size = new Size(300, 200);                                    // Size image to 300 x 200
            resultBox.Location = new Point(                                         // Position Image to center top of the page
                (this.ClientSize.Width / 2) - (resultBox.Width / 2),
                20);
            this.Controls.Add(resultBox);                                           // Create new picturebox on page

            // Create and initialize labels and text boxes
            CreateResultControls();

            // Populate text boxes with data from the last upload
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
                MessageBox.Show("No data available for the results.");
            }

            // Call CenterControls for page resize Events
            CenterControls();
            this.Resize += ResultsPage_Resize;
        }

        private void CreateResultControls()
        {
            // Initialize and configure labels and text boxes for each result
            Font commonFont = new Font("Arial", 10, FontStyle.Regular); // Set a common font for all controls

            uploadNameLabel = CreateLabel("Upload Name:", commonFont);
            uploadNameTextBox = CreateSeamlessTextBox(commonFont);

            dateUploadedLabel = CreateLabel("Date Uploaded:", commonFont);
            dateUploadedTextBox = CreateSeamlessTextBox(commonFont);

            dateOfSampleLabel = CreateLabel("Date of Sample:", commonFont);
            dateOfSampleTextBox = CreateSeamlessTextBox(commonFont);

            sampleLocationLabel = CreateLabel("Location of Sample:", commonFont);
            sampleLocationTextBox = CreateSeamlessTextBox(commonFont);

            sheepBreedLabel = CreateLabel("Sheep Breed:", commonFont);
            sheepBreedTextBox = CreateSeamlessTextBox(commonFont);

            // Add controls to the form
            this.Controls.Add(uploadNameLabel);
            this.Controls.Add(uploadNameTextBox);

            this.Controls.Add(dateUploadedLabel);
            this.Controls.Add(dateUploadedTextBox);

            this.Controls.Add(dateOfSampleLabel);
            this.Controls.Add(dateOfSampleTextBox);

            this.Controls.Add(sampleLocationLabel);
            this.Controls.Add(sampleLocationTextBox);

            this.Controls.Add(sheepBreedLabel);
            this.Controls.Add(sheepBreedTextBox);
        }

        private Label CreateLabel(string labelText, Font font)
        {
            Label label = new Label();
            label.Text = labelText;
            label.Font = font;
            label.AutoSize = true;
            return label;
        }

        private TextBox CreateSeamlessTextBox(Font font)
        {
            TextBox textBox = new TextBox();
            textBox.Size = new Size(200, 20);  // Slightly smaller size for text boxes
            textBox.ReadOnly = true;  // Make the text boxes read-only
            textBox.BorderStyle = BorderStyle.None;  // Remove the border for a seamless look
            textBox.BackColor = this.BackColor;  // Match the background color for a seamless effect
            textBox.Font = font;  // Set the common font
            return textBox;
        }

        private void returnButton_Click(object? sender, EventArgs e)  // Method for returning to mainPage
        {
            MainPage mainPage = new MainPage();  // Initialize new main page
            mainPage.Show();    // Show new main page
            this.Hide();        // Hide current page
        }

        private void CenterControls()  // Method for centering elements of the form
        {
            // Center returnButton to bottom-center of the page
            returnButton.Location = new Point(
                (this.ClientSize.Width / 2) - (returnButton.Width / 2),
                this.ClientSize.Height - 40);

            // Center resultBox to top-center of the page
            resultBox.Location = new Point(
                (this.ClientSize.Width / 2) - (resultBox.Width / 2),
                20);  // Set top margin to 20px

            // Position labels and text boxes below the image
            int topMargin = resultBox.Bottom + 30;  // Start positioning below the image
            int labelWidth = 150;  // Fixed width for labels
            int textBoxX = (this.ClientSize.Width / 2) - (uploadNameTextBox.Width / 2);  // Center text boxes
            int verticalSpacing = 10;  // Reduce vertical spacing

            // Position upload name label and text box
            uploadNameLabel.Location = new Point(textBoxX - labelWidth - 10, topMargin);
            uploadNameTextBox.Location = new Point(textBoxX, topMargin);

            // Position date uploaded label and text box
            topMargin += uploadNameTextBox.Height + verticalSpacing;
            dateUploadedLabel.Location = new Point(textBoxX - labelWidth - 10, topMargin);
            dateUploadedTextBox.Location = new Point(textBoxX, topMargin);

            // Position date of sample label and text box
            topMargin += dateUploadedTextBox.Height + verticalSpacing;
            dateOfSampleLabel.Location = new Point(textBoxX - labelWidth - 10, topMargin);
            dateOfSampleTextBox.Location = new Point(textBoxX, topMargin);

            // Position sample location label and text box
            topMargin += dateOfSampleTextBox.Height + verticalSpacing;
            sampleLocationLabel.Location = new Point(textBoxX - labelWidth - 10, topMargin);
            sampleLocationTextBox.Location = new Point(textBoxX, topMargin);

            // Position sheep breed label and text box
            topMargin += sampleLocationTextBox.Height + verticalSpacing;
            sheepBreedLabel.Location = new Point(textBoxX - labelWidth - 10, topMargin);
            sheepBreedTextBox.Location = new Point(textBoxX, topMargin);
        }

        private void ResultsPage_Resize(object? sender, EventArgs e)  // Event Handler when the Form is resized by user
        {
            CenterControls();  // Call method for adjusting controls
        }
    }
}