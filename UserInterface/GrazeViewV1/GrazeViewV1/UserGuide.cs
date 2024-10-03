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
            this.Text = "User Guide";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Initialize RichTextBox for the Help Guide
            RichTextBox helpTextBox = new RichTextBox();
            helpTextBox.Dock = DockStyle.Fill;
            helpTextBox.ReadOnly = true;
            helpTextBox.ScrollBars = RichTextBoxScrollBars.Vertical; // Ensure a vertical scrollbar
            helpTextBox.Font = new Font("Arial", 12);
            helpTextBox.Text = LoadHelpContent(); // Load help content here

            this.Controls.Add(helpTextBox);
        }

        private string LoadHelpContent()
        {
            return @"
User Guide Will be inserted here";
        }

        public static void ShowHelpGuide()  // Static method to show user guide
        {
            if (instance == null || instance.IsDisposed)
            {
                instance = new UserGuide();
            }

            if (!instance.Visible)
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
