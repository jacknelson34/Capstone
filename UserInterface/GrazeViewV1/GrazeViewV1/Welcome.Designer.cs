namespace GrazeView_Design
{
    partial class Welcome
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataUploadButton = new Button();
            dataLibraryButton = new Button();
            helpButton = new Button();
            welcomeLabel = new Label();
            SuspendLayout();
            // 
            // dataUploadButton
            // 
            dataUploadButton.Font = new Font("Segoe UI", 13.875F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataUploadButton.Location = new Point(568, 388);
            dataUploadButton.Name = "dataUploadButton";
            dataUploadButton.Size = new Size(438, 87);
            dataUploadButton.TabIndex = 0;
            dataUploadButton.Text = "New Data Upload";
            dataUploadButton.UseVisualStyleBackColor = true;
            dataUploadButton.Click += button1_Click;
            // 
            // dataLibraryButton
            // 
            dataLibraryButton.Font = new Font("Segoe UI", 13.875F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataLibraryButton.Location = new Point(568, 532);
            dataLibraryButton.Name = "dataLibraryButton";
            dataLibraryButton.Size = new Size(438, 80);
            dataLibraryButton.TabIndex = 1;
            dataLibraryButton.Text = "Data Library";
            dataLibraryButton.UseVisualStyleBackColor = true;
            dataLibraryButton.Click += button2_Click;
            // 
            // helpButton
            // 
            helpButton.Font = new Font("Segoe UI", 13.875F, FontStyle.Regular, GraphicsUnit.Point, 0);
            helpButton.Location = new Point(568, 666);
            helpButton.Name = "helpButton";
            helpButton.Size = new Size(438, 80);
            helpButton.TabIndex = 2;
            helpButton.Text = "Help";
            helpButton.UseVisualStyleBackColor = true;
            helpButton.Click += button3_Click;
            // 
            // welcomeLabel
            // 
            welcomeLabel.AutoSize = true;
            welcomeLabel.Font = new Font("Showcard Gothic", 19.875F, FontStyle.Regular, GraphicsUnit.Point, 0);
            welcomeLabel.Location = new Point(609, 214);
            welcomeLabel.Name = "welcomeLabel";
            welcomeLabel.Size = new Size(345, 66);
            welcomeLabel.TabIndex = 3;
            welcomeLabel.Text = "Graze View";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1563, 995);
            Controls.Add(welcomeLabel);
            Controls.Add(helpButton);
            Controls.Add(dataLibraryButton);
            Controls.Add(dataUploadButton);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button dataUploadButton;
        private Button dataLibraryButton;
        private Button helpButton;
        private Label welcomeLabel;
    }
}
