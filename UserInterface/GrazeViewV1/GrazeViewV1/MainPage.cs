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
        // Initialize Controls for use across multiple methods
        private LinkLabel userGuide;
        private Button returntoWelcome;

        public MainPage()
        {
            InitializeComponent();

            // Initialize Form Properties
            this.Text = "Main Page";

            // Initialize Temporary Label

            this.Controls.Add(mainLabel);

            // Initialize Upload Button

            this.Controls.Add(dataUploadButton);

            // Initialize User Guide Button
            
            this.Controls.Add(helpButton);

            // Initialize Data Viewer Button

            this.Controls.Add(dataViewerButton);

            // Initialize Return to Welcome Page Button
            returntoWelcome = new Button();
            returntoWelcome.Text = "Return to Welcome Page";
            returntoWelcome.Font = new Font("Times New Roman", 10, FontStyle.Regular);
            returntoWelcome.AutoSize = true;
            returntoWelcome.Click += returnButton_Click;
            this.Controls.Add(returntoWelcome);

            CenterControls();  // Center Buttons/Labels when created
            this.Resize += MainPage_Resize;  // Call Resize Method if form is resized
        }

        private void dataUploadButton_Click(object? sender, EventArgs e)  // Upload Button Clicked
        {
            DataUpload dataupload = new DataUpload();
            dataupload.Show();
            this.Hide();
        }

        private void dataViewerButton_Click(object? sender, EventArgs e)  // Data Viewer Button Clicked
        {
            // Connect to Data Viewer
        }

        private void helpButton_Click(object sender, EventArgs e)
        {
            // Connect to help page
            UserGuide.ShowHelpGuide();  // Call Method to only allow one instance open at a time
        }

        private void returnButton_Click(object? sender, EventArgs e)  // Method to return to WelcomePage
        {
            WelcomePage welcomepage = new WelcomePage();   // Create page at same size as previous page
            welcomepage.Show();
            this.Hide();
        }

        private void CenterControls()  // Method for centering labels and buttons
        {
            // COME BACK TO THIS TO FIX POSITIONING!!!

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

            // Center Return button to bottom
            returntoWelcome.Location = new Point(
                (this.ClientSize.Width / 2) - (returntoWelcome.Width / 2),
                this.ClientSize.Height - 40);
        }

        private void ResizeControls()  // Method for centering labels and buttons
        {
            // COME BACK TO THIS TO FIX POSITIONING!!!

            //// Center Page Label
            //mainLabel.Size = new Size(
            //    2, 2);

            // Center Upload Button (Based on Page Label's Height)
            dataUploadButton.Size = new Size(
                (this.ClientSize.Width / 3), (this.ClientSize.Height / 9));

            // Position Data Viewer Button to bottom right corner
            dataViewerButton.Size = new Size(
                (this.ClientSize.Width / 3), (this.ClientSize.Height / 9));

            //// Center Help Button to top right corner
            //helpButton.Location = new Point(
            //    (this.ClientSize.Width - 60), 10);

            //// Center Return button to bottom
            //returntoWelcome.Location = new Point(
            //    (this.ClientSize.Width / 2) - (returntoWelcome.Width / 2),
            //    this.ClientSize.Height - 40);
        }

        private void MainPage_Resize(object? sender, EventArgs e)  // Method for aligning page components when resized
        {
            CenterControls();  // Call CenterControls when form is resized
            ResizeControls();  // Call ResizeControls when form is resized
        }

        
    }
}
