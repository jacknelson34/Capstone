namespace GrazeViewV1
{
    partial class DataLibrary
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
            backButton = new roundButton();
            previewButton = new roundButton();
            exportButton = new roundButton();
            dataGridView1 = new DataGridView();
            buttonPanel = new Panel();
            helpButton = new PictureBox();
            verticalScroll = new Panel();
            dataGridViewCheckBoxColumn1 = new DataGridViewCheckBoxColumn();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn7 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn8 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn9 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn10 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn11 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn12 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn13 = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            buttonPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)helpButton).BeginInit();
            verticalScroll.SuspendLayout();
            SuspendLayout();
            // 
            // buttonPanel
            // 
            buttonPanel.Controls.Add(exportButton);
            buttonPanel.Controls.Add(backButton);
            buttonPanel.Dock = DockStyle.Bottom;
            buttonPanel.Location = new Point(0, 1159);
            buttonPanel.BorderStyle = BorderStyle.None;
            buttonPanel.BackColor = Color.LightGray;
            buttonPanel.Name = "buttonPanel";
            buttonPanel.Padding = new Padding(10);
            buttonPanel.Size = new Size(this.ClientSize.Width, 120);
            buttonPanel.TabIndex = 2;
            // 
            // backButton
            // 
            backButton.Anchor = AnchorStyles.Left;
            backButton.borderRadius = 20;
            backButton.BackColor = Color.LightGreen;
            backButton.FlatStyle = FlatStyle.Flat;
            backButton.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            backButton.ForeColor = Color.Black;
            backButton.Location = new Point(50, 15);
            backButton.Name = "backButton";
            backButton.Size = new Size(150, 60);
            backButton.TabIndex = 3;
            backButton.Text = "Exit";
            backButton.UseVisualStyleBackColor = false;
            backButton.Click += backButton_Click;
            //
            // previewButton
            //
            previewButton.Anchor = AnchorStyles.Bottom;
            previewButton.borderRadius = 20;
            previewButton.BackColor = Color.LightGreen;
            previewButton.FlatStyle = FlatStyle.Flat;
            previewButton.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            previewButton.ForeColor = Color.Black;
            previewButton.Location = new Point((this.ClientSize.Width / 2) - 125, 15);
            previewButton.Name = "previewButton";
            previewButton.Size = new Size(250, 60);
            previewButton.TabIndex = 2;
            previewButton.Text = "Preview Image";
            previewButton.UseVisualStyleBackColor = false;
            previewButton.Click += previewButton_Click;
            // 
            // exportButton
            // 
            exportButton.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            exportButton.BackColor = Color.LightGreen;
            exportButton.borderRadius = 20;
            exportButton.FlatStyle = FlatStyle.Flat;
            exportButton.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            exportButton.ForeColor = Color.Black;
            exportButton.Location = new Point(this.ClientSize.Width - 200, 15);
            exportButton.Name = "exportButton";
            exportButton.Size = new Size(150, 60);
            exportButton.TabIndex = 2;
            exportButton.Text = "Export";
            exportButton.UseVisualStyleBackColor = false;
            exportButton.Click += exportButton_Click;
            // 
            // ADD buttonPanel buttons
            //
            buttonPanel.Controls.Add(exportButton);
            buttonPanel.Controls.Add(backButton);
            buttonPanel.Controls.Add(previewButton);
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.BorderStyle = BorderStyle.None;       
            dataGridView1.BackgroundColor = Color.LightBlue;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            // Add columns (these should match the data you're adding in AddUploadToGrid)
            dataGridView1.Columns.Add(new DataGridViewCheckBoxColumn { Name = "SelectCol", HeaderText = "Select Uploads" });            // 0
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "NameCol", HeaderText = "Upload Name", Width = 150 });     // 1
            //dataGridView1.Columns.Add(new DataGridViewImageColumn { Name = "ImageCol", HeaderText = "Sample Image Preview" });          // 2
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "QufuCol", HeaderText = "Qufu(%):" });                     // 3
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "QufuStemCol", HeaderText = "Qufu Stem(%):" });            // 4
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "NaleCol", HeaderText = "Nale(%):" });                     // 5
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "ErciCol", HeaderText = "Erci(%):" });                     // 6
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "AirBubbleCol", HeaderText = "Air Bubble(%):" });          // 7
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "SampleDateCol", HeaderText = "Date Sample Taken" });      // 8
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "SampleTimeCol", HeaderText = "Time Sample Taken" });      // 9
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "UploadDateCol", HeaderText = "Upload Date" });            // 10
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "UploadTimeCol", HeaderText = "Upload Time" });            // 11
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "SampleLocationCol", HeaderText = "Sample Location" });    // 12
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "SheepBreedCol", HeaderText = "Sheep Breed" });            // 13
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "CommentsCol", HeaderText = "Comments", Width = 300, AutoSizeMode = DataGridViewAutoSizeColumnMode.None });                 // 14

            // Set text wrapping for comments
            dataGridView1.Columns["CommentsCol"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            // Enable row height adjustment
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Font = new Font("Times New Roman", 14, FontStyle.Bold, GraphicsUnit.Pixel, 0);
            //dataGridView1.RowHeadersWidth = 82;
            dataGridView1.RowTemplate.Height = 40;
            dataGridView1.Size = new Size(2486, 1159);
            dataGridView1.TabIndex = 0;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            // 
            // helpButton
            // 
            helpButton.Location = new Point(0, 0);
            helpButton.Name = "helpButton";
            helpButton.Size = new Size(100, 50);
            helpButton.TabIndex = 0;
            helpButton.TabStop = false;
            // 
            // verticalScroll
            // 
            verticalScroll.AutoScroll = true;
            verticalScroll.AutoScrollMinSize = new Size(0, 200);
            verticalScroll.Controls.Add(dataGridView1);
            verticalScroll.Dock = DockStyle.Fill;
            verticalScroll.Location = new Point(0, 0);
            verticalScroll.Name = "verticalScroll";
            verticalScroll.Size = new Size(2486, 1159);
            verticalScroll.TabIndex = 1;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            dataGridViewCheckBoxColumn1.MinimumWidth = 10;
            dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            dataGridViewCheckBoxColumn1.Width = 250;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.MinimumWidth = 10;
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.Width = 180;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.MinimumWidth = 10;
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.Width = 150;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.MinimumWidth = 10;
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            dataGridViewTextBoxColumn3.Width = 250;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewTextBoxColumn4.MinimumWidth = 10;
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            dataGridViewTextBoxColumn4.Width = 150;
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewTextBoxColumn5.MinimumWidth = 10;
            dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            dataGridViewTextBoxColumn5.Width = 200;
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewTextBoxColumn6.MinimumWidth = 10;
            dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            dataGridViewTextBoxColumn6.Width = 200;
            // 
            // dataGridViewTextBoxColumn7
            // 
            dataGridViewTextBoxColumn7.MinimumWidth = 10;
            dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            dataGridViewTextBoxColumn7.Width = 200;
            // 
            // dataGridViewTextBoxColumn8
            // 
            dataGridViewTextBoxColumn8.MinimumWidth = 10;
            dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            dataGridViewTextBoxColumn8.Width = 200;
            // 
            // dataGridViewTextBoxColumn9
            // 
            dataGridViewTextBoxColumn9.MinimumWidth = 10;
            dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            dataGridViewTextBoxColumn9.Width = 200;
            // 
            // dataGridViewTextBoxColumn10
            // 
            dataGridViewTextBoxColumn10.MinimumWidth = 10;
            dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            dataGridViewTextBoxColumn10.Width = 200;
            // 
            // dataGridViewTextBoxColumn11
            // 
            dataGridViewTextBoxColumn11.MinimumWidth = 10;
            dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            dataGridViewTextBoxColumn11.Width = 200;
            // 
            // dataGridViewTextBoxColumn12
            // 
            dataGridViewTextBoxColumn12.MinimumWidth = 10;
            dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            dataGridViewTextBoxColumn12.Width = 200;
            // 
            // dataGridViewTextBoxColumn13
            // 
            dataGridViewTextBoxColumn13.MinimumWidth = 10;
            dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            dataGridViewTextBoxColumn13.Width = 200;
            //
            // Setting widths per title length
            //
            dataGridView1.Columns["SelectCol"].Width = 120;          // "Select Uploads"
            dataGridView1.Columns["NameCol"].Width = 160;            // "Upload Name"
            dataGridView1.Columns["QufuCol"].Width = 80;            // "Qufu(%):"
            dataGridView1.Columns["QufuStemCol"].Width = 150;        // "Qufu Stem(%):"
            dataGridView1.Columns["NaleCol"].Width = 80;            // "Nale(%):"
            dataGridView1.Columns["ErciCol"].Width = 80;            // "Erci(%):"
            dataGridView1.Columns["AirBubbleCol"].Width = 150;       // "Air Bubble(%):"
            dataGridView1.Columns["SampleDateCol"].Width = 180;      // "Date Sample Taken"
            dataGridView1.Columns["SampleTimeCol"].Width = 180;      // "Time Sample Taken"
            dataGridView1.Columns["UploadDateCol"].Width = 150;      // "Upload Date"
            dataGridView1.Columns["UploadTimeCol"].Width = 150;      // "Upload Time"
            dataGridView1.Columns["SampleLocationCol"].Width = 200;  // "Sample Location"
            dataGridView1.Columns["SheepBreedCol"].Width = 150;      // "Sheep Breed"
            dataGridView1.Columns["CommentsCol"].Width = 300;
            //
            // Temporary clear button
            //
            clearDataButton = new roundButton();
            clearDataButton.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            clearDataButton.BackColor = Color.LightGreen;
            clearDataButton.FlatStyle = FlatStyle.Flat;
            clearDataButton.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            clearDataButton.ForeColor = Color.Black;
            clearDataButton.TabIndex = 3;
            clearDataButton.UseVisualStyleBackColor = false;
            clearDataButton.Text = "Clear all Data";
            clearDataButton.borderRadius = 20;
            clearDataButton.Size = new Size(300, 60);
            clearDataButton.Location = new Point(exportButton.Location.X + 150, 15);
            clearDataButton.Click += clearDataButton_Click;
            buttonPanel.Controls.Add(clearDataButton);
            // 
            // DataLibrary
            // 
            StartPosition = FormStartPosition.Manual;
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            //ClientSize = new Size(2486, 1279);
            MinimumSize = new Size(1400, 918);
            Controls.Add(helpButton);
            Controls.Add(verticalScroll);
            Controls.Add(buttonPanel);
            Name = "DataLibrary";
            Text = "DataLibrary";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            buttonPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)helpButton).EndInit();
            verticalScroll.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private roundButton backButton;
        private roundButton previewButton;
        private DataGridView dataGridView1;
        private PictureBox helpButton;
        private Panel verticalScroll;
        private Panel buttonPanel;
        private roundButton exportButton;
        private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;

        // Temp button
        private roundButton clearDataButton;
    }
}