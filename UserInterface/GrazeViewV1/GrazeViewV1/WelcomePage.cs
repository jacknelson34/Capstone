namespace GrazeViewV1
{

    public partial class WelcomePage : ConsistentForm
    { 
        
        private Label titlelabel = new Label();
        public WelcomePage()
        {
            InitializeComponent();

            // Initialize Form Properties
            this.Text = "Welcome Page(To be designed)";             // Title for form
            this.Click += new EventHandler(WelcomePage_Click);

            // Initialize Welcome Page Title
            
            titlelabel.Text = "Welcome to GrazeView(To be designed)";           // Title in center of page
            titlelabel.Font = new Font("Times New Roman", 24, FontStyle.Bold);  // Font size/style for title
            titlelabel.AutoSize = true;                                         // Autosize title
            titlelabel.Location = new Point(                                    // Position Title to center of page
                (this.ClientSize.Width / 2) - (titlelabel.Width / 2),
                (this.ClientSize.Height / 2) - (titlelabel.Width / 2));
            this.Controls.Add(titlelabel);                                      // Add title to page

            CenterLabel();
            this.Resize += welcomeResize;                           // Call CenterLabel through welcomeResize if form is resized
            
        }
        private void WelcomePage_Click(object? sender, EventArgs e) // Click anywhere on Welcome Page
        {
            MainPage mainpage = new MainPage();                     // Create page 
            mainpage.Show();                                        // Open Main Page
            this.Hide();                                            // Close Welcome Page
        }

        private void CenterLabel()
        {
            titlelabel.Location = new Point(
                (this.ClientSize.Width / 2) - (titlelabel.Width / 2),
                this.ClientSize.Height / 4);

        }

        private void welcomeResize(object? sender, EventArgs e)
        {
            CenterLabel();
        }
    }
}
