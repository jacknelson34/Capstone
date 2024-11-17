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
            uploadPanel = new Panel();
            backButton = new roundButton();
            helpButton = new PictureBox();
            uploadButton = new roundButton();
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
            ClientSize = ConsistentForm.FormSize;
            Location = ConsistentForm.FormLocation;
            MinimumSize = new Size(1280, 975);
            //
            // uploadPanel
            //
            uploadPanel.Size = new Size(1280, 918);
            uploadPanel.BorderStyle = BorderStyle.None;
            uploadPanel.Anchor = AnchorStyles.None;
            uploadPanel.Location = new Point((this.ClientSize.Width - uploadPanel.Width) / 2,
                                           (this.ClientSize.Height - uploadPanel.Height) / 2);
            uploadPanel.BackColor = Color.Transparent;
            Controls.Add(uploadPanel);
            uploadPanel.SendToBack();
            // 
            // backButton
            // 
            backButton.Location = new Point(28, 23);
            backButton.borderRadius = 20;
            backButton.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            backButton.Name = "backButton";
            backButton.Size = new Size(123, 69);
            backButton.TabIndex = 0;
            backButton.Text = "Back";
            backButton.Click += backButton_Click;
            backButton.BringToFront();
            // 
            // helpButton
            // 
            helpButton.Cursor = Cursors.Hand;
            helpButton.Image = Properties.Resources.Help_Icon;
            helpButton.Size = new Size(73, 68);
            helpButton.Location = new Point(1156, 12);
            helpButton.Anchor = AnchorStyles.Top | AnchorStyles.Right; 
            helpButton.Name = "helpButton";
            helpButton.SizeMode = PictureBoxSizeMode.StretchImage;
            helpButton.TabIndex = 1;
            helpButton.TabStop = false;
            helpButton.Click += helpButton_Click;
            helpButton.BringToFront();
            // 
            // uploadButton
            // 
            uploadButton.Location = new Point(1002, 819);
            uploadButton.borderRadius = 50;
            uploadButton.Name = "uploadButton";
            uploadButton.Size = new Size(210, 76);
            uploadButton.TabIndex = 2;
            uploadButton.Text = "Upload";
            uploadButton.UseVisualStyleBackColor = false;
            uploadButton.Click += uploadButton_Click;
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
            fileuploadPictureBox.BackColor = Color.White;
            // File Upload Picture Box Functionality
            fileuploadPictureBox.Click += fileuploadPictureBox_Click;           // handles click-to-upload event
            fileuploadPictureBox.DragEnter += fileuploadPictureBox_DragEnter;   // handles drag enter event
            fileuploadPictureBox.DragDrop += fileuploadPictureBox_DragDrop;     // handles drag-and-drop event
            fileuploadPictureBox.Paint += fileuploadPictureBox_Prompt;          // adds prompt to picture box
            // 
            // filenameTextbox
            // 
            filenameTextbox.Cursor = Cursors.IBeam;
            filenameTextbox.Location = new Point(74, 130);
            filenameTextbox.Name = "filenameTextbox";
            filenameTextbox.Size = new Size(552, 39);
            filenameTextbox.TabIndex = 4;
            // 
            // filenameLabel
            // 
            filenameLabel.AutoSize = true;
            filenameLabel.Location = new Point(69, 95);
            filenameLabel.Name = "filenameLabel";
            filenameLabel.Size = new Size(166, 32);
            filenameLabel.Font = new Font("Times New Roman", 12, FontStyle.Regular, GraphicsUnit.Point, 0);
            filenameLabel.TabIndex = 5;
            filenameLabel.Text = "Upload Name:";
            // 
            // locationLabel
            // 
            locationLabel.AutoSize = true;
            locationLabel.Location = new Point(69, 200);
            locationLabel.Name = "locationLabel";
            locationLabel.Size = new Size(195, 32);
            locationLabel.Font = new Font("Times New Roman", 12, FontStyle.Regular, GraphicsUnit.Point, 0);
            locationLabel.TabIndex = 6;
            locationLabel.Text = "Sample Location:";
            // 
            // locationTextbox
            // 
            locationTextbox.Cursor = Cursors.IBeam;
            locationTextbox.Location = new Point(74, 235);
            locationTextbox.Name = "locationTextbox";
            locationTextbox.Size = new Size(552, 39);
            locationTextbox.TabIndex = 7;
            // 
            // datePicker
            // 
            datePicker.CustomFormat = "MM/dd/yyyy";
            datePicker.Format = DateTimePickerFormat.Custom;
            datePicker.ImeMode = ImeMode.NoControl;
            datePicker.Location = new Point(74, 362);
            datePicker.Name = "datePicker";
            datePicker.Size = new Size(210, 39);
            datePicker.TabIndex = 8;
            // 
            // datetimeLabel
            // 
            datetimeLabel.AutoSize = true;
            datetimeLabel.Location = new Point(69, 327);
            datetimeLabel.Name = "datetimeLabel";
            datetimeLabel.Size = new Size(262, 32);
            datetimeLabel.Font = new Font("Times New Roman", 12, FontStyle.Regular, GraphicsUnit.Point, 0);
            datetimeLabel.TabIndex = 9;
            datetimeLabel.Text = "Sample Date and Time:";
            // 
            // timePicker
            // 
            timePicker.Cursor = Cursors.IBeam;
            timePicker.InsertKeyMode = InsertKeyMode.Overwrite;
            timePicker.Location = new Point(337, 362);
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
            breedLabel.Location = new Point(69, 449);
            breedLabel.Name = "breedLabel";
            breedLabel.Size = new Size(155, 32);
            breedLabel.Font = new Font("Times New Roman", 12, FontStyle.Regular, GraphicsUnit.Point, 0);
            breedLabel.TabIndex = 12;
            breedLabel.Text = "Sheep Breed:";
            // 
            // breedTextbox
            // 
            breedTextbox.Cursor = Cursors.IBeam;
            breedTextbox.Location = new Point(74, 484);
            breedTextbox.Name = "breedTextbox";
            breedTextbox.Size = new Size(552, 39);
            breedTextbox.TabIndex = 13;
            // 
            // commentsLabel
            // 
            commentsLabel.AutoSize = true;
            commentsLabel.Location = new Point(69, 565);
            commentsLabel.Name = "commentsLabel";
            commentsLabel.Size = new Size(135, 32);
            commentsLabel.Font = new Font("Times New Roman", 12, FontStyle.Regular, GraphicsUnit.Point, 0);
            commentsLabel.TabIndex = 14;
            commentsLabel.Text = "Comments:";
            // 
            // commentsTextbox
            // 
            commentsTextbox.Cursor = Cursors.IBeam;
            commentsTextbox.Location = new Point(74, 600);
            commentsTextbox.Multiline = true;
            commentsTextbox.Name = "commentsTextbox";
            commentsTextbox.Size = new Size(552, 192);
            commentsTextbox.TabIndex = 15;
            // 
            // DataUpload
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            uploadPanel.Controls.Add(commentsTextbox);
            uploadPanel.Controls.Add(commentsLabel);
            uploadPanel.Controls.Add(breedTextbox);
            uploadPanel.Controls.Add(breedLabel);
            uploadPanel.Controls.Add(timePicker);
            uploadPanel.Controls.Add(datetimeLabel);
            uploadPanel.Controls.Add(datePicker);
            uploadPanel.Controls.Add(locationTextbox);
            uploadPanel.Controls.Add(locationLabel);
            uploadPanel.Controls.Add(filenameLabel);
            uploadPanel.Controls.Add(filenameTextbox);
            uploadPanel.Controls.Add(fileuploadPictureBox);
            uploadPanel.Controls.Add(uploadButton);
            Controls.Add(helpButton);
            helpButton.BringToFront();
            Controls.Add(backButton);
            backButton.BringToFront();
            Name = "DataUpload";
            Text = "DataUpload";
            BackColor = Color.LightBlue;
            ((System.ComponentModel.ISupportInitialize)helpButton).EndInit();
            ((System.ComponentModel.ISupportInitialize)fileuploadPictureBox).EndInit();
            ResumeLayout(false);
            Refresh();
            PerformLayout();
        }

        #endregion

        private Panel uploadPanel;
        private roundButton backButton;
        private PictureBox helpButton;
        private roundButton uploadButton;
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