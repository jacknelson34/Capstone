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
using System.Data.Common;
using System.Runtime.CompilerServices;
using OpenCvSharp.Flann;

namespace GrazeViewV1
{
    public partial class DataLibraryExpandedView : Form
    {
        private MainPage _mainPage;                 // Hold reference for mainPage for dataLibrary
        private Bitmap memoryImage;                 // For printing
        private bool IsNavigating;                  // Boolean to determine if the user is navigating the application
        private readonly DBConnections _dbConnections;
        private readonly DBQueries _dbQueries;
        private DBSettings _dbSettings;
        private readonly List<int> _selectedIndexes;
        private List<Bitmap> panelBitmaps;
        private int currentPrintIndex = 0;


        public DataLibraryExpandedView(MainPage mainPage, DBQueries dbQueries, List<int> selectedIndexes)
        {
            IsNavigating = false;           // Initially, set navigating to false, unless found otherwise

            _dbQueries = dbQueries ?? throw new ArgumentNullException(nameof(dbQueries));
            _selectedIndexes = selectedIndexes ?? new List<int>();


            _dbSettings = new DBSettings(
                driver: "ODBC Driver 18 for SQL Server",
                server: "sqldatabase404.database.windows.net",
                database: "404ImageDBsql",
                username: "sql404admin",
                password: "sheepstool404()",
                connectionOptions: "MultipleActiveResultSets=True;");

            _dbConnections = new DBConnections(_dbSettings);

            _mainPage = mainPage;           // Hold reference to mainPage
            InitializeComponent();          // Build DLExpanded view
            this.Text = "GrazeView";        // Add header "GRAZEVIEW" 

            if (ConsistentForm.IsFullScreen)    // Check to see if previous page was fullscreen
            {
                SetFullScreen();                // If so, set this page to fullscreen
            }

            // Debugging
            /*for(int i=0; i < selectedIndexes.Count; i++)
            {
                MessageBox.Show("Index: " + selectedIndexes[i]);
            }
            */

            // Event handler for close
            this.FormClosing += DLExpanded_XOut;        // Add event handler for user exiting the app

            // Load selected data from database
            //LoadSelectedData();
        }

