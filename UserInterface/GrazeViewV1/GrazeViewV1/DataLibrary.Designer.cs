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
            backButton = new Button();
            dataGridView1 = new DataGridView();
            /*Column1 = new DataGridViewCheckBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewLinkColumn();
            Column4 = new DataGridViewLinkColumn();
            Column5 = new DataGridViewLinkColumn();
            Column6 = new DataGridViewLinkColumn();
            Column7 = new DataGridViewComboBoxColumn();
            Column8 = new DataGridViewTextBoxColumn();
            Column9 = new DataGridViewTextBoxColumn();
            Column10 = new DataGridViewTextBoxColumn();
            Column11 = new DataGridViewImageColumn();
            Column12 = new DataGridViewButtonColumn();*/
            helpButton = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)helpButton).BeginInit();
            SuspendLayout();

            // TESTING ---------------------------------------------
            // Initialize DataGridView
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            // Add columns (these should match the data you're adding in AddUploadToGrid)
            dataGridView1.Columns.Add(new DataGridViewCheckBoxColumn { HeaderText = "Select Uploads" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Upload Name" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Date Sample Taken" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Time Sample Taken" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Upload Date" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Upload Time" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Sample Location" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Sheep Breed" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Comments" });
            dataGridView1.Columns.Add(new DataGridViewImageColumn { HeaderText = "Sample Image Preview" });
            dataGridView1.Columns.Add(new DataGridViewButtonColumn { HeaderText = "Export" });

            // Ensure dataViewGrid1 fills the page
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.BackgroundColor = Color.LightBlue;
            dataGridView1.Visible = true;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = null;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowTemplate.Height = 40;

            // Test row to ensure DataGridView works
            // dataGridView1.Rows.Add(false, "Test Upload", "01/01/2024", "12:00 PM", "01/01/2024", "12:01 PM", null, "Test Location", "Test Sheep", "Test Comment", null, "Export");



            // TESTING ---------------------------------------------------------


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
            backButton.Location = new Point(25, 22);
            backButton.Name = "backButton";
            backButton.Size = new Size(123, 69);
            backButton.TabIndex = 1;
            backButton.Text = "Back";
            backButton.UseVisualStyleBackColor = false;
            backButton.Click += backButton_Click;
            /*
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToOrderColumns = true;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column3, Column4, Column5, Column6, Column7, Column8, Column9, Column10, Column11, Column12 });
            dataGridView1.Location = new Point(26, 153);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 82;
            dataGridView1.Size = new Size(2439, 1094);
            dataGridView1.TabIndex = 2;
            // 
            // Column1
            // 
            Column1.HeaderText = "Select Uploads";
            Column1.MinimumWidth = 10;
            Column1.Name = "Column1";
            Column1.Width = 100;
            // 
            // Column2
            // 
            Column2.HeaderText = "Upload Name";
            Column2.MinimumWidth = 10;
            Column2.Name = "Column2";
            Column2.Width = 200;
            // 
            // Column3
            // 
            Column3.HeaderText = "Date Sample Taken";
            Column3.MinimumWidth = 10;
            Column3.Name = "Column3";
            Column3.Width = 200;
            // 
            // Column4
            // 
            Column4.HeaderText = "Time Sample Taken";
            Column4.MinimumWidth = 10;
            Column4.Name = "Column4";
            Column4.Width = 200;
            // 
            // Column5
            // 
            Column5.HeaderText = "Upload Date";
            Column5.MinimumWidth = 10;
            Column5.Name = "Column5";
            Column5.Width = 200;
            // 
            // Column6
            // 
            Column6.HeaderText = "Upload Time";
            Column6.MinimumWidth = 10;
            Column6.Name = "Column6";
            Column6.Width = 200;
            // 
            // Column7
            // 
            Column7.HeaderText = "Date & Time";
            Column7.MinimumWidth = 10;
            Column7.Name = "Column7";
            Column7.Width = 200;
            // 
            // Column8
            // 
            Column8.HeaderText = "Sample Location";
            Column8.MinimumWidth = 10;
            Column8.Name = "Column8";
            Column8.Width = 200;
            // 
            // Column9
            // 
            Column9.HeaderText = "Sheep Breed";
            Column9.MinimumWidth = 10;
            Column9.Name = "Column9";
            Column9.Width = 200;
            // 
            // Column10
            // 
            Column10.HeaderText = "Comments";
            Column10.MinimumWidth = 10;
            Column10.Name = "Column10";
            Column10.Width = 200;
            // 
            // Column11
            // 
            Column11.HeaderText = "Sample Image Preview";
            Column11.MinimumWidth = 10;
            Column11.Name = "Column11";
            Column11.Width = 200;
            // 
            // Column12
            // 
            Column12.HeaderText = "Export";
            Column12.MinimumWidth = 10;
            Column12.Name = "Column12";
            Column12.Width = 200;
            */
            // 
            // helpButton
            // 
            helpButton.Cursor = Cursors.Hand;
            helpButton.Image = Properties.Resources.Help_Icon;
            helpButton.Location = new Point(2401, 12);
            helpButton.Name = "helpButton";
            helpButton.Size = new Size(73, 68);
            helpButton.SizeMode = PictureBoxSizeMode.StretchImage;
            helpButton.TabIndex = 3;
            helpButton.TabStop = false;
            helpButton.Click += helpButton_Click;
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
            Controls.Add(dataGridView1);
            Controls.Add(backButton);
            Name = "DataLibrary";
            Text = "DataLibrary";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)helpButton).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button backButton;
        private DataGridView dataGridView1;
        /*private DataGridViewCheckBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewLinkColumn Column3;
        private DataGridViewLinkColumn Column4;
        private DataGridViewLinkColumn Column5;
        private DataGridViewLinkColumn Column6;
        private DataGridViewComboBoxColumn Column7;
        private DataGridViewTextBoxColumn Column8;
        private DataGridViewTextBoxColumn Column9;
        private DataGridViewTextBoxColumn Column10;
        private DataGridViewImageColumn Column11;
        private DataGridViewButtonColumn Column12;*/
        private PictureBox helpButton;
        private Panel verticalScroll;
    }
}