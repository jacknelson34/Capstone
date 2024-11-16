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
            dataUploadButton = new roundButton();
            dataViewerButton = new roundButton();
            helpButton = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)helpButton).BeginInit();
            SuspendLayout();
            ClientSize = new Size(1280, 720);
            MinimumSize = new Size(1280, 918);
            //
            // mainPagePanel
            //
            mainPanel = new Panel();
            //mainPanel.Size = new Size(1000, 600);
            mainPanel.Width = (int)(this.ClientSize.Width * 0.8);
            mainPanel.Height = (int)(this.ClientSize.Height * 0.83);
            mainPanel.BorderStyle = BorderStyle.None;
            mainPanel.Anchor = AnchorStyles.None;
            mainPanel.Location = new Point((this.ClientSize.Width - mainPanel.Width) / 2, 
                                           (this.ClientSize.Height - mainPanel.Height) / 2);
            // 
            // mainLabel
            // 
            
            mainLabel.Font = new Font("Times New Roman", 42, FontStyle.Bold, GraphicsUnit.Point, 0);
            mainLabel.Size = new Size((this.ClientSize.Width / 2), (this.ClientSize.Height / 7));
            mainLabel.AutoSize = true;
            mainLabel.Location = new Point(
                (mainPanel.Width / 2) - (mainLabel.Width / 2),
                (mainPanel.Height / 6));
            //MessageBox.Show("Label location: " + mainLabel.Location.ToString());
            mainLabel.Name = "mainLabel";
            mainLabel.TabIndex = 0;
            mainLabel.Text = "GRAZE VIEW";
            // 
            // dataUploadButton
            // 
            dataUploadButton.Size = new Size((mainPanel.Width / 3), 150);
            dataUploadButton.borderRadius = 50;
            dataUploadButton.Location = new Point(
                (mainPanel.Width / 2) - (dataUploadButton.Width / 2),
                (mainPanel.Height - mainLabel.Height) / 2 - 75);
            dataUploadButton.Name = "dataUploadButton";
            dataUploadButton.TabIndex = 1;
            dataUploadButton.Text = "Upload New Data";
            dataUploadButton.Click += dataUploadButton_Click;
            // 
            // dataViewerButton
            // 
            dataViewerButton.Size = new Size((mainPanel.Width / 3), 150);
            dataViewerButton.borderRadius = 50;
            dataViewerButton.Location = new Point(
               (mainPanel.Width / 2) - (dataViewerButton.Width / 2),
               ((mainPanel.Height - mainLabel.Height) / 2) + 125);
            dataViewerButton.Name = "dataViewerButton";
            dataViewerButton.TabIndex = 2;
            dataViewerButton.Text = "Data Viewer";
            dataViewerButton.UseVisualStyleBackColor = false;
            dataViewerButton.Click += dataViewerButton_Click;
            // 
            // helpButton
            // 
            helpButton.Cursor = Cursors.Hand;
            helpButton.Image = Properties.Resources.Help_Icon;
            helpButton.Size = new Size(50, 50);
            helpButton.Location = new Point(
                (ClientSize.Width - 60), 10);
            helpButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            helpButton.Name = "helpButton";
            
            helpButton.SizeMode = PictureBoxSizeMode.StretchImage;
            helpButton.TabIndex = 3;
            helpButton.TabStop = false;
            helpButton.Click += helpButton_Click;
            // 
            // MainPage
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightBlue;
            Controls.Add(mainPanel);
            this.Controls.Add(helpButton);
            mainPanel.Controls.Add(dataViewerButton);
            mainPanel.Controls.Add(dataUploadButton);
            mainPanel.Controls.Add(mainLabel);
            Name = "MainPage";
            Text = "MainPage";
            ((System.ComponentModel.ISupportInitialize)helpButton).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel mainPanel;
        private Label mainLabel;
        private roundButton dataUploadButton;
        private roundButton dataViewerButton;
        private PictureBox helpButton;
    }
}