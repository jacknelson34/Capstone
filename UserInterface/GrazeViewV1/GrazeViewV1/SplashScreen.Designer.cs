using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace GrazeViewV1
{
    partial class SplashScreen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            label2 = new Label();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            ClientSize = new Size(800, 450);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Size = new Size(281, 51);
            label1.Font = new Font("Times New Roman", 30F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            
            label1.Name = "label1";
            label1.TabIndex = 0;
            label1.Text = "GRAZEVIEW";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            label1.Location = new Point((ClientSize.Width - label1.Width) / 2, ClientSize.Height / 3 - 50);
            // 
            // label2 (Initializing Application...)
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Size = new Size(176, 19);
            label2.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            
            label2.Name = "label2";
            label2.TabIndex = 2;
            label2.Text = "Initializing Application...";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            label2.Location = new Point((ClientSize.Width - label2.Width) / 2, label1.Bottom + 70);
            // 
            // pictureBox1 (Loading Icon)
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Size = new Size(200, 30);
            pictureBox1.Image = Properties.Resources.SplashScreen2;
            
            pictureBox1.Name = "pictureBox1";
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            pictureBox1.Location = new Point((ClientSize.Width - pictureBox1.Width - 25) / 2, label2.Bottom + 5);
            //
            // pictureBox2 (Sheep)
            //
            pictureBox2.BackColor = Color.Transparent;
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.Size = new Size(200, 60);
            pictureBox2.Image = Properties.Resources.sheepGif;
            
            pictureBox2.Name = "pictureBox2";
            pictureBox2.TabIndex = 4;
            pictureBox2.TabStop = false;
            pictureBox2.Location = new Point((ClientSize.Width - pictureBox1.Width - 25) / 2, label1.Bottom - 5);
            // 
            // SplashScreen
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackgroundImage = Properties.Resources.BackgroundImage12;
            BackgroundImageLayout = ImageLayout.Stretch;
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
            Controls.Add(label2);
            Controls.Add(label1);

            FormBorderStyle = FormBorderStyle.None;
            Name = "SplashScreen";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SplashScreen";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
    }
}