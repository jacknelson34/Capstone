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
        // Hold reference for datalibrary
        private DataLibrary _datalibrary;

        public MainPage()
        {
            InitializeComponent();
            _datalibrary = new DataLibrary(this);           // Create dataLibrary with reference to mainpage

            this.Size = ConsistentForm.FormSize;
            this.Location = ConsistentForm.FormLocation;
            if (ConsistentForm.IsFullScreen)
            {
                SetFullScreen();
            }

            MessageBox.Show("Form Size = " + ConsistentForm.FormSize.ToString());
            MessageBox.Show("Current Size = " + this.Size.ToString());

            this.Resize += MainPage_Resize;
        }

        private void SetFullScreen()     // Class to handle screen maximization
        {
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Bounds = Screen.PrimaryScreen.Bounds;
        }

        private void dataUploadButton_Click(object? sender, EventArgs e)  // Upload Button Clicked
        {
            ConsistentForm.FormSize = this.Size;                // Adjust consistent form parameters if form was resized
            ConsistentForm.FormLocation = this.Location;        // Adjust consistent form parameters if form was relocated
            if (this.WindowState == FormWindowState.Maximized)
            {
                ConsistentForm.IsFullScreen = true;
            }
            else
            {
                ConsistentForm.IsFullScreen = false;
            }

            DataUpload dataupload = new DataUpload(this);       // Create new dataUpload form
            dataupload.Show();                                  // Show dataUpload
            this.Hide();                                        // Hide mainPage
        }

        private void dataViewerButton_Click(object? sender, EventArgs e)  // Data Viewer Button Clicked
        {
            ConsistentForm.FormSize = this.Size;                // Adjust consistent form parameters if form was resized
            ConsistentForm.FormLocation = this.Location;        // Adjust consistent form parameters if form was relocated
            if (this.WindowState == FormWindowState.Maximized)
            {
                ConsistentForm.IsFullScreen = true;
            }
            else
            {
                ConsistentForm.IsFullScreen = false;
            }

            DataLibrary datalibrary = new DataLibrary(this);    // Create new dataLibrary
            datalibrary.Show();                                 // Show dataLibrary
            this.Hide();                                        // Hide mainPage
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

        private void ResizePanel()
        {
            mainPanel.Width = (int)(this.ClientSize.Width * 0.8);
            mainPanel.Height = (int)(this.ClientSize.Height * 0.83);

            this.Refresh();
        }

        private void MainPage_Resize(object sender, EventArgs e)
        {
            ConsistentForm.FormLocation = this.Location;
            ConsistentForm.FormSize = this.Size;

            ResizePanel();
        }

        

        // -------------------------------- Previous Method for resizing controls : NO LONGER IN USE --------------------- //

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
