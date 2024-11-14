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

        public DataLibraryExpandedView(MainPage mainPage)
        {
            _mainPage = mainPage;
            InitializeComponent();
            this.Size = ConsistentForm.FormSize;
            this.Location = ConsistentForm.FormLocation;
            if (ConsistentForm.IsFullScreen)
            {
                SetFullScreen();
            }
        }

        // Event handler for exitButton
        private void exitButton_Click(object sender, EventArgs e)
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
            // Main panel for user information and thumbnail
            Panel uploadPanel = new Panel
            {
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10),
                Margin = new Padding(10),
                Width = 600,
                Height = 450,
                Anchor = AnchorStyles.Top
            };

            // TableLayoutPanel to structure userPanel, modelPanel, and thumbnail
            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 2,
                AutoSize = true
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 150)); // Reserve space for thumbnail

            // User-provided data panel
            Panel userPanel = new Panel
            {
                AutoSize = true,
                AutoScroll = true
            };
            FlowLayoutPanel userFlowLayout = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                Dock = DockStyle.Fill,
                Padding = new Padding(30, 0, 0, 0)
            };
            // Add title label with left alignment
            Label titleLabel = CreatePanelTitle("User Provided Data:");
            titleLabel.TextAlign = ContentAlignment.MiddleLeft;  // Align text to the left
            userFlowLayout.Controls.Add(titleLabel);

            // Add data labels with consistent left alignment
            userFlowLayout.Controls.Add(CreateInfoLabel("Upload Name:", uploadInfo.UploadName));
            userFlowLayout.Controls.Add(CreateInfoLabel("Date of Sample:", uploadInfo.SampleDate.ToString("MM/dd/yyyy")));
            userFlowLayout.Controls.Add(CreateInfoLabel("Sample Location:", uploadInfo.SampleLocation));
            userFlowLayout.Controls.Add(CreateInfoLabel("Sheep Breed:", uploadInfo.SheepBreed));
            userFlowLayout.Controls.Add(CreateInfoLabel("Comments:", uploadInfo.Comments));
            userPanel.Controls.Add(userFlowLayout);

            // Model-generated data panel
            Panel modelPanel = new Panel
            {
                AutoSize = true,
                AutoScroll = true
            };
            FlowLayoutPanel modelFlowLayout = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                Dock = DockStyle.Fill
            };
            Label modelTitleLabel = CreatePanelTitle("Model Generated Data:");
            modelTitleLabel.TextAlign = ContentAlignment.MiddleCenter;
            modelFlowLayout.Controls.Add(modelTitleLabel);

            // Add data labels with consistent alignment
            modelFlowLayout.Controls.Add(CreateInfoLabel("Nale (%):", mlData.nalePercentage));
            modelFlowLayout.Controls.Add(CreateInfoLabel("Qufu (%):", mlData.qufuPercentage));
            modelFlowLayout.Controls.Add(CreateInfoLabel("Erci (%):", mlData.erciPercentage));
            modelFlowLayout.Controls.Add(CreateInfoLabel("Bubbles (%):", mlData.bubblePercentage));
            modelFlowLayout.Controls.Add(CreateInfoLabel("Qufu Stem (%):", mlData.qufustemPercentage));
            modelPanel.Controls.Add(modelFlowLayout);

            // Add panels to the layout
            layout.Controls.Add(userPanel, 0, 0);
            layout.Controls.Add(modelPanel, 1, 0);

            // Add thumbnail image below the panels if it exists
            if (uploadInfo.ThumbNail != null)
            {
                PictureBox uploadImage = new PictureBox
                {
                    Image = uploadInfo.ThumbNail,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Width = 250,
                    Height = 250,
                    Anchor = AnchorStyles.None,  // Center the image in the layout
                    Margin = new Padding(0, 10, 0, 0) // Add margin above
                };
                layout.SetColumnSpan(uploadImage, 2);  // Span the image across both columns
                layout.Controls.Add(uploadImage, 0, 1);
            }

            // Add the layout to the main upload panel
            uploadPanel.Controls.Add(layout);

            // Add the main upload panel to the flow layout
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
                Margin = new Padding(3, 10, 3, 5),
                Anchor = AnchorStyles.Top
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
