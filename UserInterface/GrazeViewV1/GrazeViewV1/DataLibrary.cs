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
    public partial class DataLibrary : Form
    {
        // hold reference to Main Page form
        private MainPage _mainPage;

        public DataLibrary(MainPage mainpage)
        {
            // Form Properties
            InitializeComponent();
            _mainPage = mainpage;
            this.Text = "Data Viewer";

            // Help Button Functionality
            helpButton.Click += helpButton_Click; // handles click event
            this.Controls.Add(helpButton);

            // Handle data errors
            dataGridView1.DataError += dataGridView1_DataError;

            // Adjust positions when the form is fully shown
            this.Shown += (sender, e) => {
                AdjustButtonLayout(buttonPanel, sortByBox, exportButton, backButton);
            };

            // Adjust buttonPanel when form is resized
            this.Resize += (sender, e) =>
            {
                AdjustButtonLayout(buttonPanel, sortByBox, exportButton, backButton);
            };

        }

        // Method to adjust buttonPanel components' positioning
        private void AdjustButtonLayout(Panel buttonPanel, ComboBox sortByBox, Button exportButton, Button backButton)
        {
            int panelWidth = buttonPanel.ClientSize.Width;
            // Center ComboBox in the panel, below the SortBy Label
            sortByBox.Left = (panelWidth - sortByBox.Width) / 2;
            sortByBox.Top = ((buttonPanel.Height - sortByBox.Height) / 2) + 10;  // 20px below center of buttonPanel

            // Position the SortBy Label
            sortByLabel.Left = sortByBox.Left - 6;
            sortByLabel.Top = sortByBox.Top - 20;  // 10px padding from the top of the panel

            // Position Export button to the left of ComboBox with spacing
            exportButton.Left = sortByBox.Left - exportButton.Width - 20;  // 20px spacing from ComboBox
            exportButton.Top = sortByBox.Top;

            // Position Return button to the right of ComboBox with spacing
            backButton.Left = (panelWidth - backButton.Width) - 10;  // 20px spacing from ComboBox
            backButton.Top = sortByBox.Top;
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

        // Method for when export button is click --- TODO ---
        private void exportButton_Click(object sender, EventArgs e) 
        {
    
        }

        // Method to add data from DataUpload to the Library
        public void LoadUploadsFromGlobalData()
        {
            // Clear the grid to avoid duplicating rows
            dataGridView1.Rows.Clear();

            // Add all uploads from GlobalData to the DataGridView
            foreach (var upload in GlobalData.Uploads)
            {
                // Check if there's an image to display, otherwise pass null
                Image imageToDisplay = null;
                if (upload.ImageFile != null)  // Assuming UploadInfo has an ImageFile property (change as per your structure)
                {
                    imageToDisplay = upload.ImageFile;  // This should be an Image object
                }

                dataGridView1.Rows.Add(
                    false,                                        // Checkbox column
                    upload.UploadName,                            // Upload Name
                    upload.SampleDate.ToString("MM/dd/yyyy"),     // Date Sample Taken
                    upload.SampleTime.ToString("hh:mm tt"),       // Time Sample Taken
                    upload.UploadTime.ToString("MM/dd/yyyy"),     // Upload Date
                    upload.UploadTime.ToString("hh:mm tt"),       // Upload Time
                    upload.SampleLocation,                        // Sample Location
                    upload.SheepBreed,                            // Sheep Breed
                    upload.Comments,                              // Comments
                    imageToDisplay                               // ImageColumn (pass Image or null)
                );
            }

            // Refresh the grid to ensure the new data is visible
            dataGridView1.Refresh();
        }

        // Test
        private void LoadUploads()
        {
            foreach(var upload in GlobalData.Uploads)
            {
                LoadUploadsFromGlobalData();
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            LoadUploadsFromGlobalData();  // Reload the uploads into the DataGridView each time it's shown
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show($"Error in DataGridView: {e.Exception.Message}");
            e.ThrowException = false;  // Prevent the exception from crashing the application
        }

    }
}
