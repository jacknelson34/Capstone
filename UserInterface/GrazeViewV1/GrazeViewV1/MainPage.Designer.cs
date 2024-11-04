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
            dataUploadButton = new Button();
            dataViewerButton = new Button();
            helpButton = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)helpButton).BeginInit();
            SuspendLayout();
            ClientSize = new Size(1280, 720);
            MinimumSize = new Size(1280, 918);
            //
            // mainPagePanel
            //
            mainPanel = new Panel();
            mainPanel.Size = new Size(1000, 600);
            mainPanel.BorderStyle = BorderStyle.None;
            mainPanel.Anchor = AnchorStyles.None;
            mainPanel.Location = new Point((this.ClientSize.Width - mainPanel.Width) / 2, 
                                           (this.ClientSize.Height - mainPanel.Height) / 2);
            // 
            // mainLabel
            // 
            mainLabel.AutoSize = true;
            mainLabel.Font = new Font("Times New Roman", 28.125F, FontStyle.Bold, GraphicsUnit.Point, 0);
            mainLabel.Size = new Size(526, 85);
            mainLabel.Location = new Point(
                (mainPanel.Width / 2) - (mainLabel.Width / 2) + 25,
                (mainPanel.Height / 6) - 50);
            mainLabel.Name = "mainLabel";
            mainLabel.TabIndex = 0;
            mainLabel.Text = "GRAZE VIEW";
            // 
            // dataUploadButton
            // 
            dataUploadButton.AutoSize = true;
            dataUploadButton.BackColor = Color.White;
            dataUploadButton.FlatAppearance.BorderColor = Color.Gray;
            dataUploadButton.FlatAppearance.BorderSize = 2;
            dataUploadButton.FlatAppearance.MouseDownBackColor = Color.Silver;
            dataUploadButton.FlatAppearance.MouseOverBackColor = Color.Gainsboro;
            dataUploadButton.FlatStyle = FlatStyle.Flat;
            dataUploadButton.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataUploadButton.ForeColor = Color.Black;
            dataUploadButton.Size = new Size(421, 150);
            dataUploadButton.Location = new Point(
                (mainPanel.Width / 2) - (dataUploadButton.Width / 2),
                (mainPanel.Height - mainLabel.Height) / 2 - 75);
            dataUploadButton.Name = "dataUploadButton";
            dataUploadButton.TabIndex = 1;
            dataUploadButton.Text = "Upload New Data";
            dataUploadButton.UseVisualStyleBackColor = false;
            dataUploadButton.Click += dataUploadButton_Click;
            // 
            // dataViewerButton
            // 
            dataViewerButton.AutoSize = true;
            dataViewerButton.BackColor = Color.White;
            dataViewerButton.FlatAppearance.BorderColor = Color.Gray;
            dataViewerButton.FlatAppearance.BorderSize = 2;
            dataViewerButton.FlatAppearance.MouseDownBackColor = Color.Silver;
            dataViewerButton.FlatAppearance.MouseOverBackColor = Color.Gainsboro;
            dataViewerButton.FlatStyle = FlatStyle.Flat;
            dataViewerButton.Font = new Font("Times New Roman", 12F);
            dataViewerButton.ForeColor = Color.Black;
            dataViewerButton.Size = new Size(421, 150);
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
        private Button dataUploadButton;
        private Button dataViewerButton;
        private PictureBox helpButton;
    }
}