using System.Windows.Forms;

namespace GrazeViewV1
{
    partial class MainPage
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
            mainLabel = new Label();
            dataViewerButton = new roundButton();
            dataUploadButton = new roundButton();
            helpButton = new PictureBox();
            splashText = new SplashTextLabel();
            ((System.ComponentModel.ISupportInitialize)helpButton).BeginInit();
            SuspendLayout();
            // 
            // mainLabel
            // 
            mainLabel.AutoSize = true;
            mainLabel.Font = new Font("Times New Roman", 30F, FontStyle.Bold);
            mainLabel.Location = new Point(274, 229);
            mainLabel.Name = "mainLabel";
            mainLabel.Size = new Size(351, 57);
            mainLabel.TabIndex = 0;
            mainLabel.Text = "GRAZE VIEW";
            mainLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // dataViewerButton
            // 
            dataViewerButton.BackColor = Color.LightGreen;
            dataViewerButton.FlatStyle = FlatStyle.Flat;
            dataViewerButton.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataViewerButton.ForeColor = Color.Black;
            dataViewerButton.Location = new Point(274, 229);
            dataViewerButton.Name = "dataViewerButton";
            dataViewerButton.Size = new Size(274, 150);
            dataViewerButton.TabIndex = 2;
            dataViewerButton.Text = "View all Uploads";
            dataViewerButton.UseVisualStyleBackColor = false;
            dataViewerButton.Click += dataViewerButton_Click;
            // 
            // dataUploadButton
            // 
            dataUploadButton.BackColor = Color.LightGreen;
            dataUploadButton.FlatStyle = FlatStyle.Flat;
            dataUploadButton.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataUploadButton.ForeColor = Color.Black;
            dataUploadButton.Location = new Point(274, 229);
            dataUploadButton.Name = "dataUploadButton";
            dataUploadButton.Size = new Size(274, 150);
            dataUploadButton.TabIndex = 1;
            dataUploadButton.Text = "Upload New Data";
            dataUploadButton.UseVisualStyleBackColor = false;
            dataUploadButton.Click += dataUploadButton_Click;
            // 
            // helpButton
            // 
            helpButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            helpButton.BackColor = Color.Transparent;
            helpButton.Cursor = Cursors.Hand;
            helpButton.Image = Properties.Resources.Help_Icon;
            helpButton.Location = new Point(844, 12);
            helpButton.Name = "helpButton";
            helpButton.Size = new Size(25, 25);
            helpButton.SizeMode = PictureBoxSizeMode.StretchImage;
            helpButton.TabIndex = 3;
            helpButton.TabStop = false;
            helpButton.Click += helpButton_Click;
            // 
            // splashText
            // 
            splashText.BackColor = Color.Transparent;
            splashText.EnableWobble = true;
            splashText.Font = new Font("Arial", 12F, FontStyle.Bold | FontStyle.Italic);
            splashText.ForeColor = Color.Green;
            splashText.Location = new Point(0, 0);
            splashText.Name = "splashText";
            splashText.Size = new Size(150, 150);
            splashText.TabIndex = 3;
            splashText.Text = "Natural lawnmowers since 9000 B.C.!";
            // 
            // MainPage
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(116, 231, 247);
            BackgroundImage = Properties.Resources.BackgroundImage12;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(884, 491);
            Controls.Add(dataViewerButton);
            Controls.Add(dataUploadButton);
            Controls.Add(mainLabel);
            Controls.Add(splashText);
            Controls.Add(helpButton);
            DoubleBuffered = true;
            MinimumSize = new Size(600, 500);
            Name = "MainPage";
            Text = "MainPage";
            ((System.ComponentModel.ISupportInitialize)helpButton).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label mainLabel;
        private roundButton dataUploadButton;
        private roundButton dataViewerButton;
        private PictureBox helpButton;
        private SplashTextLabel splashText;
    }
}