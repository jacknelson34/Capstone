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

    public partial class UserGuide : Form
    {
        private static UserGuide? instance;

        public UserGuide()
        {
            // Initialize User Guide Properties
            this.Text = "User Guide";                   // Header title for User Guide
            this.Size = new Size(800, 600);             // Page Size for userguide
            this.MinimumSize = new Size(800, 600);      // Set minimum size for the user guide window
            this.StartPosition = FormStartPosition.CenterScreen;    // Set user guide to open in the center of the screen

            // Initialize RichTextBox for the Help Guide
            RichTextBox helpTextBox = new RichTextBox();             // Text box to hold entire user guide text
            helpTextBox.Dock = DockStyle.Fill;                       // Set helpTextBox to fill the page
            helpTextBox.ReadOnly = true;                             // Enable read only for the user guide
            helpTextBox.TabStop = false;                             // Don't allow user to type or highlight
            helpTextBox.BackColor = Color.LightBlue;

            helpTextBox.ScrollBars = RichTextBoxScrollBars.Vertical; // Ensure a vertical scrollbar
            helpTextBox.Font = new Font("Oswald", 12);      // Set font for user guide

            // Load and format help content
            LoadHelpContent(helpTextBox);                   // Build the user guide text with LoadHelpContent

            this.Controls.Add(helpTextBox);                 // add this control to be seen
        }

        // Helper Method to create the text within the user guide
        private void LoadHelpContent(RichTextBox helpTextBox)
        {
            helpTextBox.Clear();

            // Define content for each section
            // Section 1 - Header and How to Upload Data
            AppendSection(helpTextBox, "Welcome to GRAZEVIEW\n\n--- How to Upload Data ---",
                "1. Select Upload New Data on the Main Page\n" +
                "2. Fill out Data Fields in the Data Uploader.  None are required.\n" +
                "3. Select a Microscope Image to upload by either dragging an image into the box or by clicking the image box.\n" +
                "      - A preview of the image will appear if the upload was successful.\n" +
                "4. Click 'Upload' to save the data to the system.\n" +
                "      - Once uploaded, there is an option to return to upload more from the results page.\n");

            // Section 2 - How to View Data
            AppendSection(helpTextBox, "--- How to View Data ---",
                "1. Select Data Viewer on the Main Page\n" +
                "      - Once uploads have been saved, there is an option to view data directly from the results page. \n" +
                "2. Saved Data can be sorted by each field by clicking on the respective column header.\n" +
                "      - A preview of the image will appear if the upload was successful.\n");

            // Section 3 - How to Export and Print data
            AppendSection(helpTextBox, "--- How to Export/Print Data ---",
                "1. Ensure that Data has been uploaded previously(If there is no data uploaded, there is no data to export).\n" +
                "2. Select Data Viewer from the Main Page. \n" +
                "3. In the Data Viewer, select up to 5 uploads to export and view.\n" +
                "      - This may take a moment, depending on the size of the image files.\n" +
                "4. Once Uploads are loaded, click the print button to print the page.\n");

            // Section 4 - Clearing Data
            AppendSection(helpTextBox, "--- How to Clear all Data ---",
                "1. Once data has been uploaded, there is the option to clear it using 'Clear All Data' in the control panel.\n" +
                "2. If clicked, the application will prompt the user to confirm.\n" +
                "3. Click confirm in order to clear all data.\n" +
                "      - NOTE : Once data is cleared, there is no way to recover it.\n" +
                "      - NOTE : Clearing data occasionally will improve application performace.\n");

            AppendSection(helpTextBox, "", 
                "Created by Jack Nelson, Nate Scott, Arnav Gokhale, and Godson Edewor");

        }
        
        // Helper method to add sections into the user guide
        private void AppendSection(RichTextBox richTextBox, string title, string content)
        {
            // Add the title in bold
            richTextBox.SelectionFont = new Font(richTextBox.Font, FontStyle.Bold);
            richTextBox.AppendText(title + "\n");

            // Add the content in regular font
            richTextBox.SelectionFont = new Font(richTextBox.Font, FontStyle.Regular);
            richTextBox.AppendText(content + "\n");
        }

        public static void ShowHelpGuide() // Static method to show user guide
        {
            if (instance == null || instance.IsDisposed)  // Check to make sure User guide is created
            {
                instance = new UserGuide();
            }

            if (!instance.Visible)      // Show user guide only if instance is created
            {
                instance.Show();
            }
            else
            {
                instance.BringToFront(); // Bring the form to the front if it's already open
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Hide the form instead of closing to allow reopening
            e.Cancel = true;
            this.Hide();
        }
    }
}
