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

namespace GrazeViewV1
{
    public partial class DataLibraryExpandedView : Form
    {
        private MainPage _mainPage;                 // Hold reference for mainPage for dataLibrary
        private Bitmap memoryImage;                 // For printing
        private bool IsNavigating;                  // Boolean to determine if the user is navigating the application

        public DataLibraryExpandedView(MainPage mainPage)
        {
            IsNavigating = false;           // Initially, set navigating to false, unless found otherwise

            _mainPage = mainPage;           // Hold reference to mainPage
            InitializeComponent();          // Build DLExpanded view
            this.Text = "GrazeView";        // Add header "GRAZEVIEW" 

            if (ConsistentForm.IsFullScreen)    // Check to see if previous page was fullscreen
            {
                SetFullScreen();                // If so, set this page to fullscreen
            }

            // Event handler for close
            this.FormClosing += DLExpanded_XOut;        // Add event handler for user exiting the app

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

            var dataLibrary = new DataLibrary(_mainPage);           // Initialize a new dataLibrary with reference to mainPage
            dataLibrary.Size = this.Size;                           // Set the dataLibrary equal to this page's size
            dataLibrary.Location = this.Location;                   // and location
            dataLibrary.Show();                                     // Show the new dataLibrary
            this.Close();                                           // Close this view and return to DataLibrary
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
        public void AddUploadPanel(UploadInfo uploadInfo, MLData mlData) // Method to dynamically create and add an upload panel
        {
            // Set fixed size for the upload panel
            Panel uploadPanel = new Panel
            {
                BorderStyle = BorderStyle.Fixed3D,       // Add a 3D border for better visuals
                Padding = new Padding(10),              // Add padding inside the panel
                Margin = new Padding(10),               // Add spacing outside the panel
                Width = 1000,                           // Fixed width of the panel
                Height = 400,                           // Fixed height of the panel
                Anchor = AnchorStyles.Top               // Anchor panel to the top of the container
            };

            // Main layout to structure content
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,                  // Fill the panel
                ColumnCount = 2,                        // Split layout into two columns
                RowCount = 1,                           // Single row for the layout
                AutoSize = false                        // Prevent auto-sizing
            };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F)); // Left half for data
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F)); // Right half for the image

            // Left Panel for User and Model Data
            TableLayoutPanel dataLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,                  // Fill parent container
                ColumnCount = 1,                        // Single column for stacked sections
                RowCount = 2,                           // Two rows for user and model data
                AutoSize = false                        // Prevent auto-sizing
            };
            dataLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F)); // Top half for user data
            dataLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F)); // Bottom half for model data

            // User-provided data section
            Panel userPanel = new Panel
            {
                AutoSize = false,                       // Prevent auto-sizing
                Dock = DockStyle.Fill,                  // Fill the allocated area
                Padding = new Padding(10)               // Add internal padding
            };
            FlowLayoutPanel userFlowLayout = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,  // Stack controls vertically
                AutoSize = true,                        // Enable auto-sizing
                Dock = DockStyle.Fill,                  // Fill the parent container
                Padding = new Padding(0),               // Remove internal padding
                Margin = new Padding(0)                 // Remove external margin
            };
            Label titleLabel = CreatePanelTitle("User Provided Data:"); // Create a title for the user data section
            titleLabel.TextAlign = ContentAlignment.MiddleLeft;         // Align title to the left
            titleLabel.Dock = DockStyle.Top;                            // Dock the title at the top
            userFlowLayout.Controls.Add(titleLabel);                   // Add the title to the flow layout
            userFlowLayout.Controls.Add(CreateInfoLabel("Upload Name:", uploadInfo.UploadName));       // Add label for upload name
            userFlowLayout.Controls.Add(CreateInfoLabel("Date of Sample:", uploadInfo.SampleDate.ToString("MM/dd/yyyy"))); // Add label for sample date
            userFlowLayout.Controls.Add(CreateInfoLabel("Sample Location:", uploadInfo.SampleLocation)); // Add label for sample location
            userFlowLayout.Controls.Add(CreateInfoLabel("Sheep Breed:", uploadInfo.SheepBreed));        // Add label for sheep breed
            userFlowLayout.Controls.Add(CreateInfoLabel("Comments:", uploadInfo.Comments));            // Add label for comments
            userPanel.Controls.Add(userFlowLayout);                  // Add the flow layout to the user panel

            // Model-generated data section
            Panel modelPanel = new Panel
            {
                AutoSize = false,                       // Prevent auto-sizing
                Dock = DockStyle.Fill,                  // Fill the allocated area
                Padding = new Padding(10)               // Add internal padding
            };
            FlowLayoutPanel modelFlowLayout = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,  // Stack controls vertically
                AutoSize = true,                        // Enable auto-sizing
                Dock = DockStyle.Fill                   // Fill the parent container
            };
            modelFlowLayout.Controls.Add(CreatePanelTitle("Model Generated Data:"));                 // Add title for the model data section
            modelFlowLayout.Controls.Add(CreateInfoLabel("Nale (%):", mlData.nalePercentage));       // Add label for "Nale"
            modelFlowLayout.Controls.Add(CreateInfoLabel("Qufu (%):", mlData.qufuPercentage));       // Add label for "Qufu"
            modelFlowLayout.Controls.Add(CreateInfoLabel("Erci (%):", mlData.erciPercentage));       // Add label for "Erci"
            modelFlowLayout.Controls.Add(CreateInfoLabel("Bubbles (%):", mlData.bubblePercentage));  // Add label for "Bubbles"
            modelFlowLayout.Controls.Add(CreateInfoLabel("Qufu Stem (%):", mlData.qufustemPercentage)); // Add label for "Qufu Stem"
            modelPanel.Controls.Add(modelFlowLayout);              // Add the flow layout to the model panel

            // Add user and model panels to the data layout
            dataLayout.Controls.Add(userPanel, 0, 0);               // Add user panel to the top half
            dataLayout.Controls.Add(modelPanel, 0, 1);              // Add model panel to the bottom half

            // Right Panel for Image
            PictureBox uploadImage = new PictureBox
            {
                Image = uploadInfo.ImageFile ?? new Bitmap(250, 250), // Use provided image or a placeholder
                SizeMode = PictureBoxSizeMode.Zoom,                  // Scale the image proportionally
                Dock = DockStyle.Fill,                               // Fill the allocated area
                Margin = new Padding(10)                             // Add margin around the image
            };

            // Add data layout and image to the main layout
            mainLayout.Controls.Add(dataLayout, 0, 0);              // Add the data layout to the left column
            mainLayout.Controls.Add(uploadImage, 1, 0);             // Add the image to the right column

            // Add the main layout to the upload panel
            uploadPanel.Controls.Add(mainLayout);                   // Add the main layout to the upload panel

            // Add the upload panel to the parent flow layout
            flowLayoutPanel.Controls.Add(uploadPanel);              // Add the upload panel to the flow layout
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
