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
        private Label mainLabel;
        private Button uploadButton;
        private Button dataButton;
        private LinkLabel userGuide;
        private Button returntoWelcome;
        private Image uploadedImage;

        public MainPage()
        {
            InitializeComponent();

            // Initialize Form Properties
            this.Text = "Main Page : Utilities";

            // Initialize Temporary Label
            mainLabel = new Label();                                        // Create new label
            mainLabel.Text = "Main Page : Options Below";                   // Text in label
            mainLabel.Font = new Font("Times New Roman", 24, FontStyle.Bold);   // Select font
            mainLabel.AutoSize = true;
            this.Controls.Add(mainLabel);

            // Initialize Upload Button
            uploadButton = new Button();
            uploadButton.Text = "Upload New Data";
            uploadButton.Font = new Font("Times New Roman", 12, FontStyle.Italic);
            uploadButton.AutoSize = true;
            uploadButton.Click += uploadButton_Click;
            this.Controls.Add(uploadButton);

            // Initialize User Guide Button
            userGuide = new LinkLabel();
            userGuide.Text = "User Guide";
            userGuide.Font = new Font("Times New Roman", 10, FontStyle.Regular);
            userGuide.Size = new Size(100, 50);
            userGuide.AutoSize = true;
            userGuide.Click += HelpLabel_Click;
            this.Controls.Add(userGuide);  

            // Initialize Data Viewer Button
            dataButton = new Button();
            dataButton.Text = "Data Viewer";
            dataButton.Font = new Font("Times New Roman", 12, FontStyle.Italic);
            dataButton.AutoSize = true;
            dataButton.Click += dataButton_Click;
            this.Controls.Add(dataButton);

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

        private void uploadButton_Click(object? sender, EventArgs e)  // Upload Button Clicked
        {
            DataUpload dataupload = new DataUpload();
            dataupload.Show();
            this.Hide();
        }

        private void dataButton_Click(object? sender, EventArgs e)  // Data Viewer Button Clicked
        {
            // Connect to Data Viewer
        }

        private void HelpLabel_Click(object? sender, EventArgs e)  // User Guide Clicked
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
            uploadButton.Location = new Point(
                (this.ClientSize.Width / 2) - (uploadButton.Width / 2),
                (this.ClientSize.Height - mainLabel.Height) / 3);

            // Position Data Viewer Button to bottom right corner
            dataButton.Location = new Point(
                (this.ClientSize.Width / 2) - (dataButton.Width / 2),
                (this.ClientSize.Height - mainLabel.Height) / 2);

            // Center Help Guide Button
            userGuide.Location = new Point(
                (this.ClientSize.Width - 70),
                (this.ClientSize.Height - 20));

            // Center Return button to bottom
            returntoWelcome.Location = new Point(
                (this.ClientSize.Width / 2) - (returntoWelcome.Width / 2),
                this.ClientSize.Height - 40);
        }

        private void MainPage_Resize(object? sender, EventArgs e)  // Method for aligning page components when resized
        {
            CenterControls();  // Call CenterControls when form is resized
        }
    }
}
