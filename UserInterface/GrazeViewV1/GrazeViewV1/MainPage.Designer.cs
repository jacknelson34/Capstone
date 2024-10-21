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
            mainLabel.Font = new Font("Times New Roman", 28.125F, FontStyle.Bold, GraphicsUnit.Point, 0);
            mainLabel.Location = new Point(346, 208);
            mainLabel.Name = "mainLabel";
            mainLabel.Size = new Size(526, 85);
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
            dataUploadButton.Location = new Point(399, 366);
            dataUploadButton.Name = "dataUploadButton";
            dataUploadButton.Size = new Size(421, 116);
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
            dataViewerButton.Location = new Point(399, 514);
            dataViewerButton.Name = "dataViewerButton";
            dataViewerButton.Size = new Size(421, 116);
            dataViewerButton.TabIndex = 2;
            dataViewerButton.Text = "Data Viewer";
            dataViewerButton.UseVisualStyleBackColor = false;
            dataViewerButton.Click += dataViewerButton_Click;
            // 
            // helpButton
            // 
            helpButton.Cursor = Cursors.Hand;
            helpButton.Image = Properties.Resources.Help_Icon;
            helpButton.Location = new Point(1167, 12);
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
            BackColor = Color.LightBlue;
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