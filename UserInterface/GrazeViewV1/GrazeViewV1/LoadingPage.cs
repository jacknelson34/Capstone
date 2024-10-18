using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrazeViewV1
{
    public partial class LoadingPage: ConsistentForm
    {
        private PictureBox uploadedSlide;
        private ProgressBar loadingBar;
        private Image resultsImage;
        private TextBox statusUpdates;

        public LoadingPage(Image uploadedImage)
        {
            InitializeComponent();

            // Initialize Form Properties
            this.Text = "Loading Page";
            //this.Size = previousPageSize;    // Create page at same size as previous page
            // this.Location = previousPageLocation;

            // Initialize PictureBox Properties
            uploadedSlide = new PictureBox();
            if (uploadedSlide != null)  // Check for null
            {
                uploadedSlide.Image = uploadedImage;
                uploadedSlide.SizeMode = PictureBoxSizeMode.Zoom;  // Stretch image to fit page
                uploadedSlide.Size = new Size(200, 100);
                uploadedSlide.Location = new Point(                        // Image positioning
                    (this.ClientSize.Width / 2) - (uploadedSlide.Width / 2),
                    (this.ClientSize.Height / 6));
                this.Controls.Add(uploadedSlide);                          // Add image box
            }
            else  // Output message if null
            {
                MessageBox.Show("The Image is not initialized.");
            }

            // Initialize ProgressBar Properties

            // Create loading bar progress
            // For now, will work on a timer,
            // Once connected to ML, will provide accurate updates
            loadingBar = new ProgressBar();  
            loadingBar.Maximum = 60;                                      // Give loadingBar a max of 30 ticks
            loadingBar.Step = 1;                                           // 1 tick per step
            loadingBar.Size = new Size(600, 20);                           // Size of loadingBar
            loadingBar.Location = new Point(
                (this.ClientSize.Width / 2) - (loadingBar.Width / 2),
                (this.ClientSize.Height / 2));                             // Location for loadingBar
            this.Controls.Add(loadingBar);                                 // Add loadingBar control to page

            // Initialize Text Box with status updates
            statusUpdates = new TextBox();
            statusUpdates.Text = "Uploading Image...";
            statusUpdates.Font = new Font("Times New Roman", 12, FontStyle.Italic);
            statusUpdates.Size = new Size(250, 20);
            statusUpdates.BorderStyle = BorderStyle.None;
            statusUpdates.ReadOnly = true;
            statusUpdates.TabStop = false;
            statusUpdates.Location = new Point(
                loadingBar.Location.X,
                (this.ClientSize.Height / 2) - 20);
            this.Controls.Add(statusUpdates);



            // ----------------------- DEMO ONLY -----------------------

            // Simulation for progress (will be replaced once connected to ML)
            // Timer for demo file
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();       // Initialize Timer
            timer.Interval = 100;                                           // 100ms per cycle for timer
            timer.Tick += (s, e) =>                                         // Tick cycle for timer
            {
                // Demo Text Outputs
                if(loadingBar.Value == 10)
                {
                    statusUpdates.Text = "Splicing Image...";
                    statusUpdates.Refresh();
                }
                else if (loadingBar.Value == 20)
                {
                    statusUpdates.Text = "Analyzing Image Segments...";
                    statusUpdates.Refresh();
                }
                else if (loadingBar.Value == 30)
                {
                    statusUpdates.Text = "Comparing to known data...";
                    statusUpdates.Refresh();
                }
                else if (loadingBar.Value == 40)
                {
                    statusUpdates.Text = "Recombining Image Segments...";
                    statusUpdates.Refresh();
                }
                else if (loadingBar.Value == 50)
                {
                    statusUpdates.Text = "Generating Output...";
                    statusUpdates.Refresh();
                }

                // Demo Timer Steps
                if (loadingBar.Value < loadingBar.Maximum)                   // Check for 
                {
                    loadingBar.PerformStep();
                }
                else
                {
                    resultsImage = uploadedImage;                          // Placeholder image for demo
                    timer.Stop();                                          // End timer if = 30
                    nextPage();                                            // Call method to go to results page
                }
            };
            timer.Start();

            // ----------------------- DEMO ONLY -----------------------


        }

        private void nextPage()                         // Method for going to resultsPage
        {
            ResultPage resultsPage = new ResultPage(resultsImage);      // Initialize new page
            resultsPage.Show();                                         // Show new page
            this.Hide();                                                // Hide current page
        }

        private void CenterControls()
        {
            // Adjust Image to Center
            uploadedSlide.Location = new Point(                        // Image positioning
                    (this.ClientSize.Width / 2) - (uploadedSlide.Width / 2),
                    (this.ClientSize.Height / 6));

            // Adjust LoadingBar to Center
            loadingBar.Location = new Point(
                (this.ClientSize.Width / 2) - (loadingBar.Width / 2),
                (this.ClientSize.Height / 2));

            // Adjust text box to stick with loadingBar
            statusUpdates.Location = new Point(
                (this.ClientSize.Width / 2) - (statusUpdates.Width / 2),
                loadingBar.Height - 10);
        }

        private void loadingPage_Resize(object? sender, EventArgs e)    // Method for handling page resizing
        {
            CenterControls();  // Call method for centering page elements
        }
    }
}
