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
            helpTextBox.BackColor = Color.LightCyan;

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

            // Section 1 - Welcome & Uploading Data
            AppendSection(helpTextBox, "WELCOME TO GRAZEVIEW",
                "GrazeView allows users to upload, view, and analyze microscope images alongside sample data. " +
                "This guide explains how to use the system effectively.\n\n");

            AppendSection(helpTextBox, "HOW TO UPLOAD DATA",
                "1. Select 'Upload New Data' from the Main Page.\n" +
                "2. Enter Sample Information (Optional fields: Location, Breed, Comments, etc.).\n" +
                "3. Upload a Microscope Image by:\n" +
                "   - Dragging an image into the upload box\n" +
                "   - Clicking the upload box to select an image manually\n" +
                "   - A preview will appear if the upload is successful.\n" +
                "4. Click 'Upload' to store the data.\n" +
                "   - After uploading, you can return and upload more data.\n");

            // Section 2 - Viewing Data
            AppendSection(helpTextBox, "HOW TO VIEW DATA",
                "1. Open 'Data Viewer' from the Main Page.\n" +
                "   - You can also access the Data Viewer directly after an upload.\n" +
                "2. Sort Data: Click column headers to sort by a specific field.\n" +
                "3. Preview an Image: Click the Preview Button next to an upload.\n");

            // Section 3 - Exporting & Printing Data
            AppendSection(helpTextBox, "HOW TO EXPORT & PRINT DATA",
                "1. Ensure Data is Uploaded (Only saved data can be exported).\n" +
                "2. Open 'Data Viewer' from the Main Page.\n" +
                "3. Select Up to 5 Uploads to export.\n" +
                "   - Large image files may take extra time to load.\n" +
                "4. Click 'Print' to generate a printable view.\n");

            // Section 4 - Clearing Data
            AppendSection(helpTextBox, "HOW TO CLEAR DATA",
                "1. Click 'Clear All Data' in the Control Panel.\n" +
                "2. A confirmation prompt will appear.\n" +
                "3. Click 'Confirm' to delete all data.\n" +
                "   - WARNING: This action is irreversible.\n" +
                "   - TIP: Clearing old data occasionally improves performance.\n");

            // Section 5 - Troubleshooting
            AppendSection(helpTextBox, "TROUBLESHOOTING & SUPPORT",
                "1. Slow performance? Try clearing old data.\n" +
                "2. Image preview not working? Try re-uploading.\n" +
                "3. Database connection issues? Ensure internet is active.\n" +
                "4. App freezing? Restart the application.\n");

            // Credits
            AppendSection(helpTextBox, "CREATED BY",
                "Jack Nelson, Nate Scott, Arnav Gokhale, and Godson Edewor.");

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
