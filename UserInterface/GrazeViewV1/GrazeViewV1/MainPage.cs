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
    public partial class MainPage : ConsistentForm
    {
        // Hold reference for datalibrary
        private DataLibrary _datalibrary;

        public MainPage()
        {
            InitializeComponent();
            _datalibrary = new DataLibrary(this);

            /*CenterControls();  // Center Buttons/Labels when created
            this.Resize += MainPage_Resize;  // Call Resize Method if form is resized*/
        }

        private void dataUploadButton_Click(object? sender, EventArgs e)  // Upload Button Clicked
        {
            DataUpload dataupload = new DataUpload(this);
            dataupload.Show();
            this.Hide();
        }

        private void dataViewerButton_Click(object? sender, EventArgs e)  // Data Viewer Button Clicked
        {
            DataLibrary datalibrary = new DataLibrary(this);
            datalibrary.Show();
            this.Hide();
        }

        private void helpButton_Click(object sender, EventArgs e)
        {
            // Connect to help page
            UserGuide.ShowHelpGuide();  // Call Method to only allow one instance open at a time
        }

        private void OpenDataLibrary()
        {
            if (_datalibrary != null)  // Only create a new instance if one doesn't exist
            {
                _datalibrary.LoadUploadsFromGlobalData();
            }
            else
            {
                _datalibrary = new DataLibrary(this);
            }

            _datalibrary.Show();  // Show the existing instance
            this.Hide();          // Hide the MainPage
        }

        // Method to call DataLibrary from any page connected to main
        public DataLibrary GetDataLibrary()
        {
            return _datalibrary; 
        }

        /*private void CenterControls()  // Method for centering labels and buttons
        {
            // Center Page Label
            mainLabel.Location = new Point(
                (this.ClientSize.Width / 2) - (mainLabel.Width / 2),
                (this.ClientSize.Height / 6));

            // Center Upload Button (Based on Page Label's Height)
            dataUploadButton.Location = new Point(
                (this.ClientSize.Width / 2) - (dataUploadButton.Width / 2),
                (this.ClientSize.Height - mainLabel.Height) / 3);

            // Position Data Viewer Button to bottom right corner
            dataViewerButton.Location = new Point(
                (this.ClientSize.Width / 2) - (dataViewerButton.Width / 2),
                (this.ClientSize.Height - mainLabel.Height) / 2);

            // Center Help Button to top right corner
            helpButton.Location = new Point(
                (this.ClientSize.Width - 60), 10);
        }

        private void ResizeControls()  // Method for centering labels and buttons
        {
            // Center Upload Button (Based on Page Label's Height)
            dataUploadButton.Size = new Size(
                (this.ClientSize.Width / 3), (this.ClientSize.Height / 9));

            // Position Data Viewer Button to bottom right corner
            dataViewerButton.Size = new Size(
                (this.ClientSize.Width / 3), (this.ClientSize.Height / 9));
        }

        private void MainPage_Resize(object? sender, EventArgs e)  // Method for aligning page components when resized
        {
            CenterControls();  // Call CenterControls when form is resized
            ResizeControls();  // Call ResizeControls when form is resized
        }*/

        
    }
}
