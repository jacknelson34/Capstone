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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainPage));
            mainLabel = new Label();
            mainPanel = new Panel();
            dataViewerButton = new roundButton();
            dataUploadButton = new roundButton();
            helpButton = new PictureBox();
            //pictureBox1 = new PictureBox();
            mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)helpButton).BeginInit();
            //((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            this.DoubleBuffered = true;
            ClientSize = ConsistentForm.FormSize;
            Location = ConsistentForm.FormLocation;
            MinimumSize = new Size(1280, 918);
            // 
            // pictureBox1
            // 
            /*pictureBox1.BackgroundImageLayout = ImageLayout.None;
            pictureBox1.Dock = DockStyle.Bottom;
            pictureBox1.ForeColor = Color.Transparent;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(0, 351);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1280, 496);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 4;
            pictureBox1.TabStop = false;*/
            // 
            // mainLabel
            // 
            mainLabel.AutoSize = true;
            mainLabel.Font = new Font("Times New Roman", 42F, FontStyle.Bold, GraphicsUnit.Point, 0);
            mainLabel.Location = new Point(274, 229);
            mainLabel.Name = "mainLabel";
            mainLabel.Size = new Size(787, 128);
            mainLabel.TabIndex = 0;
            mainLabel.Text = "GRAZE VIEW";
            // 
            // mainPanel
            // 
            //mainPanel.Size = new Size(1000, 600);
            mainPanel.Width = (int)(this.ClientSize.Width * 0.8);
            mainPanel.Height = (int)(this.ClientSize.Height * 0.83);
            mainPanel.BorderStyle = BorderStyle.None;
            mainPanel.Anchor = AnchorStyles.None;
            mainPanel.Location = new Point((this.ClientSize.Width - mainPanel.Width) / 2,
                                           (this.ClientSize.Height - mainPanel.Height) / 2);
            mainPanel.BackColor = Color.Transparent;
            mainPanel.Controls.Add(dataViewerButton);
            mainPanel.Controls.Add(dataUploadButton);
            mainPanel.Controls.Add(mainLabel);
            mainPanel.Name = "mainPanel";
            mainPanel.TabIndex = 0;
            // 
            // dataViewerButton
            // 
            dataViewerButton.BackColor = Color.LightGreen;
            dataViewerButton.FlatStyle = FlatStyle.Flat;
            dataViewerButton.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataViewerButton.ForeColor = Color.Black;
            dataViewerButton.Location = new Point(274, 229);
            dataViewerButton.borderRadius = 50;
            dataViewerButton.Name = "dataViewerButton";
            dataViewerButton.Size = new Size(274, 150);
            dataViewerButton.TabIndex = 2;
            dataViewerButton.Text = "Data Viewer";
            dataViewerButton.UseVisualStyleBackColor = false;
            dataViewerButton.Click += dataViewerButton_Click;
            // 
            // dataUploadButton
            // 
            dataUploadButton.BackColor = Color.LightGreen;
            dataUploadButton.FlatStyle = FlatStyle.Flat;
            dataUploadButton.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataUploadButton.ForeColor = Color.Black;
            dataUploadButton.borderRadius = 50;
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
            helpButton.Cursor = Cursors.Hand;
            helpButton.Image = Properties.Resources.Help_Icon;
            helpButton.Location = new Point(1218, 12);
            helpButton.Name = "helpButton";
            helpButton.Size = new Size(50, 50);
            helpButton.SizeMode = PictureBoxSizeMode.StretchImage;
            helpButton.TabIndex = 3;
            helpButton.TabStop = false;
            helpButton.Click += helpButton_Click;
            // 
            // MainPage
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.MainPageBackground;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1280, 847);
            Controls.Add(mainPanel);
            Controls.Add(helpButton);
            BackColor = Color.LightBlue;
            DoubleBuffered = true;
            MinimumSize = new Size(1280, 918);
            Name = "MainPage";
            Text = "MainPage";
            mainPanel.ResumeLayout(false);
            mainPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)helpButton).EndInit();
            //((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel mainPanel;
        private Label mainLabel;
        private roundButton dataUploadButton;
        private roundButton dataViewerButton;
        private PictureBox helpButton;
        //private PictureBox pictureBox1;
    }
}