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
            this.Location = ConsistentForm.FormLocation;
            this.Size = ConsistentForm.FormSize;
            if (ConsistentForm.IsFullScreen)
            {
                SetFullScreen();
            }

            // Help Button Functionality
            helpButton.Click += helpButton_Click; // handles click event
            this.Controls.Add(helpButton);

            // Handle data errors
            dataGridView1.DataError += dataGridView1_DataError;
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;

        }

        private void SetFullScreen()     // Class to handle screen maximization
        {
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Bounds = Screen.PrimaryScreen.Bounds;
        }

        // Method for previewing an uploaded image
        private void previewButton_Click(object sender, EventArgs e)
        {
            // Determine which rows are selected
            var selectedRows = dataGridView1.Rows.Cast<DataGridViewRow>()
                                                 .Where(row => Convert.ToBoolean(row.Cells[0].Value))  // Assuming checkbox is at index 0
                                                 .ToList();

            // Only allow one upload at a time for the preview button
            if (selectedRows.Count != 1)
            {
                // Output message
                MessageBox.Show("Only select 1 upload to preview.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                // Get the selected row
                var selectedRow = selectedRows.First();
                int rowIndex = selectedRow.Index;

                // Ensure the selected index is valid
                if (rowIndex < GlobalData.Uploads.Count)
                {
                    var uploadInfo = GlobalData.Uploads[rowIndex];

                    // Check if image exists in uploadInfo
                    if (uploadInfo.ImageFile == null)
                    {
                        MessageBox.Show("No image available for the selected upload.", "Image Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Display the image in a new form
                    Form imagePreviewForm = new Form
                    {
                        Text = "Image Preview",
                        Size = new Size(800, 600) // Adjust size as needed
                    };

                    PictureBox pictureBox = new PictureBox
                    {
                        Image = uploadInfo.ImageFile,
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Dock = DockStyle.Fill
                    };

                    imagePreviewForm.Controls.Add(pictureBox);
                    imagePreviewForm.ShowDialog(); // Show as a dialog to keep the context
                }
                else
                {
                    MessageBox.Show("Invalid selection. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // when the back button is clicked on
        private void backButton_Click(object? sender, EventArgs e)
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

            _mainPage.Show();                                       // Open Main Page
            this.Hide();                                            // Close Data Upload Page
        }

        // when the help icon is clicked on
        private void helpButton_Click(object sender, EventArgs e)
        {
            UserGuide.ShowHelpGuide();  // Call Method to only allow one instance open at a time
        }

        // Method for when export button is click 
        private void exportButton_Click(object sender, EventArgs e)
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

            // List to hold each user selected upload
            // Get selected rows
            var selectedRows = dataGridView1.Rows.Cast<DataGridViewRow>()
                                                 .Where(row => Convert.ToBoolean(row.Cells[0].Value))  // Assuming checkbox is at index 0
                                                 .ToList();

            // Check to make sure at least one upload was selected
            // Output message if 0
            if (selectedRows.Count == 0)
            {
                MessageBox.Show("Must select at least one upload", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Open DataLibraryExpandedView
            DataLibraryExpandedView expandedView = new DataLibraryExpandedView(_mainPage);

            // Loop through each row and add selected rows to a list
            foreach (var row in selectedRows)
            {
                int rowIndex = row.Index;
                if (rowIndex < GlobalData.Uploads.Count && rowIndex < GlobalData.machineLearningData.Count)
                {
                    var uploadInfo = GlobalData.Uploads[rowIndex];
                    var mlData = GlobalData.machineLearningData[rowIndex];
                    expandedView.AddUploadPanel(uploadInfo, mlData);
                }
            }

            expandedView.Show();
            this.Close();

        }

        // Method to add data from DataUpload to the Library
        public void LoadUploadsFromGlobalData()
        {
            // Clear the grid to avoid duplicating rows
            dataGridView1.Rows.Clear();

            int uploadCount = GlobalData.Uploads.Count;
            int mlDataCount = GlobalData.machineLearningData.Count;

            // Add all uploads from GlobalData to the DataGridView
            for (int i = 0; i < GlobalData.Uploads.Count; i++)
            {
                var userUploads = GlobalData.Uploads[i];

                // Check if there is an image to display, if not pass null
                // Image imageToDisplay = userUploads.ThumbNail ?? null;

                // Check if there is a corresponding MLData entry (Prevents error from first upload)
                var mlData = (i < mlDataCount) ? GlobalData.machineLearningData[i] : null;

                // Add information from both UploadInfo and MLData
                dataGridView1.Rows.Add(
                    false,                                             // Checkbox column
                    userUploads.UploadName,                            // Name of upload
                                                                       // imageToDisplay,                                    // Image uploaded
                    mlData?.qufuPercentage,                            // Qufu percentage
                    mlData?.qufustemPercentage,                        // Qufu stem percentage
                    mlData?.nalePercentage,                            // Nale percentage
                    mlData?.erciPercentage,                            // Erci Percentage
                    mlData?.bubblePercentage,                          // Air bubble percentage
                    userUploads.SampleDate.ToString("MM/dd/yyyy"),     // Date Sample Taken
                    userUploads.SampleTime.ToString("hh:mm tt"),       // Time Sample Taken
                    userUploads.UploadTime.ToString("MM/dd/yyyy"),     // Upload Date
                    userUploads.UploadTime.ToString("hh:mm tt"),       // Upload Time
                    userUploads.SampleLocation,                        // Sample Location
                    userUploads.SheepBreed,                            // Sheep Breed
                    userUploads.Comments                               // Comments
                );

            }

            // Adjust comments column
            AdjustCommentsColumnHeight();

            // Refresh the grid to ensure the new data is visible
            dataGridView1.Refresh();
        }

        // Test
        private void LoadUploads()
        {
            foreach (var upload in GlobalData.Uploads)
            {
                LoadUploadsFromGlobalData();
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            LoadUploadsFromGlobalData();  // Reload the uploads into the DataGridView each time it's shown
        }

        // Method to handle data errors in the library
        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show($"Error in DataGridView: {e.Exception.Message}");
            e.ThrowException = false;  // Prevent the exception from crashing the application
        }

        // Method for adjust comments column height based on length
        private void AdjustCommentsColumnHeight()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["CommentsCol"].Value != null)
                {
                    // string to hold the comment in each row
                    string comment = row.Cells["CommentsCol"].Value.ToString();
                    using (Graphics g = dataGridView1.CreateGraphics())
                    {
                        SizeF size = g.MeasureString(comment, dataGridView1.DefaultCellStyle.Font, 500); // Measure with max width
                        int requiredHeight = (int)Math.Ceiling(size.Height) + 10; // Add padding
                        row.Height = Math.Max(requiredHeight, dataGridView1.RowTemplate.Height); // Ensure height is not less than default
                    }
                }
            }

            

        }

        // Method for adjusting row height if comments exceed 500 pixels
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["CommentsCol"].Index)
            {
                // Adjust the height of the row for the changed cell
                dataGridView1.AutoResizeRow(e.RowIndex, DataGridViewAutoSizeRowMode.AllCells);
            }
        }
    }
}
