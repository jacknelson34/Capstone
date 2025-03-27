using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrazeViewV1
{
    public partial class LoadingPage: Form
    {
        private PictureBox uploadedSlide;       // Initialize picturebox for the uploaded image
        private ProgressBar loadingBar;         // Initialize the loading bar
        private Image resultsImage;             // Initialize variable for the output image
        private TextBox statusUpdates;          // Initialize the textbox used to hold status updates from ML
        private TextBox percentageLoading;
        private PictureBox sheep;
        private System.Windows.Forms.Timer sheepTimer;
        private int sheepSpeed;

        // Hold instances of other pages
        private MainPage _mainPage;             // Initialize held instance of mainPage
        private bool IsNavigating;              // Initialize boolean to determine if user is navigating pages or exiting

        public LoadingPage(Image uploadedImage, MainPage mainPage)
        {
            _mainPage = mainPage;       // Hold instance of mainpage in _mainPage

            InitializeComponent();      // Build the page

            resultsImage = uploadedImage;

            // Initialize Form Properties
            this.Text = "GrazeView";

            // Initialize PictureBox Properties
            uploadedSlide = new PictureBox();
            if (uploadedSlide != null)  // Check for null
            {
                uploadedSlide.Image = uploadedImage;
                uploadedSlide.SizeMode = PictureBoxSizeMode.Zoom;           // Stretch image to fit page
                uploadedSlide.Size = new Size(200, 100);                    // Set picturebox size
                uploadedSlide.Location = new Point(                         // Image positioning
                    (this.ClientSize.Width / 2) - (uploadedSlide.Width / 2),
                    (this.ClientSize.Height / 6));
                this.Controls.Add(uploadedSlide);                           // Add image box
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
            //loadingBar.Maximum = 60;                                                // Give loadingBar a max of 30 ticks
            loadingBar.Step = 1;                                                    // 1 tick per step
            loadingBar.Size = new Size(600, 20);                                    // Size of loadingBar
            loadingBar.Location = new Point(
                (this.ClientSize.Width / 2) - (loadingBar.Width / 2),
                (this.ClientSize.Height / 2));                                      // Location for loadingBar
            this.Controls.Add(loadingBar);                                          // Add loadingBar control to page

            // Sheep picturebox
            sheep = new PictureBox();
            sheep.Image = Properties.Resources.sheepGif;
            sheep.SizeMode = PictureBoxSizeMode.StretchImage;
            sheep.Name = "helpButton";
            sheep.BackColor = Color.Transparent;
            sheep.Size = new Size(50, 50);
            sheep.Location = new Point((this.ClientSize.Width) - 100,
                    (this.ClientSize.Height) - 60);
            sheep.TabIndex = 4;
            sheep.TabStop = false;
            this.Controls.Add(sheep);

            sheepSpeed = 1;

            sheepTimer = new System.Windows.Forms.Timer();
            sheepTimer.Interval = 20; // milliseconds - smaller = smoother
            sheepTimer.Tick += SheepTimer_Tick;
            sheepTimer.Start();



            // Initialize Text Box with status updates
            statusUpdates = new TextBox();                                          // Initialize a new TextBox instance to display status updates
            statusUpdates.BackColor = Color.FromArgb(116, 231, 247);                              // Set the background color of the TextBox to light blue
            statusUpdates.Text = "Uploading Image to ML...";                              // Set the initial text to indicate an image upload is in progress
            statusUpdates.Font = new Font("Times New Roman", 12, FontStyle.Italic); // Set the font to Times New Roman, size 12, italic style
            statusUpdates.Size = new Size(250, 20);                                 // Set the size of the TextBox to 250x20 pixels
            statusUpdates.BorderStyle = BorderStyle.None;                           // Remove the border to give the TextBox a clean appearance
            statusUpdates.ReadOnly = true;                                          // Make the TextBox read-only so the user cannot edit its content
            statusUpdates.TabStop = false;                                          // Exclude the TextBox from the tab order
            statusUpdates.Location = new Point(
                loadingBar.Location.X,                                              // Horizontally align with the loading bar
                (this.ClientSize.Height / 2) - 20);                                 // Position 30 pixels above the vertical center of the form
            this.Controls.Add(statusUpdates);                                       // Add the TextBox to the form's controls to make it visible

            // Initialize Text Box with percentage updates
            percentageLoading = new TextBox();
            percentageLoading.BackColor = Color.FromArgb(116, 231, 247);                              // Set the background color of the TextBox to light blue
            percentageLoading.Font = new Font("Times New Roman", 12, FontStyle.Italic); // Set the font to Times New Roman, size 12, italic style
            percentageLoading.Size = new Size(250, 20);
            percentageLoading.TextAlign = HorizontalAlignment.Right;
            percentageLoading.Text = ("0%");
            percentageLoading.BorderStyle = BorderStyle.None;
            percentageLoading.ReadOnly = true;
            percentageLoading.TabStop = false;
            percentageLoading.Location = new Point(
                loadingBar.Location.X + 350,                                              // Horizontally align with the loading bar
                (this.ClientSize.Height / 2) - 20);
            this.Controls.Add(percentageLoading);
            
            /*--------------------------------------DEMO ONLY : NOT IN USE---------------------------------------*/
            /*(
            // Demo for randomized grass percentages
            // Initialize random number generator
            /*Random random = new Random();

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
                // qufustemPercentage = percentages[4].ToString("0.00") + "%"
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
                if(loadingBar.Value == 10)            // Update text when timer hits 10 seconds, ensure font stays consistent
                {
                    statusUpdates.Text = "Splicing Image...";
                    statusUpdates.Font = new Font("Times New Roman", 12, FontStyle.Italic);
                    statusUpdates.Refresh();
                }
                else if (loadingBar.Value == 20)      // Update text when timer hits 20 seconds, ensure font stays consistent
                {
                    statusUpdates.Text = "Analyzing Image Segments...";
                    statusUpdates.Font = new Font("Times New Roman", 12, FontStyle.Italic);
                    statusUpdates.Refresh();
                }
                else if (loadingBar.Value == 30)      // Update text when timer hits 30 seconds, ensure font stays consistent
                {
                    statusUpdates.Text = "Comparing to known data...";
                    statusUpdates.Font = new Font("Times New Roman", 12, FontStyle.Italic);
                    statusUpdates.Refresh();
                }
                else if (loadingBar.Value == 40)      // Update text when timer hits 40 seconds, ensure font stays consistent
                {
                    statusUpdates.Text = "Recombining Image Segments...";
                    statusUpdates.Font = new Font("Times New Roman", 12, FontStyle.Italic);
                    statusUpdates.Refresh();
                }
                else if (loadingBar.Value == 50)      // Update text when timer hits 50 seconds, ensure font stays consistent
                {
                    statusUpdates.Text = "Generating Output...";
                    statusUpdates.Font = new Font("Times New Roman", 12, FontStyle.Italic);
                    statusUpdates.Refresh();
                }

                // Demo Timer Steps
                if (loadingBar.Value < loadingBar.Maximum)                 // Check if loadingbar has reached the maximum
                {
                    loadingBar.PerformStep();                              // If not, make next step and continue loading
                }
                else
                {
                    resultsImage = uploadedImage;                          // Placeholder image for demo
                    timer.Stop();                                          // End timer if = 60
                    nextPage();                                            // Call method to go to results page
                }
            };
            timer.Start();                                                 // Start timer, with all information above
            

            ) */

            // Event handler for Form Close
            this.FormClosing += LoadingPage_Xout;

        }

        private void SheepTimer_Tick(object sender, EventArgs e)
        {
            sheep.Left -= sheepSpeed;

            // When it goes off the left side, reset to the right
            if (sheep.Right < 0)
            {
                sheep.Left = this.ClientSize.Width;
            }
        }

        // Method to set progress bar max
        public void SetProgressBarMax(int maxTiles)
        {
            if (loadingBar.InvokeRequired)
            {
                loadingBar.Invoke(new Action(() => loadingBar.Maximum = maxTiles));
            }
            else
            {
                loadingBar.Maximum = maxTiles;
            }
        }

        // Method to update the progress bar while processing tiles
        public void UpdateProgress(int progress, string status, string percentage)
        {
            if (loadingBar.InvokeRequired)
            {
                loadingBar.Invoke(new Action(() =>
                {
                    loadingBar.Value = progress;
                    statusUpdates.Text = status;
                    percentageLoading.Text = percentage;
                    statusUpdates.Refresh();
                }));
            }
            else
            {
                loadingBar.Value = progress;
                statusUpdates.Text = status;
                percentageLoading.Text = percentage;
                statusUpdates.Refresh();
            }

        }

        // Method to indicate completion
        public void CompleteProgress(string message)
        {
            if (loadingBar.InvokeRequired)
            {
                loadingBar.Invoke(new Action(() =>
                {
                    loadingBar.Value = loadingBar.Maximum;
                    statusUpdates.Text = message;
                    //MessageBox.Show("ML Processing Complete!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    nextPage();
                }));
            }
            else
            {
                loadingBar.Value = loadingBar.Maximum;
                statusUpdates.Text = message;
                //MessageBox.Show("ML Processing Complete!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                nextPage();
            }
        }

        // Event handler to confirm close
        private void LoadingPage_Xout(object sender, FormClosingEventArgs e)
        {
            if (IsNavigating)       // Check if user is navigating the app or exiting
            {
                return;     // If navigating, do not close the application
            }

            if (e.CloseReason == CloseReason.UserClosing)       // If user decides to close the app...
            {

                MessageBox.Show(
                    "Please do not close this window.",
                    "Image Processing...",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                e.Cancel = true;    
            }

        }

        private void nextPage()     // Method for going to resultsPage
        {
            IsNavigating = true;                                                                // Ensure user is navigating the app and not exiting
            ResultPage resultsPage = new ResultPage(resultsImage, _mainPage);                   // Initialize new page
            resultsPage.Show();                                                                 // Show new page
            this.Close();                                                                       // Hide current page
        }

        // Method to Center page controls
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