        public async Task PreparePanelsAsync()
        {
            try
            {
                if (_selectedIndexes == null || !_selectedIndexes.Any())
                {
                    MessageBox.Show("No data selected for expansion.", "Debug", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                // Use a list to store the tasks for parallel processing
                List<Task<Panel>> panelTasks = new List<Task<Panel>>();

                foreach (int index in _selectedIndexes)
                {
                    // Load data in the background
                    panelTasks.Add(Task.Run(() => CreateUploadPanel(index)));
                }

                // Wait for all panels to be generated before showing the form
                var panels = await Task.WhenAll(panelTasks);

                // Add them to the UI in one go (improves performance)
                flowLayoutPanel.SuspendLayout();
                flowLayoutPanel.Width = 1200;
                flowLayoutPanel.AutoScroll = true;
                foreach (var panel in panels)
                {
                    if (panel != null)
                        flowLayoutPanel.Controls.Add(panel);
                }
                flowLayoutPanel.ResumeLayout();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading selected data: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private string FormatPercentage(object value)
        {
            if (value == null) return "0.00%";
            if (float.TryParse(value.ToString(), out float percentage))
            {
                return percentage.ToString("0.00") + "%";
            }
            return "0.00%";
        }

        private void DLExpanded_XOut(object sender, FormClosingEventArgs e)
        {
            if (IsNavigating) return;
            if (e.CloseReason == CloseReason.UserClosing)
            {
                _mainPage.Close();
            }
        }

        // Method for setting the page to full screen
        private void SetFullScreen()
        {
            this.WindowState = FormWindowState.Maximized;       // Maximize the form
            this.FormBorderStyle = FormBorderStyle.Sizable;     // Allow form to still be sized
            this.Bounds = Screen.PrimaryScreen.Bounds;          // Set bounds to max
        }


        private async Task<Panel> CreateUploadPanel(int index)
        {

            var row = _dbQueries.GetRowByIndexAsync(index).Result ?? new Dictionary<string, object>();

            if (_dbQueries == null)
            {
                MessageBox.Show("Database connection is not initialized.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            var (originalImage, heatmapImage) = await _dbQueries.RetrieveImagesFromDB(index);

            UploadInfo uploadInfo = new UploadInfo
            {
                UploadName = row.ContainsKey("SourceFile") && row["SourceFile"] != null ? row["SourceFile"].ToString() : "N/A",
                SampleDate = row.ContainsKey("DateSampleTaken") && row["DateSampleTaken"] != null ? row["DateSampleTaken"].ToString() : "N/A",
                SampleLocation = row.ContainsKey("SampleLocation") && row["SampleLocation"] != null ? row["SampleLocation"].ToString() : "N/A",
                SheepBreed = row.ContainsKey("SheepBreed") && row["SheepBreed"] != null ? row["SheepBreed"].ToString() : "N/A",
                Comments = row.ContainsKey("Comments") && row["Comments"] != null ? row["Comments"].ToString() : "N/A",
                ImageFile = originalImage ?? new Bitmap(250, 250),
                HeatMap = heatmapImage ?? new Bitmap(250, 250)
            };


            MLData mlData = new MLData
            {
                qufuPercentage = row.ContainsKey("QufuPercent") ? FormatPercentage(row["QufuPercent"]) : "0.00%",
                nalePercentage = row.ContainsKey("NalePercent") ? FormatPercentage(row["NalePercent"]) : "0.00%",
                erciPercentage = row.ContainsKey("ErciPercent") ? FormatPercentage(row["ErciPercent"]) : "0.00%",
                bubblePercentage = row.ContainsKey("AirBubblePercent") ? FormatPercentage(row["AirBubblePercent"]) : "0.00%"
            };

            // debugging
            //MessageBox.Show($"Adding panel for: {uploadInfo.UploadName}", "Debug", MessageBoxButtons.OK, MessageBoxIcon.Information);


            return GeneratePanel(uploadInfo, mlData);
        }

        private Panel GeneratePanel(UploadInfo uploadInfo, MLData mlData)
        {
            Panel uploadPanel = new Panel
            {
                BorderStyle = BorderStyle.Fixed3D,
                Padding = new Padding(10),
                Margin = new Padding(10),
                Width = 800, // Keep width fixed
                Height = 400,
                AutoSize = true, // Automatically fit content
                AutoSizeMode = AutoSizeMode.GrowAndShrink // Prevent unnecessary extra space
            };

            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2, // Two columns: Left (Info) & Right (Image)
                AutoSize = true, // Let it grow dynamically
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };

            // Ensure columns take equal space
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F)); // Left: Info
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F)); // Right: Image

            // Create Info Panel (Stacked UploadInfo + MLData)
            FlowLayoutPanel infoPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true,
                Dock = DockStyle.Fill
            };

            // Add Upload Info (Stacked)
            infoPanel.Controls.Add(new Label { Text = "Upload Information", AutoSize = true, Font = new Font("Times New Roman", 12, FontStyle.Bold) });
            infoPanel.Controls.Add(CreateInfoLabel("Upload Name:", uploadInfo.UploadName));
            infoPanel.Controls.Add(CreateInfoLabel("Date of Sample:", uploadInfo.SampleDate));
            infoPanel.Controls.Add(CreateInfoLabel("Sample Location:", uploadInfo.SampleLocation));
            infoPanel.Controls.Add(CreateInfoLabel("Sheep Breed:", uploadInfo.SheepBreed));
            infoPanel.Controls.Add(CreateInfoLabel("Comments:", uploadInfo.Comments));

            // Add spacing
            infoPanel.Controls.Add(new Label { Text = " ", AutoSize = true });
            infoPanel.Controls.Add(new Label { Text = " ", AutoSize = true });
            infoPanel.Controls.Add(new Label { Text = "Percentages", AutoSize = true, Font = new Font("Times New Roman", 12, FontStyle.Bold) });

            // Add ML Data (Stacked)
            infoPanel.Controls.Add(CreateInfoLabel("Qufu (%):", mlData.qufuPercentage));    // These are flipped for a reason
            infoPanel.Controls.Add(CreateInfoLabel("Nale (%):", mlData.nalePercentage));
            infoPanel.Controls.Add(CreateInfoLabel("Erci (%):", mlData.erciPercentage));
            infoPanel.Controls.Add(CreateInfoLabel("Bubbles (%):", mlData.bubblePercentage));

            // Create Image Panel (Centered)
            FlowLayoutPanel imagePanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                AutoSize = true,
                Dock = DockStyle.Fill,
                WrapContents = false
            };

