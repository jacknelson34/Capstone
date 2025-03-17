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

            //dataGridView1.CellFormatting += dataGridView1_CellFormatting;

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
            if (e.CloseReason == CloseReason.UserClosing)
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
            int rowIndex = selectedRow.Index + 1;

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


        private async void exportButton_Click(object sender, EventArgs e)
        {
            IsNavigating = true;

            // Ensure fullscreen consistency
            ConsistentForm.IsFullScreen = (this.WindowState == FormWindowState.Maximized);

            Button btn = sender as Button;
            if (btn == null) return;

            // Create PictureBox (loader) in same position as button
            // Position and show the loading spinner over the button
            exportLoader.Location = new Point(btn.Location.X + (btn.Width - exportLoader.Width) / 2,
                                                btn.Location.Y + (btn.Height - exportLoader.Height) / 2);
            exportLoader.Visible = true;
            btn.Visible = false; // Hide the button
            exportLoader.BringToFront();

            // Collect selected row indexes
            List<int> selectedIndexes = dataGridView1.Rows
                .Cast<DataGridViewRow>()
                .Where(row => Convert.ToBoolean(row.Cells[0].Value)) // Checkbox checked
                .Select(row => row.Index + 1) // Adjust index if SQL ID starts from 1
                .ToList();

            // Debugging
            /*
            foreach (int index in selectedIndexes)
            {
                MessageBox.Show($"Fetching row for index: {index}", "Debug", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            */

            if (!selectedIndexes.Any())
            {
                MessageBox.Show("Must select at least one upload.", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                exportLoader.Visible = false;
                btn.Visible = true;
                return;
            }

            // Open DataLibraryExpandedView and pass selected indexes
            DataLibraryExpandedView expandedView = new DataLibraryExpandedView(_mainPage, _dbQueries, selectedIndexes);
            expandedView.Size = this.Size;
            expandedView.Location = this.Location;

            // Run export process in background
            await expandedView.PreparePanelsAsync();

            expandedView.Show();
            this.Close();

            // Remove loading icon and restore button after completion
            exportLoader.Visible = false;
            btn.Visible = true;

            // Debugging
            //MessageBox.Show("Export complete!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        dataGridView1.Rows[rowIndex].Cells[0].Value = false;

                        // Apply column by column formatting
                        foreach (DataGridViewColumn column in dataGridView1.Columns)
                        {
                            var cell = dataGridView1.Rows[rowIndex].Cells[column.Index];

                            // Apply percentage formatting for respective columns
                            if (column.HeaderText.EndsWith("(%):"))
                            {

                                //Debugging
                                //MessageBox.Show("Column[" + cell.ColumnIndex.ToString() + "]: " + cell.Value.ToString());

                                if (decimal.TryParse(cell.Value?.ToString(), out decimal value))
                                {
                                    cell.Value = (value / 100).ToString("P2");
                                }
                            }

                            // Apply date formatting so only the date is shown
                            if (column.HeaderText == "Date Sample Taken" || column.HeaderText == "Upload Date")
                            {
                                if (DateTime.TryParse(cell.Value?.ToString(), out DateTime dateValue))
                                {
                                    cell.Value = dateValue.ToString("MM/dd/yyyy"); // Format to "MM/dd/yyyy"
                                }
                            }

                        }

                        // Check Sample Date Values
                        if (DateTime.TryParse(dataGridView1.Rows[rowIndex].Cells[6].Value?.ToString(), out DateTime sampleDate) &&
                            DateTime.TryParse(dataGridView1.Rows[rowIndex].Cells[8].Value?.ToString(), out DateTime uploadDate))
                        {
                            // Debugging
                            MessageBox.Show("Upload Date: " + uploadDate.ToShortDateString() + "\nSample Date: " + sampleDate.ToShortDateString());

                            // Compare only the date parts (ignoring time)
                            if (sampleDate.Date == uploadDate.Date)
                            {
                                dataGridView1.Rows[rowIndex].Cells[6].Value = "N/A";
                            }
                        }

                        // Check Sample Time Values
                        if (DateTime.TryParse(dataGridView1.Rows[rowIndex].Cells[7].Value?.ToString(), out DateTime sampleTime) &&
                            DateTime.TryParse(dataGridView1.Rows[rowIndex].Cells[9].Value?.ToString(), out DateTime uploadTime))
                        {
                            // Debugging
                            MessageBox.Show("Upload Time: " + uploadTime.ToLongTimeString() + "\nSample Time: " + sampleTime.ToLongTimeString());

                            // Compare only the time parts (ignoring date), allowing up to 1-minute difference
                            if (Math.Abs((sampleTime - uploadTime).TotalMinutes) <= 1)
                            {
                                dataGridView1.Rows[rowIndex].Cells[7].Value = "N/A";
                            }
                        }



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


        // Method for clearing Data
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

        /*private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].HeaderText.EndsWith("(%)")) // Only percentage columns
            {
                if (e.Value != null && decimal.TryParse(e.Value.ToString(), out decimal value))
                {
                    e.Value = (value / 100).ToString("P2"); // Converts "16.5" → "16.50%"
                    e.FormattingApplied = true;
                }
            }

            string columnName = dataGridView1.Columns[e.ColumnIndex].HeaderText;
            var cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];

            // Format Date Fields
            if (columnName.Contains("Date Sample Taken") || columnName.Contains("Upload Date"))
            {
                if (e.Value != null && DateTime.TryParse(e.Value.ToString(), out DateTime dateValue))
                {
                    e.Value = dateValue.ToString("MM/dd/yyyy");
                    e.FormattingApplied = true;
                }
            }

            // Apply "N/A" if Sample Date/Time is within 1 hour of Upload Date/Time
            if (columnName.Contains("Time Sample Taken"))
            {
                DateTime? sampleDateTime = null;
                DateTime? uploadDateTime = null;

                // Get values
                if (DateTime.TryParse(dataGridView1.Rows[e.RowIndex].Cells["Date Sample Taken"]?.Value?.ToString(), out DateTime sampleDate) &&
                    DateTime.TryParse(e.Value?.ToString(), out DateTime sampleTime))
                {
                    sampleDateTime = sampleDate.Date.Add(sampleTime.TimeOfDay);
                }

                if (DateTime.TryParse(dataGridView1.Rows[e.RowIndex].Cells["Upload Date"]?.Value?.ToString(), out DateTime uploadDate) &&
                    DateTime.TryParse(dataGridView1.Rows[e.RowIndex].Cells["Upload Time"]?.Value?.ToString(), out DateTime uploadTime))
                {
                    uploadDateTime = uploadDate.Date.Add(uploadTime.TimeOfDay);
                }

                // Check if within 1 hour, set to "N/A"
                if (sampleDateTime.HasValue && uploadDateTime.HasValue)
                {
                    if ((uploadDateTime.Value - sampleDateTime.Value).TotalMinutes <= 60)
                    {
                        e.Value = "N/A";
                        e.FormattingApplied = true;
                    }
                }

            }


        }*/
    }
}

