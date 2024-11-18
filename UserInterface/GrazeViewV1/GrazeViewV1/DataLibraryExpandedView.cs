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
        private bool IsNavigating;

        public DataLibraryExpandedView(MainPage mainPage)
        {
            IsNavigating = false;

            _mainPage = mainPage;
            InitializeComponent();
            this.Text = "GrazeView";
            this.Size = ConsistentForm.FormSize;
            this.Location = ConsistentForm.FormLocation;
            if (ConsistentForm.IsFullScreen)
            {
                SetFullScreen();
            }
            this.Refresh();

            // Event handler for close
            this.FormClosing += DLExpanded_XOut;

        }

        // Event handler for X out
        private void DLExpanded_XOut(object sender, FormClosingEventArgs e)
        {
            if (IsNavigating)
            {
                return;
            }
            if (e.CloseReason == CloseReason.UserClosing)
            {
                _mainPage.Close();
            }
        }

        // Event handler for exitButton
        private void exitButton_Click(object sender, EventArgs e)
        {
            IsNavigating = true;

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

            var dataLibrary = new DataLibrary(_mainPage);
            dataLibrary.Show();
            this.Close(); // Close this view and return to DataLibrary
        }

        // Event handler for printButton (currently no function) --- TODO
        private void printButton_Click(object sender, EventArgs e)
        {
            CaptureScreen();
            PrintDocument printScreen = new PrintDocument();
            printScreen.PrintPage += new PrintPageEventHandler(printScreen_PrintPage);

            // Show Print Dialog
            PrintDialog printDialog = new PrintDialog
            {
                Document = printScreen
            };

            // Print if confirmed
            if(printDialog.ShowDialog() == DialogResult.OK)
            {
                printScreen.Print();
            }


        }

        private void SetFullScreen()     // Class to handle screen maximization
        {
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Bounds = Screen.PrimaryScreen.Bounds;
        }
        
        // Method to capture the screen for printing
        private void CaptureScreen()
        {
            // Calculate the area to capture by excluding the header and controlPanel
            int headerHeight = this.PointToScreen(new Point(0, 0)).Y - this.Top;
            int captureHeight = this.Height - headerHeight - controlPanel.Height;

            // Initialize memoryImage with the adjusted size
            memoryImage = new Bitmap(this.Width, captureHeight);

            // Capture the screen within the specified bounds
            using (Graphics memoryGraphics = Graphics.FromImage(memoryImage))
            {
                // Adjust the starting Y coordinate to skip the header
                int startY = this.Location.Y + headerHeight;

                // Capture the screen excluding the header and controlPanel
                memoryGraphics.CopyFromScreen(this.Location.X, startY, 0, 0, new Size(this.Width, captureHeight));
            }
        }

        // Event handler for PrintPage
        private void printScreen_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(memoryImage, 0, 0);
        }

        // Method to create a new panel per each selected upload (Public as it is called from DataLibrary)
        public void AddUploadPanel(UploadInfo uploadInfo, MLData mlData)
        {
            // Set fixed size for the upload panel
            Panel uploadPanel = new Panel
            {
                BorderStyle = BorderStyle.Fixed3D,
                Padding = new Padding(10),
                Margin = new Padding(10),
                Width = 1200,
                Height = 400,
                Anchor = AnchorStyles.Top
            };

            // Main layout to structure content
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2, // Split panel into two columns
                RowCount = 1,
                AutoSize = false
            };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F)); // Left half for data
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F)); // Right half for image

            // Left Panel for User and Model Data
            TableLayoutPanel dataLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1, // One column for stacking sections
                RowCount = 2,    // Two rows: user data and model data
                AutoSize = false
            };
            dataLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F)); // Top half for user data
            dataLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F)); // Bottom half for model data

            // User-provided data section
            Panel userPanel = new Panel
            {
                AutoSize = false,
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };
            FlowLayoutPanel userFlowLayout = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                Dock = DockStyle.Fill,
                Padding = new Padding(0),       // Remove padding and margins
                Margin = new Padding(0)
            };
            Label titleLabel = CreatePanelTitle("User Provided Data:");
            titleLabel.TextAlign = ContentAlignment.MiddleLeft;
            titleLabel.Dock = DockStyle.Top;
            userFlowLayout.Controls.Add(titleLabel);
            userFlowLayout.Controls.Add(CreateInfoLabel("Upload Name:", uploadInfo.UploadName));
            userFlowLayout.Controls.Add(CreateInfoLabel("Date of Sample:", uploadInfo.SampleDate.ToString("MM/dd/yyyy")));
            userFlowLayout.Controls.Add(CreateInfoLabel("Sample Location:", uploadInfo.SampleLocation));
            userFlowLayout.Controls.Add(CreateInfoLabel("Sheep Breed:", uploadInfo.SheepBreed));
            userFlowLayout.Controls.Add(CreateInfoLabel("Comments:", uploadInfo.Comments));
            userPanel.Controls.Add(userFlowLayout);

            // Model-generated data section
            Panel modelPanel = new Panel
            {
                AutoSize = false,
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };
            FlowLayoutPanel modelFlowLayout = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                Dock = DockStyle.Fill
            };
            modelFlowLayout.Controls.Add(CreatePanelTitle("Model Generated Data:"));
            modelFlowLayout.Controls.Add(CreateInfoLabel("Nale (%):", mlData.nalePercentage));
            modelFlowLayout.Controls.Add(CreateInfoLabel("Qufu (%):", mlData.qufuPercentage));
            modelFlowLayout.Controls.Add(CreateInfoLabel("Erci (%):", mlData.erciPercentage));
            modelFlowLayout.Controls.Add(CreateInfoLabel("Bubbles (%):", mlData.bubblePercentage));
            modelFlowLayout.Controls.Add(CreateInfoLabel("Qufu Stem (%):", mlData.qufustemPercentage));
            modelPanel.Controls.Add(modelFlowLayout);

            // Add user and model panels to the data layout
            dataLayout.Controls.Add(userPanel, 0, 0);
            dataLayout.Controls.Add(modelPanel, 0, 1);

            // Right Panel for Image
            PictureBox uploadImage = new PictureBox
            {
                Image = uploadInfo.ImageFile ?? new Bitmap(250, 250), // Placeholder if image is null
                SizeMode = PictureBoxSizeMode.Zoom,
                Dock = DockStyle.Fill,
                Margin = new Padding(10)
            };

            // Add data layout and image to the main layout
            mainLayout.Controls.Add(dataLayout, 0, 0);
            mainLayout.Controls.Add(uploadImage, 1, 0);

            // Add the main layout to the upload panel
            uploadPanel.Controls.Add(mainLayout);

            // Add the upload panel to the parent flow layout
            flowLayoutPanel.Controls.Add(uploadPanel);
        }

        // Helper method to create a label with title and value for display in the panel
        private Label CreateInfoLabel(string title, string value)
        {
            return new Label
            {
                Text = $"{title} {value}",
                Font = new Font("Times New Roman", 10, FontStyle.Regular),
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
                Font = new Font("Times New Roman", 10, FontStyle.Bold),
                AutoSize = true,
                //Margin = new Padding(3, 10, 3, 5),
                Anchor = AnchorStyles.Top
            };
        }


    }
}