            // Original image box
            PictureBox uploadImage = new PictureBox
            {
                Image = uploadInfo.ImageFile,
                SizeMode = PictureBoxSizeMode.Zoom,
                Width = 275,
                Height = 275,
                Margin = new Padding(10)
            };

            // Heatmap image box
            PictureBox heatmapImage = new PictureBox
            {
                Image = uploadInfo.HeatMap,
                SizeMode = PictureBoxSizeMode.Zoom,
                Width = 275,
                Height = 275,
                Margin = new Padding(10)
            };

            // Add both images side by side
            imagePanel.Controls.Add(uploadImage);
            imagePanel.Controls.Add(heatmapImage);

            // Add panels to main layout
            mainLayout.Controls.Add(infoPanel, 0, 0);
            mainLayout.Controls.Add(imagePanel, 1, 0);

            // Add layout to the upload panel
            uploadPanel.Controls.Add(mainLayout);

            return uploadPanel;
        }



        // Event handler for exitButton
        private void exitButton_Click(object sender, EventArgs e)
        {
            // If the user clicks the exit button, we know they are still using the application
            IsNavigating = true;

            if (this.WindowState == FormWindowState.Maximized)      // Check current form for maximization or not
            {
                ConsistentForm.IsFullScreen = true;                 // If true, make sure it is passed to the next page
            }
            else                                                    // If false, also pass this information to the next page
            {
                ConsistentForm.IsFullScreen = false;
            }

            var dbQueries = new DBQueries(_dbConnections.ConnectionString); // Ensure connectionString is correct
            var dataLibrary = new DataLibrary(_mainPage, dbQueries);           // Initialize a new dataLibrary with reference to mainPage
            dataLibrary.Size = this.Size;                           // Set the dataLibrary equal to this page's size
            dataLibrary.Location = this.Location;                   // and location
            dataLibrary.Show();                                     // Show the new dataLibrary
            this.Close();                                           // Close this view and return to DataLibrary
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

        // Event handler for printButton
        private void printButton_Click(object sender, EventArgs e) // Event handler for the print button click
        {
            CaptureScreen();                                       // Call a method to capture the current screen (implementation not shown)
            PrintDocument printScreen = new PrintDocument();       // Create a new PrintDocument instance
            printScreen.PrintPage += new PrintPageEventHandler(printScreen_PrintPage); // Attach the PrintPage event handler for printing

            // Show Print Dialog
            PrintDialog printDialog = new PrintDialog              // Create a PrintDialog instance
            {
                Document = printScreen                             // Assign the PrintDocument to the dialog
            };

            // Print if confirmed
            if (printDialog.ShowDialog() == DialogResult.OK)        // Check if the user confirms the print action
            {
                printScreen.Print();                               // Print the document if confirmed
            }
        }

        // Method to capture the screen for printing
        private void CaptureScreen()
        {
            panelBitmaps = new List<Bitmap>();

            foreach (Control control in flowLayoutPanel.Controls)
            {
                Bitmap panelImage = new Bitmap(control.Width, control.Height);
                control.DrawToBitmap(panelImage, new Rectangle(0, 0, control.Width, control.Height));
                panelBitmaps.Add(panelImage);
            }

            currentPrintIndex = 0; // reset for new print job
        }



        // Event handler for PrintPage
        private void printScreen_PrintPage(object sender, PrintPageEventArgs e)
        {
            int panelsPerPage = 3;
            int yOffset = e.MarginBounds.Top - 40; // Shift up a bit
            int printedCount = 0;

            // Reduce margins to make better use of space
            Rectangle tightBounds = new Rectangle(
                e.MarginBounds.Left - 30,   // move further left
                e.MarginBounds.Top - 20,    // move higher up
                e.MarginBounds.Width + 60,  // allow wider content
                e.MarginBounds.Height + 40  // allow more height
            );

            // Set up fonts
            Font titleFont = new Font("Times New Roman", 14, FontStyle.Bold);
            Font subFont = new Font("Times New Roman", 10, FontStyle.Italic);

            // Title
            string headerText = "GrazeView AI – Texas A&M AgriLife";
            SizeF headerSize = e.Graphics.MeasureString(headerText, titleFont);
            float xHeader = tightBounds.Left + (tightBounds.Width - headerSize.Width) / 2;
            float yHeader = yOffset;
            e.Graphics.DrawString(headerText, titleFont, Brushes.Black, xHeader, yHeader);
            yOffset += (int)headerSize.Height + 5;

            // Page number
            int currentPageNumber = (currentPrintIndex / panelsPerPage) + 1;
            string pageText = $"Page {currentPageNumber}";
            SizeF pageSize = e.Graphics.MeasureString(pageText, subFont);
            float xPage = tightBounds.Left + (tightBounds.Width - pageSize.Width) / 2;
            e.Graphics.DrawString(pageText, subFont, Brushes.Black, xPage, yOffset);
            yOffset += (int)pageSize.Height + 3;

            // Timestamp
            string timestamp = "Printed: " + DateTime.Now.ToString("MMMM d, yyyy h:mm tt");
            SizeF timeSize = e.Graphics.MeasureString(timestamp, subFont);
            float xTime = tightBounds.Left + (tightBounds.Width - timeSize.Width) / 2;
            e.Graphics.DrawString(timestamp, subFont, Brushes.Black, xTime, yOffset);
            yOffset += (int)timeSize.Height + 10;


            while (currentPrintIndex < panelBitmaps.Count && printedCount < panelsPerPage)
            {
                Bitmap bmp = panelBitmaps[currentPrintIndex];

                float scale = Math.Min(
                    (float)tightBounds.Width / bmp.Width,
                    (float)(tightBounds.Height / panelsPerPage) / bmp.Height);

                int scaledWidth = (int)(bmp.Width * scale);
                int scaledHeight = (int)(bmp.Height * scale);

                int xCentered = tightBounds.Left + (tightBounds.Width - scaledWidth) / 2;

                e.Graphics.DrawImage(bmp, xCentered, yOffset, scaledWidth, scaledHeight);
                yOffset += scaledHeight + 20; // slightly less vertical space

                currentPrintIndex++;
                printedCount++;
            }

            e.HasMorePages = currentPrintIndex < panelBitmaps.Count;
        }





        // Helper method to create a label with title and value for display in the panel
        private Label CreateInfoLabel(string title, string value) // Method to create a Label displaying a title and value
        {
            return new Label
            {
                Text = $"{title} {value}",                          // Set the label text to combine the title and value
                Font = new Font("Times New Roman", 12, FontStyle.Regular), // Use Times New Roman, size 10, with regular style
                AutoSize = true,                                    // Allow the label to adjust its size based on the content
                Margin = new Padding(3)                            // Add a 3-pixel margin around the label
            };
        }


    }
}
