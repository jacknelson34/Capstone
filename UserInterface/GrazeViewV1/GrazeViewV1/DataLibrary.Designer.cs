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
            exportButton = new roundButton();
            dataGridView1 = new DataGridView();
            buttonPanel = new Panel();
            sortByBox = new customBox();
            helpButton = new PictureBox();
            sortByLabel = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)helpButton).BeginInit();
            SuspendLayout();

            // dataGridView1 Setup -----------------------------------------------------
            // Initialize DataGridView
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            // Add columns (these should match the data you're adding in AddUploadToGrid)
            dataGridView1.Columns.Add(new DataGridViewCheckBoxColumn { Name = "SelectCol", HeaderText = "Select Uploads" });            // 0
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "NameCol", HeaderText = "Upload Name", Width = 150 });     // 1
            dataGridView1.Columns.Add(new DataGridViewImageColumn { Name = "ImageCol", HeaderText = "Sample Image Preview" });          // 2
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
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "CommentsCol", HeaderText = "Comments" });                 // 14

            // Ensure dataViewGrid1 fills the page
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.BackgroundColor = Color.LightBlue;
            dataGridView1.Visible = true;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = null;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowTemplate.Height = 40;
            // dataGridView1 Setup -----------------------------------------------------

            // buttonPanel setup -------------------------------------------------------
            // Panel to hold all buttons
            buttonPanel.Dock = DockStyle.Bottom;
            buttonPanel.Height = 120;
            buttonPanel.Padding = new Padding(10);

            // ComboBox to sort dataLibrary by different elements
            // Currently can sort by name, upload time, sample time, sheep breed
            sortByBox.Items.AddRange(new string[] { "Upload Name", "Upload Date/Time", "Sample Date/Time", "Sheep Breed", "Nale %", "Erci %", "Qufu %", "Qufu Stem %", "Air Bubble %" });
            sortByBox.DropDownStyle = ComboBoxStyle.DropDownList;
            //sortByBox.SelectedIndex = 0;
            sortByBox.Width = 300;
            //sortByBox.Anchor = AnchorStyles.None;
            //sortByBox.SelectedIndexChanged += sortByBox_SelectedIndexChanged;
            buttonPanel.Controls.Add(sortByBox);

            // SortByLabel Setup -------------------------------------------------------
            sortByLabel.Text = "Sort by:";
            sortByLabel.Width = 100;
            sortByLabel.Height = 40;
            sortByLabel.TextAlign = ContentAlignment.MiddleCenter;
            sortByLabel.Anchor = AnchorStyles.None;
            buttonPanel.Controls.Add(sortByLabel);

            // Export button to open ExpandedView of data (single or multiple)
            exportButton.Text = "Export";
            exportButton.borderRadius = 20;
            exportButton.Width = 150;
            exportButton.Height = 50;
            exportButton.Anchor = AnchorStyles.None;
            buttonPanel.Controls.Add(exportButton);
            exportButton.Click += exportButton_Click;

            // Back Button to return to mainpage
            backButton.Text = "Exit";
            backButton.Width = 150;
            backButton.Height = 50;
            backButton.borderRadius = 20;
            backButton.Anchor = AnchorStyles.None;
            buttonPanel.Controls.Add(backButton);
            backButton.Click += backButton_Click;

            // buttonPanel setup -------------------------------------------------------

            //
            // ScrollBar - Horizontal
            //
            verticalScroll = new Panel();
            verticalScroll.Dock = DockStyle.Fill;
            verticalScroll.AutoScroll = true;
            verticalScroll.AutoScrollMinSize = new Size(0, 200);
            verticalScroll.Controls.Add(dataGridView1);
            // 
            // DataLibrary
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2486, 1279);
            Controls.Add(helpButton);
            Controls.Add(verticalScroll);
            Controls.Add(buttonPanel);
            Name = "DataLibrary";
            Text = "DataLibrary";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)helpButton).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private roundButton backButton;
        private DataGridView dataGridView1;
        private PictureBox helpButton;
        private Panel verticalScroll;
        private Panel buttonPanel;
        private customBox sortByBox;
        private roundButton exportButton;
        private Label sortByLabel;
    }
}