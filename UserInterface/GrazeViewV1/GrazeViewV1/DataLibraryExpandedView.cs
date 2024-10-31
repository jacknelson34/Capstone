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
    public partial class DataLibraryExpandedView : Form
    {
        private MainPage _mainPage;                 // Hold reference for mainPage for dataLibrary

        public DataLibraryExpandedView()
        {
            InitializeComponent(); 
        }

        // Event handler for exitButton
        private void exitButton_Click(object sender, EventArgs e)
        {
            var dataLibrary = new DataLibrary(_mainPage);
            dataLibrary.Show();
            this.Close(); // Close this view and return to DataLibrary
        }

        // Event handler for printButton (currently no function) --- TODO
        private void printButton_Click(object sender, EventArgs e)
        {
            // Placeholder function - no action required
        }

        // Method to create a new panel per each selected upload (Public as it is called from DataLibrary)
        public void AddUploadPanel(UploadInfo uploadInfo, MLData mlData)
        {
            // Main panel for user information on the left
            Panel uploadPanel = new Panel
            {
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10),
                Margin = new Padding(10),
                Width = flowLayoutPanel.ClientSize.Width - 20,
                AutoSize = true
            };

            // Image at the top center
            if (uploadInfo.ThumbNail != null)
            {
                PictureBox uploadImage = new PictureBox
                {
                    Image = uploadInfo.ThumbNail,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Width = 150,
                    Height = 150,
                    Anchor = AnchorStyles.Top,
                    Margin = new Padding((uploadPanel.Width - 150) / 2, 0, 0, 10)
                };
                uploadPanel.Controls.Add(uploadImage);
            }

            // Panel for user-provided data on the left
            Panel userPanel = new Panel
            {
                Width = uploadPanel.Width / 2 - 10,
                Height = 150,
                Dock = DockStyle.Left,
                AutoSize = true
            };
            userPanel.Controls.Add(CreatePanelTitle("User Provided Data:"));
            userPanel.Controls.Add(CreateInfoLabel("Upload Name:", uploadInfo.UploadName));
            userPanel.Controls.Add(CreateInfoLabel("Date of Sample:", uploadInfo.SampleDate.ToString("MM/dd/yyyy")));
            userPanel.Controls.Add(CreateInfoLabel("Sample Location:", uploadInfo.SampleLocation));
            userPanel.Controls.Add(CreateInfoLabel("Sheep Breed:", uploadInfo.SheepBreed));
            userPanel.Controls.Add(CreateInfoLabel("Comments:", uploadInfo.Comments));

            // Panel for model-generated data on the right
            Panel modelPanel = new Panel
            {
                Width = uploadPanel.Width / 2 - 10,
                Height = 150,
                Dock = DockStyle.Right,
                AutoSize = true
            };
            modelPanel.Controls.Add(CreatePanelTitle("Model Generated Data:"));
            modelPanel.Controls.Add(CreateInfoLabel("Nale (%):", mlData.nalePercentage));
            modelPanel.Controls.Add(CreateInfoLabel("Qufu (%):", mlData.qufuPercentage));
            modelPanel.Controls.Add(CreateInfoLabel("Erci (%):", mlData.erciPercentage));
            modelPanel.Controls.Add(CreateInfoLabel("Bubbles (%):", mlData.bubblePercentage));
            modelPanel.Controls.Add(CreateInfoLabel("Qufu Stem (%):", mlData.qufustemPercentage));

            // Add user and model panels to the main panel
            uploadPanel.Controls.Add(userPanel);
            uploadPanel.Controls.Add(modelPanel);

            // Add the main upload panel to the flow layout
            flowLayoutPanel.Controls.Add(uploadPanel);
        }

        // Helper method to create a label with title and value for display in the panel
        private Label CreateInfoLabel(string title, string value)
        {
            return new Label
            {
                Text = $"{title} {value}",
                AutoSize = true,
                Margin = new Padding(3)
            };
        }

        // Helper method to create a title label for each section (User Data / Model Data)
        private Label CreatePanelTitle(string title)
        {
            return new Label
            {
                Text = title,
                Font = new Font("Arial", 10, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(3, 10, 3, 5)
            };
        }

        // Helper method to create a visual separator between sections
        private Label CreateSeparator()
        {
            return new Label
            {
                BorderStyle = BorderStyle.Fixed3D,
                Height = 2,
                Dock = DockStyle.Top,
                Margin = new Padding(0, 10, 0, 10)
            };
        }


    }
}
