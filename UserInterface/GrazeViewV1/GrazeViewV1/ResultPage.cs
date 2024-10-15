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
    public partial class ResultPage : ConsistentForm
    { 
        
        private Button returnButton;
        private PictureBox resultBox;
        public ResultPage(Image resultImage)  // Build page with resulting image from ML and previous page's size/location 
        {
            InitializeComponent();

            // Initialize Form Properties
            this.Text = "ResultsPage";
            // this.Size = previousPageSize;                            // Create page at same size as previous page
            // this.Location = previousPageLocation;

            // Initialize Button to return to MainPage  
            returnButton = new Button();  
            returnButton.Text = "Exit";                                             // Text on button to say "Exit"
            returnButton.Font = new Font("Times New Roman", 12, FontStyle.Italic);  // Font size/style
            returnButton.AutoSize = true;                                           // Autosize button
            returnButton.Click += returnButton_Click;                               // Add event of click
            this.Controls.Add(returnButton);                                        // create new control returnButton

            // Initialize Results Image
            resultBox = new PictureBox();                                           // Initialize new pictureBox to hold results
            resultBox.Image = resultImage;                                          // Insert image into pictureBox
            resultBox.SizeMode = PictureBoxSizeMode.Zoom;                           // Zoom image to fit size
            resultBox.Size = new Size(300, 200);                                    // Size image to 300 x 200
            resultBox.Location = new Point(                                         // Position Image to center of page
                (this.ClientSize.Width / 2) - (resultBox.Width / 2),                //
                (this.ClientSize.Height / 4));                                      //
            this.Controls.Add(resultBox);                                           // Create new picturebox on page


            // Call CenterControls for page resize Events
            CenterControls();
            this.Resize += ResultsPage_Resize;

        }

        private void returnButton_Click(object? sender, EventArgs e)  // Method for returning to mainPage
        {
            MainPage mainPage = new MainPage();  // Initialize new main page
            mainPage.Show();    // Show new main page
            this.Hide();        // Hide current page
        }

        private void CenterControls()  // Method for centering elements of the form
        {
            // Center returnButton to bottom-center of the page
            returnButton.Location = new Point(
                (this.ClientSize.Width / 2) - (returnButton.Width / 2),
                this.ClientSize.Height - 40);
            // Center resultBox to center of page
            resultBox.Location = new Point(
                (this.ClientSize.Width / 2) - (resultBox.Width / 2),
                (this.ClientSize.Height / 4));

        }

        private void ResultsPage_Resize(object? sender, EventArgs e)  // Event Handler when the Form is resized by user
        {
            CenterControls();  // Call method for adjusting controls
        }
    }
}
