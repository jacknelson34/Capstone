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
    // Form to use as a loading screen as pages are loaded - CURRENTLY NOT IN USE

    public partial class SplashScreen : Form
    {
        public SplashScreen()
        {
            InitializeComponent();
            this.Load += SplashScreen_Load;
        }

        private void SplashScreen_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            panelSlide.Left += 5;

            if (panelSlide.Left > 310)
            {
                panelSlide.Left = 0;
            }

            if(panelSlide.Left < 0)
            {
                int move = 2;
            }
        }
    }
}
