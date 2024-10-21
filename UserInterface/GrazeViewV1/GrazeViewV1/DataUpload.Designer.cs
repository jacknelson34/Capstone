namespace GrazeViewV1
{
    partial class DataUpload
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
            backButton = new Button();
            helpButton = new PictureBox();
            uploadButton = new Button();
            fileuploadPictureBox = new PictureBox();
            filenameTextbox = new TextBox();
            filenameLabel = new Label();
            locationLabel = new Label();
            locationTextbox = new TextBox();
            datePicker = new DateTimePicker();
            datetimeLabel = new Label();
            timePicker = new MaskedTextBox();
            breedLabel = new Label();
            breedTextbox = new TextBox();
            commentsLabel = new Label();
            commentsTextbox = new TextBox();
            ((System.ComponentModel.ISupportInitialize)helpButton).BeginInit();
            ((System.ComponentModel.ISupportInitialize)fileuploadPictureBox).BeginInit();
            SuspendLayout();
            // 
            // backButton
            // 
            backButton.BackColor = Color.White;
            backButton.FlatAppearance.BorderColor = Color.Gray;
            backButton.FlatAppearance.BorderSize = 2;
            backButton.FlatAppearance.MouseDownBackColor = Color.Silver;
            backButton.FlatAppearance.MouseOverBackColor = Color.Gainsboro;
            backButton.FlatStyle = FlatStyle.Flat;
            backButton.Font = new Font("Times New Roman", 10.125F, FontStyle.Regular, GraphicsUnit.Point, 0);
            backButton.ForeColor = Color.Black;
            backButton.Location = new Point(28, 23);
            backButton.Name = "backButton";
            backButton.Size = new Size(123, 69);
            backButton.TabIndex = 0;
            backButton.Text = "Back";
            backButton.UseVisualStyleBackColor = false;
            backButton.Click += backButton_Click;
            // 
            // helpButton
            // 
            helpButton.Cursor = Cursors.Hand;
            helpButton.Image = Properties.Resources.Help_Icon;
            helpButton.Location = new Point(1156, 12);
            helpButton.Name = "helpButton";
            helpButton.Size = new Size(73, 68);
            helpButton.SizeMode = PictureBoxSizeMode.StretchImage;
            helpButton.TabIndex = 1;
            helpButton.TabStop = false;
            helpButton.Click += helpButton_Click;
            // 
            // uploadButton
            // 
            uploadButton.BackColor = Color.White;
            uploadButton.FlatAppearance.BorderColor = Color.Gray;
            uploadButton.FlatAppearance.MouseDownBackColor = Color.Silver;
            uploadButton.FlatAppearance.MouseOverBackColor = Color.Gainsboro;
            uploadButton.FlatStyle = FlatStyle.Flat;
            uploadButton.Font = new Font("Times New Roman", 10.125F, FontStyle.Regular, GraphicsUnit.Point, 0);
            uploadButton.ForeColor = Color.Black;
            uploadButton.Location = new Point(1002, 819);
            uploadButton.Name = "uploadButton";
            uploadButton.Size = new Size(210, 76);
            uploadButton.TabIndex = 2;
            uploadButton.Text = "Upload";
            uploadButton.UseVisualStyleBackColor = false;
            // 
            // fileuploadPictureBox
            // 
            fileuploadPictureBox.AllowDrop = true;
            fileuploadPictureBox.BorderStyle = BorderStyle.FixedSingle;
            fileuploadPictureBox.Cursor = Cursors.Hand;
            fileuploadPictureBox.Location = new Point(679, 111);
            fileuploadPictureBox.Name = "fileuploadPictureBox";
            fileuploadPictureBox.Size = new Size(483, 682);
            fileuploadPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            fileuploadPictureBox.TabIndex = 3;
            fileuploadPictureBox.TabStop = false;
            fileuploadPictureBox.Tag = "";
            // 
            // filenameTextbox
            // 
            filenameTextbox.Cursor = Cursors.IBeam;
            filenameTextbox.Location = new Point(74, 163);
            filenameTextbox.Name = "filenameTextbox";
            filenameTextbox.Size = new Size(552, 39);
            filenameTextbox.TabIndex = 4;
            // 
            // filenameLabel
            // 
            filenameLabel.AutoSize = true;
            filenameLabel.Location = new Point(74, 128);
            filenameLabel.Name = "filenameLabel";
            filenameLabel.Size = new Size(166, 32);
            filenameLabel.TabIndex = 5;
            filenameLabel.Text = "Upload Name:";
            // 
            // locationLabel
            // 
            locationLabel.AutoSize = true;
            locationLabel.Location = new Point(74, 251);
            locationLabel.Name = "locationLabel";
            locationLabel.Size = new Size(195, 32);
            locationLabel.TabIndex = 6;
            locationLabel.Text = "Sample Location:";
            // 
            // locationTextbox
            // 
            locationTextbox.Cursor = Cursors.IBeam;
            locationTextbox.Location = new Point(74, 286);
            locationTextbox.Name = "locationTextbox";
            locationTextbox.Size = new Size(552, 39);
            locationTextbox.TabIndex = 7;
            // 
            // datePicker
            // 
            datePicker.CustomFormat = "MM/dd/yyyy";
            datePicker.Format = DateTimePickerFormat.Custom;
            datePicker.ImeMode = ImeMode.NoControl;
            datePicker.Location = new Point(74, 412);
            datePicker.Name = "datePicker";
            datePicker.Size = new Size(210, 39);
            datePicker.TabIndex = 8;
            // 
            // datetimeLabel
            // 
            datetimeLabel.AutoSize = true;
            datetimeLabel.Location = new Point(74, 377);
            datetimeLabel.Name = "datetimeLabel";
            datetimeLabel.Size = new Size(262, 32);
            datetimeLabel.TabIndex = 9;
            datetimeLabel.Text = "Sample Date and Time:";
            // 
            // timePicker
            // 
            timePicker.Cursor = Cursors.IBeam;
            timePicker.InsertKeyMode = InsertKeyMode.Overwrite;
            timePicker.Location = new Point(337, 412);
            timePicker.Mask = "90:00 LL";
            timePicker.Name = "timePicker";
            timePicker.Size = new Size(207, 39);
            timePicker.TabIndex = 11;
            timePicker.Text = "0800AM";
            timePicker.ValidatingType = typeof(DateTime);
            // 
            // breedLabel
            // 
            breedLabel.AutoSize = true;
            breedLabel.Location = new Point(74, 499);
            breedLabel.Name = "breedLabel";
            breedLabel.Size = new Size(155, 32);
            breedLabel.TabIndex = 12;
            breedLabel.Text = "Sheep Breed:";
            // 
            // breedTextbox
            // 
            breedTextbox.Cursor = Cursors.IBeam;
            breedTextbox.Location = new Point(74, 534);
            breedTextbox.Name = "breedTextbox";
            breedTextbox.Size = new Size(552, 39);
            breedTextbox.TabIndex = 13;
            // 
            // commentsLabel
            // 
            commentsLabel.AutoSize = true;
            commentsLabel.Location = new Point(74, 623);
            commentsLabel.Name = "commentsLabel";
            commentsLabel.Size = new Size(135, 32);
            commentsLabel.TabIndex = 14;
            commentsLabel.Text = "Comments:";
            // 
            // commentsTextbox
            // 
            commentsTextbox.Cursor = Cursors.IBeam;
            commentsTextbox.Location = new Point(74, 658);
            commentsTextbox.Multiline = true;
            commentsTextbox.Name = "commentsTextbox";
            commentsTextbox.Size = new Size(552, 192);
            commentsTextbox.TabIndex = 15;
            // 
            // DataUpload
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1241, 918);
            Controls.Add(commentsTextbox);
            Controls.Add(commentsLabel);
            Controls.Add(breedTextbox);
            Controls.Add(breedLabel);
            Controls.Add(timePicker);
            Controls.Add(datetimeLabel);
            Controls.Add(datePicker);
            Controls.Add(locationTextbox);
            Controls.Add(locationLabel);
            Controls.Add(filenameLabel);
            Controls.Add(filenameTextbox);
            Controls.Add(fileuploadPictureBox);
            Controls.Add(uploadButton);
            Controls.Add(helpButton);
            Controls.Add(backButton);
            Name = "DataUpload";
            Text = "DataUpload";
            ((System.ComponentModel.ISupportInitialize)helpButton).EndInit();
            ((System.ComponentModel.ISupportInitialize)fileuploadPictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button backButton;
        private PictureBox helpButton;
        private Button uploadButton;
        private PictureBox fileuploadPictureBox;
        private TextBox filenameTextbox;
        private Label filenameLabel;
        private Label locationLabel;
        private TextBox locationTextbox;
        private DateTimePicker datePicker;
        private Label datetimeLabel;
        private MaskedTextBox timePicker;
        private Label breedLabel;
        private TextBox breedTextbox;
        private Label commentsLabel;
        private TextBox commentsTextbox;
    }
}