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
        private MainPage _mainPage;    // hold reference to Main Page form
        private bool IsNavigating;     // boolean variable that checks if the user is still using the app
        private readonly DBQueries _dbQueries;

        public DataLibrary(MainPage mainpage, DBQueries dbQueries)
        {
            IsNavigating = false;      // user is no longer using (default setting)
            _dbQueries = dbQueries ?? throw new ArgumentNullException(nameof(dbQueries));

            // Form Properties
            InitializeComponent();
            _mainPage = mainpage;
            this.Text = "GrazeView";

            // Maintains window size
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

            // Close MainPage on close
            this.FormClosing += DataLibrary_XOut;
            _dbQueries = dbQueries;
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

        // Helper method to maintain full screen
        private void SetFullScreen()
        {
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Bounds = Screen.PrimaryScreen.Bounds;
        }

        // Helper method to close down the app if the top right exit button is pressed
        private void DataLibrary_XOut(object sender, FormClosingEventArgs e)
        {
            if (IsNavigating)
            {
                return;
            }
            if(e.CloseReason == CloseReason.UserClosing)
            {
                _mainPage.Close();
            }
        }

        // Method for previewing an uploaded image
        private async void previewButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            // Determine selected rows
            var selectedRows = dataGridView1.Rows.Cast<DataGridViewRow>()
                                                 .Where(row => Convert.ToBoolean(row.Cells[0].Value))
                                                 .ToList();

            if (selectedRows.Count < 1)
            {
                MessageBox.Show("Please select 1 Upload to Preview.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (selectedRows.Count > 1)
            {
                MessageBox.Show("Only one Upload may be selected at a time for Preview.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedRow = selectedRows.First();
            int rowIndex = selectedRow.Index;

            // Position and show the loading spinner over the button
            loadingSpinner.Location = new Point(btn.Location.X + (btn.Width - loadingSpinner.Width) / 2,
                                                btn.Location.Y + (btn.Height - loadingSpinner.Height) / 2);
            loadingSpinner.Visible = true;
            btn.Visible = false; // Hide the button

            // Load the image in the background
            Bitmap retrievedImage = await Task.Run(() =>
            {
                DBQueries dbQueries = new DBQueries("Server=sqldatabase404.database.windows.net;Database=404ImageDBsql;User Id=sql404admin;Password=sheepstool404();TrustServerCertificate=False;MultipleActiveResultSets=True;");
                return dbQueries.RetrieveImageFromDB(rowIndex);
            });

            // Hide the spinner and show the button again
            loadingSpinner.Visible = false;
            btn.Visible = true;

            if (retrievedImage == null)
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
                Image = retrievedImage,
                SizeMode = PictureBoxSizeMode.Zoom,
                Dock = DockStyle.Fill
            };

            imagePreviewForm.Controls.Add(pictureBox);
            imagePreviewForm.ShowDialog(); // Show as a dialog
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

        // Event handler for when export button is click 
        private void exportButton_Click(object sender, EventArgs e)
        {
            IsNavigating = true;

            // Ensure fullscreen consistency
            ConsistentForm.IsFullScreen = (this.WindowState == FormWindowState.Maximized);

            // Collect selected row indexes
            List<int> selectedIndexes = dataGridView1.Rows
                .Cast<DataGridViewRow>()
                .Where(row => Convert.ToBoolean(row.Cells[0].Value)) // Checkbox checked
                .Select(row => row.Index + 1) // Adjust index if SQL ID starts from 1
                .ToList();

            if (!selectedIndexes.Any())
            {
                MessageBox.Show("Must select at least one upload.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Open DataLibraryExpandedView and pass selected indexes
            DataLibraryExpandedView expandedView = new DataLibraryExpandedView(_mainPage, _dbQueries, selectedIndexes);
            expandedView.Size = this.Size;
            expandedView.Location = this.Location;

            expandedView.Show();
            this.Close();
        }


        // Method to add data from DataUpload to the Library
        public async Task LoadUploadsFromGlobalData()
        {
            try
            {
                dataGridView1.Rows.Clear();

                var csvData = await _dbQueries.GetCSVDBDataAsync();

                if (csvData.Count > 0)
                {
                    // Ensure columns match database schema
                    dataGridView1.ColumnCount = csvData[0].Count;

                    // Set column headers dynamically
                    int colIndex = 0;
                    foreach (var column in csvData[0].Keys)
                    {
                        dataGridView1.Columns[colIndex].Name = column;
                        colIndex++;
                    }

                    // Populate rows
                    foreach (var row in csvData)
                    {
                        int rowIndex = dataGridView1.Rows.Add(row.Values.ToArray());
                        //dataGridView1.Rows[rowIndex].DefaultCellStyle.Font = new Font("Times New Roman", 10, FontStyle.Regular);
                        dataGridView1.Rows[rowIndex].Cells[0].Value = false;

                    }

                }
                else
                {
                    //MessageBox.Show("No data found in CSVDB.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading CSVDB data: {ex.Message}");
            }
        }


        // Uploads all data to the data viewer
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
                        SizeF size = g.MeasureString(comment, dataGridView1.DefaultCellStyle.Font, 300); // Measure with max width
                        int requiredHeight = (int)Math.Ceiling(size.Height) + 10; // Add padding
                        row.Height = Math.Max(requiredHeight, dataGridView1.RowTemplate.Height); // Ensure height is not less than default
                    }
                }
            }

            

        }

        // Method for adjusting row height if comments exceed 500 pixels
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            /*if (e.ColumnIndex == dataGridView1.Columns["CommentsCol"].Index)
            {
                // Adjust the height of the row for the changed cell
                dataGridView1.AutoResizeRow(e.RowIndex, DataGridViewAutoSizeRowMode.AllCells);
            }*/
        }

        // Temporary Method for clearing Data
        private void clearDataButton_Click(object sender, EventArgs e)
        {

            DialogResult clearDataCheck = MessageBox.Show(
                "Are you sure you want to clear all data?  This cannot be undone.",         // Message
                "Confirm Action",                                   // Title
                MessageBoxButtons.YesNo,                            // Buttons
                MessageBoxIcon.Question                             // Icon
                );

            if (clearDataCheck == DialogResult.Yes)
            { 
                Program.ClearAllData(_dbQueries);
                dataGridView1.Rows.Clear();
            }

        }
    }
}
