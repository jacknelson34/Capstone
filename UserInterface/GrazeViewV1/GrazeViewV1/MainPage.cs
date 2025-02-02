using System; 
using System.Collections.Generic; 
using System.ComponentModel; 
using System.Data;
using System.Diagnostics;
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
            InitializeComponent(); // Initialize form components
            StartPosition = FormStartPosition.CenterScreen;
            ResizeControls(); // Adjust and position controls dynamically

            this.Text = "GrazeView"; // Set the title of the MainPage form
            _datalibrary = new DataLibrary(this); // Instantiate DataLibrary with a reference to MainPage

            this.Resize += MainPage_Resize; // Attach the resize event handler
            this.Load += MainPage_Load; // Attach the load event handler for initial adjustments
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

        // Event handler for the Data Upload button click
        private void dataUploadButton_Click(object? sender, EventArgs e)
        {
            DataUpload dataupload = new DataUpload(this); // Create a new instance of DataUpload form
            dataupload.Size = this.Size; // Set the size of the new form to match MainPage
            dataupload.Location = this.Location; // Set the location of the new form to match MainPage
            dataupload.WindowState = this.WindowState;
            dataupload.Show(); // Show the DataUpload form
            this.Hide(); // Hide the MainPage form
        }

        // Event handler for the Data Viewer button click
        private void dataViewerButton_Click(object? sender, EventArgs e)
        {
            DataLibrary datalibrary = new DataLibrary(this); // Create a new instance of DataLibrary form
            datalibrary.Size = this.Size; // Set the size of the new form to match MainPage
            datalibrary.Location = this.Location; // Set the location of the new form to match MainPage
            datalibrary.WindowState = this.WindowState;
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

        // Event handler for the form's load event
        private void MainPage_Load(object sender, EventArgs e)
        {

            ResizeControls(); // Resize and adjust the controls
            Refresh(); // Refresh the form to apply all changes
        }

        // Event handler for the form's resize event
        private void MainPage_Resize(object sender, EventArgs e)
        {
            float fontSize = Math.Max(8, this.ClientSize.Width / 30f); // Calculate font size based on form width
            mainLabel.Font = new Font("Times New Roman", fontSize, FontStyle.Bold, GraphicsUnit.Point, 0); // Set label font
            mainLabel.Size = new Size((this.ClientSize.Width / 2), (this.ClientSize.Height / 7)); // Adjust label size
            ResizeControls(); // Adjust the controls

        }

        // Method to dynamically resize and position controls
        private void ResizeControls()
        {

            //float dpiScale = (float)(this.DeviceDpi / 96F);

            mainLabel.Location = new Point((this.ClientSize.Width / 2) - (mainLabel.Width / 2), // Center the label horizontally
                                           (this.ClientSize.Height / 6) - 40); // Position the label vertically

            dataUploadButton.Size = new Size((this.ClientSize.Width / 3), 100); // Set size of the DataUpload button
            dataUploadButton.Location = new Point((this.ClientSize.Width / 2) - (dataUploadButton.Width / 2), // Center the button horizontally
                                                  ((this.ClientSize.Height - mainLabel.Height) / 2) - 30); // Position the button vertically

            dataViewerButton.Size = new Size((this.ClientSize.Width / 3), 100); // Set size of the DataViewer button
            dataViewerButton.Location = new Point((this.ClientSize.Width / 2) - (dataViewerButton.Width / 2), // Center the button horizontally
                                                  ((this.ClientSize.Height - mainLabel.Height) / 2) + 110); // Position the button vertically

            float buttonFontSize = Math.Max(8, dataViewerButton.Width / 20f); // Calculate button font size based on button width
            dataUploadButton.Font = new Font("Times New Roman", buttonFontSize, FontStyle.Regular, GraphicsUnit.Point, 0); // Set font for DataUpload button
            dataViewerButton.Font = new Font("Times New Roman", buttonFontSize, FontStyle.Regular, GraphicsUnit.Point, 0); // Set font for DataViewer button

           // MessageBox.Show("Form Size : " + this.ClientSize.ToString() + "\nButton Size : " + dataUploadButton.Size.ToString() + "\nButton Font Size : " + buttonFontSize.ToString());

            this.Refresh(); // Refresh the form to apply changes
        }


        // Helper method to resize main from other forms
        public void ExternalResize(Size s, Point p)
        {
            this.Size = s;
            this.Location = p;
        }
    }
}