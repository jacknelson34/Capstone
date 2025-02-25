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
    // Form to use as a loading screen as pages are loaded
    // In use while DB Connection is established

    public partial class SplashScreen : Form
    {
        private bool isRunning = true; //   Controls the animation loop

        public SplashScreen()
        {
            InitializeComponent();
            this.Load += SplashScreen_Load;
        }

        private async void SplashScreen_Load(object sender, EventArgs e)
        {
            // Start animation & database connection at the same time
            Task animationTask = RunProgressAnimationAsync();
            Task<bool> dbTask = ConnectToDatabaseAsync();

            bool isConnected = await dbTask; //   Waits for DB connection

            if (!isConnected)
            {
                MessageBox.Show("Failed to connect to the database. Exiting application.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0); // Close app on failure
            }

            StopAnimation(); //   Stops animation when DB is ready
            this.Close(); //   Close splash screen
        }

        private async Task<bool> ConnectToDatabaseAsync()
        {
            await Task.Delay(3000); // Simulated DB delay

            var dbConnections = new DBConnections(new DBSettings(
                server: "sqldatabase404.database.windows.net",
                database: "404ImageDBsql",
                username: "sql404admin",
                password: "sheepstool404()"
            ));
            var dbQueries = new DBQueries(dbConnections.ConnectionString);

            return await dbConnections.TestConnectionAsync(); //   Actual DB test
        }

        private async Task RunProgressAnimationAsync()
        {
            int moveSpeed = 5;
            int resetPosition = 310;

            while (isRunning) //   Keep animating until stopped
            {
                await Task.Delay(50); //   Smooth animation

                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        panelSlide.Left += moveSpeed;
                        if (panelSlide.Left > resetPosition)
                        {
                            panelSlide.Left = 0;
                        }
                    }));
                }
                else
                {
                    panelSlide.Left += moveSpeed;
                    if (panelSlide.Left > resetPosition)
                    {
                        panelSlide.Left = 0;
                    }
                }
            }
        }

        public void StopAnimation()
        {
            isRunning = false; //   Stops animation loop
        }
    }
}
