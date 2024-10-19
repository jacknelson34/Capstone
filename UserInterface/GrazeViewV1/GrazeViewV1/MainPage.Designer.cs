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
            // 
            // mainLabel
            // 
            mainLabel.AutoSize = true;
            mainLabel.Font = new Font("Times New Roman", 24F, FontStyle.Bold);
            mainLabel.Location = new Point(364, 209);
            mainLabel.Name = "mainLabel";
            mainLabel.Size = new Size(452, 73);
            mainLabel.TabIndex = 0;
            mainLabel.Text = "GRAZE VIEW";
            // 
            // dataUploadButton
            // 
            dataUploadButton.AutoSize = true;
            dataUploadButton.Cursor = Cursors.Hand;
            dataUploadButton.Font = new Font("Times New Roman", 12F);
            dataUploadButton.Location = new Point(380, 336);
            dataUploadButton.Name = "dataUploadButton";
            dataUploadButton.Size = new Size(417, 116);
            dataUploadButton.TabIndex = 1;
            dataUploadButton.Text = "Upload New Data";
            dataUploadButton.UseVisualStyleBackColor = true;
            dataUploadButton.Click += dataUploadButton_Click;
            // 
            // dataViewerButton
            // 
            dataViewerButton.AutoSize = true;
            dataViewerButton.Cursor = Cursors.Hand;
            dataViewerButton.Font = new Font("Times New Roman", 12F);
            dataViewerButton.Location = new Point(380, 484);
            dataViewerButton.Name = "dataViewerButton";
            dataViewerButton.Size = new Size(417, 116);
            dataViewerButton.TabIndex = 2;
            dataViewerButton.Text = "Data Viewer";
            dataViewerButton.UseVisualStyleBackColor = true;
            dataViewerButton.Click += dataViewerButton_Click;
            // 
            // helpButton
            // 
            helpButton.Cursor = Cursors.Hand;
            helpButton.Image = Properties.Resources.Help_Icon;
            helpButton.Location = new Point(1156, 12);
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
            ClientSize = new Size(1229, 887);
            Controls.Add(helpButton);
            Controls.Add(dataViewerButton);
            Controls.Add(dataUploadButton);
            Controls.Add(mainLabel);
            Name = "MainPage";
            Text = "MainPage";
            ((System.ComponentModel.ISupportInitialize)helpButton).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label mainLabel;
        private Button dataUploadButton;
        private Button dataViewerButton;
        private PictureBox helpButton;
    }
}