namespace GrazeViewV1
{
    public partial class WelcomePage : Form
    { 
        
        private Label titlelabel = new Label();
        public WelcomePage()
        {
            // Initialize Form Properties
            this.Text = "Welcome Page(To be designed)";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Click += new EventHandler(WelcomePage_Click);

            // Initialize Welcome Page Title
            
            titlelabel.Text = "Welcome to GrazeView(To be designed)";
            titlelabel.Font = new Font("Times New Roman", 24, FontStyle.Bold);
            titlelabel.AutoSize = true;
            titlelabel.Location = new Point((this.ClientSize.Width / 2) - (titlelabel.Width / 2), (this.ClientSize.Height / 2) - (titlelabel.Width / 2));
            this.Controls.Add(titlelabel);

            CenterLabel();
            this.Resize += welcomeResize;  // Call CenterLabel through welcomeResize if form is resized
        }
        private void WelcomePage_Click(object? sender, EventArgs e) // Click anywhere on Welcome Page
        {
            MainPage mainpage = new MainPage();
            mainpage.Show();  // Open Main Page
            this.Hide();  // Close Welcome Page
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
