using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace GrazeViewV1
{
    public partial class DataLibraryExpandedView : Form
    {
        private MainPage _mainPage;                 // Hold reference for mainPage for dataLibrary
        private Bitmap memoryImage;                 // For printing
        private bool IsNavigating;                  // Boolean to determine if the user is navigating the application
        private readonly DBConnections _dbConnections;
        private readonly DBQueries _dbQueries;
        private readonly List<int> _selectedIndexes;

        public DataLibraryExpandedView(MainPage mainPage, DBQueries dbQueries, List<int> selectedIndexes)
        {
            IsNavigating = false;           // Initially, set navigating to false, unless found otherwise

            _dbQueries = dbQueries ?? throw new ArgumentNullException(nameof(dbQueries));
            _selectedIndexes = selectedIndexes ?? new List<int>();

            _dbConnections = new DBConnections(new DBSettings(
                    server: "sqldatabase404.database.windows.net",
                    database: "404ImageDBsql",
                    username: "sql404admin",
                    password: "sheepstool404()"
                    ));

            _mainPage = mainPage;           // Hold reference to mainPage
            InitializeComponent();          // Build DLExpanded view
            this.Text = "GrazeView";        // Add header "GRAZEVIEW" 

            if (ConsistentForm.IsFullScreen)    // Check to see if previous page was fullscreen
            {
                SetFullScreen();                // If so, set this page to fullscreen
            }

            // Event handler for close
            this.FormClosing += DLExpanded_XOut;        // Add event handler for user exiting the app

            // Load selected data from database
            LoadSelectedData();
        }

        private async void LoadSelectedData()
        {
            try
            {
                if (!_selectedIndexes.Any())
                {
                    MessageBox.Show("No data selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Fetch all data from the database
                var allData = await _dbQueries.GetCSVDBDataAsync();

                if (allData == null || allData.Count == 0)
                {
                    MessageBox.Show("No data retrieved from the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                foreach (int index in _selectedIndexes)
                {
                    if (index >= 0 && index < allData.Count)
                    {
                        var row = allData[index];

                        // Extract values safely
                        string uploadName = row.ContainsKey("Upload Name") && row["Upload Name"] != null ? row["Upload Name"].ToString() : "N/A";
                        string sampleDate = row.ContainsKey("Date Sample Taken") && row["Date Sample Taken"] != null ? row["Date Sample Taken"].ToString() : "N/A";
                        string sampleLocation = row.ContainsKey("Sample Location") && row["Sample Location"] != null ? row["Sample Location"].ToString() : "N/A";
                        string sheepBreed = row.ContainsKey("Sheep Breed") && row["Sheep Breed"] != null ? row["Sheep Breed"].ToString() : "N/A";
                        string comments = row.ContainsKey("Comments") && row["Comments"] != null ? row["Comments"].ToString() : "N/A";

                        string qufuPercent = row.ContainsKey("Qufu(%)") && row["Qufu(%)"] != null ? row["Qufu(%)"].ToString() : "0.00";
                        string nalePercent = row.ContainsKey("Nale(%)") && row["Nale(%)"] != null ? row["Nale(%)"].ToString() : "0.00";
                        string erciPercent = row.ContainsKey("Erci(%)") && row["Erci(%)"] != null ? row["Erci(%)"].ToString() : "0.00";
                        string bubblePercent = row.ContainsKey("Air Bubble(%)") && row["Air Bubble(%)"] != null ? row["Air Bubble(%)"].ToString() : "0.00";

                        // Retrieve Image from database using Upload Name or ID
                        Bitmap image = _dbQueries.RetrieveImageFromDB(index);

                        // Create UploadInfo and MLData objects
                        UploadInfo uploadInfo = new UploadInfo
                        {
                            UploadName = uploadName,
                            SampleDate = sampleDate,
                            SampleLocation = sampleLocation,
                            SheepBreed = sheepBreed,
                            Comments = comments,
                            ImageFile = image
                        };

                        MLData mlData = new MLData
                        {
                            qufuPercentage = qufuPercent,
                            nalePercentage = nalePercent,
                            erciPercentage = erciPercent,
                            bubblePercentage = bubblePercent
                        };

                        MessageBox.Show("$Adding panel for Upload Name: {uploadInfo.UploadName}, Sample Date: {uploadInfo.SampleDate}");

                        // Add the data to the expanded view
                        AddUploadPanel(uploadInfo, mlData);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading selected data: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Method for setting the page to full screen
        private void SetFullScreen()
        {
            this.WindowState = FormWindowState.Maximized;       // Maximize the form
            this.FormBorderStyle = FormBorderStyle.Sizable;     // Allow form to still be sized
            this.Bounds = Screen.PrimaryScreen.Bounds;          // Set bounds to max
        }

        // Event handler for X out
        private void DLExpanded_XOut(object sender, FormClosingEventArgs e)
        {
            if (IsNavigating)       // Check if the user chose to exit the application
            {       
                return;             // If so, continue running
            }
            if (e.CloseReason == CloseReason.UserClosing)   // If not,
            {   
                _mainPage.Close();                          // Close entire application
            }
        }

        // Event handler for exitButton
        private void exitButton_Click(object sender, EventArgs e)
        {
            // If the user clicks the exit button, we know they are still using the application
            IsNavigating = true;

            if (this.WindowState == FormWindowState.Maximized)      // Check current form for maximization or not
            {
                ConsistentForm.IsFullScreen = true;                 // If true, make sure it is passed to the next page
            }
            else                                                    // If false, also pass this information to the next page
            {
                ConsistentForm.IsFullScreen = false;
            }

            var dbQueries = new DBQueries(_dbConnections.ConnectionString); // Ensure connectionString is correct
            var dataLibrary = new DataLibrary(_mainPage, dbQueries);           // Initialize a new dataLibrary with reference to mainPage
            dataLibrary.Size = this.Size;                           // Set the dataLibrary equal to this page's size
            dataLibrary.Location = this.Location;                   // and location
            dataLibrary.Show();                                     // Show the new dataLibrary
            this.Close();                                           // Close this view and return to DataLibrary
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

        // Event handler for printButton
        private void printButton_Click(object sender, EventArgs e) // Event handler for the print button click
        {
            CaptureScreen();                                       // Call a method to capture the current screen (implementation not shown)
            PrintDocument printScreen = new PrintDocument();       // Create a new PrintDocument instance
            printScreen.PrintPage += new PrintPageEventHandler(printScreen_PrintPage); // Attach the PrintPage event handler for printing

            // Show Print Dialog
            PrintDialog printDialog = new PrintDialog              // Create a PrintDialog instance
            {
                Document = printScreen                             // Assign the PrintDocument to the dialog
            };

            // Print if confirmed
            if (printDialog.ShowDialog() == DialogResult.OK)        // Check if the user confirms the print action
            {
                printScreen.Print();                               // Print the document if confirmed
            }
        }

        // Method to capture the screen for printing
        private void CaptureScreen()
        {
            // Ensure the flowLayoutPanel exists and has content
            if (flowLayoutPanel == null || flowLayoutPanel.Controls.Count == 0)
            {
                MessageBox.Show("Nothing to capture for printing.");
                return;
            }

            // Get the full size of the flowLayoutPanel's content (not just visible area)
            int totalWidth = flowLayoutPanel.DisplayRectangle.Width;
            int totalHeight = flowLayoutPanel.DisplayRectangle.Height;

            // Create a bitmap to hold the entire content
            memoryImage = new Bitmap(totalWidth, totalHeight);

            using (Graphics memoryGraphics = Graphics.FromImage(memoryImage))
            {
                // Optional: Fill with a background color (e.g., white)
                memoryGraphics.Clear(Color.White);

                // Save the original flowLayoutPanel scroll position
                Point originalScrollPosition = flowLayoutPanel.AutoScrollPosition;

                // Temporarily set the scroll position to the top-left
                flowLayoutPanel.AutoScrollPosition = new Point(0, 0);

                // Render each control manually onto the bitmap
                foreach (Control control in flowLayoutPanel.Controls)
                {
                    // Get the bounds of the control relative to the FlowLayoutPanel
                    Rectangle controlBounds = control.Bounds;

                    // Render the control onto the bitmap
                    control.DrawToBitmap(memoryImage, controlBounds);
                }

                // Restore the original scroll position
                flowLayoutPanel.AutoScrollPosition = originalScrollPosition;
            }
        }

        // Event handler for PrintPage
        private void printScreen_PrintPage(object sender, PrintPageEventArgs e) // Event handler for the PrintPage event
        {
            e.Graphics.DrawImage(memoryImage, 0, 0); // Draw the captured image (memoryImage) at the top-left corner of the page
        }

        // Method to create a new panel per each selected upload (Public as it is called from DataLibrary)
        public void AddUploadPanel(UploadInfo uploadInfo, MLData mlData)
        {
            try
            {
                // Debugging: Ensure method is being called
                MessageBox.Show($"Creating panel for: {uploadInfo.UploadName}");

                // Create a new panel for the upload
                Panel uploadPanel = new Panel
                {
                    BorderStyle = BorderStyle.Fixed3D,
                    Padding = new Padding(10),
                    Margin = new Padding(10),
                    Width = 1000,
                    Height = 400,
                    Anchor = AnchorStyles.Top
                };

                // Main layout panel
                TableLayoutPanel mainLayout = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 2,
                    RowCount = 1,
                    AutoSize = false
                };

                mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

                // Left Panel for User Data
                FlowLayoutPanel userFlowLayout = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.TopDown,
                    AutoSize = true,
                    Dock = DockStyle.Fill
                };

                userFlowLayout.Controls.Add(CreateInfoLabel("Upload Name:", uploadInfo.UploadName));
                userFlowLayout.Controls.Add(CreateInfoLabel("Date of Sample:", uploadInfo.SampleDate));
                userFlowLayout.Controls.Add(CreateInfoLabel("Sample Location:", uploadInfo.SampleLocation));
                userFlowLayout.Controls.Add(CreateInfoLabel("Sheep Breed:", uploadInfo.SheepBreed));
                userFlowLayout.Controls.Add(CreateInfoLabel("Comments:", uploadInfo.Comments));

                // Left Panel for Model Data
                FlowLayoutPanel modelFlowLayout = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.TopDown,
                    AutoSize = true,
                    Dock = DockStyle.Fill
                };

                modelFlowLayout.Controls.Add(CreateInfoLabel("Nale (%):", mlData.nalePercentage));
                modelFlowLayout.Controls.Add(CreateInfoLabel("Qufu (%):", mlData.qufuPercentage));
                modelFlowLayout.Controls.Add(CreateInfoLabel("Erci (%):", mlData.erciPercentage));
                modelFlowLayout.Controls.Add(CreateInfoLabel("Bubbles (%):", mlData.bubblePercentage));

                // Image Display
                PictureBox uploadImage = new PictureBox
                {
                    Image = uploadInfo.ImageFile ?? new Bitmap(250, 250),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Dock = DockStyle.Fill,
                    Margin = new Padding(10)
                };

                // Add components to layout
                mainLayout.Controls.Add(userFlowLayout, 0, 0);
                mainLayout.Controls.Add(modelFlowLayout, 1, 0);

                // Add layout to the panel
                uploadPanel.Controls.Add(mainLayout);

                // Add panel to the UI
                flowLayoutPanel.Controls.Add(uploadPanel);

                // Debugging: Confirm the panel is added
                MessageBox.Show($"Panel successfully added for: {uploadInfo.UploadName}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding upload panel: {ex.Message}", "Panel Creation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Helper method to create a label with title and value for display in the panel
        private Label CreateInfoLabel(string title, string value) // Method to create a Label displaying a title and value
        {
            return new Label
            {
                Text = $"{title} {value}",                          // Set the label text to combine the title and value
                Font = new Font("Times New Roman", 10, FontStyle.Regular), // Use Times New Roman, size 10, with regular style
                AutoSize = true,                                    // Allow the label to adjust its size based on the content
                Margin = new Padding(3)                            // Add a 3-pixel margin around the label
            };
        }

        // Helper method to create a title label for each section (User Data / Model Data)
        private Label CreatePanelTitle(string title) // Method to create a Label styled as a panel title
        {
            return new Label
            {
                Text = title,                                      // Set the label text to the provided title
                Font = new Font("Times New Roman", 10, FontStyle.Bold), // Use Times New Roman, size 10, with bold style
                AutoSize = true,                                  // Allow the label to adjust its size based on the content
                //Margin = new Padding(3, 10, 3, 5),               // (Optional) Uncomment to add custom padding around the label
                Anchor = AnchorStyles.Top                         // Anchor the label to the top of its parent container
            };
        }

    }
}
