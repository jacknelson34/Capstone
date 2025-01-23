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
            components = new System.ComponentModel.Container();
            label1 = new Label();
            panel1 = new Panel();
            panelSlide = new Panel();
            timer1 = new System.Windows.Forms.Timer(components);
            label2 = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top;
            label1.AutoSize = true;
            label1.Font = new Font("Times New Roman", 26.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(235, 154);
            label1.Name = "label1";
            label1.Size = new Size(299, 51);
            label1.TabIndex = 0;
            label1.Text = "GRAZEVIEW";
            label1.TextAlign = ContentAlignment.TopCenter;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top;
            panel1.BackColor = Color.FromArgb(0, 192, 192);
            panel1.Controls.Add(panelSlide);
            panel1.Location = new Point(193, 231);
            panel1.Name = "panel1";
            panel1.Size = new Size(385, 13);
            panel1.TabIndex = 1;
            // 
            // panelSlide
            // 
            panelSlide.BackColor = Color.LimeGreen;
            panelSlide.Location = new Point(287, 0);
            panelSlide.Name = "panelSlide";
            panelSlide.Size = new Size(98, 13);
            panelSlide.TabIndex = 2;
            // 
            // timer1
            // 
            timer1.Tick += timer1_Tick;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Times New Roman", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(354, 205);
            label2.Name = "label2";
            label2.Size = new Size(76, 19);
            label2.TabIndex = 2;
            label2.Text = "Loading...";
            // 
            // SplashScreen
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(116, 231, 247);
            ClientSize = new Size(800, 450);
            Controls.Add(label2);
            Controls.Add(panel1);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "SplashScreen";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SplashScreen";
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Panel panel1;
        private Panel panelSlide;
        private System.Windows.Forms.Timer timer1;
        private Label label2;
    }
}