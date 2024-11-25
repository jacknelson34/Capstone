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
    public partial class MainPage : Form 
    {
        private DataLibrary _datalibrary; // Reference to the DataLibrary form instance

        // Constructor for initializing MainPage
        public MainPage()
        {

            this.ClientSize = ConsistentForm.FormSize; // Set the initial size of the form from ConsistentForm
            InitializeComponent(); // Initialize form components
            StartPosition = FormStartPosition.CenterScreen;
            ResizeControls(); // Adjust and position controls dynamically

            this.Text = "GrazeView"; // Set the title of the MainPage form
            _datalibrary = new DataLibrary(this); // Instantiate DataLibrary with a reference to MainPage

            this.Resize += MainPage_Resize; // Attach the resize event handler
            this.Load += MainPage_Load; // Attach the load event handler for initial adjustments

            if (ConsistentForm.IsFullScreen) // Check if fullscreen mode is enabled
            {
                SetFullScreen(); // Set the form to fullscreen mode
            }
        }

        // Method to enable fullscreen mode
        public void SetFullScreen()
        {
            this.WindowState = FormWindowState.Maximized; // Maximize the window
            this.FormBorderStyle = FormBorderStyle.Sizable; // Allow resizing of the form
            this.Bounds = Screen.PrimaryScreen.Bounds; // Set the form bounds to cover the entire screen
        }

        // Event handler for the Data Upload button click
        private void dataUploadButton_Click(object? sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized) // Check if the form is maximized
            {
                ConsistentForm.IsFullScreen = true; // Update the fullscreen flag in ConsistentForm
            }
            else
            {
                ConsistentForm.IsFullScreen = false; // Update the fullscreen flag
            }

            DataUpload dataupload = new DataUpload(this); // Create a new instance of DataUpload form
            dataupload.Size = this.Size; // Set the size of the new form to match MainPage
            dataupload.Location = this.Location; // Set the location of the new form to match MainPage
            dataupload.Show(); // Show the DataUpload form
            this.Hide(); // Hide the MainPage form
        }

        // Event handler for the Data Viewer button click
        private void dataViewerButton_Click(object? sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized) // Check if the form is maximized
            {
                ConsistentForm.IsFullScreen = true; // Update the fullscreen flag
            }
            else
            {
                ConsistentForm.IsFullScreen = false; // Update the fullscreen flag
            }

            DataLibrary datalibrary = new DataLibrary(this); // Create a new instance of DataLibrary form
            datalibrary.Size = this.Size; // Set the size of the new form to match MainPage
            datalibrary.Location = this.Location; // Set the location of the new form to match MainPage
            datalibrary.Show(); // Show the DataLibrary form
            this.Hide(); // Hide the MainPage form
        }

        // Event handler for the Help button click
        private void helpButton_Click(object sender, EventArgs e)
        {
            UserGuide.ShowHelpGuide(); // Open the user guide, ensuring only one instance is active
        }

        // Method to return the current DataLibrary instance
        public DataLibrary GetDataLibrary()
        {
            return _datalibrary; // Return the reference to the DataLibrary instance
        }

        // Method to resize and reposition the main panel
        private void ResizePanel()
        {
            mainPanel.Visible = false;

            mainPanel.Size = new Size((int)(this.ClientSize.Width * 0.8), (int)(this.ClientSize.Height * 0.83)); // Adjust the size of the panel
            mainPanel.Location = new Point((this.ClientSize.Width - mainPanel.Width) / 2, // Center the panel horizontally
                                           (this.ClientSize.Height - mainPanel.Height) / 2); // Center the panel vertically

            this.Refresh(); // Refresh the form to apply the changes
            mainPanel.Visible = true;
        }

        // Event handler for the form's load event
        private void MainPage_Load(object sender, EventArgs e)
        {
            ResizePanel(); // Resize and reposition the main panel
            ResizeControls(); // Resize and adjust the controls
            Refresh(); // Refresh the form to apply all changes
        }

        // Event handler for the form's resize event
        private void MainPage_Resize(object sender, EventArgs e)
        {
            ConsistentForm.FormLocation = this.Location; // Save the current form location in ConsistentForm
            ConsistentForm.FormSize = this.Size; // Save the current form size in ConsistentForm

            if (this.WindowState == FormWindowState.Maximized) // Check if the form is maximized
            {
                ConsistentForm.IsFullScreen = true; // Update fullscreen flag
                ResizePanel(); // Adjust the panel size and position
                ResizeControls(); // Adjust the controls
            }

            if (this.WindowState != FormWindowState.Maximized) // Check if the form is not maximized
            {
                ResizePanel(); // Adjust the panel size and position
                ResizeControls(); // Adjust the controls
            }

            ResizePanel(); // Ensure the panel is resized properly
            ResizeControls(); // Ensure the controls are adjusted properly
        }

        // Method to dynamically resize and position controls
        private void ResizeControls()
        {
            mainLabel.Size = new Size((this.ClientSize.Width / 2), (this.ClientSize.Height / 7)); // Adjust label size
            mainLabel.AutoSize = true; // Enable automatic size adjustment

            mainLabel.Location = new Point((mainPanel.Width / 2) - (mainLabel.Width / 2), // Center the label horizontally
                                           (mainPanel.Height / 6) - 40); // Position the label vertically

            float fontSize = Math.Max(8, this.ClientSize.Width / 30f); // Calculate font size based on form width
            mainLabel.Font = new Font("Times New Roman", fontSize, FontStyle.Bold, GraphicsUnit.Point, 0); // Set label font

            dataUploadButton.Size = new Size((mainPanel.Width / 3), 100); // Set size of the DataUpload button
            dataUploadButton.Location = new Point((mainPanel.Width / 2) - (dataUploadButton.Width / 2), // Center the button horizontally
                                                  ((mainPanel.Height - mainLabel.Height) / 2) - 30); // Position the button vertically

            dataViewerButton.Size = new Size((mainPanel.Width / 3), 100); // Set size of the DataViewer button
            dataViewerButton.Location = new Point((mainPanel.Width / 2) - (dataViewerButton.Width / 2), // Center the button horizontally
                                                  ((mainPanel.Height - mainLabel.Height) / 2) + 110); // Position the button vertically

            float buttonFontSize = Math.Max(8, dataViewerButton.Width / 20f); // Calculate button font size based on button width
            dataUploadButton.Font = new Font("Times New Roman", buttonFontSize, FontStyle.Regular, GraphicsUnit.Point, 0); // Set font for DataUpload button
            dataViewerButton.Font = new Font("Times New Roman", buttonFontSize, FontStyle.Regular, GraphicsUnit.Point, 0); // Set font for DataViewer button

            this.Refresh(); // Refresh the form to apply changes
        }

    }
}