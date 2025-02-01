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
            Image image = Properties.Resources.BackgroundImage12;

            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainPage));
            mainLabel = new Label();
            dataViewerButton = new roundButton();
            dataUploadButton = new roundButton();
            helpButton = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)helpButton).BeginInit();
            SuspendLayout();
            DoubleBuffered = true;
            ClientSize = new Size(1280, 918);
            MinimumSize = new Size(1280, 918);
            // 
            // mainLabel
            // 
            mainLabel.AutoSize = true;
            mainLabel.Text = "GRAZE VIEW";
            mainLabel.Font = new Font("Times New Roman", 30F, FontStyle.Bold);
            mainLabel.Location = new Point(274, 229);
            mainLabel.Name = "mainLabel";
            mainLabel.Size = new Size(787, 128);
            mainLabel.TabIndex = 0;
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
            helpButton.BackColor = Color.Transparent;
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
            Controls.Add(dataViewerButton);
            Controls.Add(dataUploadButton);
            Controls.Add(mainLabel);
            AutoScaleDimensions = new SizeF(200F, 200F);
            FormBorderStyle = FormBorderStyle.Sizable;
            AutoScaleMode = AutoScaleMode.Dpi;
            BackgroundImage = image;
            BackgroundImageLayout = ImageLayout.Stretch;
            Controls.Add(helpButton);
            BackColor = Color.FromArgb(116, 231, 247);
            DoubleBuffered = true;
            Name = "MainPage";
            Text = "MainPage";
            ((System.ComponentModel.ISupportInitialize)helpButton).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label mainLabel;
        private roundButton dataUploadButton;
        private roundButton dataViewerButton;
        private PictureBox helpButton;
    }
}