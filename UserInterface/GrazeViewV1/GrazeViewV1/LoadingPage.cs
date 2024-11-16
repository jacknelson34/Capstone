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
    public partial class LoadingPage: Form
    {
        private PictureBox uploadedSlide;
        private ProgressBar loadingBar;
        private Image resultsImage;
        private TextBox statusUpdates;

        // Hold instances of other pages
        private MainPage _mainPage;
        private DataUpload _dataUpload;

        public LoadingPage(Image uploadedImage)
        {
            InitializeComponent();
            this.Size = ConsistentForm.FormSize;
            this.Location = ConsistentForm.FormLocation;
            if (ConsistentForm.IsFullScreen)
            {
                SetFullScreen();
            }

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
            statusUpdates.BackColor = Color.LightBlue;
            statusUpdates.Text = "Uploading Image...";
            statusUpdates.Font = new Font("Times New Roman", 12, FontStyle.Italic);
            statusUpdates.Size = new Size(250, 20);
            statusUpdates.BorderStyle = BorderStyle.None;
            statusUpdates.ReadOnly = true;
            statusUpdates.TabStop = false;
            statusUpdates.Location = new Point(
                loadingBar.Location.X,
                (this.ClientSize.Height / 2) - 30);
            this.Controls.Add(statusUpdates);



            // ----------------------- DEMO ONLY -----------------------

            // Demo for randomized grass percentages
            // Initialize random number generator
            Random random = new Random();

            // Create array to store the 5 numbers (one for each grass type + air bubble)
            double[] randomNumbers = new double[5];

            // Generate 5 random numbers
            for(int i = 0; i <randomNumbers.Length; i++)
            {
                randomNumbers[i] = random.NextDouble();
            }

            // Sum them all up
            double randomTotal = 0;
            foreach(var num in randomNumbers)
            {
                randomTotal += num;
            }

            // Create array for the percentages to be used (one for each grass type + air bubble)
            double[] percentages = new double[5];

            // Normalize these so that they add up to 100%
            for (int i = 0; i < percentages.Length; i++) 
            {
                percentages[i] = (randomNumbers[i] / randomTotal) * 100;
            }

            // Set each grass type percentage by accessing the properties of the object
            MLData mlData = new MLData
            {
                nalePercentage = percentages[0].ToString("0.00") + "%",
                qufuPercentage = percentages[1].ToString("0.00") + "%",
                erciPercentage = percentages[2].ToString("0.00") + "%",
                bubblePercentage = percentages[3].ToString("0.00") + "%",
                qufustemPercentage = percentages[4].ToString("0.00") + "%"
            };

            // Add random percentages to the global list
            GlobalData.machineLearningData.Add(mlData);


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
            ConsistentForm.FormSize = this.Size;
            ConsistentForm.FormLocation = this.Location;
            if (this.WindowState == FormWindowState.Maximized)
            {
                ConsistentForm.IsFullScreen = true;
            }
            else
            {
                ConsistentForm.IsFullScreen = false;
            }


            ResultPage resultsPage = new ResultPage(resultsImage, _mainPage, _dataUpload);      // Initialize new page
            resultsPage.Show();                                                                 // Show new page
            this.Close();                                                                       // Hide current page
        }

        private void SetFullScreen()     // Class to handle screen maximization
        {
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Bounds = Screen.PrimaryScreen.Bounds;
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
