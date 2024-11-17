namespace GrazeViewV1
{
    partial class ResultPage
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
            resultsPagePanel = new Panel();
            MLOutputPanel = new Panel();
            UserOutputPanel = new Panel();
            exitButton = new roundButton();
            dataViewButton = new roundButton();
            returnToUploadButton = new roundButton();
            ClientSize = new Size(1254, 847);
            resultsPagePanel.SuspendLayout();
            SuspendLayout();
            // 
            // resultsPagePanel
            // 
            resultsPagePanel.Anchor = AnchorStyles.None;
            resultsPagePanel.BorderStyle = BorderStyle.Fixed3D;
            resultsPagePanel.Name = "resultsPagePanel";
            resultsPagePanel.Size = new Size((int)(this.ClientSize.Width * 0.637), (int)(this.ClientSize.Height * .708));
            resultsPagePanel.Location = new Point((this.ClientSize.Width / 2)-(resultsPagePanel.Width /2), (this.ClientSize.Height / 2)-(resultsPagePanel.Height / 2) - 100);
            resultsPagePanel.TabIndex = 1;
            // 
            // MLOutputPanel
            // 
            MLOutputPanel.BorderStyle = BorderStyle.FixedSingle;
            MLOutputPanel.Location = new Point(500, 400);
            MLOutputPanel.Name = "MLOutputPanel";
            MLOutputPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            MLOutputPanel.Size = new Size((int)(resultsPagePanel.Width * 0.375), 200);
            MLOutputPanel.TabIndex = 0;
            // 
            // UserOutputPanel
            // 
            UserOutputPanel.BorderStyle = BorderStyle.FixedSingle;
            UserOutputPanel.Location = new Point(0, 400);
            UserOutputPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            UserOutputPanel.Name = "UserOutputPanel";
            UserOutputPanel.Size = new Size((int)(resultsPagePanel.Width * 0.625), 200);
            UserOutputPanel.TabIndex = 1;
            // 
            // exitButton
            // 
            exitButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            exitButton.BackColor = Color.LightGreen;
            exitButton.borderRadius = 20;
            exitButton.FlatStyle = FlatStyle.Flat;
            exitButton.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            exitButton.ForeColor = Color.Black;
            exitButton.Location = new Point((this.ClientSize.Width - 150), (this.ClientSize.Height - 75));
            exitButton.Name = "exitButton";
            exitButton.Size = new Size(125, 50);
            exitButton.TabIndex = 0;
            exitButton.Text = "Exit";
            exitButton.UseVisualStyleBackColor = false;
            exitButton.Click += returnButton_Click;
            // 
            // returnToUploadButton
            // 
            returnToUploadButton.BackColor = Color.LightGreen;
            returnToUploadButton.FlatAppearance.BorderSize = 0;
            returnToUploadButton.borderRadius = 20;
            returnToUploadButton.FlatStyle = FlatStyle.Flat;
            returnToUploadButton.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            returnToUploadButton.ForeColor = Color.Black;
            returnToUploadButton.Location = new Point((this.ClientSize.Width / 2) - (returnToUploadButton.Width / 2) - 275, (this.ClientSize.Height - 100));
            returnToUploadButton.Anchor = AnchorStyles.Bottom;
            returnToUploadButton.Name = "returnToUploadButton";
            returnToUploadButton.Size = new Size(300, 75);
            returnToUploadButton.TabIndex = 2;
            returnToUploadButton.Text = "Upload Data";
            returnToUploadButton.UseVisualStyleBackColor = false;
            returnToUploadButton.Click += returnToUploadButton_Click;
            //
            // dataViewButton
            //
            dataViewButton.BackColor = Color.LightGreen;
            dataViewButton.FlatAppearance.BorderSize = 0;
            dataViewButton.borderRadius = 20;
            dataViewButton.FlatStyle = FlatStyle.Flat;
            dataViewButton.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataViewButton.ForeColor = Color.Black;
            dataViewButton.Location = new Point((this.ClientSize.Width / 2) - (dataViewButton.Width / 2) + 75, (this.ClientSize.Height - 100));
            dataViewButton.Anchor = AnchorStyles.Bottom;
            dataViewButton.Name = "dataViewButton";
            dataViewButton.Size = new Size(300, 75);
            dataViewButton.TabIndex = 2;
            dataViewButton.Text = "View in Library";
            dataViewButton.UseVisualStyleBackColor = false;
            dataViewButton.Click += dataViewButton_Click;
            // 
            // ResultPage
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.LightBlue;
            CreateUserProvidedDataControls(UserOutputPanel);
            CreateMLDataControls(MLOutputPanel);
            resultsPagePanel.Controls.Add(UserOutputPanel);
            resultsPagePanel.Controls.Add(MLOutputPanel);
            Controls.Add(returnToUploadButton);
            Controls.Add(dataViewButton);
            Controls.Add(exitButton);
            Controls.Add(resultsPagePanel);
            MinimumSize = new Size(1280, 918);
            Name = "ResultPage";
            Text = "Result Page";
            resultsPagePanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        // Helper method to create a title label for each panel
        private Label CreatePanelTitle(string titleText)
        {
            Label title = new Label();
            title.Text = titleText;
            title.Font = new Font("Times New Roman", 10, FontStyle.Bold);
            title.Location = new Point(10, 10);
            title.AutoSize = true;
            return title;
        }

        // Helper method to create ML Data controls using seamless textboxes
        private void CreateMLDataControls(Panel panel)
        {
            int currentYPosition = 40;  // Starting below the title
            int labelWidth = 120;
            int textBoxWidth = 150;
            Font commonFont = new Font("Times New Roman", 10, FontStyle.Regular);

            // Create panel title
            Label title = CreatePanelTitle("Model Generated Data:");
            panel.Controls.Add(title);

            // Create and add controls for each ML data field using seamless textboxes
            qufuLabel = CreateLabel("Qufu (%):", new Point(10, currentYPosition));
            qufuTextBox = CreateSeamlessTextBox(commonFont, new Point(labelWidth+10, currentYPosition), textBoxWidth);
            panel.Controls.Add(qufuLabel);
            panel.Controls.Add(qufuTextBox);

            currentYPosition += 30;
            erciLabel = CreateLabel("Erci (%):", new Point(10, currentYPosition));
            erciTextBox = CreateSeamlessTextBox(commonFont, new Point(labelWidth + 10, currentYPosition), textBoxWidth);
            panel.Controls.Add(erciLabel);
            panel.Controls.Add(erciTextBox);

            currentYPosition += 30;
            naleLabel = CreateLabel("Nale (%):", new Point(10, currentYPosition));
            naleTextBox = CreateSeamlessTextBox(commonFont, new Point(labelWidth + 10, currentYPosition), textBoxWidth);
            panel.Controls.Add(naleLabel);
            panel.Controls.Add(naleTextBox);

            currentYPosition += 30;
            bubbleLabel = CreateLabel("Bubbles (%):", new Point(10, currentYPosition));
            bubbleTextBox = CreateSeamlessTextBox(commonFont, new Point(labelWidth+ 40, currentYPosition), textBoxWidth);
            panel.Controls.Add(bubbleLabel);
            panel.Controls.Add(bubbleTextBox);

            currentYPosition += 30;
            stemLabel = CreateLabel("Stem (%):", new Point(10, currentYPosition));
            stemTextBox = CreateSeamlessTextBox(commonFont, new Point(labelWidth+10, currentYPosition), textBoxWidth);
            panel.Controls.Add(stemLabel);
            panel.Controls.Add(stemTextBox);
        }

        // Helper method to create User Provided Data controls using seamless textboxes
        private void CreateUserProvidedDataControls(Panel panel)
        {
            int currentYPosition = 40;  // Starting below the title
            int labelWidth = 120;
            int textBoxWidth = 300;
            Font commonFont = new Font("Times New Roman", 10, FontStyle.Regular);

            // Create panel title
            Label title = CreatePanelTitle("User Provided Data:");
            panel.Controls.Add(title);

            // Create and add controls for each user data field using seamless textboxes
            uploadNameLabel = CreateLabel("Upload Name:", new Point(10, currentYPosition));
            uploadNameTextBox = CreateSeamlessTextBox(commonFont, new Point(labelWidth+60, currentYPosition), textBoxWidth);
            panel.Controls.Add(uploadNameLabel);
            panel.Controls.Add(uploadNameTextBox);

            currentYPosition += 30;
            dateUploadedLabel = CreateLabel("Date Uploaded:", new Point(10, currentYPosition));
            dateUploadedTextBox = CreateSeamlessTextBox(commonFont, new Point(labelWidth+80, currentYPosition), textBoxWidth);
            panel.Controls.Add(dateUploadedLabel);
            panel.Controls.Add(dateUploadedTextBox);

            currentYPosition += 30;
            dateOfSampleLabel = CreateLabel("Date of Sample:", new Point(10, currentYPosition));
            dateOfSampleTextBox = CreateSeamlessTextBox(commonFont, new Point(labelWidth+80, currentYPosition), textBoxWidth);
            panel.Controls.Add(dateOfSampleLabel);
            panel.Controls.Add(dateOfSampleTextBox);

            currentYPosition += 30;
            sampleLocationLabel = CreateLabel("Sample Location:", new Point(10, currentYPosition));
            sampleLocationTextBox = CreateSeamlessTextBox(commonFont, new Point(labelWidth+100, currentYPosition), textBoxWidth);
            panel.Controls.Add(sampleLocationLabel);
            panel.Controls.Add(sampleLocationTextBox);

            currentYPosition += 30;
            sheepBreedLabel = CreateLabel("Sheep Breed:", new Point(10, currentYPosition));
            sheepBreedTextBox = CreateSeamlessTextBox(commonFont, new Point(labelWidth+50, currentYPosition), textBoxWidth);
            panel.Controls.Add(sheepBreedLabel);
            panel.Controls.Add(sheepBreedTextBox);
        }

        // Helper method to create seamless textboxes
        private TextBox CreateSeamlessTextBox(Font font, Point location, int width)
        {
            TextBox textBox = new TextBox();
            textBox.Location = location;
            textBox.TabStop = false;
            textBox.Cursor = Cursors.Default;
            textBox.Size = new Size(width, 20);
            textBox.ReadOnly = true;  // Make the textboxes read-only
            textBox.BorderStyle = BorderStyle.None;  // Remove the border for a seamless look
            textBox.BackColor = this.BackColor;  // Match the background color for a seamless effect
            textBox.Font = font;  // Set the common font
            return textBox;
        }

        // Helper method to create labels
        private Label CreateLabel(string labelText, Point location)
        {
            Label label = new Label();
            label.Text = labelText;
            label.Location = location;
            label.AutoSize = true;
            return label;
        }


        #endregion

        // Panel 1 - ML Data
        private Panel MLOutputPanel;
        private Label qufuLabel;
        private TextBox qufuTextBox;
        private Label erciLabel;
        private TextBox erciTextBox;
        private Label bubbleLabel;
        private TextBox bubbleTextBox;
        private Label naleLabel;
        private TextBox naleTextBox;
        private Label stemLabel;
        private TextBox stemTextBox;

        // Panel 2 - User provided Data
        private Panel UserOutputPanel;
        private Label uploadNameLabel;
        private TextBox uploadNameTextBox;
        private Label dateUploadedLabel;
        private TextBox dateUploadedTextBox;
        private Label dateOfSampleLabel;
        private TextBox dateOfSampleTextBox;
        private Label sampleLocationLabel;
        private TextBox sampleLocationTextBox;
        private Label sheepBreedLabel;
        private TextBox sheepBreedTextBox;

        // Panel 3 - For resizing purposes
        private Panel resultsPagePanel;
        private roundButton exitButton;
        private roundButton returnToUploadButton;
        private roundButton dataViewButton;

        // if there is any easier way of initializing these please tell me lol
    }
}