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

            // Message Boxes to show updating on size and location
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
            mainPanel.Size = new Size((int)(this.ClientSize.Width * 0.8), (int)(this.ClientSize.Height * 0.83));
            mainPanel.Location = new Point((this.ClientSize.Width - mainPanel.Width) / 2,
                                           (this.ClientSize.Height - mainPanel.Height) / 2);

            this.Refresh();
        }

        private void MainPage_Resize(object sender, EventArgs e)
        {
            ConsistentForm.FormLocation = this.Location;
            ConsistentForm.FormSize = this.Size;

            ResizePanel();
            ResizeControls();
        }

        private void ResizeControls()  // Method for centering labels and buttons
        {
            // Adjust label size dynamically
            mainLabel.Size = new Size(
                (this.ClientSize.Width / 2), (this.ClientSize.Height / 7));
            mainLabel.AutoSize = true;

            // Adjust label postition dynamically
            mainLabel.Location = new Point(
                (mainPanel.Width / 2) - (mainLabel.Width / 2),
                (mainPanel.Height / 6));

            // Adjust DataUpload button size dynamically
            dataUploadButton.Size = new Size(
                (mainPanel.Width / 3), 150);

            // Adjust dataupload button position on resize
            dataUploadButton.Location = new Point(
                (mainPanel.Width / 2) - (dataUploadButton.Width / 2),
                (mainPanel.Height - mainLabel.Height) / 2 - 75);

            // Adjust DataView button size dynamically
            dataViewerButton.Size = new Size(
                (mainPanel.Width / 3), 150);

            // adjust dataview button on resize
            dataViewerButton.Location = new Point(
               (mainPanel.Width / 2) - (dataViewerButton.Width / 2),
               ((mainPanel.Height - mainLabel.Height) / 2) + 125);

            this.Refresh();
        }

        
    }
}
