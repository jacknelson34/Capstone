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
    public partial class DataLibrary : Form
    {
        // hold reference to Main Page form
        private MainPage _mainPage;

        public DataLibrary(MainPage mainpage)
        {
            // Form Properties
            InitializeComponent();
            _mainPage = mainpage;
            this.Text = "Data Viewer";

            // Back Button Functionality
            backButton.Click += backButton_Click; // handles click event
            this.Controls.Add(backButton);

            // Help Button Functionality
            helpButton.Click += helpButton_Click; // handles click event
            this.Controls.Add(helpButton);
        }

        // when the back button is clicked on
        private void backButton_Click(object? sender, EventArgs e)
        {
            _mainPage.Show();                                       // Open Main Page
            this.Hide();                                            // Close Data Upload Page
        }

        // when the help icon is clicked on
        private void helpButton_Click(object sender, EventArgs e)
        {
            UserGuide.ShowHelpGuide();  // Call Method to only allow one instance open at a time
        }
    }
}
